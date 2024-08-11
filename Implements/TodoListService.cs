using APITodoList.Interface;
using APITodoList.Models;
using APITodoList.Models.Auth;
using APITodoList.Models.Todo;
using APITodoList.SUPABASE.Models;
using Utils;

namespace APITodoList.Implements
{
	public class TodoListService : ITodoListService
	{
		private readonly Supabase.Client _supabase;
		private readonly IAuthService _authService;
		private readonly IUtilities _util;

		public TodoListService(Supabase.Client supabase, IAuthService authService, IUtilities utilies)
		{
			_supabase = supabase;
			_authService = authService;
			_util = utilies;
		}

		public async Task<Response<LoginRes>> Login(LoginReq req)
		{
			Response<LoginRes> res = new Response<LoginRes>();
			var supabase = await _supabase.InitializeAsync();
			string userName = req.UserName.ToLower();
			string password = req.Password;
			var user = await supabase.From<TodoListUsers>().Where(x => x.UserName == userName).Get();
			if (user.Models.Count > 0)
			{
				if (_util.ValidatePassword(password, user.Model.Password))
				{
					AuthRequest reqAuth = new AuthRequest();
					reqAuth.Id = user.Model.Id;
					AuthResponse resToken = _authService.GenerateToken(reqAuth);
					if (resToken.statusCode == 200)
					{
						LoginRes loginUser = new LoginRes()
						{
							Name = user.Model.Name,
							Token = resToken.token
						};
						res.statusCode = 200;
						res.message = "Exitoso!";
						res.info = loginUser;
					}
					else
					{
						res.statusCode = 400;
						res.message = "No se pudo generar el token";
					}
					return res;
				}
			}
			res.statusCode = 400;
			res.message = "Nombre de usuario o contraseña inválida";

			return res;
		}

		public async Task<Response<bool>> RegisterUser(Register req)
		{
			Response<bool> res = new Response<bool>();

			if (await ValidateUser(req.UserName.ToLower()))
			{
				res.statusCode = 400;
				res.message = "Nombre de usuario ya existe";
			}
			else
			{
				// Hash the password with the salt
				string hashedPassword = _util.HashPassword(req.Password);

				TodoListUsers usr = new TodoListUsers()
				{
					Name = req.Name,
					UserName = req.UserName.ToLower(),
					Password = hashedPassword,
					createdAt = DateTime.Now,
					UpdatedAt = DateTime.Now
				};

				var supabase = await _supabase.InitializeAsync();
				await supabase.From<TodoListUsers>().Insert(usr);
				res.statusCode = 201;
				res.message = "Usuario Creado!";
				res.info = true;
			}

			return res;
		}
		public async Task<Response<bool>> CreateTask(string idUser, TaskListReq req)
		{
			Response<bool> res = new Response<bool>();
			string fecha = DateTime.Parse(req.Date).ToString("dd/MM/yyyy");
			TodoListTask tsk = new TodoListTask()
			{
				Name = req.Name,
				Description = req.Description,
				Date = fecha,
				UserId = idUser,
				Status = "P",
				Priority = req.Priority,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			};

			var supabase = await _supabase.InitializeAsync();
			await supabase.From<TodoListTask>().Insert(tsk);
			res.statusCode = 201;
			res.message = "Tarea Creada!";
			res.info = true;
			return res;
		}

		public async Task<Response<List<TaskListRes>>> ListTask(string idUser)
		{
			Response<List<TaskListRes>> res = new Response<List<TaskListRes>>();
			var supabase = await _supabase.InitializeAsync();
			var tasks = await supabase.From<TodoListTask>().Where(x => x.UserId == idUser).Get();
			List<TaskListRes> taskList = new List<TaskListRes>();

			if (tasks.Models.Count > 0)
			{
				foreach (var item in tasks.Models)
				{
					TaskListRes task = new TaskListRes();
					task.TaskId = item.TaskId;
					task.Name = item.Name;
					task.Description = item.Description;
					task.Date = item.Date;
					task.Priority = item.Priority;
					task.Status = item.Status;
					task.CreatedAt = item.CreatedAt;
					task.UpdatedAt = item.UpdatedAt;
					taskList.Add(task);
				}
			}
			res.statusCode = 200;
			res.message = "Exitoso";
			res.info = taskList;
			return res;
		}

		public async Task<Response<bool>> UpdateStatus(string idUser,string taskId, string status)
		{
			Response<bool> res = new Response<bool>();
			var supabase = await _supabase.InitializeAsync();
			
			var tasks = await supabase.From<TodoListTask>().Where(x => x.UserId == idUser && x.TaskId == taskId).Get();

			if (tasks.Models.Count>0)
			{
				tasks.Model.Status = status;
				await supabase.From<TodoListTask>()
					.Where(x=>x.TaskId == taskId)
					.Set(x=>x.Status,tasks.Model.Status)
					.Update();
				res.statusCode = 200;
				res.message = "Exito";
				res.info = true;
			}
			else
			{
				res.statusCode = 200;
				res.message = "Tarea no encontrada!";
				res.info = false;
			}
			return res;
		}

		public async Task<Response<bool>> UpdateTask(string idUser,string idTask, TaskListReq taskReq)
		{
			Response<bool> res = new Response<bool>();
			var supabase = await _supabase.InitializeAsync();
			var tasks = await supabase.From<TodoListTask>().Where(x => x.UserId == idUser && x.TaskId == idTask).Get();
			if (tasks.Models.Count > 0)
			{
				TodoListTask taskStore = tasks.Model;
				taskStore.Name = string.IsNullOrWhiteSpace(taskReq.Name) ? taskStore.Name : taskReq.Name.Trim();
				taskStore.Description = string.IsNullOrWhiteSpace(taskReq.Description) ? taskStore.Description : taskReq.Description.Trim();
				taskStore.Date = string.IsNullOrWhiteSpace(taskReq.Date) ? taskStore.Date : DateTime.Parse(taskReq.Date).ToString("dd/MM/yyyy"); ;
				taskStore.Status = string.IsNullOrWhiteSpace(taskReq.Status) ? taskStore.Status : taskReq.Status.Trim();
				taskStore.Priority = taskReq.Priority;
				taskStore.UpdatedAt = DateTime.Now;
				await supabase.From<TodoListTask>()
					.Where(x=>x.TaskId == idTask)
					.Set(x=>x.Name, taskStore.Name)
					.Set(x => x.Description, taskStore.Description)
					.Set(x => x.Date, taskStore.Date)
					.Set(x => x.Status, taskStore.Status)
					.Set(x => x.Priority, taskStore.Priority)
					.Set(x => x.UpdatedAt, taskStore.UpdatedAt)
					.Update();
				res.statusCode = 200;
				res.message = "Exito";
				res.info = false;
			}
			else
			{
				res.statusCode = 200;
				res.message = "Tarea no encontrada!";
				res.info = false;
			}
			return res;
		}

		public async Task<bool> ValidateUser(string userName)
		{
			var supabase = await _supabase.InitializeAsync();
			var datos = await supabase.From<TodoListUsers>()
				.Select(x => new object[] { x.UserName })
				.Where(x => x.UserName == userName)
				.Get();
			return datos.Models.Count > 0;
		}
	}
}
