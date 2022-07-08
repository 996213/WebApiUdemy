﻿using System.ComponentModel.DataAnnotations;

namespace WebApiMoviesUdemy.Entidades
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }
    }
}
