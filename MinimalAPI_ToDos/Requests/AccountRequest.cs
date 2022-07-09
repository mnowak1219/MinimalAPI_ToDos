namespace MinimalAPI_ToDos.Requests;

public class AccountRequest
{
    public static IResult GetToken(IAccountService service)
    {
        var toDos = service.GetToken();
        
    return Results.Ok(toDos);
}
}
