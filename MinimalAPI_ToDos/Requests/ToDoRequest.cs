namespace MinimalAPI_ToDos.Requests;

public static class ToDoRequest
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("/todos", ToDoRequest.GetAll)
            .Produces<List<ToDo>>();

        app.MapGet("/todos/{id}", ToDoRequest.GetById)
            .Produces<ToDo>(200)
            .Produces(404);

        app.MapPost("/todos", ToDoRequest.Create)
            .Produces<ToDo>(201)
            .Accepts<ToDo>("application/json");

        app.MapPut("/todos/{id}", ToDoRequest.Update)
        .Produces<ToDo>(200)
        .Produces(404)
        .Accepts<ToDo>("application/json");

        app.MapDelete("/todos/{id}", ToDoRequest.Delete)
        .Produces<ToDo>(204)
        .Produces(404);

        return app;
    }
    public static IResult GetAll(IToDoService service)
    {
        var toDos = service.GetAll();
        return Results.Ok(toDos);
    }

    public static IResult GetById(IToDoService service, Guid id)
    {
        var toDoThing = service.GetById(id);
        if (toDoThing == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(toDoThing);
    }

    public static IResult Create(IToDoService service, ToDo toDo)
    {
        service.Create(toDo);
        return Results.Created($"/todos/{toDo.Id}", toDo);
    }

    public static IResult Update(IToDoService service, Guid id, ToDo toDo)
    {
        var toDoThing = service.GetById(id);
        if (toDoThing == null)
        {
            return Results.NotFound();
        }
        service.Update(toDo);
        return Results.Ok(toDo);        
    }

    public static IResult Delete(IToDoService service, Guid id)
    {
        var toDoThing = service.GetById(id);
        if (toDoThing == null)
        {
            return Results.NotFound();
        }
        service.Delete(id);
        return Results.NoContent();       
    }
}
