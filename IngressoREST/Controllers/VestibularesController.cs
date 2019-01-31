using AutoMapper;
using IngressoREST.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IngressoREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VestibularesController : ControllerBase
    {
        public static List<VestibularModel> m_Repositorio = new List<VestibularModel>();
        private static int m_contador = 1;

        private readonly IMapper _mapper;


        public List<CandidatoModel> Candidatos
        {
            get
            {
                return CandidatosController.m_Repositorio;

            }
        }

        public VestibularesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera os vestibulares ativos
        /// </summary>
        /// <response code="200">Retorna a lista de vestibulares ativos</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<VestibularModel>> Get()
        {
            return Ok(m_Repositorio);
        }

        /// <summary>
        /// Recupera os dados de um Vestibular especifico
        /// </summary>
        /// <param name="id">Id do vestibular</param>
        /// <returns>Retorna os dados do Vestibular</returns>
        /// <response code="200">Retorna o vestibular</response>
        /// <response code="404">Se não encontrar o vestibular</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VestibularModel> Get(int id)
        {
            var vestibular = m_Repositorio.FirstOrDefault(a => a.Id == id);

            if (vestibular == null) return NotFound();

            return Ok(vestibular);
        }

        /// <summary>
        /// Recupera os candidatos inscritos no vestibular
        /// </summary>
        /// <response code="200">Retorna dos candidatos inscritos no vestibular</response>
        /// <response code="404">Se não encontrar o vestibular</response>
        [HttpGet("{idVestibular}/candidatos")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<CandidatoModel>> GetCandidatos(int idVestibular)
        {
            var vestibular = m_Repositorio.FirstOrDefault(a => a.Id == idVestibular);

            if (vestibular == null) return NotFound();

            return Ok(Candidatos.Where(a => a.IdVestibular == idVestibular));
        }


        /// <summary>
        /// Cria um vestibular
        /// </summary>
        /// <remarks>
        /// Exemplo request:
        ///
        ///     POST /Vestibulares
        ///     {
        ///       "descricao": "Vestibular Geral UNIC 01/02",
        ///       "dataInicioInscricao": "2019-01-10",
        ///       "dataTerminoInscricao": "2019-01-31",
        ///       "dataProva": "2019-02-01"
        ///     }
        ///
        /// </remarks>
        /// <param name="novoVestibular">Dados do vestibular</param>
        /// <returns>O vestibular criado</returns>
        /// <response code="201">Retorna o novo vestibular criado</response>
        /// <response code="400">Se os dados estiver inválido</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VestibularModel> Post([FromBody] NovoVestibularModel novoVestibular)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var vestibular = _mapper.Map<VestibularModel>(novoVestibular);
            vestibular.Id = m_contador++;
            m_Repositorio.Add(vestibular);

            return Created(nameof(Get), vestibular);
        }

        /// <summary>
        /// Altera os dados de um vestibular
        /// </summary>
        /// <param name="id">Id do vestibular que deseja alterar os dados</param>
        /// <param name="novoVestibular">Novos dados do vestibular</param>
        /// <returns>Retorna o novo vestibular alterado</returns>
        /// <response code="200">Retorna o vestibular alterado</response>
        /// <response code="400">Se o dados estiver inválido</response>
        /// <response code="404">Se não encontrar o vestibular</response>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VestibularModel> Put(int id, [FromBody] NovoVestibularModel novoVestibular)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var vestibular = m_Repositorio.FirstOrDefault(a => a.Id == id);

            if (vestibular == null) return NotFound();

            _mapper.Map(novoVestibular, vestibular);

            return Ok(vestibular);
        }

        /// <summary>
        /// Exclui um vestibular especifico
        /// </summary>
        /// <param name="id">Id do vestibular para ser excluído</param>
        /// <returns></returns>
        /// <response code="404">Se não encontrar o vestibular</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            var vestibular = m_Repositorio.FirstOrDefault(a => a.Id == id);

            if (vestibular == null) return NotFound();

            m_Repositorio.Remove(vestibular);

            return Ok();
        }
    }
}
