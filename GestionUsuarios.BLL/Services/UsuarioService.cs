using GestionUsuarios.BLL.DTOs;
using GestionUsuarios.DAL.Entities;
using GestionUsuarios.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUsuarios.BLL.Services
{
    public interface IUsuarioService
    {
        List<UsuarioDto> ObtenerCandidatos();
        List<UsuarioDto> ObtenerTodos();
        bool AsignarItem(string username, ItemResumenDto itemDto);
        bool CompletarItem(string username, Guid itemId);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public List<UsuarioDto> ObtenerCandidatos()
        {
            return _usuarioRepository.ObtenerTodos()
                .Where(u => !u.EstaSaturado) // Excluye usuarios saturados
                .Select(MapearADto)
                .ToList();
        }

        public List<UsuarioDto> ObtenerTodos()
        {
            return _usuarioRepository.ObtenerTodos().Select(MapearADto).ToList();
        }

        public bool AsignarItem(string username, ItemResumenDto itemDto)
        {
            var usuario = _usuarioRepository.ObtenerPorUsername(username);
            if (usuario == null || usuario.EstaSaturado) return false;

            // Mapear DTO a Entidad
            var itemEntidad = new ItemResumen
            {
                ItemId = itemDto.ItemId,
                Titulo = itemDto.Titulo,
                FechaEntrega = itemDto.FechaEntrega,
                EsAltaRelevancia = itemDto.EsAltaRelevancia
            };

            usuario.ItemsPendientes.Add(itemEntidad);

            // Regla: Ordenar lista de pendientes del usuario después de la asignación
            usuario.ReordenarPendientes();

            return true;
        }

        public bool CompletarItem(string username, Guid itemId)
        {
            var usuario = _usuarioRepository.ObtenerPorUsername(username);
            if (usuario == null) return false;

            var item = usuario.ItemsPendientes.FirstOrDefault(i => i.ItemId == itemId);
            if (item == null) return false;

            usuario.ItemsPendientes.Remove(item);
            usuario.CantidadCompletados++;

            return true;
        }
        private static UsuarioDto MapearADto(Usuario u) => new UsuarioDto
        {
            Username = u.Username,
            TotalPendientes = u.TotalPendientes,
            CantidadAltaRelevancia = u.CantidadAltaRelevancia,
            CantidadCompletados = u.CantidadCompletados,
            EstaSaturado = u.EstaSaturado,
            ItemsPendientes = u.ItemsPendientes.Select(i => new ItemResumenDto
            {
                ItemId = i.ItemId,
                Titulo = i.Titulo,
                FechaEntrega = i.FechaEntrega,
                EsAltaRelevancia = i.EsAltaRelevancia
            }).ToList()
        };
    }
}
