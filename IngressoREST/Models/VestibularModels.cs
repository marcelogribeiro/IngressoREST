using System;
using System.ComponentModel.DataAnnotations;

namespace IngressoREST.Models
{
    public class NovoVestibularModel
    {
        [Required]
        public string Descricao { get; set; }

        [Required]
        public DateTime DataInicioInscricao { get; set; }

        [Required]
        public DateTime DataTerminoInscricao { get; set; }

        [Required]
        public DateTime DataProva { get; set; }
    }

    public class VestibularModel : NovoVestibularModel
    {
        [Required]
        public int Id { get; set; }
    }
}
