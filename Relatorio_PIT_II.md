# Relatório de Entrega — Projeto Integrador Transdisciplinar em Engenharia de Software II

> Modelo Oficial de Entrega do PIT II preenchido conforme as solicitações da disciplina e do AVA.  
> Documento preparado para acompanhamento e submissão acadêmica direta.

---

## 1. Identificação do Estudante

- **Nome:** Alisson Rodrigues da Silva Espíndola
- **RGM:** [RGM protegido no repositório público / informado no envio do AVA]
- **Data de Nascimento:** [Data de nascimento protegida no repositório público / informada no envio do AVA]
- **Curso:** Engenharia de Software
- **Semestre:** 2026/2
- **Disciplina:** Projeto Integrador Transdisciplinar em Engenharia de Software II (Turma 001)

---

## 2. Documentação do Projeto (Desafio 1)

> **Instrução oficial:** *Revisite a documentação do projeto desenvolvido na PIT I, faça as atualizações e melhorias necessárias de acordo com os pontos explicitados no material teórico. Ao terminar os ajustes, suba a documentação em um repositório GIT.*

- **Link do Repositório Git:** [https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git](https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git)

### Resumo das Atualizações e Revisões Realizadas:
1. **Escopo e Catálogo Oficial de Sabores (Ordem Invertida):** Definição dos 5 sabores gourmet da confeitaria artesanal iniciando do último para o primeiro (*Doce de Leite*, *Nutella*, *Ninho com Morango*, *Ninho com Brigadeiro* e *Chocolate*), descrições completas de ingredientes, massas e coberturas com preços unitários calibrados.
2. **Parâmetros Comerciais Operacionais:** Taxa de entrega fixa local de R$ 6,00 e prazo de atendimento estimado de 15 a 30 minutos na região de proximidade da confeitaria.
3. **Histórias de Usuário e Backlog:** 15 histórias de usuário (HU01 a HU15) estruturadas com critérios de aceitação, regras de negócio e estimativas de esforço cobrindo desde a vitrine até o painel restrito do operador.
4. **Casos de Uso e Diagramas UML:** Diagramas revisados e gerados no padrão Draw.io (`Diagramas.drawio`), contemplando Casos de Uso, Classes de Domínio e Persistência em C#, Sequência de confirmação de pedido, Atividades do ciclo de vida, Mapa Conceitual do domínio e Mapa Navegacional de rotas.
5. **Modelagem de Dados Relacional (3FN):** Projeto conceitual e lógico formalizado na Terceira Forma Normal, com scripts físicos DDL e carga DML executados no SQLite (`schema.sql` e `database.db`) e **Dicionário de Dados** completo em [`Banco_de_Dados.md`](Banco_de_Dados.md).
6. **IHC e Identidade Visual Contemporânea:** Interface elegante e responsiva com tipografia refinada ('Playfair Display' e 'Plus Jakarta Sans'), paleta em tons nobres de chocolate, caramelo e creme, toasts flutuantes, modais acessíveis e stepper de carrinho.

---

## 3. Codificação da Solução (Desafio 2 — Versão 1)

> **Instrução oficial:** *Na tabela a seguir insira as informações referentes ao desenvolvimento do código do front-end e back-end.*

| Campo | Conteúdo Oficial Preenchido |
|---|---|
| **Linguagem do Back-end** | **C# (.NET 10 — Framework ASP.NET Core MVC)** |
| **Banco de Dados** | **SQLite (Normalizado na Terceira Forma Normal — 3FN)** |
| **Hospedagem** | **Servidor Local / Localhost (Execução em ambiente de desenvolvimento .NET 10 Kestrel)** |
| **Plataforma** | **Web Responsiva (Compatível com Desktop e Dispositivos Móveis)** |
| **Modo de Codificação** | **( x ) Tradicional** &nbsp;&nbsp; ( ) Low-code &nbsp;&nbsp; ( ) No-code |
| **Link do repositório no GitHub com os códigos abertos** | [https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git](https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git) |
| **Link da solução em funcionamento** | [https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git](https://github.com/alisson-rodrigues-dev/Projeto-Transdiciplinar.git) *(Código aberto completo com manual de execução local em C#)* |
| **Link do vídeo narrado (no mínimo 5 min)** | *A ser gravado e anexado após a validação da Versão 1* |

---

## 4. Testes da Solução e Laudo de Qualidade (Desafio 3 — Versão 2)

> **Resumo da Versão 2 (V2):**  
> Foram executados 5 ciclos de validação técnica com os avaliadores oficiais da disciplina (Amanda Maia, Priscilla Pavan Ladeia da Silva, Rodivan Pereira Ramos, Heloísa Felizardo Campos e Pedro Lucas Bandeira Alves). Todas as solicitações de melhoria e correções de segurança foram implementadas na íntegra no código-fonte em C# e validadas pela suíte automatizada de testes com 100% de êxito.

### 4.1. Relatórios Individuais dos Avaliadores (Ordem Oficial Invertida):

1. **Amanda Maia (Conta de Teste: amanda.maia@exemplo.com | Senha: 123456 | Data: 09/10/2026):**
   - **O que testou e funcionou:** Acesso ao painel operacional da confeitaria e visualização do fluxo de pedidos em `Controllers/OperadorController.cs` e `Views/Operador/Index.cshtml`.
   - **O que testou e não funcionou – O que deve ser corrigido:** Constatou que o painel de pedidos da loja (`/Operador`) estava acessível sem qualquer autenticação, expondo dados sensíveis de pedidos e clientes para visitantes anônimos.
   - **Correção Aplicada na V2:** Implementada proteção estrita de rotas com controle de acesso baseado em papéis (RBAC) em `Controllers/OperadorController.cs`. Criada no banco de dados SQLite a conta de administrador `alissonadm@email.com` (senha `123456`). Visitantes deslogados são redirecionados para a tela de login (`/Conta/Login?returnUrl=/Operador`) e clientes comuns logados têm o acesso bloqueado com redirecionamento para o seu painel de usuário (`/Conta/Index`). No cabeçalho (`Views/Shared/_Layout.cshtml`), o botão "⚙️ Painel da Loja (Admin)" é renderizado exclusivamente quando o e-mail em sessão corresponde a `alissonadm@email.com`.
   - **Funcionalidade não testada:** Autenticação multifator (MFA/2FA) com envio de token via SMS ou TOTP.

2. **Priscilla Pavan Ladeia da Silva (Conta de Teste: priscilla.silva@exemplo.com | Senha: 123456 | Data: 08/10/2026):**
   - **O que testou e funcionou:** Navegação pelo funil de checkout e seleção das formas de pagamento (PIX e Cartão de Crédito) através de `Controllers/CheckoutController.cs` e `Views/Checkout/Index.cshtml`.
   - **O que testou e não funcionou – O que deve ser corrigido:** Identificou que o QR Code do PIX e a chave copia-e-cola eram liberados imediatamente na tela mesmo para visitantes deslogados antes de se autenticarem.
   - **Correção Aplicada na V2:** Implementado bloqueio condicional de visualização no checkout (`Views/Checkout/Index.cshtml` e `Controllers/CheckoutController.cs`). Quando o usuário está deslogado, visualiza um container protegido com aviso de segurança e botões de redirecionamento para Login e Cadastro. A chave PIX e o QR Code só são liberados após validação da sessão do usuário. A action de confirmação de pagamento também rejeita transações sem autenticação ativa.
   - **Funcionalidade não testada:** Liquidação bancária instantânea via Webhook do Banco Central / BACEN em ambiente de produção.

3. **Rodivan Pereira Ramos (Conta de Teste: rodivan.ramos@exemplo.com | Senha: 123456 | Data: 07/10/2026):**
   - **O que testou e funcionou:** Adição de cupcakes ao carrinho, cálculo da taxa fixa de entrega de R$ 6,00 em 15 a 30 minutos e visualização em diferentes dispositivos.
   - **O que testou e não funcionou – O que deve ser corrigido:** Constatou que os botões da barra de navegação superior (NAV) quebravam em múltiplas linhas e desestruturavam o cabeçalho em telas menores (smartphones e tablets com largura inferior a 992px).
   - **Correção Aplicada na V2:** Desenvolvida arquitetura de menu 100% responsiva com botão toggle hambúrguer animado (`#navToggleBtn`) e drawer lateral retrátil via CSS moderno (`wwwroot/css/site.css`) e scripts de interação (`wwwroot/js/site.js`). Em viewports mobile/tablet (`@media (max-width: 991px)`), o menu colapsa de forma elegante com toque suave.
   - **Funcionalidade não testada:** Compatibilidade com navegadores legados sem suporte a CSS Flexbox/Grid modernos.

4. **Heloísa Felizardo Campos (Conta de Teste: heloisa.campos@exemplo.com | Senha: 123456 | Data: 04/10/2026):**
   - **O que testou e funcionou:** Avaliação da identidade visual, elementos institucionais da loja e navegação pelos links do cabeçalho em `Views/Shared/_Layout.cshtml`.
   - **O que testou e não funcionou – O que deve ser corrigido:** Logotipo simplificado (apenas texto com emoji genérico) e ausência de uma página institucional ao clicar no logotipo, direcionando de forma prematura para a vitrine de produtos.
   - **Correção Aplicada na V2:** Criado novo logotipo vetorial profissional em formato SVG (`wwwroot/images/logo-lecupcake.svg`) com emblema de confeitaria fina e tipografia refinada. Desenvolvida a página institucional de apresentação da marca (`Views/Home/Apresentacao.cshtml` e action `Apresentacao()` em `Controllers/HomeController.cs`). Ao clicar no logotipo na NAV, o cliente é encaminhado para a apresentação da história artesanal e diferenciais da Le Cupcake Gourmet.
   - **Funcionalidade não testada:** Suporte a alternância de contraste e temas de acessibilidade no logotipo.

5. **Pedro Lucas Bandeira Alves (Conta de Teste: pedro.alves@exemplo.com | Senha: 123456 | Data: 05/10/2026):**
   - **O que testou e funcionou:** Navegação pelo catálogo de cupcakes na vitrine (`Views/Home/Index.cshtml`) e verificação das opções de perfil na barra de navegação.
   - **O que testou e não funcionou – O que deve ser corrigido:** Apontou que a vitrine não continha fotografias reais dos cupcakes gourmet (apenas ilustrações temporárias) e que a rota do painel do usuário aparecia de forma inconsistente para visitantes não autenticados.
   - **Correção Aplicada na V2:** Produzidas e integradas 5 fotografias gastronômicas de alta resolução (`wwwroot/images/cupcake-*.jpg`) para cada sabor oficial do catálogo em ordem invertida (Doce de Leite, Nutella, Ninho com Morango, Ninho com Brigadeiro e Chocolate), com atualização no banco SQLite (`schema.sql` e `database.db`). No menu de navegação, a rota "👤 Painel do Usuário" (`/Conta/Index`) agora é exibida exclusivamente quando o cliente está logado; visitantes veem somente o botão "👤 Entrar".
   - **Funcionalidade não testada:** Conversão e compressão dinâmica em tempo real para o formato WebP no pipeline do servidor ASP.NET Core.

### 4.2. Laudo de Qualidade da Aplicação (Kaizen — Versão 2)
Todas as 7 solicitações de engenharia para a Versão 2 foram integralmente implementadas e validadas com sucesso através da suíte automatizada de testes (`Tests/SuiteTestesUnitarios.cs`) com 100% de aprovação (9 testes aprovados | 0 falhas). A aplicação atende a todos os critérios de segurança da informação, usabilidade, responsividade multiplataforma e excelência visual.
