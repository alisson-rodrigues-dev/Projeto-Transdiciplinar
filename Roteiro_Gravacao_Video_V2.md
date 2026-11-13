# 🎬 Roteiro Oficial de Gravação de Vídeo — Versão 2 (V2)
## Projeto Integrador Transdisciplinar em Engenharia de Software II (Turma 001)

> **Documento:** Guia e Script de Gravação Narrada da Versão 2 (Desafio 3 — Kaizen & Homologação)  
> **Estudante:** Alisson Rodrigues da Silva Espíndola  
> **Curso:** Engenharia de Software  
> **Tecnologia:** C# (.NET 10) | ASP.NET Core MVC | SQLite (Normalizado em 3FN)  
> **Duração Máxima Exigida:** Até 5 minutos  

---

## 1. Preparação do Ambiente Pré-Gravação

Antes de iniciar a gravação, certifique-se de que os seguintes passos foram executados:

1. **Abrir a Aplicação no Navegador:**
   - Execute o projeto C# no terminal dentro da pasta `Atividade`:
     ```bash
     dotnet run
     ```
   - Abra o navegador em: `http://localhost:5000` (ou na porta configurada).
2. **Deixar o Terminal Aberto:**
   - Deixe uma aba do terminal pronta para demonstrar a suíte de testes unitários:
     ```bash
     dotnet run -- --test
     ```
3. **Credenciais para Demonstração em Vídeo:**
   - **Conta de Cliente Comum:** `cliente@exemplo.com` | Senha: `123456`
   - **Conta de Administrador (RBAC V2):** `alissonadm@email.com` | Senha: `123456`
4. **Ferramenta de Gravação de Tela:**
   - Pressione `Win + G` (Barra de Jogos do Windows) ou utilize OBS Studio / Loom / Clipchamp.
   - Resolução sugerida: 1080p (Full HD), microfone calibrado e navegador em tela cheia.

---

## 2. Mapa das Melhorias Homologadas na Versão 2 (Ciclo Kaizen)

Durante o vídeo, cite como cada ajuste atendeu aos testes dos 5 homologadores oficiais da disciplina:

| # | Homologador Oficial | Problema Identificado (V1) | Melhoria de Engenharia Aplicada (V2) |
|:---:|:---|:---|:---|
| **1** | **Amanda Maia** | Painel operacional da loja (`/Operador`) exposto sem login. | **Controle de Acesso RBAC:** Rota protegida exclusivamente para o administrador `alissonadm@email.com`. |
| **2** | **Priscilla Pavan** | QR Code do PIX e chave copia-e-cola liberados antes do login. | **Segurança no Checkout:** Trava que bloqueia dados de cobrança até autenticação do cliente. |
| **3** | **Rodivan Pereira** | Barra de navegação quebrava botões em telas menores. | **Navegação Responsiva:** Menu hambúrguer animado (`#navToggleBtn`) e drawer retrátil para mobile/tablet. |
| **4** | **Heloísa Felizardo** | Logotipo genérico e ausência de apresentação institucional da marca. | **Identidade Visual e Página Institucional:** Criação da página `/Home/Apresentacao` acessível ao clicar na marca. |
| **5** | **Pedro Lucas** | Falta de fotos reais dos cupcakes e visibilidade inconsistente da conta. | **Fotografias Gastronômicas Reais:** 5 fotos em alta resolução no catálogo 3FN e rota "Painel do Usuário" condicional. |

---

## 3. Roteiro Cronometrado Passo a Passo (Até 5 Minutos)

---

### ⏱️ BLOCO 1: Abertura e Identificação Institucional
* **Tempo sugerido:** `00:00` a `00:35` (35 segundos)
* **O que mostrar na tela:** Página inicial da confeitaria aberta no navegador, exibindo o cabeçalho e a marca **🧁 Le Cupcake — Gourmet & Artesanal**.
* **Fala recomendada:**
  > *"Olá, professor e banca avaliadora!  
  > Meu nome é **Alisson Rodrigues da Silva Espíndola**, estudante de Engenharia de Software na disciplina Projeto Integrador Transdisciplinar II (Turma 001).  
  > Neste vídeo apresento a **Versão 2 (V2)** da plataforma de e-commerce da Confeitaria **Le Cupcake Gourmet**. A solução foi desenvolvida em C# utilizando o padrão arquitetural ASP.NET Core MVC (.NET 10) e banco de dados relacional SQLite estruturado na Terceira Forma Normal (3FN).  
  > Demonstrarei as melhorias e correções de qualidade implementadas a partir do feedback dos 5 homologadores do projeto."*

---

### ⏱️ BLOCO 2: Responsividade da NAV e Apresentação da Marca
* **Tempo sugerido:** `00:35` a `01:20` (45 segundos)
* **O que mostrar na tela:** 
  1. Mostre a barra de navegação no desktop com os 7 links alinhados em linha única.
  2. Redimensione a janela do navegador ou pressione `F12` (modo dispositivo móvel / tablet): mostre o botão de menu hambúrguer aparecendo e abrindo a gaveta de navegação suavemente.
  3. Clique na marca **🧁 Le Cupcake** para abrir a nova página institucional `/Home/Apresentacao`.
* **Fala recomendada:**
  > *"Iniciando pelas melhorias de interface e experiência de usuário:  
  > O avaliador **Rodivan Pereira** observou que a barra de navegação quebrava os botões em telas intermediárias e celulares. Na Versão 2 desenvolvemos uma arquitetura de navegação 100% responsiva, com botão hambúrguer expansível e drawer lateral que garante navegabilidade perfeita em smartphones e tablets.  
  > Atendendo também à sugestão de **Heloísa Felizardo**, o clique sobre a marca agora direciona o cliente para a página de **Apresentação da Confeitaria** (`/Home/Apresentacao`), apresentando os valores artesanais, a história e os diferenciais da marca antes da vitrine."*

---

### ⏱️ BLOCO 3: Fotografias Reais e Catálogo em Ordem Invertida
* **Tempo sugerido:** `01:20` a `02:05` (45 segundos)
* **O que mostrar na tela:** 
  1. Clique em **"Cardápio & Vitrine"** na barra de navegação.
  2. Role pelos 5 produtos, apontando as fotos reais em alta resolução e a ordem dos sabores.
* **Fala recomendada:**
  > *"No catálogo de produtos, o homologador **Pedro Lucas** pontuou a ausência de fotos reais dos produtos na vitrine.  
  > Na Versão 2 integramos 5 fotografias gastronômicas de alta resolução, vinculadas diretamente às entidades do banco SQLite através da coluna `ImagemUrl`.  
  > O menu segue rigorosamente a ordem invertida de sabores exigida no escopo:  
  > 1º Doce de Leite, 2º Nutella, 3º Ninho com Morango, 4º Ninho com Brigadeiro e 5º Chocolate, todos com cálculo dinâmico e modal de ingredientes."*

---

### ⏱️ BLOCO 4: Segurança no Pagamento PIX (Trava até Login)
* **Tempo sugerido:** `02:05` a `02:55` (50 segundos)
* **O que mostrar na tela:** 
  1. Estando deslogado, adicione um cupcake ao carrinho (+ Pedir).
  2. Vá ao Checkout e selecione a aba **PIX**: mostre que o QR Code e o código copia-e-cola estão **bloqueados**, exibindo o alerta de segurança instruindo o cliente a entrar na conta.
  3. Clique em **"Entrar"**, faça login com a conta de teste (`cliente@exemplo.com` / `123456`) e volte ao checkout: mostre que agora o QR Code e a chave PIX são renderizados.
* **Fala recomendada:**
  > *"No módulo de pagamentos, a avaliadora **Priscilla Pavan** identificou uma vulnerabilidade de fluxo: a liberação prematura do QR Code e chave PIX para visitantes não autenticados.  
  > Na Versão 2 aplicamos uma regra de segurança condicional no Razor e no Controller C#: enquanto o cliente não possuir sessão ativa autenticada, o QR Code permanece protegido com aviso instruindo o login ou cadastro. Assim que o cliente faz login, os dados de cobrança são gerados e associados de forma segura ao seu pedido."*

---

### ⏱️ BLOCO 5: Proteção de Rotas (RBAC) e Painel do Administrador
* **Tempo sugerido:** `02:55` a `03:55` (60 segundos)
* **O que mostrar na tela:** 
  1. Como cliente comum logado (`cliente@exemplo.com`), mostre que a barra de navegação exibe **"👤 Painel do Usuário"** e **não** exibe o painel da loja.
  2. Digite `/Operador` na URL do navegador: veja a mensagem de bloqueio redirecionando para o painel do cliente.
  3. Clique em **"Sair"** e faça login com a conta administrativa: **`alissonadm@email.com`** (senha `123456`).
  4. Mostre que agora surge na barra de navegação o botão **"⚙️ Painel da Loja (Admin)"**.
  5. Acesse o painel e altere o status de um pedido (ex: de *RECEBIDO* para *PREPARANDO*).
* **Fala recomendada:**
  > *"Um dos pontos mais críticos, destacado por **Amanda Maia**, foi a exposição pública do painel de pedidos da loja.  
  > Para solucionar, implementamos Controle de Acesso Baseado em Funções (RBAC). Criamos a conta de administração mestre `alissonadm@email.com`.  
  > Usuários anônimos e clientes comuns são estritamente impedidos de acessar a rota `/Operador`. Apenas ao autenticar com a conta administrativa o botão 'Painel da Loja' surge no menu, permitindo que a confeitaria gerencie e atualize a esteira de pedidos."*

---

### ⏱️ BLOCO 6: Validação Automatizada — Suíte de Testes Unitários
* **Tempo sugerido:** `03:55` a `04:30` (35 segundos)
* **O que mostrar na tela:** Alterne para a janela do terminal e execute:
  ```bash
  dotnet run -- --test
  ```
  Deixe o terminal rodar e mostre o output colorido com os **9 testes passando**:
  - `[PASSOU] 1. Catálogo Oficial: 5 Sabores Gourmet na Ordem Invertida`
  - `[PASSOU] 2. Carrinho: Subtotal, Taxa Fixa de R$ 6,00 e Total`
  - `[PASSOU] 3. Carrinho Vazio: Total e Subtotal Zerados`
  - `[PASSOU] 4. Parâmetros Comerciais: Prazo de Entrega Estimado de 15 a 30 Minutos`
  - `[PASSOU] 5. Domínio Cliente: Validação de E-mail e Integridade de Campos`
  - `[PASSOU] 6. Checkout: Validação de Formas de Pagamento Permitidas (PIX e CARTAO_CREDITO)`
  - `[PASSOU] 7. Banco de Dados SQLite (3FN): Consulta de Pedidos e Histórico`
  - `[PASSOU] 8. RBAC / Autenticação: Conta de Administrador alissonadm@email.com`
  - `[PASSOU] 9. Ativos V2: Presença de Imagens dos Cupcakes e Logotipo Vetorial`
  - Resultado final: **9 Aprovados | 0 Falhas**
* **Fala recomendada:**
  > *"Para comprovar a conformidade e a robustez da engenharia, implementamos uma suíte completa de testes de unidade e integração em C#.  
  > Executando o comando `dotnet run -- --test`, todos os 9 cenários de validação — abrangendo integridade de cálculo, persistência relacional, autenticação RBAC e validação de ativos — são aprovados com 100% de êxito e zero falhas."*

---

### ⏱️ BLOCO 7: Conclusão e Encerramento
* **Tempo sugerido:** `04:30` a `05:00` (30 segundos)
* **O que mostrar na tela:** Retorne à página inicial da aplicação no navegador.
* **Fala recomendada:**
  > *"Com todas essas implementações, a Versão 2 consolida uma arquitetura limpa, segura e escalável, cumprindo todos os requisitos dos Desafios 1, 2 e 3 do Projeto Integrador Transdisciplinar II.  
  > O código-fonte integral e a documentação técnica estão disponibilizados no GitHub.  
  > Muito obrigado pela atenção e uma ótima avaliação!"*

---

## 4. Dicas Extras para Gravação Perfeita

1. **Ritmo:** Mantenha um tom de voz firme, calmo e seguro.
2. **Cursor do Mouse:** Movimente o mouse com suavidade ao apontar os elementos na tela.
3. **Respeito ao Tempo:** Pratique uma vez antes de gravar para garantir que o tempo final fique entre 4 e 5 minutos.
4. **Link do Vídeo:** Após gravar, suba o arquivo no YouTube (como "Não listado") ou no Google Drive (com permissão pública de visualização) e insira o link na Tabela 6 do relatório do PIT II.
