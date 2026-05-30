using Microsoft.EntityFrameworkCore;
using ApiTareas.Models;

namespace ApiTareas.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tarea> Tareas { get; set; }
}