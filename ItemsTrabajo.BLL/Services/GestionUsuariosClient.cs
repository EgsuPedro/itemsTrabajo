using System.Net.Http.Json;
using ItemsTrabajo.BLL.DTOs;

namespace ItemsTrabajo.BLL.Services
{
    public interface IGestionUsuariosClient
    {
        Task<List<UsuarioCandidatoDto>> ObtenerUsuariosCandidatosAsync();
        Task<bool> NotificarAsignacionAsync(string username, Guid itemId, string titulo, DateTime fechaEntrega, bool esAltaRelevancia);
    }

    public class GestionUsuariosClient : IGestionUsuariosClient
    {
        private readonly HttpClient _httpClient;

        public GestionUsuariosClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UsuarioCandidatoDto>> ObtenerUsuariosCandidatosAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<UsuarioCandidatoDto>>("api/usuarios/candidatos");
            return response ?? new List<UsuarioCandidatoDto>();
        }

        public async Task<bool> NotificarAsignacionAsync(string username, Guid itemId, string titulo, DateTime fechaEntrega, bool esAltaRelevancia)
        {
            var body = new
            {
                ItemId = itemId,
                Titulo = titulo,
                FechaEntrega = fechaEntrega,
                EsAltaRelevancia = esAltaRelevancia
            };

            var response = await _httpClient.PostAsJsonAsync($"api/usuarios/{username}/asignar", body);
            return response.IsSuccessStatusCode;
        }
    }
}