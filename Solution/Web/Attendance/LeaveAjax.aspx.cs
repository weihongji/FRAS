using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Entity;

public partial class Attendance_LeaveAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		bool success;
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "get":
				LeaveInfo leave = LeaveBiz.GetEntity(GetQSInteger("id"));
				Response.Write(leave.ToJson());
				break;
			case "save":
				int id = LeaveBiz.Save(BuildEntity());
				Response.Write(id);
				break;
			case "delete":
				success = LeaveBiz.Delete(GetFormInteger("id"));
				Response.Write(success ? "true" : "false");
				break;
			case "approve":
				success = LeaveBiz.Approve(GetFormInteger("id"));
				Response.Write(success ? "true" : "false");
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private LeaveInfo BuildEntity() {
		LeaveInfo leave = new LeaveInfo();
		leave.ID = GetFormInteger("id");
		leave.UserID = Request.Form["userId"];
		leave.StartDate = Request.Form["startDate"];
		leave.EndDate = Request.Form["endDate"];
		leave.Type = GetFormInteger("type");

		return leave;
	}
}