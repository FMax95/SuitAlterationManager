using System;
using System.Collections.Generic;
using System.Text;

namespace SuitAlterationManager.Infrastructure.Auth
{
	public class AuthOptions
	{
		public string Type { get; set; }
		public string Secret { get; set; }
		public short TokenMinutesLifetime { get; set; }
		public short RefreshTokenDaysLifetime { get; set; }
		public short RefreshTokenDaysTTL { get; set; }
		public int ResetTokenMinutesLifetime { get; set; }
		public int VerificationTokenMinutesLifetime { get; set; }
	}
}
