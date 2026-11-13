using Microsoft.AspNetCore.Mvc;
using LojaCupcakes.Data;
using LojaCupcakes.Models;
using LojaCupcakes.Services;

namespace LojaCupcakes.Controllers;

public class CheckoutController : Controller
{
    private readonly DatabaseHelper _db;
    private readonly CarrinhoService _carrinho;

    public CheckoutController(DatabaseHelper db, CarrinhoService carrinho)
    {
        _db = db;
        _carrinho = carrinho;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cart = _carrinho.ObterCarrinho();
        if (!cart.Itens.Any())
        {
            TempData["Aviso"] = "Seu carrinho está vazio. Adicione cupcakes antes de prosseguir para o checkout.";
            return RedirectToAction("Index", "Home");
        }

        var vm = new CheckoutViewModel
        {
            Carrinho = cart,
            FormaPagamento = "PIX"
        };

        // Se o cliente estiver logado, preenche automaticamente os dados cadastrais
        var clienteId = HttpContext.Session.GetInt32(ContaController.SessionUsuarioId);
        if (clienteId.HasValue)
        {
            var cliente = _db.ObterClientePorId(clienteId.Value);
            if (cliente != null)
            {
                vm.Nome = cliente.Nome;
                vm.Email = cliente.Email;
                vm.Telefone = cliente.Telefone ?? string.Empty;
                vm.EnderecoEntrega = cliente.EnderecoPadrao ?? string.Empty;
            }
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Finalizar(CheckoutViewModel model)
    {
        var cart = _carrinho.ObterCarrinho();
        if (!cart.Itens.Any())
        {
            TempData["Erro"] = "O carrinho expirou ou está vazio.";
            return RedirectToAction("Index", "Home");
        }

        model.Carrinho = cart;

        // V2: Bloqueio estrito de pagamento PIX sem autenticação prévia
        var sessaoCliId = HttpContext.Session.GetInt32(ContaController.SessionUsuarioId);
        if (model.FormaPagamento == "PIX" && !sessaoCliId.HasValue)
        {
            TempData["Erro"] = "Para pagar via PIX e gerar o QR Code de cobrança oficial, faça login ou cadastre-se previamente.";
            return RedirectToAction("Login", "Conta", new { returnUrl = "/Checkout" });
        }

        if (!ModelState.IsValid)
        {
            return View("Index", model);
        }

        // 1. Identificar ou cadastrar o cliente automaticamente
        int clienteId;
        if (sessaoCliId.HasValue)
        {
            clienteId = sessaoCliId.Value;
        }
        else
        {
            var existente = _db.ObterClientePorEmail(model.Email);
            if (existente != null)
            {
                clienteId = existente.Id;
            }
            else
            {
                clienteId = _db.CadastrarCliente(new Cliente
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    SenhaHash = "123456", // Senha temporária padrão
                    Telefone = model.Telefone,
                    EnderecoPadrao = model.EnderecoEntrega
                });
            }

            // Iniciar sessão
            HttpContext.Session.SetInt32(ContaController.SessionUsuarioId, clienteId);
            HttpContext.Session.SetString(ContaController.SessionUsuarioNome, model.Nome);
            HttpContext.Session.SetString(ContaController.SessionUsuarioEmail, model.Email);
        }

        // 2. Montar objeto do Pedido
        var pedido = new Pedido
        {
            ClienteId = clienteId,
            DataPedido = DateTime.UtcNow,
            ValorProdutos = cart.Subtotal,
            TaxaEntrega = cart.TaxaEntrega,
            ValorTotal = cart.Total,
            FormaPagamento = model.FormaPagamento.ToUpper() == "CARTAO_CREDITO" ? "CARTAO_CREDITO" : "PIX",
            StatusPagamento = "CONFIRMADO", // Simulação imediata com sucesso
            StatusEntrega = "RECEBIDO",
            EnderecoEntrega = $"{model.EnderecoEntrega}{(string.IsNullOrWhiteSpace(model.Complemento) ? "" : " - " + model.Complemento)}{(string.IsNullOrWhiteSpace(model.Bairro) ? "" : ", Bairro " + model.Bairro)}{(string.IsNullOrWhiteSpace(model.Cidade) ? "" : ", " + model.Cidade)}",
            PrazoEstimado = "15 a 30 minutos",
            Observacoes = model.Observacoes
        };

        // 3. Gravação Atômica no Banco de Dados SQLite (3FN)
        var novoPedidoId = _db.CriarPedido(pedido, cart.Itens);

        // 4. Limpar o carrinho
        _carrinho.Limpar();

        TempData["Sucesso"] = $"Pedido #{novoPedidoId:D4} confirmado com sucesso!";
        return RedirectToAction("Index", "Rastreio", new { id = novoPedidoId });
    }
}
