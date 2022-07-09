namespace MinimalAPI_ToDos.Requests;

public static class ToDoRequest
{
    public static IResult GetAll(IToDoService service)
    {
        var toDos = service.GetAll();
        return Results.Ok(toDos);
    }

    public static IResult GetById(IToDoService service, Guid id)
    {
        var toDo = service.GetById(id);
        if (toDo == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(toDo);
    }

    public static IResult Create(IToDoService service, ToDo toDo)
    {
        service.Create(toDo);
        return Results.Created($"/todos/{toDo.Id}", toDo);
    }

    public static IResult Update(IToDoService service, Guid id, ToDo toDo)
    {
        var toDoOriginal = service.GetById(id);
        if (toDoOriginal == null)
        {
            return Results.NotFound();
        }
        service.Update(toDo);
        return Results.Ok(toDo);        
    }

    public static IResult Delete(IToDoService service, Guid id)
    {
        var toDo = service.GetById(id);
        if (toDo == null)
        {
            return Results.NotFound();
        }
        service.Delete(id);
        return Results.NoContent();       
    }
}
