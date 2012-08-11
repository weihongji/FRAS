using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Entity;

public partial class Attendance_HolidayAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "get":
				HolidayInfo holiday = HolidayBiz.GetEntity(GetQSInteger("id"));
				Response.Write(holiday.ToJson());
				break;
			case "save":
				int id = HolidayBiz.Save(BuildEntity());
				Response.Write(id);
				break;
			case "delete":
				bool success = HolidayBiz.Delete(GetFormInteger("id"));
				Response.Write(success ? "true" : "false");
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private HolidayInfo BuildEntity() {
		HolidayInfo holiday = new HolidayInfo();
		holiday.ID = GetFormInteger("id");
		holiday.Name = Request.Form["name"];
		holiday.StartDate = Request.Form["startDate"];
		holiday.EndDate = Request.Form["endDate"];
		holiday.Active = Request.Form["active"] == "true";

		return holiday;
	}
}