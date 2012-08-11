using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class UserCardDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static int Add(UserCardInfo card) {
			SqlParameter[] parameters = new SqlParameter[2];
			parameters[0] = new SqlParameter("@CardID", SqlDbType.VarChar, 50);
			parameters[0].Value = card.CardID;
			parameters[1] = new SqlParameter("@UserID", SqlDbType.VarChar, 6);
			parameters[1].Value = card.UserID;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("INSERT INTO UserCard (cardId, userId)");
			sql.AppendLine("VALUES (@CardID, @UserID)");
			sql.AppendLine("SELECT SCOPE_IDENTITY();");
			object newID = helper.ExecuteScalar(sql.ToString(), parameters);
			return Convert.ToInt32(newID);
		}

		public static bool Update(UserCardInfo card) {
			SqlParameter[] parameters = new SqlParameter[2];
			parameters[0] = new SqlParameter("@CardID", SqlDbType.VarChar, 50);
			parameters[0].Value = card.CardID;
			parameters[1] = new SqlParameter("@UserID", SqlDbType.VarChar, 6);
			parameters[1].Value = card.UserID;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE UserCard SET cardId = @CardID WHERE userId = @UserID");
			int affectedRows = (int)helper.ExecuteNonQuery(sql.ToString(), parameters);
			return affectedRows>0;
		}

		public static UserCardInfo GetEntityByUserID(string UserID) {
			DataTable table = helper.ExecuteDataTable("SELECT TOP 1 * FROM UserCard WHERE userId = '" + UserID.Replace("'", "''").ToString() + "'");
			if (table.Rows.Count > 0) {
				return new UserCardInfo(table.Rows[0]);
			}
			return null;
		}
	}
}
