using System.Text.Json;
using LojaCupcakes.Models;

namespace LojaCupcakes.Services;

public class CarrinhoService
{
    private const string SessionKey = "Carrinho_Session_Key";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CarrinhoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session => _httpContextAccessor.HttpContext!.Session;

    public CarrinhoViewModel ObterCarrinho()
    {
        var json = Session.GetString(SessionKey);
        if (string.IsNullOrEmpty(json))
        {
            return new CarrinhoViewModel();
        }

        try
        {
            return JsonSerializer.Deserialize<CarrinhoViewModel>(json) ?? new CarrinhoViewModel();
        }
        catch
        {
            return new CarrinhoViewModel();
        }
    }

    public void SalvarCarrinho(CarrinhoViewModel carrinho)
    {
        var json = JsonSerializer.Serialize(carrinho);
        Session.SetString(SessionKey, json);
    }

    public void Adicionar(Produto produto, int quantidade = 1)
    {
        if (quantidade <= 0) quantidade = 1;
        var carrinho = ObterCarrinho();

        var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produto.Id);
        if (item != null)
        {
            item.Quantidade += quantidade;
        }
        else
        {
            carrinho.Itens.Add(new ItemCarrinho
            {
                ProdutoId = produto.Id,
                Nome = produto.Nome,
                PrecoUnitario = produto.Preco,
                Quantidade = quantidade,
                ImagemUrl = produto.ImagemUrl
            });
        }

        SalvarCarrinho(carrinho);
    }

    public void AtualizarQuantidade(int produtoId, int quantidade)
    {
        var carrinho = ObterCarrinho();
        var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (item != null)
        {
            if (quantidade <= 0)
            {
                carrinho.Itens.Remove(item);
            }
            else
            {
                item.Quantidade = quantidade;
            }
            SalvarCarrinho(carrinho);
        }
    }

    public void Remover(int produtoId)
    {
        var carrinho = ObterCarrinho();
        var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (item != null)
        {
            carrinho.Itens.Remove(item);
            SalvarCarrinho(carrinho);
        }
    }

    public void Limpar()
    {
        Session.Remove(SessionKey);
    }
}
