using System.Text;
using XSystem.Security.Cryptography;

namespace Utils
{
	public interface IUtilities
	{
		bool ValidatePassword(string reqPassword, string passwordStore);
		string HashPassword(string password);
	}
	public class Utilities : IUtilities
	{
		public bool ValidatePassword(string reqPassword, string passwordStore)
		{
			string pass = HashPassword(reqPassword);
			return pass == passwordStore;
		}
		public string HashPassword(string password)
		{
				using (var sha256 = new SHA256Managed())
				{
					byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

					// Hash the concatenated password and salt
					byte[] hashedPass = sha256.ComputeHash(passwordBytes);
					return Convert.ToBase64String(hashedPass);
				}
		}
	}
}
