using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Entity;

namespace DAL
{
	public class ParamDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static ParamInfo GetEntity(int ID) {
			DataTable table = helper.ExecuteDataTable("SELECT * FROM Param WHERE ID = " + ID.ToString());
			if (table.Rows.Count > 0) {
				return new ParamInfo(table.Rows[0]);
			}
			return null;
		}

		public static DataTable GetList(XEnum.ParamType type) {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("SELECT * FROM Param");
			if (type != XEnum.ParamType.All) {
				sql.AppendLine("WHERE paraType = " + ((int)type).ToString());
			}
			sql.AppendLine("ORDER BY paraName");
			return helper.ExecuteDataTable(sql.ToString());
		}

		// Used to populate dropdown lists
		public static DataTable GetBriefList(XEnum.ParamType type) {
			string sql = "SELECT ID, Name = paraValue FROM Param WHERE paraType = " + ((int)type).ToString() + " ORDER BY paraName";
			return helper.ExecuteDataTable(sql);
		}
	}
}
