using System.ComponentModel.DataAnnotations;

namespace ChallengeAndreani.Models
{
    public class SolicitudGeolocalizacion
    {
        [Required]
        public string calle { get; set; }
        [Required]
        public string numero { get; set; }
        [Required]
        public string ciudad { get; set; }
        [Required]
        public string codigo_postal { get; set; }
        [Required]
        public string provincia { get; set; }
        [Required]
        public string pais { get; set; }
    }
}