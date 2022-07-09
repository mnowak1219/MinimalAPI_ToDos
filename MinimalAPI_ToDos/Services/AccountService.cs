namespace MinimalAPI_ToDos.Services;
public interface IAccountService
{
    string GetToken();
}

public class AccountService : IAccountService
{
    private readonly AuthenticationSettings _authenticationSettings;
    public AccountService(AuthenticationSettings authenticationSettings)
    {
        _authenticationSettings = authenticationSettings;
    }
    public string GetToken()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "user-id"),
            new Claim(ClaimTypes.Name, "Test Name"),
            new Claim(ClaimTypes.Role, "Admin"),
        };

        var token = new JwtSecurityToken
        (
            issuer: _authenticationSettings.JwtIssuer,
            audience: _authenticationSettings.JwtIssuer,
            claims: claims,
            expires: DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)),
                SecurityAlgorithms.HmacSha256)
        );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return jwtToken;
    }
}