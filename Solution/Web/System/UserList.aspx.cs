using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class System_UserList : PrivilegePage
{
	protected string ListQS {
		get {
			string qs = Request.ServerVariables["QUERY_STRING"];
			return string.IsNullOrEmpty(qs) ? "" : "&" + qs;
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		if (GetQSInteger("show") == 1) {
			DataTable users = UserBiz.GetList(GetQSInteger("dept", -1), Request.QueryString["name"]);
			this.repeaterUser.DataSource = users;
			this.repeaterUser.DataBind();
		}

		DataTable depts = DeptBiz.GetBriefList();
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();
	}
}