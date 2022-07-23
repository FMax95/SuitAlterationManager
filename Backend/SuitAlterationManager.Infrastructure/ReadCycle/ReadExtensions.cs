using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Execution;
using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SuitAlterationManager.Infrastructure.ReadCycle
{
	public static class ReadExtensions
	{
		public static TResult Read<TResult>(this IDbConnection connection, Func<IDbTransaction, TResult> func)
		{
			connection.Open();
			using var trans = connection.BeginTransaction();
			var result = func(trans);
			trans.Commit();

			return result;
		}

		public static TResult Read<TResult>(this QueryFactory qf, Func<IDbTransaction, TResult> func)
		{
			qf.Connection.Open();
			using var trans = qf.Connection.BeginTransaction();
			var result = func(trans);
			trans.Commit();

			return result;
		}
	}
}
