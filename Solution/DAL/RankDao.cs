using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class RankDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static bool Exist(int id, string name) {
			if (string.IsNullOrWhiteSpace(name)) return false;

			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@Name", SqlDbType.VarChar, 30);
			parameters[0].Value = name;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("IF EXISTS(SELECT * FROM RankInfo WHERE rank = @Name" + (id>0? " AND ID != " + id.ToString() : "") + ")");
			sql.AppendLine("	SELECT 1");
			sql.AppendLine("ELSE");
			sql.AppendLine("	SELECT 0");
			return (int)helper.ExecuteScalar(sql.ToString(), parameters) == 1;
		}

		public static int Add(RankInfo rank) {
			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@Name", SqlDbType.VarChar, 30);
			parameters[0].Value = rank.Name;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("DECLARE @ID int");
			sql.AppendLine("INSERT INTO RankInfo (Rank)");
			sql.AppendLine("VALUES (@Name)");
			sql.AppendLine("SET @ID = SCOPE_IDENTITY()");
			sql.AppendLine("SELECT @ID");
			int newID = (int)helper.ExecuteScalar(sql.ToString(), parameters);
			return newID;
		}

		public static bool Update(RankInfo rank) {
			SqlParameter[] parameters = new SqlParameter[2];
			parameters[0] = new SqlParameter("@ID", SqlDbType.Int);
			parameters[0].Value = rank.ID;
			parameters[1] = new SqlParameter("@Name", SqlDbType.VarChar, 30);
			parameters[1].Value = rank.Name;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE RankInfo SET Rank = @Name");
			sql.AppendLine("WHERE ID = @ID");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());
			return rowCount > 0;
		}

		public static bool Delete(int ID) {
			int rowCount = helper.ExecuteNonQuery("DELETE FROM RankInfo WHERE ID = " + ID.ToString());
			return rowCount > 0;
		}

		public static RankInfo GetEntity(int ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM RankInfo WHERE ID = " + ID.ToString());
			if (table.Rows.Count > 0) {
				return new RankInfo(table.Rows[0]);
			}
			return null;
		}

		public static DataTable GetList() {
			string sql = "SELECT * FROM RankInfo ORDER BY ID";
			return helper.ExecuteDataTable(sql);
		}

		// Used to populate dropdown lists
		public static DataTable GetBriefList() {
			string sql = "SELECT ID = ID, Name = RTrim(Rank) FROM RankInfo ORDER BY ID";
			return helper.ExecuteDataTable(sql);
		}
	}
}
