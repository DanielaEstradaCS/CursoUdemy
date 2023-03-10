using Back.Domain.IServices;
using Back.Domain.Models;
using Back.DTO;
using Back.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            try
            {
                var validateExistence = await _usuarioService.ValidateExistence(usuario);
                if(validateExistence)
                {
                    return BadRequest(new { message = $"El usuario {usuario.NombreUsuario} ya existe" });
                }

                await _usuarioService.SaveUser(usuario);
                return Ok(new { message = "Usuario registrado con éxito" });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    
        // localhost:xxx/api/Usuario/CambiarPassword
        [Route("CambiarPassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordDTO cambiarPassword)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;

                int idUsuario = 2;
                string password = cambiarPassword.PasswordAnterior;
                var usuario = await _usuarioService.ValidatePassword(idUsuario, password);

                if(usuario == null )
                {
                    return BadRequest(new { message = "Contraseña incorrecta" });
                } else
                {
                    usuario.Password = cambiarPassword.NuevaPassword;
                    await _usuarioService.UpdatePassword(usuario);
                    return Ok(new { message = "La contraseña fue actualizada con éxito" });
                }
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
