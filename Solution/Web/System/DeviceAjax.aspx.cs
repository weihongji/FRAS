using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Entity;

public partial class System_DeviceAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "get":
				DeviceInfo user = DeviceBiz.GetEntity(GetQSInteger("id"));
				Response.Write(user.ToJson());
				break;
			case "save":
				int id = DeviceBiz.Save(BuildEntity());
				Response.Write(id);
				break;
			case "delete":
				bool success = DeviceBiz.Delete(GetFormInteger("id"));
				Response.Write(success ? "true" : "false");
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private DeviceInfo BuildEntity() {
		DeviceInfo user = new DeviceInfo();
		user.ID = GetFormInteger("id");
		user.IP = Request.Form["ip"];
		user.Port = Request.Form["port"];
		user.DeviceType = Request.Form["deviceType"];
		user.UserName = Request.Form["userName"];
		user.Password = Request.Form["password"];
		user.AntNo = GetFormInteger("antNo");
		user.AccessFlag = GetFormInteger("accessFlag");
		user.Active = Request.Form["active"] == "true";
		return user;
	}
}