using APITodoList.Interface;
using APITodoList.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace APITodoList.Controllers
{
	[ApiController]
	[Route("api/[controller]/")]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IAuthService _authService;
		public AuthController(IConfiguration configuration, IAuthService authService)
		{
			_configuration = configuration;
			_authService = authService;
		}

		[HttpPost]
		[Route("GenerateToken")]
		public IActionResult GenerateToken(AuthRequest user)
		{
			AuthResponse res = _authService.GenerateToken(user);
			return StatusCode(res.statusCode, res);
		}
	}
}
