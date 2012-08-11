using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class Attendance_HolidayForm : PrivilegePage
{
	public int HolidayID {
		get {
			return this.GetQSInteger("id");
		}
	}

	public string PageTitle {
		get {
			return HolidayID == 0 ? "新增节假日信息" : "修改节假日信息";
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		this.Page.Title = PageTitle;
	}
}