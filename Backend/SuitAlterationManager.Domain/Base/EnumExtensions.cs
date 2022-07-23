using SuitAlterationManager.Domain.Base.Validation;
using System;

namespace SuitAlterationManager.Domain.Base
{
    public static class EnumExtensions
	{
		public static T ToEnum<T>(this string value) where T : struct
		{
			T result;
			if (string.IsNullOrEmpty(value) || !Enum.TryParse<T>(value, true, out result))
				throw new DomainException("InvalidEnum", $"The possible value for {typeof(T).Name} are {string.Join(",", Enum.GetNames(typeof(T)))}");

			return result;
		}
	}
}
