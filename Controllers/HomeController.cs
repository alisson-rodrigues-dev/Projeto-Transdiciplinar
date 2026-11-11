using Microsoft.AspNetCore.Mvc;
using LojaCupcakes.Data;
using LojaCupcakes.Services;

namespace LojaCupcakes.Controllers;

public class HomeController : Controller
{
    private readonly DatabaseHelper _db;
    private readonly CarrinhoService _carrinho;

    public HomeController(DatabaseHelper db, CarrinhoService carrinho)
    {
        _db = db;
        _carrinho = carrinho;
    }

    public IActionResult Index()
    {
        // 5 Sabores Gourmet na Ordem Invertida (Doce de Leite, Nutella, Ninho c/ Morango, Ninho c/ Brigadeiro, Chocolate)
        var produtos = _db.ObterProdutos();
        ViewBag.QtdCarrinho = _carrinho.ObterCarrinho().QuantidadeTotalItens;
        return View(produtos);
    }

    [HttpGet]
    public IActionResult Detalhes(int id)
    {
        var produto = _db.ObterProdutoPorId(id);
        if (produto == null)
        {
            return NotFound(new { mensagem = "Cupcake não encontrado." });
        }
        return Json(produto);
    }

    public IActionResult Sobre()
    {
        return View();
    }
}
