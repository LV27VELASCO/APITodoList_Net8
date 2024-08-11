namespace APITodoList.Models.Todo
{
	public class TaskListRes
	{
		public string TaskId { get; set; }
		public string? Name { get; set; }

		public string? Description { get; set; }

		public string? Status { get; set; }

		public bool? Priority { get; set; } = false;
		public string? Date { get; set; }

		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

	}
}
