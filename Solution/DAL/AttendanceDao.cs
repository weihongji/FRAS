using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class AttendanceDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static int Add(AttendanceInfo attendance, bool checkDuplication = true) {
			SqlParameter[] parameters = new SqlParameter[6];
			parameters[0] = new SqlParameter("@UserID", SqlDbType.VarChar, 6);
			parameters[0].Value = attendance.UserID;
			parameters[1] = new SqlParameter("@Date", SqlDbType.VarChar, 20);
			parameters[1].Value = attendance.Date;
			parameters[2] = new SqlParameter("@Duration", SqlDbType.Decimal);
			parameters[2].Value = attendance.Duration;
			parameters[3] = new SqlParameter("@InWell", SqlDbType.Int);
			parameters[3].Value = attendance.InWell ? 1:0;
			parameters[4] = new SqlParameter("@NightWork", SqlDbType.Int);
			parameters[4].Value = attendance.NightWork;
			parameters[5] = new SqlParameter("@Approved", SqlDbType.Int);
			parameters[5].Value = attendance.Approved ? 1 : 0;

			StringBuilder sql = new StringBuilder();
			if (checkDuplication) {
				sql.AppendLine("DECLARE @ID int");
				sql.AppendLine("IF EXISTS(SELECT * FROM AttendanceMng WHERE userId = @UserID AND DATE = @Date) BEGIN");
				sql.AppendLine("	SET @ID = 0");
				sql.AppendLine("END");
				sql.AppendLine("ELSE BEGIN");
				sql.AppendLine("	INSERT INTO AttendanceMng (userId, DATE, workDurs, ifIn, nightWork, state)");
				sql.AppendLine("	VALUES (@UserID, @Date, @Duration, @InWell, @NightWork, @Approved)");
				sql.AppendLine("	SET @ID = SCOPE_IDENTITY()");
				sql.AppendLine("END");
				sql.AppendLine("SELECT @ID");
			}
			else {
				sql.AppendLine("DECLARE @ID int");
				sql.AppendLine("INSERT INTO AttendanceMng (userId, DATE, workDurs, ifIn, nightWork, state)");
				sql.AppendLine("VALUES (@UserID, @Date, @Duration, @InWell, @NightWork, @Approved)");
				sql.AppendLine("SET @ID = SCOPE_IDENTITY()");
				sql.AppendLine("SELECT @ID");
			}
			int newID = (int)helper.ExecuteScalar(sql.ToString(), parameters);
			return newID;
		}

		public static bool BatchAdd(string UserIDs, string date, decimal Duration, bool InWell, int NightWork, out string Message) {
			SqlParameter[] parameters = new SqlParameter[5];
			parameters[0] = new SqlParameter("@UserIDs", SqlDbType.Text);
			parameters[0].Value = UserIDs;
			parameters[1] = new SqlParameter("@Date", SqlDbType.VarChar, 20);
			parameters[1].Value = date;
			parameters[2] = new SqlParameter("@Duration", SqlDbType.Decimal);
			parameters[2].Value = Duration;
			parameters[3] = new SqlParameter("@InWell", SqlDbType.Int);
			parameters[3].Value = InWell ? 1 : 0;
			parameters[4] = new SqlParameter("@NightWork", SqlDbType.Int);
			parameters[4].Value = NightWork;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("SELECT DISTINCT userId FROM AttendanceMng");
			sql.AppendLine("WHERE DATE = @Date AND userId IN (SELECT value FROM dbo.sfTableFromItemList(@UserIDs))");
			DataTable table = helper.ExecuteDataTable(sql.ToString(), parameters.ToArray());
			StringBuilder userIDs = new StringBuilder();
			foreach (DataRow row in table.Rows) {
				if (userIDs.Length > 0) { userIDs.Append(","); }
				userIDs.Append(row["userId"].ToString().Trim());
			}
			Message = userIDs.Length > 0 ? userIDs.ToString() : "";

			int rowCount = 0;
			if (Message.Length == 0) {
				sql.Clear();
				sql.AppendLine("INSERT INTO AttendanceMng (userId, DATE, workDurs, ifIn, nightWork)");
				sql.AppendLine("SELECT value, @Date, @Duration, @InWell, @NightWork FROM dbo.sfTableFromItemList(@UserIDs)");
				rowCount = (int)helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());
			}
			return rowCount > 0;
		}

		public static bool Update(AttendanceInfo attendance) {
			SqlParameter[] parameters = new SqlParameter[5];
			parameters[0] = new SqlParameter("@Date", SqlDbType.VarChar, 20);
			parameters[0].Value = attendance.Date;
			parameters[1] = new SqlParameter("@Duration", SqlDbType.Decimal);
			parameters[1].Value = attendance.Duration;
			parameters[2] = new SqlParameter("@InWell", SqlDbType.Int);
			parameters[2].Value = attendance.InWell ? 1 : 0;
			parameters[3] = new SqlParameter("@NightWork", SqlDbType.Int);
			parameters[3].Value = attendance.NightWork;
			parameters[4] = new SqlParameter("@ID", SqlDbType.Int);
			parameters[4].Value = attendance.ID;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE AttendanceMng SET DATE = @Date, workDurs = @Duration, ifIn = @InWell, nightWork = @NightWork");
			sql.AppendLine("WHERE ID = @ID");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());
			return rowCount > 0;
		}

		public static bool Delete(int ID) {
			int rowCount = helper.ExecuteNonQuery("DELETE FROM AttendanceMng WHERE ID = " + ID.ToString());
			return rowCount > 0;
		}

		public static bool Approve(int ID) {
			int rowCount = helper.ExecuteNonQuery("UPDATE AttendanceMng SET state = 1 WHERE ID = " + ID.ToString());
			return rowCount > 0;
		}

		public static int Import(string filePath, out string Message) {
			Message = "";
			if (System.IO.Path.GetExtension(filePath) != ".xls") {
				return 0;
			}
			string cnnString = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = '" + filePath + "'; Extended Properties=Excel 8.0";
			OleDbConnection cnnxls = new OleDbConnection(cnnString);
			OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$] ", cnnxls);
			DataSet dataset = new DataSet();
			adapter.Fill(dataset);
			cnnxls.Close();
			DataTable table = dataset.Tables[0];
			StringBuilder sql = new StringBuilder();

			sql.Clear();
			foreach (DataRow row in table.Rows) {
				if (Utility.StringUtility.IsNumeric(row.ItemArray[0].ToString())) {
					if (sql.Length > 0) { sql.AppendLine("UNION"); }
					sql.AppendLine("SELECT TOP 1 userId FROM AttendanceMng");
					sql.AppendLine("WHERE userId = '" + row.ItemArray[0].ToString().Trim() + "' AND DATE = '" + row.ItemArray[2].ToString() + "'");
				}
			}
			if (sql.Length > 0) {
				DataTable table_exist = helper.ExecuteDataTable(sql.ToString());
				StringBuilder userIDs = new StringBuilder();
				foreach (DataRow row in table_exist.Rows) {
					if (userIDs.Length > 0) { userIDs.Append(","); }
					userIDs.Append(row["userId"].ToString().Trim());
				}
				Message = userIDs.Length > 0 ? userIDs.ToString() : "";
			}

			sql.Clear();
			foreach (DataRow row in table.Rows) {
				if (Utility.StringUtility.IsNumeric(row.ItemArray[0].ToString())) {
					sql.AppendLine("INSERT INTO AttendanceMng (userId, Date, workDurs, ifIn, nightWork, state)");
					sql.AppendLine("VALUES ('" + row.ItemArray[0].ToString() + "','" + row.ItemArray[2].ToString() + "'," + row.ItemArray[3].ToString() + "," + row.ItemArray[4].ToString() + "," + row.ItemArray[5].ToString() + ", 1)");
				}
			}
			int count = 0;
			if (sql.Length > 0 && string.IsNullOrEmpty(Message)) {
				count = helper.ExecuteNonQuery(sql.ToString());
			}
			return count;
		}

		public static AttendanceInfo GetEntity(int ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM AttendanceMngView WHERE ID = " + ID.ToString());
			if (table.Rows.Count > 0) {
				return new AttendanceInfo(table.Rows[0]);
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
						case "state":
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
			string sql = "SELECT * FROM AttendanceMngView " + conditionSql + " ORDER BY ID";
			return helper.ExecuteDataTable(sql, paramList.ToArray());
		}

		public static DataTable GetReportList(int dept, int device, int userType, DateTime start, DateTime end, int Year, int Month) {
			SqlParameter[] parameters = new SqlParameter[5];
			parameters[0] = new SqlParameter("@DeptId", dept);
			parameters[1] = new SqlParameter("@UserType", userType);
			parameters[2] = new SqlParameter("@DeviceId", device);
			if (Year > 2000 && Month>=1 && Month<=12) {
				parameters[3] = new SqlParameter("@Year", Year);
				parameters[4] = new SqlParameter("@Month", Month);
			}
			else {
				parameters[3] = new SqlParameter("@StartDate", SqlDbType.DateTime);
				parameters[3].Value = start;
				parameters[4] = new SqlParameter("@EndDate", SqlDbType.DateTime);
				parameters[4].Value = end;
			}
			return helper.ExecuteDataTable("spAttendanceReport", CommandType.StoredProcedure, parameters);
		}
	}
}
