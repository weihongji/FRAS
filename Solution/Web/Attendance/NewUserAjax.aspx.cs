using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using BLL;
using Entity;

public partial class Attendance_NewUserAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "get":
				UserInfo user = null;
				if (!string.IsNullOrEmpty(Request.QueryString["id"])) {
					user = UserBiz.GetEntity(Request.QueryString["id"]);
				}
				else if (!string.IsNullOrEmpty(Request.QueryString["name"])) {
					user = UserBiz.GetEntityByName(Request.QueryString["name"]);
				}
				if (user == null) {
					Response.Write("{}");
				}
				else {
					Response.Write(user.ToJson());
				}
				break;
			case "save":
				bool success = UserBiz.UpdateCard(Request.Form["userId"], GetFormInteger("copyType"));
				if (success) {
					success = UserCardBiz.Save(BuildEntity());
				}
				Response.Write(success ? "true" : "false");
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private UserCardInfo BuildEntity() {
		UserCardInfo card = new UserCardInfo();
		card.CardID = Request.Form["cardNo"];
		card.UserID = Request.Form["userId"];
		return card;
	}
}