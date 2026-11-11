using System.Data;
using Microsoft.Data.Sqlite;
using LojaCupcakes.Models;

namespace LojaCupcakes.Data;

public class DatabaseHelper
{
    private readonly string _connectionString;

    public DatabaseHelper(IConfiguration configuration, IWebHostEnvironment env)
    {
        var dbPath = Path.Combine(env.ContentRootPath, "database.db");
        _connectionString = $"Data Source={dbPath}";
        InicializarBancoSeNecessario(dbPath);
    }

    private void InicializarBancoSeNecessario(string dbPath)
    {
        if (!File.Exists(dbPath))
        {
            var schemaPath = Path.Combine(Path.GetDirectoryName(dbPath)!, "schema.sql");
            if (File.Exists(schemaPath))
            {
                var sql = File.ReadAllText(schemaPath);
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }
    }

    private SqliteConnection GetConnection()
    {
        var conn = new SqliteConnection(_connectionString);
        conn.Open();
        return conn;
    }

    // ==========================================
    // PRODUTOS (5 Sabores Oficiais Invertidos)
    // ==========================================
    public List<Produto> ObterProdutos()
    {
        var lista = new List<Produto>();
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nome, Descricao, Preco, Disponivel, Categoria, ImagemUrl, CriadoEm FROM Produtos WHERE Disponivel = 1 ORDER BY Id ASC";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Produto
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Descricao = reader.GetString(2),
                Preco = reader.GetDecimal(3),
                Disponivel = reader.GetBoolean(4),
                Categoria = reader.IsDBNull(5) ? "Cupcake Gourmet" : reader.GetString(5),
                ImagemUrl = reader.IsDBNull(6) ? "/images/cupcake-default.svg" : reader.GetString(6),
                CriadoEm = reader.GetDateTime(7)
            });
        }
        return lista;
    }

    public Produto? ObterProdutoPorId(int id)
    {
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nome, Descricao, Preco, Disponivel, Categoria, ImagemUrl, CriadoEm FROM Produtos WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Produto
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Descricao = reader.GetString(2),
                Preco = reader.GetDecimal(3),
                Disponivel = reader.GetBoolean(4),
                Categoria = reader.IsDBNull(5) ? "Cupcake Gourmet" : reader.GetString(5),
                ImagemUrl = reader.IsDBNull(6) ? "/images/cupcake-default.svg" : reader.GetString(6),
                CriadoEm = reader.GetDateTime(7)
            };
        }
        return null;
    }

    // ==========================================
    // CLIENTES
    // ==========================================
    public Cliente? ObterClientePorEmail(string email)
    {
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nome, Email, SenhaHash, Telefone, EnderecoPadrao, CriadoEm FROM Clientes WHERE LOWER(Email) = LOWER(@email)";
        cmd.Parameters.AddWithValue("@email", email.Trim());

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Cliente
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Email = reader.GetString(2),
                SenhaHash = reader.GetString(3),
                Telefone = reader.IsDBNull(4) ? null : reader.GetString(4),
                EnderecoPadrao = reader.IsDBNull(5) ? null : reader.GetString(5),
                CriadoEm = reader.GetDateTime(6)
            };
        }
        return null;
    }

    public Cliente? ObterClientePorId(int id)
    {
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Id, Nome, Email, SenhaHash, Telefone, EnderecoPadrao, CriadoEm FROM Clientes WHERE Id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Cliente
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Email = reader.GetString(2),
                SenhaHash = reader.GetString(3),
                Telefone = reader.IsDBNull(4) ? null : reader.GetString(4),
                EnderecoPadrao = reader.IsDBNull(5) ? null : reader.GetString(5),
                CriadoEm = reader.GetDateTime(6)
            };
        }
        return null;
    }

    public int CadastrarCliente(Cliente cliente)
    {
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Clientes (Nome, Email, SenhaHash, Telefone, EnderecoPadrao)
            VALUES (@nome, @email, @senha, @tel, @end);
            SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("@nome", cliente.Nome);
        cmd.Parameters.AddWithValue("@email", cliente.Email.Trim().ToLower());
        cmd.Parameters.AddWithValue("@senha", cliente.SenhaHash);
        cmd.Parameters.AddWithValue("@tel", (object?)cliente.Telefone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@end", (object?)cliente.EnderecoPadrao ?? DBNull.Value);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    // ==========================================
    // PEDIDOS & TRANSAÇÃO ATÔMICA 3FN
    // ==========================================
    public int CriarPedido(Pedido pedido, List<ItemCarrinho> itens)
    {
        using var conn = GetConnection();
        using var trans = conn.BeginTransaction();

        try
        {
            // 1. Inserir Pedido principal
            using var cmdPedido = conn.CreateCommand();
            cmdPedido.Transaction = trans;
            cmdPedido.CommandText = @"
                INSERT INTO Pedidos (
                    ClienteId, ValorProdutos, TaxaEntrega, ValorTotal, FormaPagamento,
                    StatusPagamento, StatusEntrega, EnderecoEntrega, PrazoEstimado, Observacoes
                ) VALUES (
                    @cliId, @valProd, @taxa, @valTotal, @forma,
                    @statusPag, @statusEnt, @endereco, @prazo, @obs
                );
                SELECT last_insert_rowid();";

            cmdPedido.Parameters.AddWithValue("@cliId", pedido.ClienteId);
            cmdPedido.Parameters.AddWithValue("@valProd", pedido.ValorProdutos);
            cmdPedido.Parameters.AddWithValue("@taxa", pedido.TaxaEntrega);
            cmdPedido.Parameters.AddWithValue("@valTotal", pedido.ValorTotal);
            cmdPedido.Parameters.AddWithValue("@forma", pedido.FormaPagamento);
            cmdPedido.Parameters.AddWithValue("@statusPag", pedido.StatusPagamento);
            cmdPedido.Parameters.AddWithValue("@statusEnt", pedido.StatusEntrega);
            cmdPedido.Parameters.AddWithValue("@endereco", pedido.EnderecoEntrega);
            cmdPedido.Parameters.AddWithValue("@prazo", pedido.PrazoEstimado);
            cmdPedido.Parameters.AddWithValue("@obs", (object?)pedido.Observacoes ?? DBNull.Value);

            var pedidoId = Convert.ToInt32(cmdPedido.ExecuteScalar());

            // 2. Inserir Itens do Pedido (N:M normalizada)
            foreach (var item in itens)
            {
                using var cmdItem = conn.CreateCommand();
                cmdItem.Transaction = trans;
                cmdItem.CommandText = @"
                    INSERT INTO ItensPedido (PedidoId, ProdutoId, Quantidade, PrecoUnitario, Subtotal)
                    VALUES (@pedId, @prodId, @qtd, @preco, @subtotal);";

                cmdItem.Parameters.AddWithValue("@pedId", pedidoId);
                cmdItem.Parameters.AddWithValue("@prodId", item.ProdutoId);
                cmdItem.Parameters.AddWithValue("@qtd", item.Quantidade);
                cmdItem.Parameters.AddWithValue("@preco", item.PrecoUnitario);
                cmdItem.Parameters.AddWithValue("@subtotal", item.Subtotal);

                cmdItem.ExecuteNonQuery();
            }

            trans.Commit();
            return pedidoId;
        }
        catch
        {
            trans.Rollback();
            throw;
        }
    }

    public Pedido? ObterPedidoPorId(int id)
    {
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT p.Id, p.ClienteId, p.DataPedido, p.ValorProdutos, p.TaxaEntrega, p.ValorTotal,
                   p.FormaPagamento, p.StatusPagamento, p.StatusEntrega, p.EnderecoEntrega, p.PrazoEstimado, p.Observacoes,
                   c.Nome, c.Email, c.Telefone
            FROM Pedidos p
            INNER JOIN Clientes c ON p.ClienteId = c.Id
            WHERE p.Id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        Pedido? pedido = null;
        using (var reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                pedido = new Pedido
                {
                    Id = reader.GetInt32(0),
                    ClienteId = reader.GetInt32(1),
                    DataPedido = reader.GetDateTime(2),
                    ValorProdutos = reader.GetDecimal(3),
                    TaxaEntrega = reader.GetDecimal(4),
                    ValorTotal = reader.GetDecimal(5),
                    FormaPagamento = reader.GetString(6),
                    StatusPagamento = reader.GetString(7),
                    StatusEntrega = reader.GetString(8),
                    EnderecoEntrega = reader.GetString(9),
                    PrazoEstimado = reader.IsDBNull(10) ? "15 a 30 minutos" : reader.GetString(10),
                    Observacoes = reader.IsDBNull(11) ? null : reader.GetString(11),
                    Cliente = new Cliente
                    {
                        Id = reader.GetInt32(1),
                        Nome = reader.GetString(12),
                        Email = reader.GetString(13),
                        Telefone = reader.IsDBNull(14) ? null : reader.GetString(14)
                    }
                };
            }
        }

        if (pedido != null)
        {
            // Carregar itens do pedido
            using var cmdItens = conn.CreateCommand();
            cmdItens.CommandText = @"
                SELECT i.Id, i.PedidoId, i.ProdutoId, i.Quantidade, i.PrecoUnitario, i.Subtotal,
                       pr.Nome, pr.ImagemUrl
                FROM ItensPedido i
                INNER JOIN Produtos pr ON i.ProdutoId = pr.Id
                WHERE i.PedidoId = @pedId";
            cmdItens.Parameters.AddWithValue("@pedId", id);

            using var readerItens = cmdItens.ExecuteReader();
            while (readerItens.Read())
            {
                pedido.Itens.Add(new ItemPedido
                {
                    Id = readerItens.GetInt32(0),
                    PedidoId = readerItens.GetInt32(1),
                    ProdutoId = readerItens.GetInt32(2),
                    Quantidade = readerItens.GetInt32(3),
                    PrecoUnitario = readerItens.GetDecimal(4),
                    Subtotal = readerItens.GetDecimal(5),
                    Produto = new Produto
                    {
                        Id = readerItens.GetInt32(2),
                        Nome = readerItens.GetString(6),
                        ImagemUrl = readerItens.IsDBNull(7) ? "/images/cupcake-default.svg" : readerItens.GetString(7)
                    }
                });
            }
        }

        return pedido;
    }

    public List<Pedido> ObterTodosPedidos()
    {
        var lista = new List<Pedido>();
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT p.Id, p.ClienteId, p.DataPedido, p.ValorProdutos, p.TaxaEntrega, p.ValorTotal,
                   p.FormaPagamento, p.StatusPagamento, p.StatusEntrega, p.EnderecoEntrega, p.PrazoEstimado,
                   c.Nome, c.Email
            FROM Pedidos p
            INNER JOIN Clientes c ON p.ClienteId = c.Id
            ORDER BY p.Id DESC";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Pedido
            {
                Id = reader.GetInt32(0),
                ClienteId = reader.GetInt32(1),
                DataPedido = reader.GetDateTime(2),
                ValorProdutos = reader.GetDecimal(3),
                TaxaEntrega = reader.GetDecimal(4),
                ValorTotal = reader.GetDecimal(5),
                FormaPagamento = reader.GetString(6),
                StatusPagamento = reader.GetString(7),
                StatusEntrega = reader.GetString(8),
                EnderecoEntrega = reader.GetString(9),
                PrazoEstimado = reader.IsDBNull(10) ? "15 a 30 minutos" : reader.GetString(10),
                Cliente = new Cliente
                {
                    Id = reader.GetInt32(1),
                    Nome = reader.GetString(11),
                    Email = reader.GetString(12)
                }
            });
        }
        return lista;
    }

    public List<Pedido> ObterPedidosPorCliente(int clienteId)
    {
        var lista = new List<Pedido>();
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Id, ClienteId, DataPedido, ValorProdutos, TaxaEntrega, ValorTotal,
                   FormaPagamento, StatusPagamento, StatusEntrega, EnderecoEntrega, PrazoEstimado
            FROM Pedidos
            WHERE ClienteId = @cliId
            ORDER BY Id DESC";
        cmd.Parameters.AddWithValue("@cliId", clienteId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Pedido
            {
                Id = reader.GetInt32(0),
                ClienteId = reader.GetInt32(1),
                DataPedido = reader.GetDateTime(2),
                ValorProdutos = reader.GetDecimal(3),
                TaxaEntrega = reader.GetDecimal(4),
                ValorTotal = reader.GetDecimal(5),
                FormaPagamento = reader.GetString(6),
                StatusPagamento = reader.GetString(7),
                StatusEntrega = reader.GetString(8),
                EnderecoEntrega = reader.GetString(9),
                PrazoEstimado = reader.IsDBNull(10) ? "15 a 30 minutos" : reader.GetString(10)
            });
        }
        return lista;
    }

    public void AtualizarStatusEntrega(int pedidoId, string novoStatus)
    {
        using var conn = GetConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Pedidos SET StatusEntrega = @status WHERE Id = @id";
        cmd.Parameters.AddWithValue("@status", novoStatus);
        cmd.Parameters.AddWithValue("@id", pedidoId);
        cmd.ExecuteNonQuery();
    }
}
