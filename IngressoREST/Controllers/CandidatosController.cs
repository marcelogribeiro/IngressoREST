using AutoMapper;
using IngressoREST.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IngressoREST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatosController : ControllerBase
    {
        public static List<CandidatoModel> m_Repositorio = new List<CandidatoModel>();
        private static int m_contador = 1;

        private readonly IMapper _mapper;

        public List<VestibularModel> Vestibulares
        {
            get
            {
                return VestibularesController.m_Repositorio;

            }
        }

        public CandidatosController(IMapper mapper)
        {
            _mapper = mapper;

        }

        //// GET: api/Candidatos
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public ActionResult<List<CandidatoModel>> Get()
        //{
        //    return Ok(m_Repositorio);
        //}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CandidatoModel> GetByVestibular([FromQuery] int? idVestibular)
        {
            var candidatos = idVestibular.HasValue ? m_Repositorio.Where(a => a.IdVestibular == idVestibular) : m_Repositorio;

            return Ok(candidatos);
        }

        // GET: api/Candidatos/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CandidatoModel> Get(int id)
        {
            var candidato = m_Repositorio.FirstOrDefault(a => a.Id == id);

            if (candidato == null) return NotFound();

            return Ok(candidato);
        }

        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CandidatoModel> GetByCPF(string cpf)
        {
            var candidato = m_Repositorio.FirstOrDefault(a => a.Cpf == cpf);

            if (candidato == null) return NotFound();

            return Ok(candidato);
        }

        // POST: api/Candidatos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<CandidatoModel> Post([FromBody] NovoCandidatoModel novoCandidato)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var vestibular = Vestibulares.FirstOrDefault(v => v.Id == novoCandidato.IdVestibular);
            if (vestibular == null)
                return Conflict($"Vestibular id {novoCandidato.IdVestibular} não encontrado.");
            if (vestibular.DataInicioInscricao > DateTime.Now || vestibular.DataTerminoInscricao < DateTime.Now)
                return Conflict($"Vestibular fora do periodo de inscrição de {vestibular.DataInicioInscricao} à {vestibular.DataTerminoInscricao}");
            if (m_Repositorio.Any(a => a.Cpf == novoCandidato.Cpf && a.IdVestibular == novoCandidato.IdVestibular))
                return Conflict($"Já existe uma inscrição para o cpf {novoCandidato.Cpf} no vestibular {novoCandidato.IdVestibular}");

            var candidato = _mapper.Map<CandidatoModel>(novoCandidato);
            candidato.Id = m_contador++;
            candidato.DataInscricao = DateTime.Now;
            m_Repositorio.Add(candidato);

            return Created(nameof(Get), candidato);
        }

        // PUT: api/Candidatos/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CandidatoModel> Put(int id, [FromBody] NovoCandidatoModel novoCandidato)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var candidato = m_Repositorio.FirstOrDefault(a => a.Id == id);

            if (candidato == null) return NotFound();

            var vestibular = Vestibulares.FirstOrDefault(v => v.Id == novoCandidato.IdVestibular);
            if (vestibular == null)
                return Conflict($"Vestibular id {novoCandidato.IdVestibular} não encontrado.");
            if (vestibular.DataInicioInscricao > DateTime.Now || vestibular.DataTerminoInscricao < DateTime.Now)
                return Conflict($"Vestibular fora do periodo de inscrição de {vestibular.DataInicioInscricao} à {vestibular.DataTerminoInscricao}");
            if (m_Repositorio.Any(a => a.Id != id && a.Cpf == novoCandidato.Cpf && a.IdVestibular == novoCandidato.IdVestibular))
                return Conflict($"Já existe uma inscrição para o cpf {novoCandidato.Cpf} no vestibular {novoCandidato.IdVestibular}");


            _mapper.Map(novoCandidato, candidato);

            return Ok(candidato);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            var candidato = m_Repositorio.FirstOrDefault(a => a.Id == id);

            if (candidato == null) return NotFound();

            m_Repositorio.Remove(candidato);

            return Ok();
        }
    }
}
