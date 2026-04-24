## Arquitetura

O projeto foi estruturado usando Arquitetura Cebola, mantendo uma boa separação de responsabilidades.
Como o problema é relativamente simples, essa arquitetura já atende bem, deixando o código organizado e as regras de negócio fáceis de entender, sem adicionar complexidade desnecessária.
Optei por não usar CQRS, pois para esse cenário não faria muito sentido separar leitura e escrita. Acabaria deixando a aplicação mais complexa sem trazer um ganho real.

### Core
  Domain
  * Entidades e enums

Application
* Services
* DTOs
* Regras de negócio (ex: cálculo de desconto)

### Infrastructure
  Infrastructure
  * Implementação de repositórios
  * Entity Framework Core
  * Configuração do banco de dados

### Presentation
  API
  * Controllers
  * Configuração da aplicação

### Frontend
  APP (Blazor)
  * Interface do usuário
  * Consumo da API

### Tests
  * Testes unitarios dos serviços de produto e pedido

## Modelagem
Produto
* Nome
* Preço
* Tipo de Produto (Sanduiche, Batata-frita, Refrigerante)
  
Pedido
* Subtotal
* Discount
* Total
  
Itens do Pedido
* Nome do produto
* Tipo do produto
* Preço no momento da compra

O preço é salvo no pedido para evitar inconsistências em relatórios futuros, pois como é possivel editar o preço de um produto, não faria sentido o resultado 
usar os valores atuais do produto sendo que na data que foi realizado o pedido, os valores eram diferentes.

O produto possui um enum para diferenciar se é um sanduiche ou refri ou batatas por exemplo, pois amarrar ID de um registro a uma funcionalidade não é ideal, pois se o registro 
for deletado do banco a funcionalidade se perde. Então o enum pode ser usado para aplicar corretamente as regras de desconto, até mesmo se mais produtos forem cadastrados.

## Tecnologias usadas:

* `Net 8.0` para desenvolvimento da API
* `PostgreSQL` para banco de dados
* `EntityFrameworkCore` como ORM
* `Blazor Wasm` como Framework para desenvolvimento frontend
* `Bootstrap 5` para estilização do frontend
* `Swagger` para documentação de API
* `xUnit` + `Moq` (testes unitários)

## Configuração do projeto
* No appsettings.Development.json configurar a string de conexão com banco de dados POSTGRESQL:
  
``` "DatabaseConfig": { "ConnectionString": "Server = 127.0.0.1; Database = GoodHamburgerDB; Port = 5432; User Id = postgres; Password = 12345678; Pooling = true;" } ```

* Na pasta GoodHamburger.Infrastructure abrir o console e rodar o seguinte comando para aplicar as migrations:

~~~c#
dotnet ef migration update
~~~

**obs: a migration irá criar tanto as tabelas como salvar alguns produtos por padrão.**

* Nas pasta GoodHamburger.API abrir o console e rodar o seguinte comando para iniciar o backend:

~~~c#
dotnet run
~~~

* Nas pasta GoodHamburger.APP abrir o console e rodar o seguinte comando para iniciar o frontend:

~~~c#
dotnet run
~~~

O frontend iniciará na porta http://localhost:5097

O backend iniciará na porta http://localhost:5001

Para acessar o swagger basta acessar http://localhost:5002/swagger
