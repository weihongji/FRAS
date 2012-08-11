using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Entity;

public partial class System_LogonUserAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "get":
				LogonUserInfo user = LogonUserBiz.GetEntity(GetQSInteger("id"));
				Response.Write(user.ToJson());
				break;
			case "save":
				int id = LogonUserBiz.Save(BuildEntity());
				Response.Write(id);
				break;
			case "delete":
				bool success = LogonUserBiz.Delete(GetFormInteger("id"));
				Response.Write(success ? "true" : "false");
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private LogonUserInfo BuildEntity() {
		LogonUserInfo user = new LogonUserInfo();
		user.ID = GetFormInteger("id");
		user.Code = Request.Form["code"];
		user.Password = Request.Form["password"];
		user.RoleType = GetFormInteger("role");
		user.DeptID = GetFormInteger("dept");
		user.Active = Request.Form["active"] == "true";
		return user;
	}
}