# Sistema de Gerenciamento de Cursos — Full Stack

Aplicação web desenvolvida para Avalição do curso de programação para praticar a construção de uma aplicação Full Stack. O sistema permite cadastrar, consultar, editar e excluir cursos, integrando uma interface Angular a uma API REST em C# e a um banco de dados SQL Server.

## Tecnologias utilizadas

- **Backend:** C#, ASP.NET Web API (.NET Framework 4.8)
- **Frontend:** Angular, TypeScript, HTML e CSS
- **Banco de dados:** SQL Server

## Funcionalidades

- Cadastro de cursos.
- Listagem e consulta de cursos por identificador.
- Edição de cursos existentes.
- Exclusão de cursos, respeitando as regras de negócio.
- Validação dos dados recebidos pela API.
- Tratamento de exceções e registro de erros em log.
- Integração entre frontend, API REST e banco de dados.

## API REST

A rota base utilizada pelo projeto é `/api/Cursos`.

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/Cursos` | Lista os cursos. |
| `GET` | `/api/Cursos/{id}` | Consulta um curso pelo identificador. |
| `POST` | `/api/Cursos` | Cadastra um curso. |
| `PUT` | `/api/Cursos/{id}` | Atualiza um curso existente. |
| `DELETE` | `/api/Cursos/{id}` | Exclui um curso, quando permitido pelas regras de negócio. |

A API utiliza respostas HTTP para indicar os resultados das operações, incluindo `201 Created`, `404 Not Found`, `204 No Content` e `409 Conflict`, conforme o caso.

## Regras de negócio e decisões técnicas

- **Validação da data de início:** no cadastro, a data de início não pode estar no passado. A validação personalizada é executada em `CursoController.PostAsync`; a operação de edição (`PutAsync`) não repete essa verificação.
- **Exclusão de cursos ativos:** antes de excluir, `CursoRepository.DeleteAsync` consulta o estado do curso no banco de dados. Se estiver ativo, lança a exceção personalizada `CursoAtivoException`. O controller trata essa exceção e retorna `409 Conflict` com uma mensagem apropriada. A regra fica no Repository porque depende do estado atual do registro no banco.
- **Restrições no banco:** a tabela aplica validações ao nome (entre 3 e 150 caracteres), à carga horária (maior que zero) e ao valor (maior ou igual a zero), além de usar `NOT NULL` nos campos obrigatórios.
- **Tratamento de erros:** exceções inesperadas, incluindo erros do banco de dados, são registradas por `Logger.Log.WriteAsync`. A API retorna um erro interno sem expor detalhes da infraestrutura ao cliente.
- **CORS:** habilitado em `App_Start/WebApiConfig.cs` para permitir que o frontend Angular, executado em outra origem durante o desenvolvimento, consuma a API. Essa configuração é voltada ao desenvolvimento; em produção, as origens permitidas devem ser restritas conforme o ambiente.
- **Configuração:** a connection string e o caminho do arquivo de log são lidos do `Web.config` por meio da classe `Configurations.WebConfig`, em vez de ficarem definidos diretamente no código.
- **Formato JSON:** a API retorna dados em JSON e configura os nomes das propriedades em camelCase.

## Banco de dados

O script de criação está em [`Script-BD/escoladb.sql`](Script-BD/escoladb.sql). Ele cria o banco `escoladb`, a tabela `curso`, as restrições de validação e 10 registros fictícios para facilitar os testes, incluindo cursos ativos e inativos, online e presenciais.

As datas dos dados de exemplo usam o formato ISO, por exemplo `2026-09-10T19:00:00`, para evitar ambiguidades de interpretação regional no SQL Server.

## Como executar localmente

### Pré-requisitos

- Windows.
- Visual Studio com suporte a ASP.NET e .NET Framework 4.8.
- SQL Server (por exemplo, SQL Server Express) e uma ferramenta para executar scripts SQL, como SQL Server Management Studio (SSMS).
- Node.js e npm para instalar e executar o frontend Angular.

### 1. Criar o banco de dados

1. Abra `Script-BD/escoladb.sql` no SSMS ou em outra ferramenta compatível.
2. Execute o script para criar o banco `escoladb`, a tabela `curso` e os registros de exemplo.

### 2. Configurar e executar a API

1. Abra no Visual Studio o projeto `web-api-cursos/web-api-cursos/web-api-cursos.csproj`.
2. No arquivo `web-api-cursos/web-api-cursos/Web.config`, confira a connection string chamada `curso` e ajuste-a se sua instância do SQL Server for diferente. A configuração local de exemplo é:

   ```xml
   <add name="curso" connectionString="Server=.\SQLEXPRESS;Database=escoladb;Trusted_Connection=True;" />
   ```

3. Restaure os pacotes NuGet, se o Visual Studio não fizer isso automaticamente, e execute a API.

### 3. Configurar e executar o frontend

1. Abra um terminal na pasta `frontend`.
2. Instale as dependências:

   ```bash
   npm install
   ```

3. Inicie o servidor de desenvolvimento:

   ```bash
   npm start
   ```

4. Abra `http://localhost:4200` no navegador.

O frontend está configurado para consumir a API em `https://localhost:44339/api`. Se a API iniciar em outra URL ou porta, atualize o valor de `apiUrl` nos arquivos `frontend/src/environments/environment.ts` e `frontend/src/environments/environment.development.ts`.

## Observações

- Este é um projeto de estudo e portfólio. A connection string e as configurações de execução devem ser ajustadas ao ambiente local de quem for executar a aplicação.
- O script SQL contém dados fictícios para facilitar os testes.
