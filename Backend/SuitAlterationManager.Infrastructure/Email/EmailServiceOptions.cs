namespace SuitAlterationManager.Infrastructure.Email
{
	public class EmailServiceOptions
	{
		public string From { get; set; }
		public string FromDisplayName { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string CustomTemplatesFolder { get; set; }
		public string VirtualPlaceUrl { get; set; }
	}
}
