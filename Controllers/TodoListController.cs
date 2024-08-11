using APITodoList.Interface;
using APITodoList.Models.Todo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APITodoList.Controllers
{
	[ApiController]
	[Route("api/[controller]/")]
	public class TodoListController : ControllerBase
	{

		private readonly ILogger<TodoListController> _logger;
		private readonly ITodoListService _todo;

		public TodoListController(ILogger<TodoListController> logger, ITodoListService todo)
		{
			_logger = logger;
			_todo = todo;
		}

		[HttpPost]
		[Route("Register")]
		public async Task<ActionResult> RegisterUser(Register req)
		{
			var res = await _todo.RegisterUser(req);
			return StatusCode(res.statusCode, res);
		}

		[HttpPost]
		[Route("Login")]
		public async Task<ActionResult> Login(LoginReq req)
		{
			var res = await _todo.Login(req);
			return StatusCode(res.statusCode, res);
		}

		[HttpPost]
		[Authorize]
		[Route("CreateTask")]
		public async Task<ActionResult> CreateTask(TaskListReq req)
		{
			var claims = HttpContext.User.Identity as ClaimsIdentity;
			string IdUser = claims.FindFirst("Id").Value;
			var res = await _todo.CreateTask(IdUser, req);
			return StatusCode(res.statusCode, res);
		}

		[HttpGet]
		[Authorize]
		[Route("UpdateStatus/{idTask}/{status}")]
		public async Task<ActionResult> UpdateStatus(string idTask,string status)
		{
			var claims = HttpContext.User.Identity as ClaimsIdentity;
			string IdUser = claims.FindFirst("Id").Value;
			var res = await _todo.UpdateStatus(IdUser,idTask, status);
			return StatusCode(res.statusCode, res);
		}

		[HttpPost]
		[Authorize]
		[Route("UpdateTask/{idTask}")]
		public async Task<ActionResult> UpdateTask(string idTask, [FromBody] TaskListReq body)
		{
			var claims = HttpContext.User.Identity as ClaimsIdentity;
			string IdUser = claims.FindFirst("Id").Value;
			var res = await _todo.UpdateTask(IdUser, idTask, body);
			return StatusCode(res.statusCode, res);
		}

		[HttpGet]
		[Authorize]
		[Route("ListTask")]
		public async Task<ActionResult> ListTask()
		{
			var claims = HttpContext.User.Identity as ClaimsIdentity;
			string IdUser = claims.FindFirst("Id").Value;
			var res = await _todo.ListTask(IdUser);
			return StatusCode(res.statusCode, res);
		}


	}
}
