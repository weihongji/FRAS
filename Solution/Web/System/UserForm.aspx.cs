using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class System_UserForm : PrivilegePage
{
	public string UserID {
		get {
			return Request.QueryString["id"];
		}
	}

	public string PageTitle {
		get {
			return UserID == "0" ? "新增员工信息" : "修改员工信息";
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		this.Page.Title = PageTitle;

		DataTable depts = DeptBiz.GetBriefList();
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();

		DataTable ranks = RankBiz.GetBriefList();
		this.repeaterRank.DataSource = ranks;
		this.repeaterRank.DataBind();
	}
}