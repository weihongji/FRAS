using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class Attendance_RosteringForm : PrivilegePage
{
	public int RosteringID {
		get {
			return this.GetQSInteger("id");
		}
	}

	public string PageTitle {
		get {
			return RosteringID == 0 ? "新增班次" : "修改班次";
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		this.Page.Title = PageTitle;
	}
}