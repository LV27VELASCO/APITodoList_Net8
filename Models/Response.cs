namespace APITodoList.Models
{
	public class Response<T>
	{
		public int statusCode {  get; set; }
		public string message { get; set; }
		public T info { get; set; }
	}
}	
