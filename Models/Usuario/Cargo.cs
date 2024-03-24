using KaibaSystem_Back_End.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace KaibaSystem_Back_End.Models.Usuario
{
    [Table("Cargos")]
    public class Cargo : Entity
    {
        public String Nome { get; set; }
    }
}