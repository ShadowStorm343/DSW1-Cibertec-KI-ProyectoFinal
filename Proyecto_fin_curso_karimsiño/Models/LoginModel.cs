using System.ComponentModel.DataAnnotations;

namespace Proyecto_fin_curso_karimsiño.Models
{
    public class LoginModel
    {
        // PASO 2:
        // Creamos el LoginModel para recibir los datos del formulario de login,
        // que es el mismo que el RegistroModel pero sin el correo electrónico.
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingresa un nombre válido, por favor.")]
        [StringLength(50, ErrorMessage = "Es imposible que tus nombres superen los 100 caracteres.")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Ingresa una clave correcta.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "La contraseña debe tener entre 4 y 100 carácteres.")]
        public string clave { get; set; }
    }
}
