# Projeto de Banco de Dados — Le Cupcake Gourmet

> **Projeto Integrador Transdisciplinar em Engenharia de Software II**  
> **Estudante:** Alisson R. S. Espíndola *(identificação acadêmica completa preservada no relatório oficial AVA)*  
> **SGBD:** SQLite 3  
> **Normalização:** Terceira Forma Normal (3FN)  
> **Script Físico:** [`schema.sql`](schema.sql)  
> **Base Executada:** `database.db`  

---

## 1. Modelo Conceitual de Dados

O modelo conceitual do banco de dados representa as entidades essenciais do negócio e seus relacionamentos:

- **Cliente**: Representa o usuário que se cadastra e efetua compras;
- **Produto (Cupcake)**: Catálogo com os 5 sabores nobres da confeitaria;
- **Pedido**: Transação comercial atômica com frete de R$ 6,00, endereço e pagamento;
- **ItemPedido**: Entidade associativa que resolve a relação N:M entre *Pedido* e *Produto*, preservando o preço histórico da venda.

### Relacionamentos e Cardinalidades:
- **Cliente (1) ➔ (0..*) Pedido**: Um cliente pode realizar múltiplos pedidos; cada pedido pertence obrigatoriamente a um único cliente.
- **Pedido (1) ➔ (1..*) ItemPedido**: Cada pedido é composto por um ou mais itens; cada item pertence exclusivamente a um pedido (deleção em cascata).
- **Produto (1) ➔ (0..*) ItemPedido**: Um cupcake pode compor diversos itens de pedidos ao longo do tempo.

---

## 2. Modelo Lógico Normalizado (3FN)

A estrutura relacional atende rigorosamente às formas normais:
1. **Primeira Forma Normal (1FN):** Todos os atributos são atômicos e indivisíveis. Não há grupos de repetição ou vetores multivalorados.
2. **Segunda Forma Normal (2FN):** A base está na 1FN e todos os atributos não-chave dependem funcionalmente da totalidade da chave primária.
3. **Terceira Forma Normal (3FN):** A base está na 2FN e nenhum atributo não-chave depende transitivamente de outro atributo não-chave.

---

## 3. Dicionário de Dados

### Tabela: `Clientes`
| Coluna | Tipo | Nulo? | Chave | Descrição |
|---|---|:---:|:---:|---|
| `Id` | INTEGER | NÃO | PK | Identificador único autoincrementado do cliente |
| `Nome` | VARCHAR(100) | NÃO | - | Nome completo do cliente |
| `Email` | VARCHAR(120) | NÃO | UNIQUE | E-mail corporativo/pessoal único para autenticação |
| `SenhaHash` | VARCHAR(255) | NÃO | - | Hash criptografado da senha |
| `Telefone` | VARCHAR(20) | SIM | - | Telefone ou WhatsApp para contato |
| `EnderecoPadrao` | VARCHAR(255) | SIM | - | Endereço residencial padrão do cliente |
| `CriadoEm` | DATETIME | NÃO | - | Data e hora de criação do registro |

### Tabela: `Produtos`
| Coluna | Tipo | Nulo? | Chave | Descrição |
|---|---|:---:|:---:|---|
| `Id` | INTEGER | NÃO | PK | Identificador único do cupcake |
| `Nome` | VARCHAR(80) | NÃO | - | Sabor oficial do cupcake |
| `Descricao` | TEXT | NÃO | - | Composição de massa, recheio e cobertura |
| `Preco` | DECIMAL(10,2) | NÃO | - | Preço unitário (CHECK: Preco > 0) |
| `Disponivel` | BOOLEAN | NÃO | - | Indicador de disponibilidade na vitrine |
| `Categoria` | VARCHAR(40) | SIM | - | Categoria do produto (Padrão: Cupcake Gourmet) |
| `ImagemUrl` | VARCHAR(255) | SIM | - | Caminho local do asset ilustrativo |
| `CriadoEm` | DATETIME | NÃO | - | Data de cadastro do produto |

### Tabela: `Pedidos`
| Coluna | Tipo | Nulo? | Chave | Descrição |
|---|---|:---:|:---:|---|
| `Id` | INTEGER | NÃO | PK | Número oficial do pedido |
| `ClienteId` | INTEGER | NÃO | FK | Vínculo com a tabela `Clientes(Id)` |
| `DataPedido` | DATETIME | NÃO | - | Carimbo de data/hora do pedido |
| `ValorProdutos` | DECIMAL(10,2) | NÃO | - | Soma dos subtotais dos itens |
| `TaxaEntrega` | DECIMAL(10,2) | NÃO | - | Taxa fixa de entrega local (R$ 6,00) |
| `ValorTotal` | DECIMAL(10,2) | NÃO | - | Total geral (Produtos + Taxa) |
| `FormaPagamento` | VARCHAR(20) | NÃO | - | 'PIX' ou 'CARTAO_CREDITO' |
| `StatusPagamento`| VARCHAR(20) | NÃO | - | 'PENDENTE', 'CONFIRMADO' ou 'RECUSADO' |
| `StatusEntrega`  | VARCHAR(20) | NÃO | - | 'RECEBIDO', 'PREPARANDO', 'EM_ROTA', 'ENTREGUE' |
| `EnderecoEntrega`| VARCHAR(255) | NÃO | - | Endereço completo informado no checkout |
| `PrazoEstimado`  | VARCHAR(30) | SIM | - | Prazo estimado (Padrão: 15 a 30 minutos) |
| `Observacoes`    | TEXT | SIM | - | Observações adicionais para a entrega |

### Tabela: `ItensPedido`
| Coluna | Tipo | Nulo? | Chave | Descrição |
|---|---|:---:|:---:|---|
| `Id` | INTEGER | NÃO | PK | Identificador único da linha do item |
| `PedidoId` | INTEGER | NÃO | FK | Vínculo com a tabela `Pedidos(Id)` (ON DELETE CASCADE) |
| `ProdutoId` | INTEGER | NÃO | FK | Vínculo com a tabela `Produtos(Id)` |
| `Quantidade` | INTEGER | NÃO | - | Quantidade solicitada (CHECK > 0) |
| `PrecoUnitario`| DECIMAL(10,2)| NÃO | - | Preço praticado no momento da compra |
| `Subtotal` | DECIMAL(10,2)| NÃO | - | Quantidade * PrecoUnitario |

---

## 4. Seeds Iniciais (Ordem Invertida)

A carga inicial no banco [`schema.sql`](schema.sql) reflete os 5 sabores oficiais:
1. **Doce de Leite** (R$ 13,50)
2. **Nutella** (R$ 16,00)
3. **Ninho com Morango** (R$ 15,00)
4. **Ninho com Brigadeiro** (R$ 14,00)
5. **Chocolate** (R$ 13,00)
