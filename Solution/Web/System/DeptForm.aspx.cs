using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class System_DeptForm : PrivilegePage
{
	public int DeptID {
		get {
			return this.GetQSInteger("id", -1);
		}
	}

	public string PageTitle {
		get {
			return DeptID < 0 ? "新增部门信息" : "修改部门信息";
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		this.Page.Title = PageTitle;
	}
}