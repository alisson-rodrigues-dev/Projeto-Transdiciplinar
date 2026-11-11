# 🧁 Le Cupcake Gourmet — Confeitaria Fina

> **Projeto Integrador Transdisciplinar em Engenharia de Software II (PIT II — Turma 001)**  
> **Estudante:** Alisson R. S. Espíndola *(identificação pessoal completa resguardada no repositório público e informada no relatório oficial AVA)*  
> **Arquitetura:** ASP.NET Core MVC em C# (.NET 10)  
> **Banco de Dados:** SQLite (Terceira Forma Normal — 3FN)  
> **Repositório Oficial:** [https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git](https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git)

---

## 📌 Visão Geral do Projeto (Versão 1)

O **Le Cupcake Gourmet** é uma aplicação Web desenvolvida para uma confeitaria artesanal de alto padrão. O sistema une a experiência de compra do cliente ao fluxo operacional de atendimento em tempo de execução, seguindo com rigor o padrão arquitetural **Model-View-Controller (MVC)** em **C# (.NET 10)** com banco de dados **SQLite (3FN)**.

### ✨ Funcionalidades Entregues na Versão 1:
1. **Vitrine Gourmet (5 Sabores na Ordem Invertida):**
   - #1 Doce de Leite (R$ 13,50)
   - #2 Nutella (R$ 16,00)
   - #3 Ninho com Morango (R$ 15,00)
   - #4 Ninho com Brigadeiro (R$ 14,00)
   - #5 Chocolate (R$ 13,00)
2. **Carrinho de Compras Interativo:**
   - Adição assíncrona com feedback imediato via toast e badge de contagem no topo;
   - Ajuste de quantidade (+ e -) e remoção de itens;
   - Cálculo automático de subtotais e da taxa fixa de entrega de R$ 6,00 (15 a 30 min).
3. **Identificação e Autenticação:**
   - Cadastro completo de novos clientes e login seguro com sessão HTTP.
4. **Checkout e Pagamento Demonstrativo:**
   - Preenchimento/validação de endereço de entrega;
   - Seleção entre **PIX** (com QR Code simulado e chave copia-e-cola) ou **Cartão de Crédito**;
   - Persistência atômica com transação SQLite.
5. **Acompanhamento do Pedido (Rastreamento):**
   - Linha do tempo visual em 4 estágios (*Recebido* ➔ *Em Preparo* ➔ *Em Rota* ➔ *Entregue*).
6. **Painel Operacional da Confeitaria:**
   - Métricas operacionais em tempo real e atualização de status de entrega.

---

## 🧭 Atalhos e Documentação

| Arquivo | Descrição |
|---|---|
| [`manual_de_uso.md`](manual_de_uso.md) | Passo a passo de instalação, compilação e execução em C# / .NET 10 |
| [`Planejamento.md`](Planejamento.md) | Escopo, 15 histórias de usuário, critérios de aceitação e backlog |
| [`Banco_de_Dados.md`](Banco_de_Dados.md) | Modelos conceitual e lógico (3FN), DDL e dicionário de dados |
| [`schema.sql`](schema.sql) | Script SQL executável de criação das tabelas e seeds |
| [`Diagramas.drawio`](Diagramas.drawio) | 6 diagramas UML editáveis (Casos de uso, Classes C#, Sequência, Atividades e Mapas) |
| [`database.db`](database.db) | Base de dados física em SQLite pronta para uso |

---

## 🚀 Como Executar a Aplicação Localmente

Certifique-se de possuir o **.NET 10 SDK** instalado em sua máquina.

### Opção 1: Comando direto a partir da raiz do repositório
```bash
dotnet run --project Atividade
```

### Opção 2: A partir da pasta `Atividade`
```bash
# 1. Acessar a pasta da atividade
cd Atividade

# 2. Restaurar dependências
dotnet restore

# 3. Compilar a solução
dotnet build

# 4. Executar a aplicação
dotnet run
```

Abra o navegador e acesse: **`http://localhost:5000`** ou **`http://localhost:5118`** (ou **`https://localhost:7015`**).

### 🧪 Executar Testes Automatizados no Terminal:
```bash
dotnet run -- --test
```

### Conta de Demonstração Inicial:
- **E-mail:** `cliente@exemplo.com`
- **Senha:** `123456`

---

## 📊 Status do Projeto e Próximas Etapas

- [x] **Situação 1 — Revisão do Planejamento:** Escopo, novos diagramas Draw.io, modelagem 3FN e dicionário de dados.
- [x] **Situação 2 — Desenvolvimento da Solução (Versão 1):** Aplicação C# ASP.NET Core MVC completa, novo design responsivo, vitrine invertida, checkout PIX/Cartão, rastreio e manual de uso.
- [ ] **Situação 3 — Testes, Correções e Entrega (Versão 2):** *Aguardando confirmação para início.*
