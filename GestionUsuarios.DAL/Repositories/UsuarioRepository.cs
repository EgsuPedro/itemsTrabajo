using GestionUsuarios.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionUsuarios.DAL.Repositories
{
    public interface IUsuarioRepository
    {
        List<Usuario> ObtenerTodos();
        Usuario? ObtenerPorUsername(string username);
        void Guardar(Usuario usuario);
    }

    public class UsuarioRepository : IUsuarioRepository
    {
        // Almacenamiento en memoria con datos semilla para pruebas
        private static readonly List<Usuario> _usuarios = new()
        {
            new Usuario
            {
                Username = "UsuarioA",
                CantidadCompletados = 0,
                ItemsPendientes = new List<ItemResumen>
                {
                    new ItemResumen { ItemId = Guid.NewGuid(), Titulo = "Tarea A1", FechaEntrega = DateTime.Now.AddDays(5), EsAltaRelevancia = true },
                    new ItemResumen { ItemId = Guid.NewGuid(), Titulo = "Tarea A2", FechaEntrega = DateTime.Now.AddDays(10), EsAltaRelevancia = true },
                    new ItemResumen { ItemId = Guid.NewGuid(), Titulo = "Tarea A3", FechaEntrega = DateTime.Now.AddDays(4), EsAltaRelevancia = false }
                }
            },
            new Usuario
            {
                Username = "UsuarioB",
                CantidadCompletados = 0,
                ItemsPendientes = new List<ItemResumen>
                {
                    new ItemResumen { ItemId = Guid.NewGuid(), Titulo = "Tarea B1", FechaEntrega = DateTime.Now.AddDays(6), EsAltaRelevancia = false }
                }
            }
        };

        public List<Usuario> ObtenerTodos() => _usuarios;

        public Usuario? ObtenerPorUsername(string username) =>
            _usuarios.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

        public void Guardar(Usuario usuario)
        {
            var existente = ObtenerPorUsername(usuario.Username);
            if (existente == null)
            {
                _usuarios.Add(usuario);
            }
        }
    }
}
