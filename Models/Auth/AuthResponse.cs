namespace APITodoList.Models.Auth
{
    public class AuthResponse
    {
		public int statusCode { get; set; }
		public string message { get; set; }
		public string? token { get; set; } = null;

    }
}
