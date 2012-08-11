<%@ Application Language="C#" %>
<%@ Import Namespace="System.Data" %>
<script RunAt="server">

	void Application_Start(object sender, EventArgs e) {
		BuildMenus();
	}

	void Application_End(object sender, EventArgs e) {
		//  Code that runs on application shutdown

	}

	void Application_Error(object sender, EventArgs e) {
		string s = "Error in:   	" + Request.Url.ToString();
		if (Server.GetLastError().InnerException == null) {
			s += "\r\n" + GetExceptions(Server.GetLastError());
		}
		else { // In most cases, it should go here since Server.GetLastError() is the exception thrown by error handler and its InnerException is the real exception.
			s += "\r\n" + GetExceptions(Server.GetLastError().InnerException);
		}
		Utility.LogUtility.LogError(s);
	}

	void Session_Start(object sender, EventArgs e) {
		// Code that runs when a new session is started

	}

	void Session_End(object sender, EventArgs e) {
		// Code that runs when a session ends. 
		// Note: The Session_End event is raised only when the sessionstate mode
		// is set to InProc in the Web.config file. If session mode is set to StateServer 
		// or SQLServer, the event is not raised.

	}

	private void BuildMenus() {
		DataTable table = new DataTable();
		table.Columns.Add(new DataColumn("Module", typeof(System.String)));
		table.Columns.Add(new DataColumn("Name", typeof(System.String)));
		table.Columns.Add(new DataColumn("Url", typeof(System.String)));

		DataRow row;
		//Query
		row = table.NewRow();
		row["Module"] = "Query";
		row["Name"] = "考勤概况";
		row["Url"] = "~/Query/AttendanceOverview.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "Query";
		row["Name"] = "综合查询";
		row["Url"] = "~/Query/DurationQuery.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "Query";
		row["Name"] = "流水查询";
		row["Url"] = "~/Query/AccessQuery.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "Query";
		row["Name"] = "考勤报表";
		row["Url"] = "~/Query/AttendanceReport.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "Query";
		row["Name"] = "考勤汇总";
		row["Url"] = "~/Query/DailyAttendance.aspx";
		table.Rows.Add(row);
		
		//Attendance
		row = table.NewRow();
		row["Module"] = "Attendance";
		row["Name"] = "模板录入";
		row["Url"] = "~/Attendance/NewUserForm.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "Attendance";
		row["Name"] = "排班管理";
		row["Url"] = "~/Attendance/RosteringList.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "Attendance";
		row["Name"] = "请假管理";
		row["Url"] = "~/Attendance/LeaveList.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "Attendance";
		row["Name"] = "节假日管理";
		row["Url"] = "~/Attendance/HolidayList.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "Attendance";
		row["Name"] = "出勤管理";
		row["Url"] = "~/Attendance/AttendanceList.aspx";
		table.Rows.Add(row);
		
		//System
		row = table.NewRow();
		row["Module"] = "System";
		row["Name"] = "员工信息管理";
		row["Url"] = "~/System/UserList.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "System";
		row["Name"] = "部门信息管理";
		row["Url"] = "~/System/DeptList.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "System";
		row["Name"] = "职位信息管理";
		row["Url"] = "~/System/RankList.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "System";
		row["Name"] = "设备信息管理";
		row["Url"] = "~/System/DeviceList.aspx";
		table.Rows.Add(row);
		row = table.NewRow();
		row["Module"] = "System";
		row["Name"] = "登录帐号管理";
		row["Url"] = "~/System/LogonUserList.aspx";
		table.Rows.Add(row);

		Application["Features"] = table;
	}

	private string GetExceptions(Exception e, int layer = 1) {
		string s = "";
		string fileName;
		if (e != null) {
			System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(e, true);
			s += "Error Message:	" + e.Message + "\r\n";
			try {
				for (int i = 0; i < trace.FrameCount; i++) {
					fileName = trace.GetFrame(i).GetFileName();
					if (!string.IsNullOrEmpty(fileName)) {
						if (fileName.Length > 1 && fileName.Substring(1, 1) == ":") { // Capitalize the first character.
							fileName = fileName.Substring(0, 1).ToUpper() + fileName.Substring(1);
						}
						s += "Source File:	" + fileName + "   Line: " + trace.GetFrame(i).GetFileLineNumber() + "   Column: " + trace.GetFrame(i).GetFileColumnNumber() + "\r\n";
						break;
					}
				}
			}
			catch (Exception) {
			}
			s += "Stack Trace: \r\n" + e.StackTrace + "\r\n";
			if (e.InnerException != null) {
				if (layer == 1 && e.InnerException.InnerException == null) {
					s += "------------- Inner Exception -------------\r\n";
				}
				else {
					s += "------------- Inner Exception (" + layer.ToString() + ") -------------\r\n";
				}
				s += GetExceptions(e.InnerException, ++layer);
			}
		}
		return s;
	}
</script>
