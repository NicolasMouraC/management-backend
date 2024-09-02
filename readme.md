# Sistema para Gerenciamento de Faturas

## Visão Geral

Este sistema foi desenvolvido para facilitar o gerenciamento de faturas de clientes. O projeto utiliza Dotnet como motor principal. Há uso de containers Docker para facilitar a configuração de ambiente de desenvolvimento e produção. Postgres é usado como banco de dados persistente e Caddy é usado como provedor de servidor HTTPS.

## Instalação

1. **Clone o repositório:**

    ```bash
    git clone https://github.com/seu-usuario/seu-repositorio.git
    cd seu-repositorio
    ```

2. **Iniciar containers Docker:**

    Com [Docker](https://www.docker.com/):

    ```bash
    docker compose up --build -d
    ```

3. **Criar e aplicar migrações do banco de dados:**

    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

4. **Executar o projeto:**

    ```bash
    dotnet run
    ```
    

## Estrutura do Projeto

A estrutura padrão do projeto é a seguinte:

```
/
  ├── Controllers       # Controladores da aplicação
  ├── Data   
        └── ApplicationDbContext.cs    #  Configuração da conexão com banco de dados
  ├── Migrations        # Migração das colunas para banco de dados
  ├── Models            # Modelos da aplicação
        └── Dtos                       #  Modelos de transferência de dados
  └── Properties        # Propriedades da aplicação
```