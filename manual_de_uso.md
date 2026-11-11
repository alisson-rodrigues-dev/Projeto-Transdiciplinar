# Manual de Uso e Instalação — Le Cupcake Gourmet

> **Projeto Integrador Transdisciplinar em Engenharia de Software II (Turma 001)**  
> **Estudante:** Alisson R. S. Espíndola *(identificação acadêmica completa preservada para submissão no AVA)*  
> **Arquitetura:** ASP.NET Core MVC em C# (.NET 10)  
> **Banco de Dados:** SQLite (Modelagem Relacional na Terceira Forma Normal — 3FN)  
> **Repositório:** [https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git](https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git)

---

## 1. Visão Geral da Solução (Versão 1)

O **Le Cupcake Gourmet** é um sistema de e-commerce e gestão operacional para uma confeitaria artesanal de alto padrão. O sistema foi desenvolvido estritamente de acordo com as exigências pedagógicas e a rubrica do PIT II:

- **Vitrine Gourmet (5 Sabores Oficiais na Ordem Invertida):**
  1. *Doce de Leite* (R$ 13,50)
  2. *Nutella* (R$ 16,00)
  3. *Ninho com Morango* (R$ 15,00)
  4. *Ninho com Brigadeiro* (R$ 14,00)
  5. *Chocolate* (R$ 13,00)
- **Identificação do Cliente:** Cadastro de novo cliente e autenticação de sessão com hash seguro;
- **Carrinho de Compras:** Adição assíncrona, ajuste dinâmico de quantidades, remoção e cálculo em tempo real;
- **Regras Comerciais:** Taxa fixa de entrega local de R$ 6,00 e prazo de atendimento estimado de 15 a 30 minutos;
- **Checkout e Pagamento Demonstrativo:** Validação de endereço completo e opções de pagamento simulado via **PIX instantâneo** (com QR Code e chave copia-e-cola) ou **Cartão de Crédito**;
- **Acompanhamento do Pedido (Rastreamento):** Linha do tempo visual em 4 estágios (*Recebido* ➔ *Em Preparo* ➔ *Em Rota* ➔ *Entregue*);
- **Painel Operacional da Loja:** Módulo restrito para visualização das métricas, fila de pedidos e alteração do status de entrega pela equipe da confeitaria.

---

## 2. Requisitos de Sistema e Dependências

### Tecnologias Utilizadas:
- **Plataforma:** .NET 10 SDK / Runtime
- **Linguagem:** C# 13+
- **Padrão Arquitetural:** ASP.NET Core MVC (Model-View-Controller)
- **Banco de Dados:** SQLite 3 (Driver oficial `Microsoft.Data.Sqlite 10.0.14`)
- **Front-end:** Razor Views (.cshtml), HTML5 semântico, CSS3 personalizado responsivo e Vanilla JavaScript com suporte a AJAX.

---

## 3. Instruções de Instalação e Execução Local

### Comando Rápido no Terminal (da raiz do repositório):
```bash
dotnet run --project Atividade
```

### Passo a Passo Detalhado:

#### Passo 1: Clonar o repositório e acessar a pasta do projeto
```bash
git clone https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git
cd Projeto-Transdiciplinar/Atividade
```

#### Passo 2: Restaurar as dependências .NET
```bash
dotnet restore
```

#### Passo 3: Compilar o projeto
```bash
dotnet build
```

#### Passo 4: Executar a aplicação
```bash
dotnet run
```

Ao iniciar, o console exibirá o endereço de acesso local:
```text
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000 (ou http://localhost:5118 / https://localhost:7015)
```

Abra o seu navegador web de preferência e acesse:
**`http://localhost:5000`** ou **`http://localhost:5118`**

#### Passo 5 (Opcional): Executar a suíte de testes de integração e persistência
```bash
dotnet run -- --test
```

---

## 4. Estrutura do Projeto MVC

A organização dos arquivos no padrão arquitetural MVC segue as melhores práticas da plataforma .NET:

```text
Atividade/
├── Controllers/
│   ├── HomeController.cs        # Vitrine dos 5 sabores e detalhes do cupcake
│   ├── CarrinhoController.cs    # Lógica de carrinho, quantidades e subtotais
│   ├── CheckoutController.cs    # Validação de endereço e persistência atômica
│   ├── ContaController.cs       # Login, cadastro, logout e perfil
│   ├── RastreioController.cs    # Linha do tempo do status da entrega
│   └── OperadorController.cs    # Gestão operacional dos pedidos da loja
├── Models/
│   ├── Dominio.cs               # Entidades de Domínio (Produto, Cliente, Pedido, ItemPedido)
│   ├── Carrinho.cs              # ItemCarrinho e CarrinhoViewModel
│   └── ViewModels.cs            # ViewModels de Login, Cadastro, Checkout, Rastreio e Operador
├── Data/
│   └── DatabaseHelper.cs        # Conexão SQLite, CRUD parametrizado e transações atômicas
├── Services/
│   └── CarrinhoService.cs       # Gerenciamento do carrinho na sessão HTTP do cliente
├── Views/
│   ├── Shared/_Layout.cshtml    # Template master gourmet contemporâneo
│   ├── Home/Index.cshtml        # Vitrine com os 5 sabores em ordem invertida
│   ├── Carrinho/Index.cshtml    # Tabela do carrinho com frete de R$ 6,00
│   ├── Checkout/Index.cshtml    # Formulário de endereço e abas PIX/Cartão
│   ├── Conta/Login.cshtml       # Autenticação de usuários
│   ├── Conta/Cadastro.cshtml    # Cadastro de novos clientes
│   ├── Conta/Index.cshtml       # Perfil e histórico de pedidos do cliente
│   ├── Rastreio/Index.cshtml    # Linha do tempo visual dos 4 estágios
│   └── Operador/Index.cshtml    # Dashboard operacional da confeitaria
├── wwwroot/
│   ├── css/site.css             # Folha de estilo sofisticada da confeitaria
│   ├── js/site.js               # Interações, modais, AJAX e toasts
│   └── images/                  # Ilustrações em SVG dos cupcakes
├── database.db                  # Banco de dados SQLite populado (3FN)
├── schema.sql                   # Script DDL e seeds iniciais
├── Diagramas.drawio             # Diagramas UML revisados (6 diagramas)
├── LojaCupcakes.csproj          # Configuração de compilação .NET
└── README.md                    # Apresentação do projeto e checklist
```

---

## 5. Contas de Demonstração para Avaliação

O banco SQLite já possui um cliente demonstrativo para teste imediato de login e checkout:

| Usuário | E-mail de Acesso | Senha Padrão | Cenário de Demonstração |
|---|---|---|---|
| **Cliente Demonstração** | `cliente@exemplo.com` | `123456` | Conta demonstrativa para validação de fluxo de compra e histórico |

Também é possível cadastrar novos clientes em tempo real através da tela de cadastro (`/Conta/Cadastro`).

---

## 6. Rotas Principais da Aplicação

| Rota | Descrição |
|---|---|
| `/` ou `/Home/Index` | Vitrine gourmet com os 5 sabores e cálculo rápido |
| `/Carrinho` | Visualização, alteração de quantidade e resumo de frete |
| `/Checkout` | Dados de entrega, seleção PIX ou Cartão e finalização |
| `/Rastreio/{id}` | Acompanhamento do pedido em tempo real (4 etapas) |
| `/Conta/Login` | Acesso à conta do cliente |
| `/Conta/Cadastro` | Formulário de registro de novos usuários |
| `/Conta` | Perfil do cliente e histórico de pedidos anteriores |
| `/Operador` | Painel restrito para a confeitaria atualizar o status dos pedidos |
