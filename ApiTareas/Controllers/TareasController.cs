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
    public async Task<IActionResult> GetAll()
    {
        var tareas = await _context.Tareas.ToListAsync();
        return Ok(tareas);
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