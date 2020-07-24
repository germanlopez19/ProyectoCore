using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolAgregar
    {
        public class Ejecuta : IRequest
        {
            public string Username { get; set; }
            public string RolNombre { get; set; }
        }
        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.RolNombre).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role =  await _roleManager.FindByNameAsync(request.RolNombre);
                if(role == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { mensaje = "El rol no existe" });
                }

                var usuarioIden = await _userManager.FindByNameAsync(request.Username);
                if (usuarioIden == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound, new { mensaje = "El usuario no existe" });
                }

                var resultado = await _userManager.AddToRoleAsync(usuarioIden, request.RolNombre);
                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new System.Exception("No se pudo agregar el Rol al Usuario"); 
            }
        }
    }
}
