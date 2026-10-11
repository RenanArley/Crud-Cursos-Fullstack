using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using web_api_cursos.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web.Http.Cors;
using System.Runtime.Caching;

namespace web_api_cursos.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CursosController : ApiController
    {
        private readonly Repository.SQLServerRepository.CursoRepository repository;
        private readonly Logger.Log logger;

        public CursosController()
        {
            repository = new Repository.SQLServerRepository.CursoRepository(Configurations.WebConfig.GetConnectionString());
            logger = new Logger.Log(HttpContext.Current.Server.MapPath(Configurations.WebConfig.GetLogFullPath()));
        }
        [HttpPost]
        public async Task<IHttpActionResult> PostAsync([FromBody] Models.Curso curso)
        {
            try
            {
                if (curso == null)
                    return BadRequest("Corpo da requisição vazio");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var ValidacaoData = new Models.DataInicioFuturoAtribute()
                    .GetValidationResult(curso.DataInicio, new ValidationContext(curso) { MemberName = nameof(Curso.DataInicio) });

                if (ValidacaoData != ValidationResult.Success)
                {
                    ModelState.AddModelError(nameof(Curso.DataInicio), ValidacaoData.ErrorMessage);
                    return BadRequest(ModelState);
                }

                return Content(HttpStatusCode.Created, await repository.CreateAsync(curso));
            }
            catch (Exception ex)
            {
                await logger.WriteAsync(ex);
                return InternalServerError();
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync()
        {
            try
            {                
                return Ok(await repository.ReadAsync());
            }
            catch (Exception ex)
            {
                await logger.WriteAsync(ex);
                return InternalServerError();
            }

        }
        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(int id)
        {
            try
            {
                Models.Curso curso = await repository.ReadAsync(id);

                if (curso == null)
                    return NotFound();
                return Ok(curso);
            }
            catch (Exception ex)
            {
                await logger.WriteAsync(ex);
                return InternalServerError();

            }
        }
        [HttpPut]
        public async Task<IHttpActionResult> PutAsync(int id, [FromBody] Curso curso)
        {
            try
            {
                if (curso == null)
                    return BadRequest("Os dados do curso não foram enviados!");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                if (id != curso.Id)
                    return BadRequest("Id da rota diferente do id do corpo da requisição");

                bool isOk = await repository.UpdateAsync(curso);
                    if (!isOk)
                    return NotFound();
                return Ok(curso);
            }
            catch (Exception ex)
            {
                await logger.WriteAsync(ex);
                return InternalServerError();
            }
        }
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(int id)
        {
            try
            {
                bool isOK = await repository.DeleteAsync(id);
                if (!isOK)
                    return NotFound();
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Repository.CursoAtivoException ex)
            {
                return Content(HttpStatusCode.Conflict, new { mensagem = ex.Message });
                
            }
            catch (Exception ex)
            {
                await logger.WriteAsync(ex);
                return InternalServerError();
            }

        }
    }
}
