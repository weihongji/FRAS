using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using BLL;

public partial class Query_AttendanceOverviewContent : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		int deptId = GetQSInteger("deptId", -1);
		DateTime queryOn = DateTime.Today; //Convert.ToDateTime("2011-03-07")
		if (deptId >= 0) {
			DataTable table = WorkDurationBiz.GetWorkingUserList(deptId, queryOn);
			DataRow []rowArray = table.Select("durationId > 0", "userName");
			this.photoRepeater.DataSource = rowArray;
			this.photoRepeater.DataBind();

			StringBuilder msg = new StringBuilder();
			foreach (DataRow row in table.Rows) {
				if ((int)row["featureId"] == 0) {
					msg.AppendLine("<div>姓名【" + row["userName"].ToString() + "】工号【" + row["userId"].ToString() + "】尚未进行模板信息登记，无法记录考勤！</div>");
				}
				else if ((int)row["durationId"] == 0) {
					msg.AppendLine("<div>姓名【" + row["userName"].ToString() + "】工号【" + row["userId"].ToString() + "】为非考勤状态！</div>");
				}
			}
			if (rowArray.Length > 0) {
				msg.AppendLine(String.Format("【{0}】出勤人数为【{1}】！", Request.QueryString["deptName"], rowArray.Length));
			}
			else {
				msg.AppendLine(String.Format("【{0}】暂无人为出勤状态！", Request.QueryString["deptName"]));
			}
			this.litMsg.Text = msg.ToString();
		}
		else if (deptId == -1) {
			int count = WorkDurationBiz.GetWorkingUserCount(queryOn);
			this.litMsg.Text = String.Format("总考勤人数为:【{0}】,请选择具体部门查看！", count);
		}
	}

	protected string GetField(object dataItem, string fieldName) {
		return ((System.Data.DataRow)dataItem)[fieldName].ToString();
	}
}