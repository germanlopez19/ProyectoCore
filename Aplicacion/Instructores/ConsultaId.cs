﻿using Aplicacion.ManejadorError;
using MediatR;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class Ejecuta : IRequest<InstructorModel>
        {
            public Guid Id { get; set; }
        }
        public class Manejador : IRequestHandler<Ejecuta, InstructorModel>
        {
            private readonly IInstructor _instructorRepository;
            public Manejador(IInstructor instructorRepository)
            {
                _instructorRepository = instructorRepository;
            }

            public async Task<InstructorModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructor = await _instructorRepository.ObtenerPorId(request.Id);
                if (instructor == null)
                {
                    throw new ManejadorException(HttpStatusCode.NotFound,
                        new { mensaje = "No se encontro el instructor" });
                }
                return instructor;
            }
        }

    }
}
