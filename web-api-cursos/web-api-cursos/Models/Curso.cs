using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace web_api_cursos.Models
{
    public class Curso
    {
        public int Id { get; set; }
        [Required(ErrorMessage =("O nome do curso é obrigatorio"))]
        [StringLength(150, MinimumLength =3, ErrorMessage =("O nome deve ter entre 3 e 150 caracteres"))]
        public string Nome { get; set; }
        [Range(1, int.MaxValue, ErrorMessage =("Carga Horaria deve ser maio que zero"))]
        public int CargaHoraria { get; set; }
        [Range(0, double.MaxValue,ErrorMessage =("O valor não pode ser negativo"))]
        public decimal Valor { get; set; }       
        public DateTime DataInicio { get; set; }
        public bool Online { get; set; }
        public bool Ativo { get; set; }

    }

    public class DataInicioFuturoAtribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime data)
                return data.Date >= DateTime.Today;

            return true;
        }
    }
}