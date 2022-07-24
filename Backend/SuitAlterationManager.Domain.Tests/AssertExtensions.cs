using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuitAlterationManager.Domain.Base.Validation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.Tests
{
    public static class AssertExtensions
	{
		public static async Task<TException> ThrowsWithCodeAsync<TException>(this Assert _, string code, Func<Task> func) where TException : Exception
		{
			var ex = await Assert.ThrowsExceptionAsync<TException>(func);

			Handle(ex, code);

			return ex;
		}

		public static TException ThrowsWithCode<TException>(this Assert _, string code, Action func) where TException : Exception
		{
			var ex = Assert.ThrowsException<TException>(func);

			Handle(ex, code);

			return ex;
		}

		private static void Handle<TException>(TException ex, string code) where TException : Exception
		{
			switch (ex)
			{
				case DomainException d:
					Assert.AreEqual(code, d.Code);
					break;
				default:
					Assert.Fail();
					break;
			}
		}

	}
}
