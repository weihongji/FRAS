using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class Attendance_HolidayList : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		DataTable holidays = HolidayBiz.GetList();
		this.repeaterHoliday.DataSource = holidays;
		this.repeaterHoliday.DataBind();
	}
}