using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class KitapTuru
    {

        [Key]   //primery key
        public int Id { get; set; }
        [Required(ErrorMessage = "Kitap Tür Adı Boş Bırakılamaz!")] // not null
        [MaxLength(25)]
        
        public string Ad { get; set; }

    }
}
