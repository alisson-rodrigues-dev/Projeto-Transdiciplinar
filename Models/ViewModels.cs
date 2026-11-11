using System.ComponentModel.DataAnnotations;

namespace LojaCupcakes.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}

public class CadastroViewModel
{
    [Required(ErrorMessage = "O nome completo é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail com formato válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone de contato é obrigatório.")]
    [Phone(ErrorMessage = "Telefone em formato inválido.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "O endereço padrão de entrega é obrigatório.")]
    public string EnderecoPadrao { get; set; } = string.Empty;
}

public class CheckoutViewModel
{
    public CarrinhoViewModel Carrinho { get; set; } = new();

    [Required(ErrorMessage = "O nome completo é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail de confirmação é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O telefone para entrega é obrigatório.")]
    public string Telefone { get; set; } = string.Empty;

    [Required(ErrorMessage = "O endereço de entrega é obrigatório.")]
    public string EnderecoEntrega { get; set; } = string.Empty;

    public string? Complemento { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }

    [Required(ErrorMessage = "Selecione a forma de pagamento.")]
    public string FormaPagamento { get; set; } = "PIX"; // PIX ou CARTAO_CREDITO

    // Dados opcionais para Cartão de Crédito
    public string? NumeroCartao { get; set; }
    public string? NomeCartao { get; set; }
    public string? ValidadeCartao { get; set; }
    public string? CvvCartao { get; set; }

    public string? Observacoes { get; set; }
}

public class RastreioViewModel
{
    public Pedido Pedido { get; set; } = new();
    public int EtapaAtual => Pedido.StatusEntrega switch
    {
        "RECEBIDO" => 1,
        "PREPARANDO" => 2,
        "EM_ROTA" => 3,
        "ENTREGUE" => 4,
        _ => 1
    };

    public string EtapaDescricao => Pedido.StatusEntrega switch
    {
        "RECEBIDO" => "Pedido recebido pela cozinha artesanal",
        "PREPARANDO" => "Em preparo e confeitaria dos cupcakes",
        "EM_ROTA" => "Em rota de entrega expressa",
        "ENTREGUE" => "Pedido entregue com sucesso!",
        _ => "Aguardando atualização"
    };
}

public class OperadorViewModel
{
    public List<Pedido> Pedidos { get; set; } = new();
    public int TotalRecebidos => Pedidos.Count(p => p.StatusEntrega == "RECEBIDO");
    public int TotalPreparando => Pedidos.Count(p => p.StatusEntrega == "PREPARANDO");
    public int TotalEmRota => Pedidos.Count(p => p.StatusEntrega == "EM_ROTA");
    public int TotalEntregues => Pedidos.Count(p => p.StatusEntrega == "ENTREGUE");
    public decimal FaturamentoTotal => Pedidos.Sum(p => p.ValorTotal);
}
