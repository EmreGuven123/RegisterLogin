using System.ComponentModel.DataAnnotations;

namespace registerLogin.Models
{
	public class RegPassword
	{
		public int ID { get; set; }

		public string Password { get; set; }


		public void Test()
		{
			bool isValid = false;

			string text = null;
			
			text = isValid ? "Doğru" : "Hata";


			if (isValid)
			{
				text = "Doğru";
			}
			else
			{
				text = "Hata";
			}

		}
	}
}
