using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class LogonUserDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static bool Exist(string code, string password = "") {
			if (string.IsNullOrWhiteSpace(code)) return false;
			if (string.IsNullOrWhiteSpace(password)) password = "";

			SqlParameter[] parameters = new SqlParameter[2];
			parameters[0] = new SqlParameter("@Code", SqlDbType.VarChar, 20);
			parameters[0].Value = code;
			parameters[1] = new SqlParameter("@Password", SqlDbType.VarChar, 20);
			parameters[1].Value = password;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("IF EXISTS(SELECT * FROM LogonUser WHERE logonUser = @Code AND (@Password = '' OR passwd = @Password))");
			sql.AppendLine("	SELECT 1");
			sql.AppendLine("ELSE");
			sql.AppendLine("	SELECT 0");
			return (int) helper.ExecuteScalar(sql.ToString(), parameters) == 1;
		}

		public static int Add(LogonUserInfo user) {
			SqlParameter[] parameters = new SqlParameter[5];
			parameters[0] = new SqlParameter("@Code", SqlDbType.VarChar, 20);
			parameters[0].Value = user.Code;
			parameters[1] = new SqlParameter("@Password", SqlDbType.VarChar, 20);
			parameters[1].Value = user.Password;
			parameters[2] = new SqlParameter("@RoleType", SqlDbType.Int);
			parameters[2].Value = user.RoleType;
			parameters[3] = new SqlParameter("@DeptID", SqlDbType.Int);
			parameters[3].Value = user.DeptID>=0 ? (object) user.DeptID : DBNull.Value;
			parameters[4] = new SqlParameter("@Flag", SqlDbType.Int);
			parameters[4].Value = user.Active ? 1 : 0;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("DECLARE @ID int");
			sql.AppendLine("INSERT INTO LogonUser (logonUser, passwd, roleType, flag, depId)");
			sql.AppendLine("VALUES (@Code, @Password, @RoleType, @Flag, @DeptID)");
			sql.AppendLine("SET @ID = SCOPE_IDENTITY()");
			sql.AppendLine("SELECT @ID");
			int newID = (int) helper.ExecuteScalar(sql.ToString(), parameters);
			return newID;
		}

		public static bool Update(LogonUserInfo user) {
			SqlParameter[] parameters = new SqlParameter[6];
			parameters[0] = new SqlParameter("@Code", SqlDbType.VarChar, 20);
			parameters[0].Value = user.Code;
			parameters[1] = new SqlParameter("@Password", SqlDbType.VarChar, 20);
			parameters[1].Value = user.Password;
			parameters[2] = new SqlParameter("@RoleType", SqlDbType.Int);
			parameters[2].Value = user.RoleType;
			parameters[3] = new SqlParameter("@DeptID", SqlDbType.Int);
			parameters[3].Value = user.DeptID >= 0 ? (object)user.DeptID : DBNull.Value;
			parameters[4] = new SqlParameter("@Flag", SqlDbType.Int);
			parameters[4].Value = user.Active ? 1 : 0;
			parameters[5] = new SqlParameter("@ID", SqlDbType.Int);
			parameters[5].Value = user.ID;
			
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE LogonUser SET logonUser = @Code, passwd = @Password, roleType = @RoleType, depId = @DeptID, flag = @Flag");
			sql.AppendLine("WHERE ID = @ID");
			return helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray()) > 0;
		}

		public static bool Delete(int ID) {
			int rowCount = helper.ExecuteNonQuery("DELETE FROM LogonUser WHERE ID = " + ID.ToString());
			return rowCount > 0;
		}

		public static LogonUserInfo GetEntity(int ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM LogonUserView WHERE ID = " + ID.ToString());
			if (table.Rows.Count > 0) {
				return new LogonUserInfo(table.Rows[0]);
			}
			return null;
		}

		public static LogonUserInfo GetEntity(String code) {
			if (String.IsNullOrWhiteSpace(code)) {
				return null;
			}

			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@Code", SqlDbType.VarChar, 20);
			parameters[0].Value = code;
			DataTable table = helper.ExecuteDataTable("SELECT * FROM LogonUserView WHERE logonUser = @Code", parameters);
			if (table.Rows.Count > 0) {
				return new LogonUserInfo(table.Rows[0]);
			}
			return null;
		}
		
		/// <summary>
		/// Get a list of users with specified parameters.
		/// </summary>
		/// <param name="conditions">Condition items will be connected by "AND"</param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="sortBy"></param>
		/// <returns></returns>
		/// <example>
		///		SqlCondition[] conditions = new SqlCondition[2];
		///		conditions[0] = new SqlCondition("Active", new String[] { "0", "1" }, SqlCondition.EnumConstraintType.In);
		///		conditions[1] = new SqlCondition("PositionID", new String[] { "0", "1" }, SqlCondition.EnumConstraintType.Between);
		///		DataTable userTable = UserDao.GetList(conditions, 1, 6, new String[] { "PositionID", "Active DESC" });
		/// </example>
		public static DataTable GetList(SqlCondition[] conditions, int pageIndex, int pageSize, string sortBy, out int pageCount) {
			String conditionSql = "";
			StringBuilder conditionStringBuilder = new StringBuilder();
			List<SqlParameter> paramList = new List<SqlParameter>();
			SqlParameter[] paramItem;
			String sort = "";

			// WHERE clause
			if (conditions != null) {
				foreach (SqlCondition condition in conditions) {
					switch (condition.ColumnName.ToLower()) {
						case "id":
							condition.ColumnName = "ID";
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.Int);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						case "code":
							condition.ColumnName = "logonUser";
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.VarChar, 20);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						case "roletype":
							condition.ColumnName = "roleType";
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.Int);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						case "active":
							condition.ColumnName = "flag";
							object[] flagValue = {(bool)condition.ColumnValue[0] ? 1 : 0};
							condition.ColumnValue = flagValue;
							conditionSql = condition.GetConditionSqlInParameter(out paramItem, SqlDbType.Int);
							if (conditionSql.Length > 0) {
								conditionStringBuilder.AppendLine("AND " + conditionSql);
								paramList.AddRange(paramItem);
							}
							break;
						case "deptid":
							condition.ColumnName = "depId";
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

			// ORDER BY clause
			if (!string.IsNullOrWhiteSpace(sortBy)) {
				string sortDirection = "ASC";
				if (sortBy.ToLower().IndexOf(" desc") > 0) {
					sortDirection = "DESC";
				}
				string sortColumnName = "";
				sortColumnName = sortBy.ToLower().Replace(" asc", "").Replace(" desc", "");
				switch (sortColumnName) {
					case "id":
						sortColumnName = "ID";
						break;
					case "code":
						sortColumnName = "logonUser";
						break;
					case "password":
						sortColumnName = "passwd";
						break;
					case "roletype":
						sortColumnName = "roleType";
						break;
					case "active":
						sortColumnName = "flag";
						break;
					case "deptid":
						sortColumnName = "depId";
						break;
					default:
						sortColumnName = "ID";
						break;
				}
				sort = "ORDER BY " + sortColumnName + " " + sortDirection;
			}
			if (sort.Length == 0) {
				sort = "ORDER BY ID";
			}

			//Make up final query statment
			StringBuilder sql = new StringBuilder();
			if (pageIndex > 0 && pageSize > 0) {
				sql.AppendLine("SELECT * FROM");
				sql.AppendLine("	(");
				sql.AppendLine("		SELECT TOP " + (pageIndex * pageSize).ToString() + " ROW_NUMBER() OVER(" + sort + ") AS RowNo, * FROM LogonUserView");
				sql.AppendLine("		" + conditionSql);
				sql.AppendLine("		" + sort);
				sql.AppendLine("	) AS UsersTopRows");
				sql.AppendLine("WHERE RowNo>" + ((pageIndex - 1) * pageSize).ToString());
			}
			else {
				sql.AppendLine("SELECT * FROM LogonUserView");
				sql.AppendLine(conditionSql);
				sql.AppendLine(sort);
			}

			if (pageSize > 0) {
				int rowCount = int.Parse(helper.ExecuteScalar("SELECT COUNT(*) FROM LogonUserView " + conditionSql, paramList.ToArray()).ToString());
				pageCount = (rowCount - 1) / pageSize + 1;
			}
			else {
				pageCount = 0;
			}

			return helper.ExecuteDataTable(sql.ToString(), paramList.ToArray());
		}
	}
}
