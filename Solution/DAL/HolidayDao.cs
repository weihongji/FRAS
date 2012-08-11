using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class HolidayDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static bool Exist(string name) {
			if (string.IsNullOrWhiteSpace(name)) return false;

			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@Name", SqlDbType.VarChar, 50);
			parameters[0].Value = name;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("IF EXISTS(SELECT * FROM HolidayInfo WHERE holidayName = @Name)");
			sql.AppendLine("	SELECT 1");
			sql.AppendLine("ELSE");
			sql.AppendLine("	SELECT 0");
			return (int)helper.ExecuteScalar(sql.ToString(), parameters) == 1;
		}

		public static int Add(HolidayInfo holiday) {
			SqlParameter[] parameters = new SqlParameter[5];
			parameters[0] = new SqlParameter("@Name", SqlDbType.VarChar, 20);
			parameters[0].Value = holiday.Name;
			parameters[1] = new SqlParameter("@StartDate", SqlDbType.VarChar, 20);
			parameters[1].Value = holiday.StartDate;
			parameters[2] = new SqlParameter("@EndDate", SqlDbType.VarChar, 20);
			parameters[2].Value = holiday.EndDate;
			parameters[3] = new SqlParameter("@Flag", SqlDbType.Int);
			parameters[3].Value = holiday.Active ? 1:0;
			parameters[4] = new SqlParameter("@Days", SqlDbType.Int);
			parameters[4].Value = holiday.Days;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("DECLARE @ID int");
			sql.AppendLine("INSERT INTO HolidayInfo (holidayName, startDate, endDate, flag, days)");
			sql.AppendLine("VALUES (@Name, @StartDate, @EndDate, @Flag, @Days)");
			sql.AppendLine("SET @ID = SCOPE_IDENTITY()");
			sql.AppendLine("SELECT @ID");
			int newID = (int)helper.ExecuteScalar(sql.ToString(), parameters);
			return newID;
		}

		public static bool Update(HolidayInfo holiday) {
			SqlParameter[] parameters = new SqlParameter[6];
			parameters[0] = new SqlParameter("@Name", SqlDbType.VarChar, 20);
			parameters[0].Value = holiday.Name;
			parameters[1] = new SqlParameter("@StartDate", SqlDbType.VarChar, 20);
			parameters[1].Value = holiday.StartDate;
			parameters[2] = new SqlParameter("@EndDate", SqlDbType.VarChar, 20);
			parameters[2].Value = holiday.EndDate;
			parameters[3] = new SqlParameter("@Flag", SqlDbType.Int);
			parameters[3].Value = holiday.Active ? 1 : 0;
			parameters[4] = new SqlParameter("@Days", SqlDbType.Int);
			parameters[4].Value = holiday.Days;
			parameters[5] = new SqlParameter("@ID", SqlDbType.Int);
			parameters[5].Value = holiday.ID;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE HolidayInfo SET holidayName = @Name, startDate = @StartDate, endDate = @EndDate, flag = @Flag, days = @Days");
			sql.AppendLine("WHERE ID = @ID");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());
			return rowCount > 0;
		}

		public static bool Delete(int ID) {
			int rowCount = helper.ExecuteNonQuery("DELETE FROM HolidayInfo WHERE ID = " + ID.ToString());
			return rowCount > 0;
		}

		public static HolidayInfo GetEntity(int ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM HolidayInfoView WHERE ID = " + ID.ToString());
			if (table.Rows.Count > 0) {
				return new HolidayInfo(table.Rows[0]);
			}
			return null;
		}

		public static DataTable GetList() {
			string sql = "SELECT * FROM HolidayInfoView ORDER BY ID";
			return helper.ExecuteDataTable(sql);
		}
	}
}
