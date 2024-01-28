namespace SwiftChat.Models.Configurations
{
	public class GmailEmailSettings
	{
		public string? ClientId => Environment.GetEnvironmentVariable("SWIFTCHAT_GCLIENT_ID");
		public string? ClientSecret => Environment.GetEnvironmentVariable("SWIFTCHAT_GCLIENT_SECRET");
		public string[]? Scopes { get; set; }
		public string? ApplicationName { get; set; }
	}
}
