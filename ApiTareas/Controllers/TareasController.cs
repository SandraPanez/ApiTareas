using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTareas.Data;
using ApiTareas.Models;

namespace ApiTareas.Controllers;

[ApiController]
[Route("api/tareas")]
public class TareasController : ControllerBase
{
    private readonly AppDbContext _context;

    public TareasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? estado,
        [FromQuery] string? prioridad,
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin)
    {
        if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
            return BadRequest("La fecha de inicio no puede ser mayor que la fecha fin.");

        if (estado != null && !Enum.TryParse<EstadoTarea>(estado, out _))
            return BadRequest("Estado no válido. Use: Pendiente, EnProceso, Completada.");

        if (prioridad != null && !Enum.TryParse<PrioridadTarea>(prioridad, out _))
            return BadRequest("Prioridad no válida. Use: Baja, Media, Alta.");

        var query = _context.Tareas.AsQueryable();

        if (estado != null)
        {
            var estadoEnum = Enum.Parse<EstadoTarea>(estado);
            query = query.Where(t => t.Estado == estadoEnum);
        }

        if (prioridad != null)
        {
            var prioridadEnum = Enum.Parse<PrioridadTarea>(prioridad);
            query = query.Where(t => t.Prioridad == prioridadEnum);
        }

        if (fechaInicio.HasValue)
            query = query.Where(t => t.FechaVencimiento >= fechaInicio.Value);

        if (fechaFin.HasValue)
            query = query.Where(t => t.FechaVencimiento <= fechaFin.Value);

        return Ok(await query.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea == null) return NotFound();
        return Ok(tarea);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Tarea tarea)
    {
        if (tarea.FechaVencimiento < DateTime.UtcNow)
            return BadRequest("La fecha de vencimiento no puede ser menor a la fecha actual.");

        tarea.FechaCreacion = DateTime.UtcNow;
        _context.Tareas.Add(tarea);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = tarea.Id }, tarea);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Tarea tarea)
    {
        if (id != tarea.Id) return BadRequest();

        if (tarea.FechaVencimiento < DateTime.UtcNow)
            return BadRequest("La fecha de vencimiento no puede ser menor a la fecha actual.");

        _context.Entry(tarea).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea == null) return NotFound();
        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}