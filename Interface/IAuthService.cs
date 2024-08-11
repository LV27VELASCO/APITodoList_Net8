using APITodoList.Models.Auth;

namespace APITodoList.Interface
{
	public interface IAuthService
	{
		AuthResponse GenerateToken(AuthRequest req);
	}
}
