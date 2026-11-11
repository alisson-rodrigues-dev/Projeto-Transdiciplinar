using Microsoft.AspNetCore.Mvc;
using LojaCupcakes.Data;
using LojaCupcakes.Models;

namespace LojaCupcakes.Controllers;

public class OperadorController : Controller
{
    private readonly DatabaseHelper _db;

    public OperadorController(DatabaseHelper db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        var pedidos = _db.ObterTodosPedidos();
        var vm = new OperadorViewModel
        {
            Pedidos = pedidos
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AtualizarStatus(int pedidoId, string novoStatus)
    {
        var statusValidos = new[] { "RECEBIDO", "PREPARANDO", "EM_ROTA", "ENTREGUE" };
        if (!statusValidos.Contains(novoStatus))
        {
            TempData["Erro"] = "Status de entrega inválido.";
            return RedirectToAction("Index");
        }

        _db.AtualizarStatusEntrega(pedidoId, novoStatus);
        TempData["Sucesso"] = $"Status do pedido #{pedidoId:D4} atualizado para {novoStatus}!";
        return RedirectToAction("Index");
    }
}
