using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class System_DeptList : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		DataTable depts = DeptBiz.GetList();
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();
	}
}