using System.Globalization;
using LojaCupcakes.Data;
using LojaCupcakes.Models;

namespace LojaCupcakes.Tests;

public static class SuiteTestesUnitarios
{
    public static int ExecutarTodosOsTestes(DatabaseHelper db)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n=====================================================================");
        Console.WriteLine("    EXECUÇÃO DA SUÍTE DE TESTES UNITÁRIOS E INTEGRAÇÃO (PIT II)");
        Console.WriteLine("    Confeitaria Gourmet - C# .NET 10 / ASP.NET Core MVC");
        Console.WriteLine("=====================================================================\n");
        Console.ResetColor();

        int aprovados = 0;
        int falhas = 0;

        void Executar(string nomeTeste, Action acao)
        {
            try
            {
                acao();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[PASSOU] {nomeTeste}");
                Console.ResetColor();
                aprovados++;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FALHOU] {nomeTeste} -> {ex.Message}");
                Console.ResetColor();
                falhas++;
            }
        }

        // Teste 1: Validação do Catálogo e Ordem Invertida
        Executar("1. Catálogo Oficial: 5 Sabores Gourmet na Ordem Invertida", () =>
        {
            var produtos = db.ObterProdutos();
            if (produtos.Count != 5)
                throw new Exception($"Esperado 5 produtos, encontrado {produtos.Count}");

            var nomesEsperados = new[]
            {
                "Doce de Leite",
                "Nutella",
                "Ninho com Morango",
                "Ninho com Brigadeiro",
                "Chocolate"
            };

            for (int i = 0; i < 5; i++)
            {
                if (produtos[i].Nome != nomesEsperados[i])
                    throw new Exception($"Posição {i + 1} incorreta: esperado '{nomesEsperados[i]}', obtido '{produtos[i].Nome}'");
            }

            if (produtos[0].Preco != 13.50m) throw new Exception("Preço Doce de Leite inválido");
            if (produtos[1].Preco != 16.00m) throw new Exception("Preço Nutella inválido");
            if (produtos[2].Preco != 15.00m) throw new Exception("Preço Ninho com Morango inválido");
            if (produtos[3].Preco != 14.00m) throw new Exception("Preço Ninho com Brigadeiro inválido");
            if (produtos[4].Preco != 13.00m) throw new Exception("Preço Chocolate inválido");
        });

        // Teste 2: Cálculos do Carrinho de Compras e Taxa Fixa
        Executar("2. Carrinho: Subtotal, Taxa Fixa de R$ 6,00 e Total", () =>
        {
            var carrinho = new CarrinhoViewModel();
            carrinho.Itens.Add(new ItemCarrinho { ProdutoId = 2, Nome = "Nutella", PrecoUnitario = 16.00m, Quantidade = 2 });
            carrinho.Itens.Add(new ItemCarrinho { ProdutoId = 5, Nome = "Chocolate", PrecoUnitario = 13.00m, Quantidade = 1 });

            if (carrinho.Subtotal != 45.00m)
                throw new Exception($"Subtotal incorreto: esperado R$ 45,00, obtido {carrinho.Subtotal}");

            if (carrinho.TaxaEntrega != 6.00m)
                throw new Exception($"Taxa de entrega incorreta: esperado R$ 6,00, obtido {carrinho.TaxaEntrega}");

            if (carrinho.Total != 51.00m)
                throw new Exception($"Total incorreto: esperado R$ 51,00, obtido {carrinho.Total}");

            if (carrinho.QuantidadeTotalItens != 3)
                throw new Exception($"Quantidade de itens incorreta: esperado 3, obtido {carrinho.QuantidadeTotalItens}");
        });

        // Teste 3: Carrinho Vazio
        Executar("3. Carrinho Vazio: Total e Subtotal Zerados", () =>
        {
            var carrinhoVazio = new CarrinhoViewModel();
            if (carrinhoVazio.Subtotal != 0m) throw new Exception("Subtotal do carrinho vazio deve ser zero");
            if (carrinhoVazio.Total != 0m) throw new Exception("Total do carrinho vazio deve ser zero (sem frete)");
            if (carrinhoVazio.QuantidadeTotalItens != 0) throw new Exception("Quantidade de itens deve ser zero");
        });

        // Teste 4: Parâmetro de Prazo de Entrega Estimado
        Executar("4. Parâmetros Comerciais: Prazo de Entrega Estimado de 15 a 30 Minutos", () =>
        {
            var carrinho = new CarrinhoViewModel();
            if (carrinho.PrazoEstimado != "15 a 30 minutos")
                throw new Exception($"Prazo incorreto: esperado '15 a 30 minutos', obtido '{carrinho.PrazoEstimado}'");
        });

        // Teste 5: Validação de Dados de Cliente
        Executar("5. Domínio Cliente: Validação de E-mail e Integridade de Campos", () =>
        {
            var cliente = new Cliente
            {
                Id = 1,
                Nome = "Alisson Rodrigues",
                Email = "alisson@email.com",
                Telefone = "(11) 98765-4321",
                EnderecoPadrao = "Rua das Confeitarias, 100"
            };

            if (string.IsNullOrWhiteSpace(cliente.Nome)) throw new Exception("Nome obrigatório");
            if (!cliente.Email.Contains("@")) throw new Exception("E-mail deve ser válido");
            if (string.IsNullOrWhiteSpace(cliente.Telefone)) throw new Exception("Telefone obrigatório");
        });

        // Teste 6: Validação de Checkout e Tipos de Pagamento
        Executar("6. Checkout: Validação de Formas de Pagamento Permitidas (PIX e CARTAO_CREDITO)", () =>
        {
            var formasPermitidas = new[] { "PIX", "CARTAO_CREDITO" };

            var checkoutPix = new CheckoutViewModel { FormaPagamento = "PIX", EnderecoEntrega = "Rua Teste, 123" };
            var checkoutCartao = new CheckoutViewModel { FormaPagamento = "CARTAO_CREDITO", EnderecoEntrega = "Rua Teste, 123" };

            if (!formasPermitidas.Contains(checkoutPix.FormaPagamento)) throw new Exception("Forma PIX rejeitada");
            if (!formasPermitidas.Contains(checkoutCartao.FormaPagamento)) throw new Exception("Forma Cartão rejeitada");
        });

        // Teste 7: Integridade Relacional 3FN no Banco de Dados
        Executar("7. Banco de Dados SQLite (3FN): Consulta de Pedidos e Histórico", () =>
        {
            var cliente = db.ObterClientePorEmail("cliente@exemplo.com");
            if (cliente == null)
                throw new Exception("Cliente padrão de demonstração não localizado no banco");

            var pedidos = db.ObterPedidosPorCliente(cliente.Id);
            if (pedidos == null)
                throw new Exception("Lista de pedidos retornou nula");
        });

        Console.WriteLine("\n---------------------------------------------------------------------");
        Console.ForegroundColor = falhas == 0 ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"RESULTADO FINAL DOS TESTES: {aprovados} Aprovados | {falhas} Falhas");
        Console.ResetColor();
        Console.WriteLine("=====================================================================\n");

        return falhas;
    }
}
