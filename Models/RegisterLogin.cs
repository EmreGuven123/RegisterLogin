namespace registerLogin.Models
{
	public class RegisterLogin
	{
        public int Id { get; set; }
		public string Mail { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Password { get; set; }
		public int InvalidLoginCount { get; set; }
		public bool IsBlocked { get; set; }

	}
}
