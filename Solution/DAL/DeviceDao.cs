﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class DeviceDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static int Add(DeviceInfo device) {
			SqlParameter[] parameters = new SqlParameter[9];
			parameters[0] = new SqlParameter("@IP", SqlDbType.VarChar, 30);
			parameters[0].Value = device.IP;
			parameters[1] = new SqlParameter("@Port", SqlDbType.VarChar, 10);
			parameters[1].Value = device.Port;
			parameters[2] = new SqlParameter("@DeviceType", SqlDbType.VarChar, 2);
			parameters[2].Value = device.DeviceType;
			parameters[3] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
			parameters[3].Value = device.UserName;
			parameters[4] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
			parameters[4].Value = device.Password;
			parameters[5] = new SqlParameter("@AntNo", SqlDbType.Int);
			parameters[5].Value = device.AntNo;
			parameters[6] = new SqlParameter("@AccessFlag", SqlDbType.Int);
			parameters[6].Value = device.AccessFlag;
			parameters[7] = new SqlParameter("@Flag", SqlDbType.Int);
			parameters[7].Value = device.Active ? 1 : 0;
			parameters[8] = new SqlParameter("@Location", SqlDbType.NVarChar, 50);
			parameters[8].Value = device.Location;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("DECLARE @ID int");
			sql.AppendLine("INSERT INTO DevInfo (devIp, devPort, devType, devUserName, devPassword, antNo, accessFlag, flag, Location)");
			sql.AppendLine("VALUES (@IP, @Port, @DeviceType, @UserName, @Password, @AntNo, @AccessFlag, @Flag, @Location)");
			sql.AppendLine("SET @ID = SCOPE_IDENTITY()");
			sql.AppendLine("SELECT @ID");
			int newID = (int)helper.ExecuteScalar(sql.ToString(), parameters);
			return newID;
		}

		public static bool Update(DeviceInfo device) {
			SqlParameter[] parameters = new SqlParameter[10];
			parameters[0] = new SqlParameter("@IP", SqlDbType.VarChar, 30);
			parameters[0].Value = device.IP;
			parameters[1] = new SqlParameter("@Port", SqlDbType.VarChar, 10);
			parameters[1].Value = device.Port;
			parameters[2] = new SqlParameter("@DeviceType", SqlDbType.VarChar, 2);
			parameters[2].Value = device.DeviceType;
			parameters[3] = new SqlParameter("@UserName", SqlDbType.VarChar, 50);
			parameters[3].Value = device.UserName;
			parameters[4] = new SqlParameter("@Password", SqlDbType.VarChar, 50);
			parameters[4].Value = device.Password;
			parameters[5] = new SqlParameter("@AntNo", SqlDbType.Int);
			parameters[5].Value = device.AntNo;
			parameters[6] = new SqlParameter("@AccessFlag", SqlDbType.Int);
			parameters[6].Value = device.AccessFlag;
			parameters[7] = new SqlParameter("@Flag", SqlDbType.Int);
			parameters[7].Value = device.Active ? 1 : 0;
			parameters[8] = new SqlParameter("@ID", SqlDbType.Int);
			parameters[8].Value = device.ID;
			parameters[9] = new SqlParameter("@Location", SqlDbType.NVarChar, 50);
			parameters[9].Value = device.Location;

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("UPDATE DevInfo SET devIp = @IP, devPort = @Port");
			sql.AppendLine("	, devType = @DeviceType, devUserName = @UserName, devPassword = @Password");
			sql.AppendLine("	, antNo = @AntNo, accessFlag = @AccessFlag, flag = @Flag, Location = @Location");
			sql.AppendLine("WHERE ID = @ID");
			return helper.ExecuteNonQuery(sql.ToString(), parameters.ToArray()) > 0;
		}

		public static bool Delete(int ID) {
			int rowCount = helper.ExecuteNonQuery("DELETE FROM DevInfo WHERE ID = " + ID.ToString());
			return rowCount > 0;
		}

		public static DeviceInfo GetEntity(int ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM DevInfoView WHERE ID = " + ID.ToString());
			if (table.Rows.Count > 0) {
				return new DeviceInfo(table.Rows[0]);
			}
			return null;
		}

		public static DeviceInfo GetEntity(String code) {
			if (String.IsNullOrWhiteSpace(code)) {
				return null;
			}

			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = new SqlParameter("@Code", SqlDbType.VarChar, 20);
			parameters[0].Value = code;
			DataTable table = helper.ExecuteDataTable("SELECT * FROM DevInfoView WHERE logonUser = @Code", parameters);
			if (table.Rows.Count > 0) {
				return new DeviceInfo(table.Rows[0]);
			}
			return null;
		}

		public static DataTable GetList(bool ActiveOnly) {
			string sql = "SELECT * FROM DevInfoView" + (ActiveOnly ? " WHERE flag = 1":"") + " ORDER BY ID";
			return helper.ExecuteDataTable(sql);
		}

		public static DataTable GetListForMonitor() {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("DECLARE @maxTime smalldatetime");
			sql.AppendLine("SET @maxTime = (SELECT MAX(LastConnectedTime) FROM DevInfo)");
			sql.AppendLine("SELECT Sort = 0, Location = N'服务端程序', [Status] = (CASE WHEN ISNULL(DateDiff(MINUTE, @maxTime, GETDATE()), 999) < 5 THEN N'正常运行' ELSE N'异常退出' END)");
			sql.AppendLine("UNION ALL");
			sql.AppendLine("SELECT Sort = ID, Location");
			sql.AppendLine("	, Status = (CASE WHEN ConnectionStatus = 0 THEN N'连接正常' ELSE N'连接异常' END)");
			sql.AppendLine("FROM DevInfo");
			sql.AppendLine("WHERE flag = 1");
			sql.AppendLine("ORDER BY Sort");
			return helper.ExecuteDataTable(sql.ToString());
		}
	}
}
