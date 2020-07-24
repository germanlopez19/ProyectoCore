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
    public class RolNuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
        }

        public class ValidaEjecuta : AbstractValidator<Ejecuta>
        {
            public ValidaEjecuta()
            {
                RuleFor(x => x.Nombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly RoleManager<IdentityRole> _rolManager;
            public Manejador(RoleManager<IdentityRole> rolManager)
            {
                _rolManager = rolManager;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await _rolManager.FindByNameAsync(request.Nombre);
                if (role != null)
                {
                    throw new ManejadorException(HttpStatusCode.BadRequest, new { mensaje = "Ya existe el rol" });
                }
                var resultado = await _rolManager.CreateAsync(new IdentityRole(request.Nombre)); 
                if(resultado.Succeeded)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo guardar el rol");
                    
            }
        }
    }
}
