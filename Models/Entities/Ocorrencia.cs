using System.ComponentModel.DataAnnotations.Schema;

namespace KaibaSystem_Back_End.Models.Entities
{
    [Table("OcorrenciaLog")]
    public class Ocorrencia
    {
        public Int32 ID { get; set; }
        public Int32 IDErro { get; set; }
        public String Aplicacao { get; set; }
        public DateTime Data { get; set; }
    }
}