using SuitAlterationManager.Domain.Base.Models;
using SuitAlterationManager.Domain.SystemManagement.ValueObjects;
using System;
using System.Security.Cryptography;

namespace SuitAlterationManager.Domain.SystemManagement
{
	public class RefreshToken : Entity<RefreshTokenID>
	{
		public virtual UserID IdUser { get; protected set; }
		public string Token { get; protected set; }
		public DateTimeOffset ExpirationDate { get; protected set; }
		public bool IsExpired => DateTimeOffset.UtcNow >= ExpirationDate;
		public DateTimeOffset CreationDate { get; protected set; }
		public string CreatedByIp { get; protected set; }
		public DateTimeOffset? RevocationDate { get; protected set; }
		public string RevokedByIp { get; protected set; }
		public string ReplacedByToken { get; protected set; }
		public bool IsActive => RevocationDate == null && !IsExpired;

		protected RefreshToken() { }

		internal static RefreshToken Create(UserID idUser, string ipAddress, DateTimeOffset creationDate, short refreshTokenDaysLifetime)
		{
			using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
			var randomBytes = new byte[64];
			rngCryptoServiceProvider.GetBytes(randomBytes);

			return new RefreshToken
			{
				Id = new RefreshTokenID(Guid.NewGuid()),
				IdUser = idUser,
				Token = Convert.ToBase64String(randomBytes),
				ExpirationDate = creationDate.AddDays(refreshTokenDaysLifetime),
				CreationDate = creationDate,
				CreatedByIp = ipAddress
			};
		}

		internal void Revoke(string newToken, string ipAddress, DateTimeOffset revocationDate)
		{
			RevocationDate = revocationDate;
			RevokedByIp = ipAddress;
			ReplacedByToken = newToken;
		}
	}
}
