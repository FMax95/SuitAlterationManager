using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace SuitAlterationManager.Infrastructure.EF
{
	public static class PropertyBuilderExtentions
	{
		public static PropertyBuilder<TEnum> HasEnumToStringConversion<TEnum>(this PropertyBuilder<TEnum> builder) where TEnum : struct =>
			builder.HasConversion(
				value => value.ToString(),
				value => Enum.IsDefined(typeof(TEnum), value)
					? (TEnum)Enum.Parse(typeof(TEnum), value)
					: default);

		public static PropertyBuilder<TEnum?> HasNullableEnumToStringConversion<TEnum>(this PropertyBuilder<TEnum?> builder) where TEnum : struct =>
			builder.HasConversion(
				value => value.ToString(),
				value => Enum.IsDefined(typeof(TEnum), value)
					? (TEnum)Enum.Parse(typeof(TEnum), value)
					: default);
	}
}
