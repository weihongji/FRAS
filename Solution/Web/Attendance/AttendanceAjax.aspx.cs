using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Entity;
using System.Data.OleDb;
using System.Data;
using System.Text;

public partial class Attendance_AttendanceAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		bool success = false;
		string msg;

		switch (queryType) {
			case "get":
				AttendanceInfo attendance = AttendanceBiz.GetEntity(GetQSInteger("id"));
				Response.Write(attendance.ToJson());
				break;
			case "save":
				int id = AttendanceBiz.Save(BuildEntity(), Request.Form["checkDuplicate"] == "true");
				Response.Write(id);
				break;
			case "batch_save":
				success = AttendanceBiz.BatchAdd(Request.Form["userIds"], Request.Form["date"], Convert.ToDecimal(Request.Form["duration"]), Request.Form["inWell"] == "true", GetFormInteger("nightWork"), out msg);
				if (string.IsNullOrEmpty(msg)) {
					Response.Write(success ? "true" : "false");
				}
				else {
					Response.Write("EXIST|" + msg);
				}
				break;
			case "delete":
				success = AttendanceBiz.Delete(GetFormInteger("id"));
				Response.Write(success ? "true" : "false");
				break;
			case "approve":
				success = AttendanceBiz.Approve(GetFormInteger("id"));
				Response.Write(success ? "true" : "false");
				break;
			case "import":
				string filePath = Server.MapPath("~/Import") + "\\" + (string.IsNullOrEmpty(Request.Form["name"]) ? Request.QueryString["name"] : Request.Form["name"]);
				int count = AttendanceBiz.Import(filePath, out msg);
				if (string.IsNullOrEmpty(msg)) {
					Response.Write(count);
				}
				else {
					Response.Write("EXIST|" + msg);
				}
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private AttendanceInfo BuildEntity() {
		AttendanceInfo attendance = new AttendanceInfo();
		attendance.ID = GetFormInteger("id");
		attendance.UserID = Request.Form["userId"];
		attendance.Date = Request.Form["date"];
		attendance.Duration = Convert.ToDecimal(Request.Form["duration"]);
		attendance.InWell = Request.Form["inWell"] == "true";
		attendance.NightWork = GetFormInteger("nightWork");

		return attendance;
	}

	private string Import(string filePath) {
		string mystring = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = '" + filePath + "'; Extended Properties=Excel 8.0";
		OleDbConnection cnnxls = new OleDbConnection(mystring);
		OleDbDataAdapter myDa = new OleDbDataAdapter("SELECT * FROM [Sheet1$] ", cnnxls);
		DataSet myDs = new DataSet();
		myDa.Fill(myDs);
		DataTable table = myDs.Tables[0];
		string s = "";
		foreach (DataRow row in table.Rows) {
			s += "<br/>z" + row.ItemArray[0].ToString() + "," + row.ItemArray[1].ToString() + "," + row.ItemArray[2].ToString() + "," + row.ItemArray[3].ToString() + "," + row.ItemArray[4].ToString();
		}
		return s;
	}
}