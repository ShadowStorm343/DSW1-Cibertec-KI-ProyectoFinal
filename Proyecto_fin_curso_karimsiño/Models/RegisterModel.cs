using System.ComponentModel.DataAnnotations;

namespace Proyecto_fin_curso_karimsiño.Models
{
    public class RegisterModel
    {
        // PASO 1:
        //Ya tenemos el model para el registro de usuarios,
        //ahora vamos a crear el model para el login de usuarios,
        //que es el mismo pero sin el correo electrónico.
        public int Id { get; set; }
        [Required(ErrorMessage ="Ingresa un nombre válido, por favor.")]
        [StringLength(50,ErrorMessage ="Es imposible que tus nombres superen los 100 caracteres.")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(20, ErrorMessage = "No existe rol que supere los 20 caracteres.")]
        public string rol { get; set; }
        [Required(ErrorMessage = "Ingresa una clave correcta.")]
        [DataType(DataType.Password)]
        [StringLength(100,MinimumLength =4, ErrorMessage ="La contraseña debe tener entre 4 y 100 carácteres.")]
        public string clave { get; set; }
        [EmailAddress(ErrorMessage ="Email inválido.")]
        public string correo { get; set; }
    }
}
