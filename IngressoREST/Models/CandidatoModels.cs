using System;
using System.ComponentModel.DataAnnotations;

namespace IngressoREST.Models
{
    public class NovoCandidatoModel
    {
        [Required]
        public int IdVestibular { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cpf { get; set; }
    }

    public class CandidatoModel : NovoCandidatoModel
    {
        [Required]
        public int Id { get; set; }


        [Required]
        public DateTime DataInscricao { get; set; }
    }
}
