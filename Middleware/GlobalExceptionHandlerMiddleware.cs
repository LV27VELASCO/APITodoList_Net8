using APITodoList.Models;

namespace Middleware
{
	public class GlobalExceptionHandlerMiddleware : IMiddleware
	{
		private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

		public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
		{
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				var requestTime = DateTime.Now;
				var requestMethod = context.Request.Method;
				var requestPath = context.Request.Path;
				_logger.LogInformation($" Request\n Time : {requestTime}\n Method Type :{requestMethod}\n  Path :{requestPath}");

				await next(context);

				var responseTime = DateTime.Now;
				var statusCode = context.Response.StatusCode;
				_logger.LogInformation($"Response \n Time : {responseTime} \n Status Code : {statusCode}");
			}
			catch (Exception ex)
			{
				Response<bool> response = new Response<bool>();
				response.statusCode = 500;
				response.message = string.Format("Ocurrió un error: {0}", ex.Message);
				response.info = false;
				context.Response.StatusCode = StatusCodes.Status500InternalServerError;
				await context.Response.WriteAsJsonAsync(response);
			}
		}
	}
}
