using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Entity;

public partial class System_DeptAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		bool success;
		switch (queryType) {
			case "duplicate_new": // check duplication for adding a new dept
				int new_id = GetQSInteger("id", -1);
				string new_name = Request.QueryString["name"];
				string new_result = "";
				if (DeptBiz.Exist(new_id)) {
					new_result = "id";
				}
				else if (DeptBiz.Exist(-1, new_name)) {
					new_result = "name";
				}
				Response.Write(new_result);
				break;
			case "duplicate_exist": // check duplication for updating an existing dept
				string exist_result = "";
				if (DeptBiz.Exist(GetQSInteger("id", -1), Request.QueryString["name"])) {
					exist_result = "name";
				}
				Response.Write(exist_result);
				break;
			case "get":
				DeptInfo dept = DeptBiz.GetEntity(GetQSInteger("id", -1));
				Response.Write(dept.ToJson());
				break;
			case "save":
				success = DeptBiz.Save(BuildEntity());
				Response.Write(success ? "true" : "false");
				break;
			case "delete":
				success = DeptBiz.Delete(GetFormInteger("id", -1));
				Response.Write(success ? "true" : "false");
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private DeptInfo BuildEntity() {
		DeptInfo dept = new DeptInfo();
		dept.ID = GetFormInteger("id", -1);
		dept.Name = Request.Form["name"];
		return dept;
	}
}