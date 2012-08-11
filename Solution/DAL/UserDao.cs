using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class UserDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static bool Exist(string ID) {
			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@ID", SqlDbType.VarChar, 6);
			parameters[0].Value = ID;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("IF EXISTS(SELECT * FROM UserInfo WHERE userId = @ID)");
			sql.AppendLine("	SELECT 1");
			sql.AppendLine("ELSE");
			sql.AppendLine("	SELECT 0");
			return (int)helper.ExecuteScalar(sql.ToString(), parameters) == 1;
		}

		public static string Add(UserInfo user) {
			SqlParameter[] parameters = new SqlParameter[8];
			parameters[0] = new SqlParameter("@ID", SqlDbType.VarChar, 6);
			parameters[0].Value = user.ID;
			parameters[1] = new SqlParameter("@Name", SqlDbType.VarChar, 10);
			parameters[1].Value = user.Name;
			parameters[2] = new SqlParameter("@DeptID", SqlDbType.Int);
			parameters[2].Value = user.DeptID;
			parameters[3] = new SqlParameter("@FeatureID", SqlDbType.Int);
			parameters[3].Value = user.FeatureID;
			parameters[4] = new SqlParameter("@RankID", SqlDbType.Int);
			parameters[4].Value = user.RankID;
			parameters[5] = new SqlParameter("@SenderID", SqlDbType.VarChar, 6);
			parameters[5].Value = user.SenderID;
			parameters[6] = new SqlParameter("@RosteringID", SqlDbType.Int);
			parameters[6].Value = user.RosteringID;
			parameters[7] = new SqlParameter("@Type", SqlDbType.Int);
			parameters[7].Value = user.Type;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("INSERT INTO UserInfo (userId, userName, deptId, featureId, rankId, senderId, rosteringId, [type])");
			sql.AppendLine("VALUES (@ID, @Name, @DeptID, @FeatureID, @RankID, @SenderID, @RosteringID, @Type)");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters);
			return rowCount > 0 ? user.ID : "0";
		}

		public static bool Update(UserInfo user) {
			SqlParameter[] parameters = new SqlParameter[6];
			parameters[0] = new SqlParameter("@ID", SqlDbType.VarChar, 6);
			parameters[0].Value = user.ID;
			parameters[1] = new SqlParameter("@Name", SqlDbType.VarChar, 10);
			parameters[1].Value = user.Name;
			parameters[2] = new SqlParameter("@DeptID", SqlDbType.Int);
			parameters[2].Value = user.DeptID;
			parameters[3] = new SqlParameter("@RankID", SqlDbType.Int);
			parameters[3].Value = user.RankID;
			parameters[4] = new SqlParameter("@SenderID", SqlDbType.VarChar, 6);
			parameters[4].Value = user.SenderID;
			parameters[5] = new SqlParameter("@Type", SqlDbType.Int);
			parameters[5].Value = user.Type;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE UserInfo SET userName = @Name");
			sql.AppendLine("	, deptId = @DeptID");
			sql.AppendLine("	, rankId = @RankID");
			sql.AppendLine("	, senderId = @SenderID");
			sql.AppendLine("	, [type] = @Type");
			sql.AppendLine("WHERE userId = @ID");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());
			return rowCount > 0;
		}

		public static bool UpdateCard(string ID, int CopyType) {
			SqlParameter[] parameters = new SqlParameter[2];
			parameters[0] = new SqlParameter("@ID", SqlDbType.VarChar, 6);
			parameters[0].Value = ID;
			parameters[1] = new SqlParameter("@CopyType", SqlDbType.Int);
			parameters[1].Value = CopyType;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE UserInfo SET CopyType = @CopyType, flag = 0");
			sql.AppendLine("WHERE userId = @ID");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());
			return rowCount > 0;
		}

		public static bool Delete(string ID) {
			int rowCount = helper.ExecuteNonQuery("DELETE FROM UserInfo WHERE userId = '" + ID.Replace("'", "''").ToString() + "'");
			return rowCount > 0;
		}

		public static UserInfo GetEntity(string ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM UserInfoView WHERE userId = '" + ID.Replace("'", "''").ToString() + "'");
			if (table.Rows.Count > 0) {
				return new UserInfo(table.Rows[0]);
			}
			return null;
		}

		public static DataTable GetList(SqlCondition[] conditions, string SortColumn = "userId") {
			String conditionSql = "";
			StringBuilder conditionStringBuilder = new StringBuilder();
			List<SqlParameter> paramList = new List<SqlParameter>();
			SqlParameter[] paramItem;

			// WHERE clause
			if (conditions != null) {
				foreach (SqlCondition condition in conditions) {
					switch (condition.ColumnName.ToLower()) {
						case "id":
							condition.ColumnName = "userId";
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.VarChar, 6);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						case "name":
							condition.ColumnName = "userName";
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.VarChar, 10);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						case "deptid":
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.Int);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						case "senderid":
							condition.ColumnName = "senderId";
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.VarChar, 6);
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
			string sql = "SELECT * FROM UserInfoView " + conditionSql;
			if (!String.IsNullOrWhiteSpace(SortColumn)) {
				sql += " ORDER BY " + SortColumn;
			}
			return helper.ExecuteDataTable(sql, paramList.ToArray());
		}

		public static DataTable GetBriefList(int DeptID) {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("SELECT ID = RTrim(U.userId), Name = RTrim(U.userName)");
			sql.AppendLine("FROM UserInfo U");
			sql.AppendLine("WHERE U.featureId > 0");
			if (DeptID >= 0) {
				sql.AppendLine("AND U.deptId = " + DeptID.ToString());
			}
			sql.AppendLine("ORDER BY U.userName");
			return helper.ExecuteDataTable(sql.ToString());
		}

		public static DataTable GetBriefListForRostering(int DeptID) {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("SELECT ID = RTrim(U.userId)");
			sql.AppendLine("	, Name = RTrim(U.userName) + N' 【' + (CASE WHEN P.ID IS NULL THEN N'--未进行预设排班--' WHEN P.UpdateTime IS NULL THEN N'预设时间未知' ELSE N'在' + CONVERT(varchar(19), P.UpdateTime, 120) + N'进行过预设' END) + '】'");
			sql.AppendLine("FROM UserInfo U");
			sql.AppendLine("	LEFT JOIN PreRostering P ON P.userId = U.userId");
			sql.AppendLine("WHERE 1 > 0");
			if (DeptID >= 0) {
				sql.AppendLine("AND U.deptId = " + DeptID.ToString());
			}
			sql.AppendLine("ORDER BY U.userName");
			return helper.ExecuteDataTable(sql.ToString());
		}
	}
}
