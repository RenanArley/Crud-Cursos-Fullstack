# Decisões do projeto

- # Regra "DataInicio não pode ser no passado só na criação
  o atributo de validação customizado DataInicioFuturoAtribute
  Models/DataInicioFuturoAtribute.cs, herdando de ValidationAttribute,
  conforme exigido no item 4.1. diretamente na propriedade DataInicio do Curso,  
  validando apenas na criação, não na edição
  CursoController.PostAsync chama a validação manualmente
  com GetValidationResult, apenas nesse método. O PutAsync
  não realiza essa checagem, permitindo editar cursos com DataInicio
  antiga normalmente.
  
- # Regra não excluir curso ativo: implementada em
  `CursoRepository.DeleteAsync` (camada Repository), que primeiro
  consulta o campo `Ativo` do curso no banco antes de excluir. Se o
  curso estiver ativo, é lançada a exceção customizada
  `CursoAtivoException`. Optei por manter essa regra no Repository (e
  não no Controller) porque ela depende do estado atual do dado no
  banco — o Repository já precisa consultar o registro de qualquer
  forma antes de decidir se apaga. O `CursoController.DeleteAsync`
  captura especificamente essa exceção e retorna `409 Conflict` com
  uma mensagem amigável, sem repassar detalhes internos.

- **Tratamento de erros do banco**: exceções não previstas (`SqlException`
  e outras) são capturadas no `catch (Exception ex)` genérico do
  Controller, registradas via `Logger.Log.WriteAsync`, e retornam
  `InternalServerError()` sem detalhes de infraestrutura ao cliente.

- **CORS**: habilitado em `App_Start/WebApiConfig.cs` via
  `EnableCorsAttribute`, permitindo que o frontend Angular (rodando em
  outra porta) consuma a API.

- **Connection string e caminho de log**: configurados no `Web.config`
  (`connectionStrings` e `appSettings`), lidos através da classe
  `Configurations.WebConfig` — nunca hardcoded no código-fonte.

## Banco de dados

Script `.sql` disponível em `[caminho do seu script]`, contendo a
criação do banco `escoladb`, a tabela `curso` (com `CHECK`s para
`Nome`, `CargaHoraria` e `Valor`, e colunas `NOT NULL` onde aplicável)
e 10 registros de exemplo, variando cursos ativos/inativos e
online/presenciais.

> Observação: as datas de exemplo foram escritas no formato ISO
> completo (`AAAA-MM-DDTHH:MI:SS`), que é interpretado de forma
> consistente pelo SQL Server independentemente da configuração
> regional/idioma da instância — evitando ambiguidade entre
> ano-mês-dia e ano-dia-mês.

## Como executar

1. Executar o script `.sql` em uma instância local do SQL Server
   (SSMS ou similar).
2. Conferir/ajustar a connection string em `Web.config`
   (`connectionStrings > add name="curso"`) para apontar para sua
   instância.
3. Abrir a solução no Visual Studio (.NET Framework 4.8), restaurar
   os pacotes NuGet (já listados em `packages.config`:
   `Microsoft.AspNet.WebApi`, `Microsoft.AspNet.WebApi.Cors`,
   `Newtonsoft.Json`) e rodar via IIS Express.
