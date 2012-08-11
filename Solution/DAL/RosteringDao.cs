using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;
using Utility;

namespace DAL
{
	public class RosteringDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static int Add(RosteringInfo rostering, RosteringInfo rostering2) {
			SqlParameter[] parameters = new SqlParameter[11];
			parameters[0] = new SqlParameter("@Name", SqlDbType.VarChar, 20);
			parameters[0].Value = rostering.Name;
			parameters[1] = new SqlParameter("@StartTime", SqlDbType.VarChar, 20);
			parameters[1].Value = DateUtility.FormatTime(rostering.StartTime);
			parameters[2] = new SqlParameter("@EarlyRange", SqlDbType.Int);
			parameters[2].Value = rostering.EarlyRange;
			parameters[3] = new SqlParameter("@EndTime", SqlDbType.VarChar, 20);
			parameters[3].Value = DateUtility.FormatTime(rostering.EndTime);
			parameters[4] = new SqlParameter("@LateRange", SqlDbType.Int);
			parameters[4].Value = rostering.LateRange;
			parameters[5] = new SqlParameter("@MultipleType", SqlDbType.Int);
			parameters[5].Value = rostering2 == null ? 1 : 2;
			parameters[6] = new SqlParameter("@Duration", SqlDbType.Decimal);
			parameters[6].Value = rostering.Duration;
			parameters[7] = new SqlParameter("@NightWork", SqlDbType.Int);
			parameters[7].Value = rostering.NightWork;
			parameters[8] = new SqlParameter("@Flag", SqlDbType.Int);
			parameters[8].Value = rostering.Active ? 1 : 0;
			parameters[9] = new SqlParameter("@RealStartTime", SqlDbType.VarChar, 20);
			parameters[9].Value = rostering.RealStartTime;
			parameters[10] = new SqlParameter("@RealEndTime", SqlDbType.VarChar, 20);
			parameters[10].Value = rostering.RealEndTime;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("DECLARE @ID int");
			sql.AppendLine("INSERT INTO Rostering (bcName, startTime, earlyRange, endTime, lateRange, multType, mulripleDur, nightWork, flag, realStartTime, realEndTime)");
			sql.AppendLine("VALUES (@Name, @StartTime, @EarlyRange, @EndTime, @LateRange, @MultipleType, @Duration, @NightWork, @Flag, @RealStartTime, @RealEndTime)");
			sql.AppendLine("SET @ID = SCOPE_IDENTITY()");
			sql.AppendLine("SELECT @ID");
			int newID = (int)helper.ExecuteScalar(sql.ToString(), parameters);

			//Save the record of 'afternoon'.
			if (rostering2 != null) {
				SqlParameter[] parameters2 = new SqlParameter[11];
				parameters2[0] = new SqlParameter("@Name", SqlDbType.VarChar, 20);
				parameters2[0].Value = rostering.Name;
				parameters2[1] = new SqlParameter("@StartTime", SqlDbType.VarChar, 20);
				parameters2[1].Value = DateUtility.FormatTime(rostering2.StartTime);
				parameters2[2] = new SqlParameter("@EarlyRange", SqlDbType.Int);
				parameters2[2].Value = rostering2.EarlyRange;
				parameters2[3] = new SqlParameter("@EndTime", SqlDbType.VarChar, 20);
				parameters2[3].Value = DateUtility.FormatTime(rostering2.EndTime);
				parameters2[4] = new SqlParameter("@LateRange", SqlDbType.Int);
				parameters2[4].Value = rostering2.LateRange;
				parameters2[5] = new SqlParameter("@MultipleType", SqlDbType.Int);
				parameters2[5].Value = 0;
				parameters2[6] = new SqlParameter("@Duration", SqlDbType.Decimal);
				parameters2[6].Value = rostering2.Duration;
				parameters2[7] = new SqlParameter("@NightWork", SqlDbType.Int);
				parameters2[7].Value = rostering2.NightWork;
				parameters2[8] = new SqlParameter("@Flag", SqlDbType.Int);
				parameters2[8].Value = rostering.Active ? 1 : 0;
				parameters2[9] = new SqlParameter("@RealStartTime", SqlDbType.VarChar, 20);
				parameters2[9].Value = rostering2.RealStartTime;
				parameters2[10] = new SqlParameter("@RealEndTime", SqlDbType.VarChar, 20);
				parameters2[10].Value = rostering2.RealEndTime;
				sql.Clear();
				sql.AppendLine("DECLARE @ID int");
				sql.AppendLine("INSERT INTO Rostering (bcName, startTime, earlyRange, endTime, lateRange, multType, mulripleDur, nightWork, flag, realStartTime, realEndTime)");
				sql.AppendLine("VALUES (@Name, @StartTime, @EarlyRange, @EndTime, @LateRange, @MultipleType, @Duration, @NightWork, @Flag, @RealStartTime, @RealEndTime)");
				sql.AppendLine("SET @ID = SCOPE_IDENTITY()");
				sql.AppendLine("SELECT @ID");
				int nextID = (int)helper.ExecuteScalar(sql.ToString(), parameters2);

				//Assign the second record's ID to nextID column of the first record.
				helper.ExecuteNonQuery("UPDATE Rostering SET nextID = " + nextID.ToString() + " WHERE ID = " + newID.ToString());
			}
			return newID;
		}

		public static bool Update(RosteringInfo rostering, RosteringInfo rostering2) {
			SqlParameter[] parameters = new SqlParameter[11];
			parameters[0] = new SqlParameter("@Name", SqlDbType.VarChar, 20);
			parameters[0].Value = rostering.Name;
			parameters[1] = new SqlParameter("@StartTime", SqlDbType.VarChar, 20);
			parameters[1].Value = DateUtility.FormatTime(rostering.StartTime);
			parameters[2] = new SqlParameter("@EarlyRange", SqlDbType.Int);
			parameters[2].Value = rostering.EarlyRange;
			parameters[3] = new SqlParameter("@EndTime", SqlDbType.VarChar, 20);
			parameters[3].Value = DateUtility.FormatTime(rostering.EndTime);
			parameters[4] = new SqlParameter("@LateRange", SqlDbType.Int);
			parameters[4].Value = rostering.LateRange;
			parameters[5] = new SqlParameter("@Duration", SqlDbType.Decimal);
			parameters[5].Value = rostering.Duration;
			parameters[6] = new SqlParameter("@NightWork", SqlDbType.Int);
			parameters[6].Value = rostering.NightWork;
			parameters[7] = new SqlParameter("@Flag", SqlDbType.Int);
			parameters[7].Value = rostering.Active ? 1 : 0;
			parameters[8] = new SqlParameter("@RealStartTime", SqlDbType.VarChar, 20);
			parameters[8].Value = rostering.RealStartTime;
			parameters[9] = new SqlParameter("@RealEndTime", SqlDbType.VarChar, 20);
			parameters[9].Value = rostering.RealEndTime;
			parameters[10] = new SqlParameter("@ID", SqlDbType.Int);
			parameters[10].Value = rostering.ID;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE Rostering SET bcName = @Name");
			sql.AppendLine("	, startTime = @StartTime");
			sql.AppendLine("	, earlyRange = @EarlyRange");
			sql.AppendLine("	, endTime = @EndTime");
			sql.AppendLine("	, lateRange = @LateRange");
			sql.AppendLine("	, mulripleDur = @Duration");
			sql.AppendLine("	, nightWork = @NightWork");
			sql.AppendLine("	, flag = @Flag");
			sql.AppendLine("	, realStartTime = @RealStartTime");
			sql.AppendLine("	, realEndTime = @RealEndTime");
			sql.AppendLine("WHERE ID = @ID");
			int rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray());

			//Update the record of 'afternoon'
			if (rostering2 != null) {
				SqlParameter[] parameters2 = new SqlParameter[11];
				parameters2[0] = new SqlParameter("@Name", SqlDbType.VarChar, 20);
				parameters2[0].Value = rostering.Name;
				parameters2[1] = new SqlParameter("@StartTime", SqlDbType.VarChar, 20);
				parameters2[1].Value = DateUtility.FormatTime(rostering2.StartTime);
				parameters2[2] = new SqlParameter("@EarlyRange", SqlDbType.Int);
				parameters2[2].Value = rostering2.EarlyRange;
				parameters2[3] = new SqlParameter("@EndTime", SqlDbType.VarChar, 20);
				parameters2[3].Value = DateUtility.FormatTime(rostering2.EndTime);
				parameters2[4] = new SqlParameter("@LateRange", SqlDbType.Int);
				parameters2[4].Value = rostering2.LateRange;
				parameters2[5] = new SqlParameter("@Duration", SqlDbType.Decimal);
				parameters2[5].Value = rostering2.Duration;
				parameters2[6] = new SqlParameter("@NightWork", SqlDbType.Int);
				parameters2[6].Value = rostering2.NightWork;
				parameters2[7] = new SqlParameter("@Flag", SqlDbType.Int);
				parameters2[7].Value = rostering.Active ? 1 : 0;
				parameters2[8] = new SqlParameter("@RealStartTime", SqlDbType.VarChar, 20);
				parameters2[8].Value = rostering2.RealStartTime;
				parameters2[9] = new SqlParameter("@RealEndTime", SqlDbType.VarChar, 20);
				parameters2[9].Value = rostering2.RealEndTime;
				parameters2[10] = new SqlParameter("@ID", SqlDbType.Int);
				parameters2[10].Value = rostering.ID;

				sql.Clear();
				sql.AppendLine("UPDATE Rostering SET bcName = @Name");
				sql.AppendLine("	, startTime = @StartTime");
				sql.AppendLine("	, earlyRange = @EarlyRange");
				sql.AppendLine("	, endTime = @EndTime");
				sql.AppendLine("	, lateRange = @LateRange");
				sql.AppendLine("	, mulripleDur = @Duration");
				sql.AppendLine("	, nightWork = @NightWork");
				sql.AppendLine("	, flag = @Flag");
				sql.AppendLine("	, realStartTime = @RealStartTime");
				sql.AppendLine("	, realEndTime = @RealEndTime");
				sql.AppendLine("WHERE ID = (SELECT nextID FROM Rostering WHERE ID = @ID)");
				rowCount = helper.ExecuteNonQuery(sql.ToString(), parameters2.ToArray());
			}
			return rowCount > 0;
		}

		public static bool Delete(int ID) {
			int rowCount = helper.ExecuteNonQuery("DELETE FROM Rostering WHERE ID = " + ID.ToString());
			return rowCount > 0;
		}

		public static RosteringInfo GetEntity(int ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM RosteringView WHERE ID = " + ID.ToString());
			if (table.Rows.Count > 0) {
				return new RosteringInfo(table.Rows[0]);
			}
			return null;
		}

		public static DataTable GetList() {
			string sql = "SELECT * FROM RosteringView ORDER BY ID";
			return helper.ExecuteDataTable(sql);
		}

		public static DataTable GetPreRostering(string UserId) {
			string sql = "SELECT * FROM PreRosteringView WHERE userId = @UserId";
	
			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@UserId", SqlDbType.VarChar, 6);
			parameters[0].Value = UserId;

			return helper.ExecuteDataTable(sql, parameters);
		}

		public static bool SavePreRostering(string UserIDs, int RosteringId, int Start, int End) {
			if (!(0 < Start && Start <= End && End < 32) || string.IsNullOrWhiteSpace(UserIDs)) {
				return false;
			}
			SqlParameter[] parameters = new SqlParameter[4];
			parameters[0] = new SqlParameter("@UserIDs", SqlDbType.VarChar, 8000);
			parameters[0].Value = UserIDs;
			parameters[1] = new SqlParameter("@RosteringID", SqlDbType.Int);
			parameters[1].Value = RosteringId;
			parameters[2] = new SqlParameter("@Start", SqlDbType.Int);
			parameters[2].Value = Start;
			parameters[3] = new SqlParameter("@End", SqlDbType.Int);
			parameters[3].Value = End;
			helper.ExecuteNonQuery("spSavePreRostering", CommandType.StoredProcedure, parameters);
			return true;
		}
	}
}
