using Microsoft.AspNetCore.Mvc;
using LojaCupcakes.Data;
using LojaCupcakes.Services;

namespace LojaCupcakes.Controllers;

public class CarrinhoController : Controller
{
    private readonly DatabaseHelper _db;
    private readonly CarrinhoService _carrinho;

    public CarrinhoController(DatabaseHelper db, CarrinhoService carrinho)
    {
        _db = db;
        _carrinho = carrinho;
    }

    public IActionResult Index()
    {
        var vm = _carrinho.ObterCarrinho();
        return View(vm);
    }

    [HttpPost]
    public IActionResult Adicionar(int produtoId, int quantidade = 1, bool isAjax = false)
    {
        var produto = _db.ObterProdutoPorId(produtoId);
        if (produto == null)
        {
            if (isAjax) return Json(new { sucesso = false, mensagem = "Produto não encontrado." });
            TempData["Erro"] = "Produto não encontrado.";
            return RedirectToAction("Index", "Home");
        }

        if (quantidade <= 0) quantidade = 1;
        _carrinho.Adicionar(produto, quantidade);

        if (isAjax)
        {
            var cart = _carrinho.ObterCarrinho();
            return Json(new
            {
                sucesso = true,
                mensagem = $"{produto.Nome} adicionado ao carrinho!",
                totalItens = cart.QuantidadeTotalItens,
                subtotal = cart.SubtotalFormatado,
                total = cart.TotalFormatado
            });
        }

        TempData["Sucesso"] = $"{produto.Nome} adicionado ao carrinho!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult AtualizarQtd(int produtoId, int quantidade)
    {
        _carrinho.AtualizarQuantidade(produtoId, quantidade);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remover(int produtoId)
    {
        _carrinho.Remover(produtoId);
        TempData["Aviso"] = "Item removido do carrinho.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Limpar()
    {
        _carrinho.Limpar();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ResumoJson()
    {
        var cart = _carrinho.ObterCarrinho();
        return Json(new
        {
            totalItens = cart.QuantidadeTotalItens,
            subtotal = cart.SubtotalFormatado,
            total = cart.TotalFormatado
        });
    }
}
