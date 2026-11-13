-- =====================================================================
-- PROJETO INTEGRADOR TRANSDISCIPLINAR EM ENGENHARIA DE SOFTWARE II
-- Confeitaria Gourmet de Cupcakes — Script Físico de Banco de Dados (SQLite)
-- Padrão Arquitetural: MVC em C# (ASP.NET Core)
-- Normalização: Terceira Forma Normal (3FN)
-- =====================================================================

PRAGMA foreign_keys = ON;

-- ---------------------------------------------------------------------
-- 1. TABELA: Clientes
-- Armazena os dados cadastrais dos clientes da confeitaria.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Clientes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(120) NOT NULL UNIQUE,
    SenhaHash VARCHAR(255) NOT NULL,
    Telefone VARCHAR(20),
    EnderecoPadrao VARCHAR(255),
    CriadoEm DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- ---------------------------------------------------------------------
-- 2. TABELA: Produtos (Cupcakes)
-- Catálogo oficial de cupcakes artesanais.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Produtos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome VARCHAR(80) NOT NULL,
    Descricao TEXT NOT NULL,
    Preco DECIMAL(10, 2) NOT NULL CHECK (Preco > 0),
    Disponivel BOOLEAN NOT NULL DEFAULT 1,
    Categoria VARCHAR(40) DEFAULT 'Cupcake Gourmet',
    ImagemUrl VARCHAR(255),
    CriadoEm DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- ---------------------------------------------------------------------
-- 3. TABELA: Pedidos
-- Registra as compras efetuadas, vinculadas ao cliente e endereço.
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS Pedidos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClienteId INTEGER NOT NULL,
    DataPedido DATETIME DEFAULT CURRENT_TIMESTAMP,
    ValorProdutos DECIMAL(10, 2) NOT NULL CHECK (ValorProdutos >= 0),
    TaxaEntrega DECIMAL(10, 2) NOT NULL DEFAULT 6.00 CHECK (TaxaEntrega >= 0),
    ValorTotal DECIMAL(10, 2) NOT NULL CHECK (ValorTotal > 0),
    FormaPagamento VARCHAR(20) NOT NULL CHECK (FormaPagamento IN ('PIX', 'CARTAO_CREDITO')),
    StatusPagamento VARCHAR(20) NOT NULL DEFAULT 'PENDENTE' CHECK (StatusPagamento IN ('PENDENTE', 'CONFIRMADO', 'RECUSADO')),
    StatusEntrega VARCHAR(20) NOT NULL DEFAULT 'RECEBIDO' CHECK (StatusEntrega IN ('RECEBIDO', 'PREPARANDO', 'EM_ROTA', 'ENTREGUE')),
    EnderecoEntrega VARCHAR(255) NOT NULL,
    PrazoEstimado VARCHAR(30) DEFAULT '15 a 30 minutos',
    Observacoes TEXT,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id) ON DELETE RESTRICT
);

-- ---------------------------------------------------------------------
-- 4. TABELA: ItensPedido
-- Registra os itens associados a cada pedido (relação N:M resolvida).
-- ---------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS ItensPedido (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PedidoId INTEGER NOT NULL,
    ProdutoId INTEGER NOT NULL,
    Quantidade INTEGER NOT NULL CHECK (Quantidade > 0),
    PrecoUnitario DECIMAL(10, 2) NOT NULL CHECK (PrecoUnitario > 0),
    Subtotal DECIMAL(10, 2) NOT NULL CHECK (Subtotal > 0),
    FOREIGN KEY (PedidoId) REFERENCES Pedidos(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProdutoId) REFERENCES Produtos(Id) ON DELETE RESTRICT
);

-- ---------------------------------------------------------------------
-- ÍNDICES PARA OTIMIZAÇÃO DE BUSCA E INTEGRIDADE
-- ---------------------------------------------------------------------
CREATE INDEX IF NOT EXISTS idx_clientes_email ON Clientes(Email);
CREATE INDEX IF NOT EXISTS idx_pedidos_cliente ON Pedidos(ClienteId);
CREATE INDEX IF NOT EXISTS idx_itens_pedido_pedido ON ItensPedido(PedidoId);
CREATE INDEX IF NOT EXISTS idx_itens_pedido_produto ON ItensPedido(ProdutoId);

-- =====================================================================
-- CARGA INICIAL DE DADOS (SEEDS)
-- Ordem dos Sabores: Invertida (do último para o primeiro)
-- =====================================================================

INSERT INTO Produtos (Id, Nome, Descricao, Preco, Disponivel, ImagemUrl) VALUES
(1, 'Doce de Leite', 'Massa de especiarias suaves com toque de canela, recheio e cobertura generosa de doce de leite caseiro cremoso polvilhado com canela.', 13.50, 1, '/images/cupcake-doce-de-leite.jpg'),
(2, 'Nutella', 'Massa de cacau nobre, recheio puro de creme de avelã Nutella e cobertura de brigadeiro de Nutella com finalização em avelãs picadas tostadas.', 16.00, 1, '/images/cupcake-nutella.jpg'),
(3, 'Ninho com Morango', 'Massa leve de baunilha, recheio de geleia artesanal de morangos frescos e cobertura aveludada de mousse de leite Ninho finalizada com morango fresco.', 15.00, 1, '/images/cupcake-ninho-morango.jpg'),
(4, 'Ninho com Brigadeiro', 'Massa amanteigada de baunilha, recheio generoso de leite Ninho cremoso e cobertura dupla com brigadeiro tradicional e brigadeiro de Ninho.', 14.00, 1, '/images/cupcake-ninho-brigadeiro.jpg'),
(5, 'Chocolate', 'Massa fofinha de cacau nobre 100%, recheio cremoso de ganache de chocolate meio amargo e cobertura generosa de brigadeiro gourmet com granulado belga.', 13.00, 1, '/images/cupcake-chocolate.jpg');

-- Cliente padrão de teste inicial
INSERT INTO Clientes (Id, Nome, Email, SenhaHash, Telefone, EnderecoPadrao) VALUES
(1, 'Cliente Demonstração', 'cliente@exemplo.com', '123456', '(11) 98765-4321', 'Rua das Confeitarias, 100, Bairro Jardim Doce - SP');

-- Administrador da Confeitaria (Gestão Operacional na V2)
INSERT INTO Clientes (Id, Nome, Email, SenhaHash, Telefone, EnderecoPadrao) VALUES
(2, 'Alisson Administrador', 'alissonadm@email.com', '123456', '(11) 99999-0001', 'Av. Central, 1000 - Gestão da Loja');
