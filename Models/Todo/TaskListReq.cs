namespace APITodoList.Models.Todo
{
	public class TaskListReq
	{
			public string? Name { get; set; }

			public string? Description { get; set; }

			public string? Status { get; set; }

			public bool? Priority { get; set; } = false;
			public string? Date { get; set; }

	}
}
