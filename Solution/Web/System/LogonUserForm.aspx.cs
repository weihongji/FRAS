using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class System_LogonUserForm : PrivilegePage
{
	public int UserID {
		get {
			return this.GetQSInteger("id");
		}
	}

	public string PageTitle {
		get {
			return UserID == 0 ? "新增登录帐号" : "修改登录帐号";
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		this.Page.Title = PageTitle;
		DataTable depts = DeptBiz.GetBriefList();
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();
	}
}