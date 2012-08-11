using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class DeptDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static bool Exist(int id) {
			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@ID", SqlDbType.VarChar, 50);
			parameters[0].Value = id;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("IF EXISTS(SELECT * FROM DeptInfo WHERE deptId = @ID)");
			sql.AppendLine("	SELECT 1");
			sql.AppendLine("ELSE");
			sql.AppendLine("	SELECT 0");
			return (int)helper.ExecuteScalar(sql.ToString(), parameters) == 1;
		}

		public static bool Exist(int id, string name) {
			if (string.IsNullOrWhiteSpace(name)) return false;

			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@Name", SqlDbType.VarChar, 50);
			parameters[0].Value = name;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("IF EXISTS(SELECT * FROM DeptInfo WHERE deptName = @Name" + (id>=0 ? " AND deptId != " + id.ToString(): "") + ")");
			sql.AppendLine("	SELECT 1");
			sql.AppendLine("ELSE");
			sql.AppendLine("	SELECT 0");
			return (int)helper.ExecuteScalar(sql.ToString(), parameters) == 1;
		}

		public static bool Add(DeptInfo dept) {
			SqlParameter[] parameters = new SqlParameter[4];
			parameters[0] = new SqlParameter("@ID", SqlDbType.Int);
			parameters[0].Value = dept.ID;
			parameters[1] = new SqlParameter("@Name", SqlDbType.VarChar, 50);
			parameters[1].Value = dept.Name;
			parameters[2] = new SqlParameter("@UpID", SqlDbType.Int);
			parameters[2].Value = dept.UpID;
			parameters[3] = new SqlParameter("@Level", SqlDbType.Int);
			parameters[3].Value = dept.Level;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("INSERT INTO DeptInfo (deptId, deptName, upID, [level])");
			sql.AppendLine("VALUES (@ID, @Name, @UpID, @Level)");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());
			return rowCount > 0;
		}

		public static bool Update(DeptInfo dept) {
			SqlParameter[] parameters = new SqlParameter[4];
			parameters[0] = new SqlParameter("@ID", SqlDbType.Int);
			parameters[0].Value = dept.ID;
			parameters[1] = new SqlParameter("@Name", SqlDbType.VarChar, 50);
			parameters[1].Value = dept.Name;
			parameters[2] = new SqlParameter("@UpID", SqlDbType.Int);
			parameters[2].Value = dept.UpID;
			parameters[3] = new SqlParameter("@Level", SqlDbType.Int);
			parameters[3].Value = dept.Level;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE DeptInfo SET deptName = @Name, upID = @UpID, [level] = @Level");
			sql.AppendLine("WHERE deptId = @ID");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());
			return rowCount > 0;
		}

		public static bool Delete(int ID) {
			int rowCount = helper.ExecuteNonQuery("DELETE FROM DeptInfo WHERE deptId = " + ID.ToString());
			return rowCount > 0;
		}

		public static DeptInfo GetEntity(int ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM DeptInfo WHERE deptId = " + ID.ToString());
			if (table.Rows.Count > 0) {
				return new DeptInfo(table.Rows[0]);
			}
			return null;
		}

		public static DataTable GetList() {
			string sql = "SELECT * FROM DeptInfo ORDER BY deptId";
			return helper.ExecuteDataTable(sql);
		}

		// Used to populate dropdown lists
		public static DataTable GetBriefList(int ID = -1) {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("SELECT ID = deptId, Name = RTrim(deptName) FROM DeptInfo");
			if (ID >= 0) {
				sql.AppendLine("WHERE deptId = " + ID.ToString());
			}
			sql.AppendLine("ORDER BY deptId");
			return helper.ExecuteDataTable(sql.ToString());
		}
	}
}
