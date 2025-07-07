using KFD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations; // Necesario para las validaciones
using System.Linq;

namespace KFD.Areas.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        #region API

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] ApplicationUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve errores de validación del DTO
            }

            var user = new ApplicationUser
            {
                UserName = model.Email, // Identity usa UserName para el login, a menudo es el email
                Email = model.Email,
                Name = model.Name,       // Asigna el nombre
                Address = model.Address, // Asigna la dirección
                IsEnabled = 1            // Establece el estado de habilitación por defecto
            };

            var result = await _userManager.CreateAsync(user, model.PasswordHash);

            if (result.Succeeded)
            {
                // Opcional: Asignar un rol por defecto, ej. "Usuario"
                await _userManager.AddToRoleAsync(user, Utilities.StaticValues.Role_Customer);

                return Ok(new { Message = "Registro de usuario exitoso." });
            }

            // Si el registro falló, extrae los errores de Identity y devuélvelos
            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { Errors = errors });
        }

        // Permite que el usuario autenticado obtenga su nombre, correo y dirección
        [HttpGet("getuserprofile/{id}")] // Plantilla de ruta modificada
        public async Task<IActionResult> GetUserProfile(string id) // Asegurarse de que el id no sea anulable si siempre se espera
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound(new { message = "Usuario no encontrado." }); 

            return Ok(new UserProfileDto
            {
                Email = user.Email,
                Name = user.Name,
                Address = user.Address
            });
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return Unauthorized(new { message = "Usuario no autenticado." });
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            if (changePasswordResult.Succeeded)
            {
                return Ok(new { message = "Contraseña cambiada exitosamente." });
            }
            else
            {
                
                var errors = changePasswordResult.Errors.Select(e => e.Description);
                return BadRequest(new { message = "Error al cambiar la contraseña.", errors = errors });
            }
        }

        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ApplicationUser model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return Unauthorized(new { message = "Usuario no autenticado." });
            }

            user.Name = model.Name;
            user.Address = model.Address;

            if (user.Email != model.Email)
            {
                if (user.UserName == user.Email)
                {
                    var setUserNameResult = await _userManager.SetUserNameAsync(user, model.Email);
                    if (!setUserNameResult.Succeeded)
                    {
                        var fail = setUserNameResult.Errors.Select(e => e.Description);
                        return BadRequest(new { message = "Error al actualizar el nombre de usuario (email).", errors = fail });
                    }
                }
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    var fail = setEmailResult.Errors.Select(e => e.Description);
                    return BadRequest(new { message = "Error al actualizar el correo electrónico.", errors = fail });
                }

            }


            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { message = "Perfil actualizado correctamente." });
            }

            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(new { message = "Error al actualizar el perfil.", errors = errors });
        }

        #endregion

    }

    

    public class UserProfileDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}