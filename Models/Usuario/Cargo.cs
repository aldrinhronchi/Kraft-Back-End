using Kraft_Back_CS.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kraft_Back_CS.Models.Usuario
{
    [Table("Cargos")]
    public class Cargo : Entity
    {
        public String Nome { get; set; }
    }
}