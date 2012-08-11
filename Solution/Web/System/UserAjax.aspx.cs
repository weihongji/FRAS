using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using BLL;
using Entity;

public partial class System_UserAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "get":
				UserInfo user = UserBiz.GetEntity(Request.QueryString["id"]);
				Response.Write(user.ToJson());
				break;
			case "save":
				string id = UserBiz.Save(BuildEntity());
				Response.Write(id);
				break;
			case "delete":
				bool success = UserBiz.Delete(Request.Form["id"]);
				Response.Write(success ? "true" : "false");
				break;
			case "brief_list":
				Response.Write(GetBriefList());
				break;
			case "rostering_list":
				Response.Write(GetBriefListForRostering());
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private UserInfo BuildEntity() {
		UserInfo user = new UserInfo();
		user.ID = Request.Form["id"];
		user.Name = Request.Form["name"];
		user.DeptID = GetFormInteger("deptId");
		user.RankID = GetFormInteger("rankId");
		user.SenderID = Request.Form["senderId"];
		user.Type = GetFormInteger("type");
		return user;
	}

	private string GetBriefList() {
		StringBuilder s = new StringBuilder();
		DataTable table = UserBiz.GetBriefList(GetQSInteger("deptId", -1));
		foreach (DataRow row in table.Rows) {
			s.Append(", {id:'" + row["ID"].ToString() + "', name:'" + row["Name"].ToString() + "'}");
		}
		return "[" + (s.Length > 0 ? s.ToString().Substring(2) : "") + "]";
	}

	private string GetBriefListForRostering() {
		StringBuilder s = new StringBuilder();
		DataTable table = UserBiz.GetBriefListForRostering(GetQSInteger("deptId", -1));
		foreach (DataRow row in table.Rows) {
			s.Append(", {id:'" + row["ID"].ToString() + "', name:'" + row["Name"].ToString() + "'}");
		}
		return "[" + (s.Length > 0 ? s.ToString().Substring(2) : "") + "]";
	}
}