using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TO_Do_List.Model
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdB { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [ForeignKey("User")]
        public int IdUserFK { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede superar 150 caracteres.")]
        public string TitleB { get; set; }

        public string? DescriptionB { get; set; }

        [StringLength(40, ErrorMessage = "El estado no puede superar 40 caracteres.")]
        public string? StatusB { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        // Propiedad de navegación opcional
        // public User User { get; set; }
    }
}
