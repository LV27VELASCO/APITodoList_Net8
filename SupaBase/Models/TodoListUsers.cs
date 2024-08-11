using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace APITodoList.SUPABASE.Models
{
	[Table("TodoListUsers")]
	public class TodoListUsers : BaseModel
	{
		[PrimaryKey("Id", false)]
		public string Id { get; set; }

		[Column("UserName")]
		public string UserName { get; set; }

		[Column("Name")]
		public string? Name { get; set; }

		[Column("Password")]
		public string? Password { get; set; }

		[Column("CreatedAt")]
		public DateTime? createdAt { get; set; }

		[Column("UpdatedAt")]
		public DateTime? UpdatedAt { get; set; }


	}
}
