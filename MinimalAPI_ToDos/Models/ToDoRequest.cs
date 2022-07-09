namespace MinimalAPI_ToDos.Models;

public class ToDoRequest
{
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
