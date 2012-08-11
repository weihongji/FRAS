using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class Attendance_RosteringList : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		DataTable rosterings = RosteringBiz.GetList();
		this.repeaterRostering.DataSource = rosterings;
		this.repeaterRostering.DataBind();
	}

	protected string GetSelect(int multType, int id) {
		return (multType == 1 || multType == 2) ? "<input type='checkbox' class='p' id='chk'" + id.ToString() + " value='" + id.ToString() + "' />" : ""; ;
	}

	protected string GetID(int multType, int id) {
		return (multType == 1 || multType == 2) ? id.ToString() : "";
	}

	protected string GetMultiple(int multType, string multipleName) {
		return (multType == 1 || multType == 2) ? multipleName : "";
	}

	protected string GetModify(int multType, int id) {
		return (multType == 1 || multType == 2) ? "<a href='RosteringForm.aspx?id=" + id.ToString() + "&type=" + multType.ToString() + "'>修改</a>" : "";
	}
}