using Microsoft.AspNetCore.Mvc;
using LojaCupcakes.Data;
using LojaCupcakes.Models;

namespace LojaCupcakes.Controllers;

public class ContaController : Controller
{
    private readonly DatabaseHelper _db;
    public const string SessionUsuarioId = "Usuario_Id";
    public const string SessionUsuarioNome = "Usuario_Nome";
    public const string SessionUsuarioEmail = "Usuario_Email";

    public ContaController(DatabaseHelper db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (HttpContext.Session.GetInt32(SessionUsuarioId) != null)
        {
            return Redirect(returnUrl ?? Url.Action("Index", "Conta")!);
        }
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var cliente = _db.ObterClientePorEmail(model.Email);
        if (cliente == null || cliente.SenhaHash != model.Senha) // Senha padrão ou cadastrada
        {
            ModelState.AddModelError("", "E-mail ou senha incorretos.");
            return View(model);
        }

        // Salvar sessão do cliente autenticado
        HttpContext.Session.SetInt32(SessionUsuarioId, cliente.Id);
        HttpContext.Session.SetString(SessionUsuarioNome, cliente.Nome);
        HttpContext.Session.SetString(SessionUsuarioEmail, cliente.Email);

        TempData["Sucesso"] = $"Bem-vindo de volta, {cliente.Nome}!";

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Cadastro(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new CadastroViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastro(CadastroViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existente = _db.ObterClientePorEmail(model.Email);
        if (existente != null)
        {
            ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
            return View(model);
        }

        var novoCliente = new Cliente
        {
            Nome = model.Nome,
            Email = model.Email,
            SenhaHash = model.Senha,
            Telefone = model.Telefone,
            EnderecoPadrao = model.EnderecoPadrao
        };

        var novoId = _db.CadastrarCliente(novoCliente);

        // Iniciar sessão
        HttpContext.Session.SetInt32(SessionUsuarioId, novoId);
        HttpContext.Session.SetString(SessionUsuarioNome, novoCliente.Nome);
        HttpContext.Session.SetString(SessionUsuarioEmail, novoCliente.Email);

        TempData["Sucesso"] = "Cadastro realizado com sucesso! Aproveite nossos cupcakes gourmet.";

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove(SessionUsuarioId);
        HttpContext.Session.Remove(SessionUsuarioNome);
        HttpContext.Session.Remove(SessionUsuarioEmail);

        TempData["Aviso"] = "Você encerrou sua sessão.";
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Index()
    {
        var clienteId = HttpContext.Session.GetInt32(SessionUsuarioId);
        if (clienteId == null)
        {
            return RedirectToAction("Login", new { returnUrl = Url.Action("Index", "Conta") });
        }

        var cliente = _db.ObterClientePorId(clienteId.Value);
        if (cliente == null)
        {
            return RedirectToAction("Logout");
        }

        var pedidos = _db.ObterPedidosPorCliente(clienteId.Value);
        ViewBag.Pedidos = pedidos;

        return View(cliente);
    }
}
