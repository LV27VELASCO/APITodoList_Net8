using APITodoList.Models;
using APITodoList.Models.Todo;

namespace APITodoList.Interface
{
	public interface ITodoListService
	{
		Task<Response<bool>> RegisterUser(Register req);
		Task<Response<LoginRes>> Login(LoginReq req);
		Task<Response<bool>> CreateTask(string idUser, TaskListReq req);
		Task<Response<List<TaskListRes>>> ListTask(string idUser);
		Task<Response<bool>> UpdateStatus(string idUser, string idTask, string status);
		Task<Response<bool>> UpdateTask(string idUser, string idTask, TaskListReq task);
	}
}
