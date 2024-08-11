using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace APITodoList.SUPABASE.Models
{
    [Table("TodoListTask")]
    public class TodoListTask : BaseModel
    {
        [PrimaryKey("TaskId", false)]
        public string TaskId { get; set; }

		[Column("Name")]
		public string? Name { get; set; }

		[Column("Description")]
		public string? Description { get; set; }

		[Column("Date")]
		public string? Date { get; set; }

		[Column("Status")]
		public string? Status { get; set; }

		[Column("Priority")]
		public bool? Priority { get; set; }

		[Column("UserId")]
		public string? UserId { get; set; }

		[Column("CreatedAt")]
		public DateTime? CreatedAt { get; set; }

		[Column("UpdatedAt")]
		public DateTime? UpdatedAt { get; set; }

	}
}
