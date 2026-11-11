using Microsoft.AspNetCore.Mvc;
using LojaCupcakes.Data;
using LojaCupcakes.Models;

namespace LojaCupcakes.Controllers;

public class RastreioController : Controller
{
    private readonly DatabaseHelper _db;

    public RastreioController(DatabaseHelper db)
    {
        _db = db;
    }

    public IActionResult Index(int? id)
    {
        if (!id.HasValue)
        {
            // Se nenhum ID for passado, tenta pegar o último pedido do usuário logado
            var cliId = HttpContext.Session.GetInt32(ContaController.SessionUsuarioId);
            if (cliId.HasValue)
            {
                var pedidos = _db.ObterPedidosPorCliente(cliId.Value);
                if (pedidos.Any())
                {
                    return RedirectToAction("Index", new { id = pedidos.First().Id });
                }
            }

            TempData["Aviso"] = "Informe o número do pedido para acompanhar a entrega.";
            return RedirectToAction("Index", "Home");
        }

        var pedido = _db.ObterPedidoPorId(id.Value);
        if (pedido == null)
        {
            TempData["Erro"] = "Pedido não encontrado no sistema.";
            return RedirectToAction("Index", "Home");
        }

        var vm = new RastreioViewModel
        {
            Pedido = pedido
        };

        return View(vm);
    }

    [HttpGet]
    public IActionResult StatusJson(int id)
    {
        var pedido = _db.ObterPedidoPorId(id);
        if (pedido == null) return NotFound();

        var vm = new RastreioViewModel { Pedido = pedido };
        return Json(new
        {
            id = pedido.Id,
            statusEntrega = pedido.StatusEntrega,
            etapaAtual = vm.EtapaAtual,
            descricao = vm.EtapaDescricao,
            prazo = pedido.PrazoEstimado
        });
    }
}
