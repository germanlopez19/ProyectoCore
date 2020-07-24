using Aplicacion.ManejadorError;
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
    public class RolEliminar
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
        }


        public class EjecutaValida : AbstractValidator<Ejecuta>
        {
            public EjecutaValida()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> _rolManager;
            public Manejador (RoleManager<IdentityRole> rolManager)
            {
                _rolManager = rolManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var role = await _rolManager.FindByNameAsync(request.Nombre);
                if (role == null)
                {
                    throw new ManejadorException(HttpStatusCode.BadRequest, new { mensaje = "No existe rol" });
                }
                var resultado = await _rolManager.DeleteAsync(role);
                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new SystemException("No se pudo eliminar el rol");
            }
        }
    }
}
