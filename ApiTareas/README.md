# ApiTareas

API REST para gestionar tareas, desarrollada como evaluación continua N°3.

## Tecnologías
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core + SQLite
- ML.NET
- Swagger

## Endpoints

| Método | Ruta | Descripción |
|--------|------|-------------|
| GET | /api/tareas | Lista tareas (filtra por estado, prioridad, fechas) |
| GET | /api/tareas/{id} | Obtiene una tarea |
| POST | /api/tareas | Crea una tarea |
| PUT | /api/tareas/{id} | Actualiza una tarea |
| DELETE | /api/tareas/{id} | Elimina una tarea |
| GET | /api/tareas-externas | Lista tareas de jsonplaceholder |
| GET | /api/tareas-externas/{id} | Obtiene tarea externa por ID |
| POST | /api/ml/sentimiento | Analiza sentimiento de un comentario |

## API externa
Consume https://jsonplaceholder.typicode.com/todos y mapea la respuesta a un DTO propio.

## ML.NET
Modelo de clasificación binaria entrenado con frases en español para detectar si un comentario es positivo o negativo.

## Migraciones
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```