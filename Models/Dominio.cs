namespace LojaCupcakes.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public bool Disponivel { get; set; } = true;
    public string Categoria { get; set; } = "Cupcake Gourmet";
    public string ImagemUrl { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public string PrecoFormatado => Preco.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
}

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? EnderecoPadrao { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.UtcNow;
    public decimal ValorProdutos { get; set; }
    public decimal TaxaEntrega { get; set; } = 6.00m;
    public decimal ValorTotal { get; set; }
    public string FormaPagamento { get; set; } = "PIX";
    public string StatusPagamento { get; set; } = "CONFIRMADO";
    public string StatusEntrega { get; set; } = "RECEBIDO";
    public string EnderecoEntrega { get; set; } = string.Empty;
    public string PrazoEstimado { get; set; } = "15 a 30 minutos";
    public string? Observacoes { get; set; }

    public Cliente? Cliente { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();

    public string TotalFormatado => ValorTotal.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
    public string SubtotalFormatado => ValorProdutos.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
    public string TaxaFormatada => TaxaEntrega.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
}

public class ItemPedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }

    public Produto? Produto { get; set; }

    public string PrecoUnitarioFormatado => PrecoUnitario.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
    public string SubtotalFormatado => Subtotal.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
}
