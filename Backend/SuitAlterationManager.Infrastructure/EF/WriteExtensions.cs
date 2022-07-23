using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.EF
{
	public static class WriteExtensions
	{
		public static async Task Execute(this DbContext context, Func<Task> function)
		{			
			await using (var transaction = await context.Database.BeginTransactionAsync())
			{
				await function();

				await context.SaveChangesAsync();

				await transaction.CommitAsync();
			}
		}

		public static async Task<T> Execute<T>(this DbContext context, Func<Task<T>> function)
		{			
			T result;

			await using (var transaction = await context.Database.BeginTransactionAsync())
			{
				result = await function();

				await context.SaveChangesAsync();

				await transaction.CommitAsync();
			}

			return result;
		}

		public static async Task Execute(this DbContext context, Action function)
		{		
			await using (var transaction = await context.Database.BeginTransactionAsync())
			{
				function();

				await context.SaveChangesAsync();

				await transaction.CommitAsync();
			}
		}

		public static async Task<T> Execute<T>(this DbContext context, Func<T> function)
		{			
			T result;

			await using (var transaction = await context.Database.BeginTransactionAsync())
			{
				result = function();

				await context.SaveChangesAsync();

				await transaction.CommitAsync();
			}

			return result;
		}
	}
}
