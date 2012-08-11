using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Entity;

public partial class System_RankAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "exist":
				bool exist = RankBiz.Exist(GetQSInteger("id"), Request.QueryString["name"]);
				Response.Write(exist ? "true" : "false");
				break;
			case "get":
				RankInfo rank = RankBiz.GetEntity(GetQSInteger("id"));
				Response.Write(rank.ToJson());
				break;
			case "save":
				int id = RankBiz.Save(BuildEntity());
				Response.Write(id);
				break;
			case "delete":
				bool success = RankBiz.Delete(GetFormInteger("id"));
				Response.Write(success ? "true" : "false");
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private RankInfo BuildEntity() {
		RankInfo rank = new RankInfo();
		rank.ID = GetFormInteger("id");
		rank.Name = Request.Form["name"];

		return rank;
	}
}