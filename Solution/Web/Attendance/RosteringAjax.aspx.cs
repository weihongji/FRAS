using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using BLL;
using Entity;

public partial class Attendance_RosteringAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "get":
				RosteringInfo rostering = RosteringBiz.GetEntity(GetQSInteger("id"));
				Response.Write(rostering.ToJson());
				break;
			case "save":
				int id = RosteringBiz.Save(BuildEntity(), BuildEntity2());
				Response.Write(id);
				break;
			case "delete":
				bool success = RosteringBiz.Delete(GetFormInteger("id"));
				Response.Write(success ? "true" : "false");
				break;
			case "get_pr":
				DataTable table = RosteringBiz.GetPreRostering(Request.QueryString["userId"]);
				Response.Write(GetPreRosteringJson(table));
				break;
			case "save_pr":
				bool success_pr = RosteringBiz.SavePreRostering(Request.Form["userIds"], GetFormInteger("rosteringId"), GetFormInteger("start"), GetFormInteger("end"));
				Response.Write(success_pr ? "true" : "false");
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private RosteringInfo BuildEntity() {
		RosteringInfo rostering = new RosteringInfo();
		rostering.ID = GetFormInteger("id");
		rostering.Name = Request.Form["name"];
		rostering.StartTime = Request.Form["startTime"];
		rostering.EarlyRange = GetFormInteger("earlyRange");
		rostering.EndTime = Request.Form["endTime"];
		rostering.LateRange = GetFormInteger("lateRange");
		rostering.Duration = Convert.ToDecimal(Request.Form["duration"]);
		rostering.NightWork = GetFormInteger("nightWork");
		rostering.Active = Request.Form["active"] == "true";

		return rostering;
	}

	private RosteringInfo BuildEntity2() {
		if (string.IsNullOrEmpty(Request.Form["startTime2"])) {
			return null;
		}
		RosteringInfo rostering = new RosteringInfo();
		rostering.StartTime = Request.Form["startTime2"];
		rostering.EarlyRange = GetFormInteger("earlyRange2");
		rostering.EndTime = Request.Form["endTime2"];
		rostering.LateRange = GetFormInteger("lateRange2");
		rostering.Duration = Convert.ToDecimal(Request.Form["duration2"]);

		return rostering;
	}

	private string GetPreRosteringJson(DataTable table) {
		if (table == null || table.Rows.Count == 0) {
			return "{}";
		}
		DataRow row = table.Rows[0];
		StringBuilder s = new StringBuilder();
		s.Append("id: " + row["ID"].ToString());
		s.Append(", userId: " + row["userId"].ToString());
		s.Append(", names: ['" + GetRosteringName(row["d1Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d2Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d3Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d4Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d5Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d6Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d7Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d8Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d9Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d10Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d11Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d12Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d13Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d14Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d15Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d16Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d17Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d18Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d19Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d20Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d21Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d22Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d23Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d24Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d25Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d26Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d27Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d28Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d29Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d30Name"]) + "'");
		s.Append(", '" + GetRosteringName(row["d31Name"]) + "'");
		s.Append("]");
		return "{" + s.ToString() + "}";
	}

	private string GetRosteringName(object val) {
		if (val == DBNull.Value) {
			return "--";
		}
		else {
			return (string) val;
		}
	}
}