# Korp_Teste_Gabriel_De_Melo_Lima
Este repositório é responsável por entregar minha solução para o problema descrito no desafio: `Projeto técnico: Sistema de emissão de Notas Fiscais`

# Indices:
* [Escopo do problema](#escopo-do-problema)
* [Solução do problema](#solução-do-problema) 
* [Ferramentas usadas](#ferramentas-usadas)
    - [Frameworks](#frameworks)
    - [Bibliotecas](#Bibliotecas)

# Escopo do problema

## Funcionalidades a serem desenvolvidas

### Cadastro de Produto

Campos obrigatórios:

* Código
* Descrição (nome do produto)
* Saldo (quantidade disponível em estoque)

**Resultado esperado:** permitir que um produto seja previamente cadastrado para posterior utilização em notas fiscais.

### Cadastro de Notas Fiscais

Campos obrigatórios:

* Numeração sequencial
* Status: *Aberta* ou *Fechada*
* Inclusão de múltiplos produtos com respectivas quantidades

**Resultado esperado:** permitir a criação de uma nota fiscal com numeração sequencial e status inicial *Aberta*.

### Impressão de Notas Fiscais

* Botão de impressão visível e intuitivo em tela.

Resultado esperado:

- Ao clicar no botão, exibir indicador de processamento;
- Após finalização, atualizar o status da nota para *Fechada*;
- Não permitir a impressão de notas com status diferente de *Aberta*;
- Atualizar o saldo dos produtos conforme a quantidade utilizada na nota.

      Exemplo: saldo anterior = 10; nota utiliza 2 unidades -> novo saldo = 8.

## Requisitos obrigatórios

* Arquitetura de Microsserviços:
Estruturar o sistema com no mínimo dois microsserviços:
   - Serviço de Estoque – controle de produtos e saldos;
   -  Serviço de Faturamento – gestão de notas fiscais.
* Tratamento de Falhas:
Implementar um cenário em que um dos microsserviços falha.
O sistema deve ser capaz de se recuperar da falha e fornecer
feedback apropriado ao usuário sobre o erro.
* Conexão Real com banco de dados:
É esperado que os cadastros sejam persistidos fisicamente em um banco
de dados de sua escolha.

## Requisitos opcionais
O candidato poderá, a seu critério, implementar também:
* Tratamento de Concorrência:

    Cenário: produto com saldo 1 sendo utilizado simultaneamente por duas notas.

    
* Uso de Inteligência Artificial:

    Implementar alguma funcionalidade do sistema que utilize IA.


* Implementação de Idempotência:

    Garantir que operações repetidas não causem efeitos colaterais indesejados.




# Solução do problema

# Ferramentas usadas


## Frameworks 

* **`angular.js`** 
    - Sass (SCSS)
    - GitHub Copilot
* **`node.js`**

## Bibliotecas

### Node.js
- express
- http-proxy-middleware
- cors

## C#
- Microsoft.EntityFrameworkCore
- Npgsql.EntityFrameworkCore.PostgreSQL
- Microsoft.EntityFrameworkCore.Tools


- dotnet-ef