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
        var clienteId = HttpContext.Session.GetInt32(ContaController.SessionUsuarioId);
        if (!clienteId.HasValue)
        {
            TempData["Erro"] = "Acesso restrito: Faça login com a conta de administrador para acessar o Painel da Loja.";
            return RedirectToAction("Login", "Conta", new { returnUrl = "/Operador" });
        }

        var clienteEmail = HttpContext.Session.GetString(ContaController.SessionUsuarioEmail);
        if (!string.Equals(clienteEmail, ContaController.EmailAdmin, StringComparison.OrdinalIgnoreCase))
        {
            TempData["Erro"] = "Acesso negado: Apenas a conta de administrador (alissonadm@email.com) tem permissão para acessar o Painel da Loja.";
            return RedirectToAction("Index", "Conta");
        }

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
        var clienteId = HttpContext.Session.GetInt32(ContaController.SessionUsuarioId);
        if (!clienteId.HasValue)
        {
            return RedirectToAction("Login", "Conta", new { returnUrl = "/Operador" });
        }

        var clienteEmail = HttpContext.Session.GetString(ContaController.SessionUsuarioEmail);
        if (!string.Equals(clienteEmail, ContaController.EmailAdmin, StringComparison.OrdinalIgnoreCase))
        {
            TempData["Erro"] = "Acesso negado: Apenas o administrador pode alterar o status de pedidos.";
            return RedirectToAction("Index", "Conta");
        }

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
