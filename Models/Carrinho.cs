namespace LojaCupcakes.Models;

public class ItemCarrinho
{
    public int ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public int Quantidade { get; set; }
    public string ImagemUrl { get; set; } = string.Empty;

    public decimal Subtotal => PrecoUnitario * Quantidade;
    public string PrecoUnitarioFormatado => PrecoUnitario.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
    public string SubtotalFormatado => Subtotal.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
}

public class CarrinhoViewModel
{
    public List<ItemCarrinho> Itens { get; set; } = new();
    public decimal TaxaEntrega { get; set; } = 6.00m;
    public string PrazoEstimado { get; set; } = "15 a 30 minutos";

    public decimal Subtotal => Itens.Sum(i => i.Subtotal);
    public decimal Total => Itens.Any() ? Subtotal + TaxaEntrega : 0m;
    public int QuantidadeTotalItens => Itens.Sum(i => i.Quantidade);

    public string SubtotalFormatado => Subtotal.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
    public string TaxaFormatada => TaxaEntrega.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
    public string TotalFormatado => Total.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
}
