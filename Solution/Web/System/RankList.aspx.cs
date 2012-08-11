using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class System_RankList : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		DataTable ranks = RankBiz.GetList();
		this.repeaterRank.DataSource = ranks;
		this.repeaterRank.DataBind();
	}
}