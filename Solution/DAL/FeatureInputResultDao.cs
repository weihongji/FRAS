using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class FeatureInputResultDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static bool Exist(string token) {
			if (string.IsNullOrWhiteSpace(token)) return false;

			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@Token", SqlDbType.VarChar, 50);
			parameters[0].Value = token;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("IF EXISTS(SELECT * FROM FeatureInputResult WHERE Token = @Token)");
			sql.AppendLine("	SELECT 1");
			sql.AppendLine("ELSE");
			sql.AppendLine("	SELECT 0");
			return (int)helper.ExecuteScalar(sql.ToString(), parameters) == 1;
		}

		public static FeatureInputResultInfo GetEntity(string token) {
			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@Token", SqlDbType.VarChar, 50);
			parameters[0].Value = token;
			DataTable table = helper.ExecuteDataTable("SELECT * FROM FeatureInputResult WITH (NOLOCK) WHERE Token = @Token", parameters);
			if (table.Rows.Count > 0) {
				return new FeatureInputResultInfo(table.Rows[0]);
			}
			return null;
		}
	}
}
