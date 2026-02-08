using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MantenimientoTrabajadores.Models
{
    public class Trabajador
    {
        public int Id { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        public string TipoDocumento { get; set; }

        [Required]
        public string NumeroDocumento { get; set; }

        [Required]
        public string Sexo { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string? Foto { get; set; }

        [NotMapped]
        public IFormFile? FotoFile { get; set; }

        public string? Direccion { get; set; }
    }
}