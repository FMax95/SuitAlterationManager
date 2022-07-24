using SuitAlterationManager.Domain.AlterationManagement;
using SuitAlterationManager.Domain.AlterationManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuitAlterationManager.Domain.Tests.RetailManagement.Builders
{
	public class AlterationBuilder
	{
		AlterationStatus? status = null;
		string customerEmail = "customer@mail.it";
		int measure = -5;
		AlterationType alterationType = AlterationType.ShortenSleeves;
		AlterationTypeDirection direction = AlterationTypeDirection.Left;

		public AlterationBuilder WithStatus(AlterationStatus value)
		{
			status = value;
			return this;
		}

		public AlterationBuilder WithEmail(string value)
		{
			customerEmail = value;
			return this;
		}
		public AlterationBuilder WithMeasure(int value)
		{
			measure = value;
			return this;
		}
		public AlterationBuilder WithDirection(AlterationTypeDirection value)
		{
			direction = value;
			return this;
		}
		public AlterationBuilder WithAlterationType(AlterationType value)
		{
			alterationType = value;
			return this;
		}

		public static implicit operator Alteration(AlterationBuilder builder)
		{
			var entity = Alteration.Create(customerEmail: builder.customerEmail,
											alterationType :builder.alterationType,
											alterationTypeDirection:builder.direction,
											measure:builder.measure);

			if (builder.status != null)
				entity.Status = builder.status.Value;

			return entity;
		}
	}
}
