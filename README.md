# EasyShop — Persistência Poliglota (SQL + NoSQL locais)

Protótipo de mini e-commerce em **.NET MAUI (Android)** que usa dois bancos locais no mesmo app:

| Domínio | Paradigma | Tecnologia | Por quê |
|---|---|---|---|
| Catálogo de Produtos | NoSQL (documentos) | **LiteDB** (`products.db`) | Cada produto tem atributos diferentes (cor, voltagem, RAM...). O documento aceita um dicionário livre, sem alterar schema nem criar colunas nulas. |
| Cadastro de Clientes | SQL (relacional) | **SQLite** via `sqlite-net-pcl` (`customers.db3`) | Dados estruturados, com chave primária e e-mail único garantido pelo próprio banco. |

## Integrantes

- JUSSARA FARIA DE OLIVEIRA
- DIOGO FERNANDES SOUZA

## Arquitetura em camadas

```
Views / ViewModels  (Apresentação)  -> só captura a interação e exibe dados
        |
Services            (Negócio)       -> validações e orquestração (ProductService, CustomerService)
        |
Data / Repositories (Acesso a dados)-> único lugar que conhece SQLite e LiteDB
```

```
EasyShop/
├── Models/        Customer (SQLite) e Product (documento LiteDB)
├── Data/          ICustomerRepository + CustomerRepository (SQL)
│                  IProductRepository  + ProductRepository  (NoSQL)
├── Services/      CustomerService, ProductService, BusinessException
├── ViewModels/    BaseViewModel, CustomersViewModel, ProductsViewModel
├── Views/         ProductsPage (aba NoSQL), CustomersPage (aba SQL)
├── MauiProgram.cs Injeção de dependência (monta as camadas)
└── Platforms/Android
```

Regras de dependência: a UI só enxerga *Services*; os *Services* só enxergam interfaces de *Repository*; somente os *Repositories* usam `SQLite` e `LiteDB`.

## Funcionalidades

**Aba Produtos (NoSQL)**: cadastro de Nome, Preço, Categoria (opcional) e Atributos livres no formato `chave=valor; chave=valor` (ex.: `Cor=Azul; Tamanho=M; Garantia=12 meses`). A lista é lida do LiteDB.

**Aba Clientes (SQL)**: cadastro de Nome completo e E-mail, com validação (nome e sobrenome, formato de e-mail, e-mail duplicado bloqueado). A lista é lida do SQLite.

Para demonstrar a flexibilidade do NoSQL, cadastre dois produtos de categorias diferentes:

- `Camiseta Dry Fit` — R$ 79,90 — Vestuário — `Tamanho=M; Cor=Preto`
- `Notebook Pro 14` — R$ 5499,00 — Informática — `Processador=i7; Memória RAM=16GB; Garantia=24 meses`

## Como executar

Pré-requisitos: .NET 8 SDK, workload MAUI (`dotnet workload install maui`), Android SDK e um emulador Android criado (Android Studio > Device Manager) ou Visual Studio 2022 com a carga de trabalho "Desenvolvimento para dispositivos móveis com .NET".

```bash
dotnet restore
dotnet build -t:Run -f net8.0-android
```

Ou abra a pasta no Visual Studio, escolha o emulador Android na barra superior e pressione F5.

Os dados ficam em `FileSystem.AppDataDirectory` do aplicativo e persistem entre execuções.

