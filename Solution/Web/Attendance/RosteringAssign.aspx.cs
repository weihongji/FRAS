using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class Attendance_RosteringAssign : PrivilegePage
{

	private RosteringInfo m_Rostering;

	protected RosteringInfo Rostering {
		get {
			if (m_Rostering == null) {
				m_Rostering = RosteringBiz.GetEntity(GetQSInteger("id"));
			}
			return m_Rostering;
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		DataTable depts = DeptBiz.GetBriefList(LoginUserDeptID);
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();
	}
}