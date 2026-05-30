using Microsoft.AspNetCore.Mvc;
using ApiTareas.Models;
using System.Text.Json;

namespace ApiTareas.Controllers;

[ApiController]
[Route("api/tareas-externas")]
public class TareasExternasController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public TareasExternasController(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos");
            if (!response.IsSuccessStatusCode)
                return StatusCode(502, "Error al conectar con la API externa.");

            var json = await response.Content.ReadAsStringAsync();
            var todos = JsonSerializer.Deserialize<List<JsonElement>>(json);

            var tareas = todos!.Select(t => new TareaExternaDto
            {
                ExternalId = t.GetProperty("id").GetInt32(),
                Titulo = t.GetProperty("title").GetString()!,
                Completado = t.GetProperty("completed").GetBoolean()
            }).ToList();

            return Ok(tareas);
        }
        catch
        {
            return StatusCode(502, "La API externa no está disponible.");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/todos/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound("Tarea externa no encontrada.");

            if (!response.IsSuccessStatusCode)
                return StatusCode(502, "Error al conectar con la API externa.");

            var json = await response.Content.ReadAsStringAsync();
            var t = JsonSerializer.Deserialize<JsonElement>(json);

            var tarea = new TareaExternaDto
            {
                ExternalId = t.GetProperty("id").GetInt32(),
                Titulo = t.GetProperty("title").GetString()!,
                Completado = t.GetProperty("completed").GetBoolean()
            };

            return Ok(tarea);
        }
        catch
        {
            return StatusCode(502, "La API externa no está disponible.");
        }
    }
}