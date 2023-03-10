using Back.Domain.IRepositories;
using Back.Domain.Models;
using Back.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Persistence.Repositories
{
    public class RespuestaCuestionarioRepository : IRespuestaCuestionarioRepository
    {
        private readonly AplicationDbContext _context; 
        public RespuestaCuestionarioRepository(AplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RespuestaCuestionario> BuscarRespuestaCuestionario(int idRespuesta, int idUsuario)
        {
            var respuestaCuestionario = await _context.RespuestaCuestionario.Where(x => x.Id == idRespuesta &&
                                                                                   x.Cuestionario.UsuarioId == idUsuario &&
                                                                                   x.Activo == 1)
                                                                                   .FirstOrDefaultAsync();
            return respuestaCuestionario;                                                                    

        }

        public async Task EliminarRespuestaCuestionario(RespuestaCuestionario respuestaCuestionario)
        {
            respuestaCuestionario.Activo = 0;
            _context.Entry(respuestaCuestionario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<RespuestaCuestionario>> ListRespuestaCuestionario(int idCuestionario, int idUsuario)
        {
            var listRespuestaCuestionario = await _context.RespuestaCuestionario.Where(x => x.CuestionarioId == idCuestionario
                                                                                       && x.Activo == 1 && x.Cuestionario.UsuarioId == idUsuario)
                                                                                       .OrderByDescending(x => x.FechaCreacion)
                                                                                       .ToListAsync();
            return listRespuestaCuestionario;
        }

        public async Task SaveRespuestaCuestionario(RespuestaCuestionario RespuestaCuestionario)
        {
            RespuestaCuestionario.Activo = 1;
            RespuestaCuestionario.FechaCreacion = DateTime.Now;
            _context.Add(RespuestaCuestionario);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetIdCuestionarioByIdRespuesta(int IdCuestionario)
        {
            var cuestionario = await _context.RespuestaCuestionario.Where(x => x.Id == IdCuestionario 
                                                                          && x.Activo == 1).FirstOrDefaultAsync();

            return cuestionario.CuestionarioId;
        }

        public async Task<List<RespuestaCuestionarioDetalle>> GetListRespuestas(int idRespuestaCuestionario)
        {
            var listRespuestas = await _context.RespuestaCuestionarioDetalle.Where(x => x.RespuestaCuestionarioId == idRespuestaCuestionario)
                                                                                   .Select(x => new RespuestaCuestionarioDetalle
                                                                                   {
                                                                                       RespuestaId = x.RespuestaId
                                                                                   }).ToListAsync();
            return listRespuestas;
        }
    }
}
