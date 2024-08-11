using APITodoList.Interface;
using APITodoList.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIEcomerce.Implements
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;

		public AuthService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public AuthResponse GenerateToken(AuthRequest req)
		{
			AuthResponse res = new AuthResponse();

			var claims = new List<Claim> {
			new Claim("Id", req.Id.ToString())};

			var jwtToken = new JwtSecurityToken(
					claims: claims,
					notBefore: DateTime.UtcNow,
					expires: DateTime.UtcNow.AddDays(2),
					signingCredentials: new SigningCredentials(
						new SymmetricSecurityKey(
						   Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Secret_Key"))),
						SecurityAlgorithms.HmacSha256Signature)
					);

			res.statusCode = 200;
			res.message = "Token Generado Exítosamente!";
			res.token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
			return res;
		}

	}
}
