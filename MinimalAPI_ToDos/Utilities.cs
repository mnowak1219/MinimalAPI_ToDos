using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public static class Utilities
{
    public static RouteHandlerBuilder WithValidator<T>(this RouteHandlerBuilder routeHandlerBuilder) where T : class
    {
        routeHandlerBuilder.Add(endpointBuilder =>
        {
            var originalDelegate = endpointBuilder.RequestDelegate;

            endpointBuilder.RequestDelegate = async httpContext =>
            {
                var validator = httpContext.RequestServices.GetRequiredService<IValidator<T>>();

                httpContext.Request.EnableBuffering();

                var body = await httpContext.Request.ReadFromJsonAsync<T>();
                if (body == null)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsync("Couldn't map body to request model");
                    return;
                }
                var validationResult = validator.Validate(body);

                if (!validationResult.IsValid)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync(validationResult.Errors);
                    return;
                }

                httpContext.Request.Body.Position = 0;
                await originalDelegate(httpContext);
            };
        });
        return routeHandlerBuilder;
    }

    public static WebApplication RegisterToDoEndpoints(this WebApplication app)
    {
        app.MapGet("/todos", ToDoRequest.GetAll)
            .Produces<List<ToDo>>(StatusCodes.Status200OK)
            .WithTags("To Dos")
            .AllowAnonymous();

        app.MapGet("/todos/{id}", ToDoRequest.GetById)
            .Produces<ToDo>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("To Dos");

        app.MapPost("/todos", ToDoRequest.Create)
            .Produces<ToDo>(StatusCodes.Status201Created)
            .Accepts<ToDo>("application/json")
            .WithTags("To Dos")
            .WithValidator<ToDo>();

        app.MapPut("/todos/{id}", ToDoRequest.Update)
            .Produces<ToDo>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Accepts<ToDo>("application/json")
            .WithTags("To Dos")
            .WithValidator<ToDo>();

        app.MapDelete("/todos/{id}", ToDoRequest.Delete)
            .Produces<ToDo>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("To Dos")
            .ExcludeFromDescription()
            .RequireAuthorization(/*optional policy*/);

        return app;
    }

    public static WebApplication RegisterAccountEndpoints(this WebApplication app)
    {
        app.MapGet("/account/token", AccountRequest.GetToken)
            .Produces<string>(StatusCodes.Status200OK)
            .WithTags("Account");

        return app;
    }
}
