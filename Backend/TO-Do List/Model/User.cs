using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TO_Do_List.Model
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdU { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(40, ErrorMessage = "El nombre no puede superar 40 caracteres.")]
        public string NameU { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [StringLength(40, ErrorMessage = "El email no puede superar 40 caracteres.")]
        [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
        public string EmailU { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(255, ErrorMessage = "La contraseña no puede superar 255 caracteres.")]
        public string PasswordU { get; set; }

        public DateTime DateU { get; set; }

        [StringLength(40, ErrorMessage = "El estado no puede superar 40 caracteres.")]
        public string? StatusU { get; set; }
    }
}
