# 🏦 FunctionApp1 - Gerador de Código de Barras de Boletos

> ![#0078D4](https://via.placeholder.com/15/0078D4/000000?text=+) **Azure Function em .NET 8**

Este projeto é uma Azure Function desenvolvida em .NET 8 para geração de códigos de barras de boletos bancários. Ele recebe dados via requisição HTTP, gera o código de barras no padrão bancário, cria uma imagem do código e envia os dados para uma fila do Azure Service Bus.

## ✨ Funcionalidades

- 🏷️ Geração de código de barras de boleto (44 dígitos)
- 🖼️ Criação de imagem do código de barras (PNG, base64)
- ☁️ Envio dos dados para uma fila do Azure Service Bus

## 🚀 Como usar

1. Faça uma requisição HTTP POST para o endpoint da função `barcode-generate` com o seguinte corpo JSON: { "valor": "100.00", "dataVencimento": "2025-06-15" }

2. O retorno será um JSON contendo:
   - `barcode`: código de barras gerado
   - `valorOriginal`: valor informado
   - `dataVencimento`: data de vencimento
   - `imageBase64`: imagem do código de barras em base64

## ⚙️ Configuração

- Defina a variável de ambiente `ServiceBusConnectionString` com a string de conexão do Azure Service Bus.

## 📦 Requisitos

- .NET 8
- Azure Functions v4
- Azure Service Bus

---

# 🕵️ fnValidaBoleto - Validador de Código de Barras de Boletos

> ![#228B22](https://via.placeholder.com/15/228B22/000000?text=+) **Azure Function em .NET 8**

Este projeto é uma Azure Function desenvolvida em .NET 8 para validação de códigos de barras de boletos bancários. Ele recebe um código de barras via requisição HTTP e valida seu formato e data de vencimento.

## ✨ Funcionalidades

- 🔢 Validação do tamanho do código de barras (44 dígitos)
- 📅 Validação da data de vencimento embutida no código

## 🚀 Como usar

1. Faça uma requisição HTTP POST para o endpoint da função `barcode-validate` com o seguinte corpo JSON: { "barcode": "0081506202510000000...0000" }

2. O retorno será um JSON indicando se o boleto é válido e a data de vencimento extraída.

## 📦 Requisitos

- .NET 8
- Azure Functions v4

---