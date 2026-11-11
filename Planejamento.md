# Planejamento da Loja de Cupcakes Gourmet — Versão 1

> **Projeto Integrador Transdisciplinar em Engenharia de Software II**  
> **Estudante:** Alisson R. S. Espíndola *(identificação completa preservada no relatório oficial AVA)*  
> **Stack Tecnológica:** C# (.NET 10) / ASP.NET Core MVC / SQLite 3FN  
> **Repositório:** [https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git](https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git)

---

## 1. Escopo e Objetivos do Projeto

O objetivo deste projeto integrador é desenvolver uma aplicação Web completa para uma confeitaria artesanal especializada em cupcakes finos, viabilizando:
1. Vitrine virtual com catálogo dinâmico de cupcakes gourmet;
2. Montagem e gerenciamento de carrinho de compras;
3. Cadastro e autenticação segura de clientes;
4. Checkout com validação de endereço e regras comerciais (prazo de 15 a 30 minutos e taxa de entrega fixa de R$ 6,00);
5. Pagamento demonstrativo instantâneo via **PIX** ou **Cartão de Crédito**;
6. Acompanhamento em tempo real da entrega em 4 estágios (*Recebido* ➔ *Em Preparo* ➔ *Em Rota* ➔ *Entregue*);
7. Painel operacional restrito para a confeitaria gerenciar e atualizar os pedidos em tempo de execução.

---

## 2. Parâmetros Comerciais e Catálogo Oficial

### 2.1. Catálogo Oficial dos 5 Sabores Gourmet (Ordem Invertida)

Conforme a diretriz de personalização do projeto, o catálogo foi cadastrado e é apresentado começando do último sabor para o primeiro:

| ID | Sabor do Cupcake | Ingredientes e Cobertura | Preço Unitário |
|:---:|---|---|:---:|
| 1 | **Doce de Leite** | Massa de especiarias suaves com canela, recheio generoso e cobertura de doce de leite caseiro cremoso polvilhado com canela. | R$ 13,50 |
| 2 | **Nutella** | Massa de cacau nobre, recheio puro de creme de avelã Nutella e cobertura de brigadeiro de Nutella com avelãs tostadas picadas. | R$ 16,00 |
| 3 | **Ninho com Morango** | Massa leve de baunilha, recheio de geleia artesanal de morangos frescos e cobertura aveludada de mousse de leite Ninho com morango fresco. | R$ 15,00 |
| 4 | **Ninho com Brigadeiro** | Massa amanteigada de baunilha, recheio de leite Ninho cremoso e cobertura dupla com brigadeiro tradicional e brigadeiro de Ninho. | R$ 14,00 |
| 5 | **Chocolate** | Massa fofinha de cacau 100%, recheio cremoso de ganache de chocolate meio amargo e cobertura generosa de brigadeiro gourmet com granulado belga. | R$ 13,00 |

### 2.2. Parâmetros de Entrega e Pagamento
- **Prazo de entrega:** 15 a 30 minutos na região de proximidade da confeitaria.
- **Taxa de entrega:** Taxa fixa de R$ 6,00 para pedidos locais.
- **Formas de pagamento:**
  1. **PIX:** Confirmação instantânea com geração simulada de QR Code e chave copia-e-cola.
  2. **Cartão de Crédito:** Validação de campos e aprovação em tempo de execução.

---

## 3. Definição da Stack Tecnológica (Padrão MVC em C#)

| Componente | Tecnologia Escolhida | Justificativa Técnica |
|---|---|---|
| **Linguagem do Back-end** | **C# 13 (.NET 10)** | Linguagem fortemente tipada, de alto desempenho, robusta, indicada oficialmente na matriz de requisitos do PIT II (p. 34). |
| **Framework Web** | **ASP.NET Core MVC** | Padrão arquitetural canônico **Model-View-Controller**, com injeção de dependência nativa, tipagem estrita de ViewModels e alta manutenibilidade. |
| **Banco de Dados** | **SQLite (3FN)** | SGBD relacional serverless, portátil, normalizado rigorosamente na Terceira Forma Normal, com suporte a transações ACID atômicas. |
| **Front-end** | **HTML5, CSS3 e JavaScript Vanilla** | Interface moderna com tipografia 'Playfair Display' e 'Plus Jakarta Sans', cards dinâmicos, modais acessíveis e suporte a requisições assíncronas via `fetch`. |
| **Hospedagem** | **Servidor Local / Localhost** | Ambiente ágil de desenvolvimento, testes e gravação com execução local em .NET 10. |
| **Plataforma** | **Web Responsiva** | Compatibilidade multiplataforma direta em navegadores modernos desktop e mobile sem apps adicionais. |
| **Modo de Codificação** | **Tradicional** | Desenvolvimento de código-fonte aberto, estruturado e versionado em repositório público do GitHub. |

---

## 4. Histórias de Usuário (15 Histórias do Backlog)

| ID | História de Usuário | Critério de Aceitação | Prioridade |
|---|---|---|:---:|
| HU01 | Como cliente, quero consultar a vitrine para conhecer os 5 sabores disponíveis | Exibir os 5 sabores gourmet cadastrados com foto, nome e preço | 1 |
| HU02 | Como cliente, quero ver os detalhes do cupcake para conhecer os ingredientes | Modal interativo exibindo a receita e composição completa | 2 |
| HU03 | Como cliente, quero me cadastrar para salvar meu endereço e dados | Formulário com nome, e-mail único, telefone e validações | 1 |
| HU04 | Como cliente cadastrado, quero fazer login para agilizar meus pedidos | Autenticação de credenciais com sessão HTTP segura | 1 |
| HU05 | Como cliente, quero adicionar cupcakes ao carrinho com quantidade ajustável | Inserção assíncrona ao carrinho sem sair da vitrine | 1 |
| HU06 | Como cliente, quero consultar o carrinho para conferir os itens e frete | Tabela com subtotais, taxa fixa de R$ 6,00 e valor total | 1 |
| HU07 | Como cliente, quero alterar a quantidade de itens no carrinho | Atualização imediata dos valores com botões de stepper (+ e -) | 2 |
| HU08 | Como cliente, quero remover itens do carrinho para ajustar minha compra | Exclusão do item e recálculo automático do valor total | 2 |
| HU09 | Como cliente, quero informar meu endereço para a entrega expressa | Validação de rua, número, bairro e prazo de 15 a 30 min | 1 |
| HU10 | Como cliente, quero revisar o resumo do pedido antes de pagar | Apresentação detalhada dos produtos, taxa e valor final | 1 |
| HU11 | Como cliente, quero confirmar o pedido para formalizar a compra | Gravação atômica em transação no SQLite (3FN) | 1 |
| HU12 | Como cliente, quero escolher entre PIX e Cartão de Crédito | Abas de pagamento com chave/QR Code simulados ou campos de cartão | 1 |
| HU13 | Como cliente, quero receber confirmação com o número do meu pedido | Exibição do número oficial do pedido e recibo financeiro | 1 |
| HU14 | Como cliente, quero acompanhar a entrega em tempo real | Linha do tempo visual em 4 estágios do status do pedido | 1 |
| HU15 | Como operador da loja, quero visualizar os pedidos e atualizar o status | Painel administrativo com métricas e alteração de entrega | 1 |
