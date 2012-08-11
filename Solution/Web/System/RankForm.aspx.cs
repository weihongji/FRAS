using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class System_RankForm : PrivilegePage
{
	public int RankID {
		get {
			return this.GetQSInteger("id");
		}
	}

	public string PageTitle {
		get {
			return RankID <= 0 ? "新增职位信息" : "修改职位信息";
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		this.Page.Title = PageTitle;
	}
}