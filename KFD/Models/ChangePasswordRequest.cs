using System.ComponentModel.DataAnnotations;

namespace KFD.Models
{
    public class ChangePasswordRequest
    {
        // Nota: Este UserId podría ser redundante si siempre obtienes el usuario autenticado del HttpContext.User
        // Pero lo mantenemos si hay un caso de uso donde un admin cambie la de otro, o para consistencia.
        [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y un máximo de {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confirmación de la contraseña no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
