using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;
using BLL;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://www.SND.com/FRAS/20120721", Name="SND_FRAS_Service")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

	public WebService() {

		//Uncomment the following line if using designed components 
		//InitializeComponent(); 
	}

	/// <summary>
	/// Get data of all employees' attendance records on the specified date.
	/// The data is stored in a string list.
	/// </summary>
	/// <param name="Date"></param>
	/// <returns>
	/// A list of strings that are composed by columns in attendance records.
	/// But, the first string in the list is for column names.
	/// Attendance data starts from the second string.
	/// </returns>
	[WebMethod]
	public string[] GetKQReport(DateTime Date) {
		List<string> result = new List<string>();
		StringBuilder s = new StringBuilder();

		DataTable table = AttendanceBiz.GetReportList(-1, 0, -1, Date, Date);
		int columnCount = table.Columns.Count;

		//Column names
		result.Add("部门,工号,姓名,应出勤,日常出勤,迟到,早退,旷工,日常加班,节假日加班,前夜,后夜,入井,休假,事假,病假,工伤,年休,婚假,产假,丧假,探亲假,出差");

		//Attendance data
		foreach (DataRow row in table.Rows) {
			s.Clear();
			for (int i = 0; i < columnCount; i++) {
				if (i > 0) {
					s.Append(",");
				}
				if (row[i] != DBNull.Value) {
					s.Append(row[i].ToString());
				}
			}
			result.Add(s.ToString());
		}
		return result.ToArray();
	}

}
