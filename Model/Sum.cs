using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Sum
    {
        [Key]
        public int Id { get; set; }

        public String nombre {get; set; }

        public int IdConsorcio { get; set; }
        [ForeignKey("IdConsorcio")]
        public virtual Consorcio Consorcio { get; set; }

        public virtual ICollection<ReservaSum> Reservas { get; set; }


    }
}
