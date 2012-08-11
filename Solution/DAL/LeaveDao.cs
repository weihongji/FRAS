using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class LeaveDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static int Add(LeaveInfo leave) {
			SqlParameter[] parameters = new SqlParameter[6];
			parameters[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 6);
			parameters[0].Value = leave.UserID;
			parameters[1] = new SqlParameter("@StartDate", SqlDbType.VarChar, 20);
			parameters[1].Value = leave.StartDate;
			parameters[2] = new SqlParameter("@EndDate", SqlDbType.VarChar, 20);
			parameters[2].Value = leave.EndDate;
			parameters[3] = new SqlParameter("@Type", SqlDbType.Int);
			parameters[3].Value = leave.Type;
			parameters[4] = new SqlParameter("@Flag", SqlDbType.Int);
			parameters[4].Value = leave.Approved ? 1 : 0;
			parameters[5] = new SqlParameter("@Days", SqlDbType.Int);
			parameters[5].Value = leave.Days;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("DECLARE @ID int");
			sql.AppendLine("IF EXISTS(SELECT * FROM LeaveInfo WHERE userId = @UserID AND startDate <= @EndDate AND endDate >= @StartDate) BEGIN");
			sql.AppendLine("	SET @ID = 0");
			sql.AppendLine("END");
			sql.AppendLine("ELSE BEGIN");
			sql.AppendLine("	INSERT INTO LeaveInfo (userId, startDate, endDate, type, flag, days)");
			sql.AppendLine("	VALUES (@UserID, @StartDate, @EndDate, @Type, @Flag, @Days)");
			sql.AppendLine("	SET @ID = SCOPE_IDENTITY()");
			sql.AppendLine("END");
			sql.AppendLine("SELECT @ID");
			int newID = (int)helper.ExecuteScalar(sql.ToString(), parameters);
			return newID;
		}

		public static bool Update(LeaveInfo leave) {
			SqlParameter[] parameters = new SqlParameter[5];
			parameters[0] = new SqlParameter("@StartDate", SqlDbType.VarChar, 20);
			parameters[0].Value = leave.StartDate;
			parameters[1] = new SqlParameter("@EndDate", SqlDbType.VarChar, 20);
			parameters[1].Value = leave.EndDate;
			parameters[2] = new SqlParameter("@Type", SqlDbType.Int);
			parameters[2].Value = leave.Type;
			parameters[3] = new SqlParameter("@Days", SqlDbType.Int);
			parameters[3].Value = leave.Days;
			parameters[4] = new SqlParameter("@ID", SqlDbType.Int);
			parameters[4].Value = leave.ID;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE LeaveInfo SET startDate = @StartDate, endDate = @EndDate, type = @Type, days = @Days");
			sql.AppendLine("WHERE ID = @ID");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());
			return rowCount > 0;
		}

		public static bool Delete(int ID) {
			int rowCount = helper.ExecuteNonQuery("DELETE FROM LeaveInfo WHERE ID = " + ID.ToString());
			return rowCount > 0;
		}

		public static bool Approve(int ID) {
			int rowCount = helper.ExecuteNonQuery("UPDATE LeaveInfo SET flag = 1 WHERE ID = " + ID.ToString());
			return rowCount > 0;
		}

		public static LeaveInfo GetEntity(int ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM LeaveInfoView WHERE ID = " + ID.ToString());
			if (table.Rows.Count > 0) {
				return new LeaveInfo(table.Rows[0]);
			}
			return null;
		}

		public static DataTable GetList(SqlCondition[] conditions) {
			String conditionSql = "";
			StringBuilder conditionStringBuilder = new StringBuilder();
			List<SqlParameter> paramList = new List<SqlParameter>();
			SqlParameter[] paramItem;

			// WHERE clause
			if (conditions != null) {
				foreach (SqlCondition condition in conditions) {
					switch (condition.ColumnName.ToLower()) {
						case "deptid":
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.Int);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						case "userid":
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.VarChar, 6);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						case "flag":
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.Int);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						default:
							break;
					}
				}
				if (conditionStringBuilder.Length > 0) {
					conditionSql = conditionStringBuilder.ToString();
					conditionSql = "WHERE " + conditionSql.Substring("AND ".Length);
				}
			}
			string sql = "SELECT * FROM LeaveInfoView " + conditionSql + " ORDER BY ID";
			return helper.ExecuteDataTable(sql, paramList.ToArray());
		}
	}
}
