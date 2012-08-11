using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;
using System.Drawing;
using System.Reflection;
using BLL;
using Entity;

public partial class Query_AttendanceAjax : PrivilegePage
{
	private DateTime m_StartDate;
	private DateTime m_EndDate;
	private bool m_IsMonthly;

	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "export":
				string result = ExportExcel();
				Response.Write(result);
				break;
			default:
				Response.Write("Unknown query type: " + queryType);
				break;
		}
	}

	private System.Data.DataTable GetReportList() {
		int dept = GetQSInteger("dept", -1);
		int device = GetQSInteger("device");
		int userType = GetQSInteger("usertype", -1);

		if (String.IsNullOrEmpty(Request.QueryString["year"])) {
			m_IsMonthly = false;
			m_StartDate = Convert.ToDateTime(Request.QueryString["start"]);
			m_EndDate = Convert.ToDateTime(Request.QueryString["end"]);
			return AttendanceBiz.GetReportList(dept, device, userType, m_StartDate, m_EndDate);
		}
		else {
			m_IsMonthly = true;
			int year = GetQSInteger("year");
			int month = GetQSInteger("month");
			m_StartDate = new DateTime(year, month, 1);
			m_EndDate = Utility.DateUtility.GetLastDay(m_StartDate);
			return AttendanceBiz.GetReportList(dept, device, userType, year, month);
		}
	}

	private string ExportExcel() {
		System.Data.DataTable table = GetReportList();
		if (m_IsMonthly) {
			return ExportByMonth(table);
		}
		else {
			return ExportByDateRange(table);
		}
	}

	private string ExportByMonth(System.Data.DataTable table) {
		string[] symbols = { "△", "○", "□", "◈", "◇", "☆", "⊙", "▣", "◎", "*", "√", "#" };
		string fileName; //导出的Excel报表文件名
		fileName = "月考勤统计表_" + m_StartDate.ToString("yyMMdd") + "_" + m_EndDate.ToString("yyMMdd") + ".xls";

		Microsoft.Office.Interop.Excel.Application theExcelApp = new Microsoft.Office.Interop.Excel.Application();
		Workbook theExcelBook = theExcelApp.Workbooks.Add(true);
		Worksheet theSheet = (Worksheet)theExcelBook.ActiveSheet;
		Range theCell;

		int rowCount = 3 + table.Rows.Count; // 报表总行数
		int columnCount = 53; // 报表总列数

		//整体设置
		theSheet.Cells.Font.Name = "宋体";
		theSheet.Cells.Font.Size = 10;
		theSheet.Cells.RowHeight = 14.25;

		//标题
		theCell = theSheet.Range[theSheet.Cells[1, 1], theSheet.Cells[1, columnCount]];
		theCell.Merge();
		theCell.RowHeight = 30;
		theCell.Value2 = "员  工  考  勤  报  表";
		theCell.Font.Name = "华文中宋";
		theCell.Font.Size = 16;
		theCell.Font.Bold = true;
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;

		//制表时间
		theCell = theSheet.Range[theSheet.Cells[2, 1], theSheet.Cells[2, 3]];
		theCell.Merge();
		theCell.Value2 = "时间: " + m_StartDate.ToString("yyyy-MM-dd") + " 至 " + m_EndDate.ToString("yyyy-MM-dd");
		theCell = theSheet.Range[theSheet.Cells[2, 18], theSheet.Cells[2, 30]];
		theCell.Merge();
		theCell.Value2 = "制表时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //2012-05-13 14:41:14

		//表头整体
		theCell = theSheet.Range[theSheet.Cells[3, 1], theSheet.Cells[3, columnCount]];
		theCell.RowHeight = 42; // height of 3 normal rows
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
		theCell.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

		//表头各列
		((Range)theSheet.Cells[3, 1]).Value2 = "单位名称";
		((Range)theSheet.Cells[3, 1]).ColumnWidth = 12;
		((Range)theSheet.Cells[3, 2]).Value2 = "工号";
		((Range)theSheet.Cells[3, 2]).ColumnWidth = 9;
		((Range)theSheet.Cells[3, 3]).Value2 = "姓名";
		((Range)theSheet.Cells[3, 3]).ColumnWidth = 12;
		for (int d = 1; d <= 31; d++) {
			((Range)theSheet.Cells[3, d + 3]).Value2 = d.ToString();
			((Range)theSheet.Cells[3, d + 3]).ColumnWidth = 2;
		}
		((Range)theSheet.Cells[3, 35]).Value2 = "出勤";
		((Range)theSheet.Cells[3, 35]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 36]).Value2 = "迟到";
		((Range)theSheet.Cells[3, 36]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 37]).Value2 = "早退";
		((Range)theSheet.Cells[3, 37]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 38]).Value2 = "旷工";
		((Range)theSheet.Cells[3, 38]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 39]).Value2 = "日常\r\n加班";
		((Range)theSheet.Cells[3, 39]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 40]).Value2 = "节日\r\n加班";
		((Range)theSheet.Cells[3, 40]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 41]).Value2 = "前夜";
		((Range)theSheet.Cells[3, 41]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 42]).Value2 = "后夜";
		((Range)theSheet.Cells[3, 42]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 43]).Value2 = "入井";
		((Range)theSheet.Cells[3, 43]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 44]).Value2 = "休假";
		((Range)theSheet.Cells[3, 44]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 45]).Value2 = "事假";
		((Range)theSheet.Cells[3, 45]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 46]).Value2 = "病假";
		((Range)theSheet.Cells[3, 46]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 47]).Value2 = "工伤";
		((Range)theSheet.Cells[3, 47]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 48]).Value2 = "年休";
		((Range)theSheet.Cells[3, 48]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 49]).Value2 = "婚假";
		((Range)theSheet.Cells[3, 49]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 50]).Value2 = "产假";
		((Range)theSheet.Cells[3, 50]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 51]).Value2 = "丧假";
		((Range)theSheet.Cells[3, 51]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 52]).Value2 = "探亲假";
		((Range)theSheet.Cells[3, 52]).ColumnWidth = 4.5;
		((Range)theSheet.Cells[3, 53]).Value2 = "出差";
		((Range)theSheet.Cells[3, 53]).ColumnWidth = 4;

		//数据行整体
		theCell = theSheet.Range[theSheet.Cells[4, 1], theSheet.Cells[rowCount, columnCount]];
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
		theCell.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

		//填充数据行
		System.Data.DataRow row;
		int flag;
		for (int i = 0; i < table.Rows.Count; i++) {
			row = table.Rows[i];
			((Range)theSheet.Cells[i + 4, 1]).Value2 = row["deptName"].ToString().Trim();
			((Range)theSheet.Cells[i + 4, 2]).Value2 = "'" + row["userId"].ToString().Trim();
			((Range)theSheet.Cells[i + 4, 3]).Value2 = row["userName"];
			for (int d = 1; d <= 31; d++) {
				if (row["d" + d.ToString()] != DBNull.Value) {
					flag = Convert.ToInt32(row["d" + d.ToString()]);
					((Range)theSheet.Cells[i + 4, d + 3]).Value2 = symbols[flag-1];
				}
			}
			((Range)theSheet.Cells[i + 4, 35]).Value2 = row["normal"];
			((Range)theSheet.Cells[i + 4, 36]).Value2 = row["late"];
			((Range)theSheet.Cells[i + 4, 37]).Value2 = row["quit"];
			((Range)theSheet.Cells[i + 4, 38]).Value2 = row["kuang"];
			((Range)theSheet.Cells[i + 4, 39]).Value2 = row["overtime_non_holiday"];
			((Range)theSheet.Cells[i + 4, 40]).Value2 = row["overtime_holiday"];
			((Range)theSheet.Cells[i + 4, 41]).Value2 = row["front"];
			((Range)theSheet.Cells[i + 4, 42]).Value2 = row["back"];
			((Range)theSheet.Cells[i + 4, 43]).Value2 = row["well"];
			((Range)theSheet.Cells[i + 4, 44]).Value2 = row["leave1"];
			((Range)theSheet.Cells[i + 4, 45]).Value2 = row["leave2"];
			((Range)theSheet.Cells[i + 4, 46]).Value2 = row["leave3"];
			((Range)theSheet.Cells[i + 4, 47]).Value2 = row["leave4"];
			((Range)theSheet.Cells[i + 4, 48]).Value2 = row["leave5"];
			((Range)theSheet.Cells[i + 4, 49]).Value2 = row["leave6"];
			((Range)theSheet.Cells[i + 4, 50]).Value2 = row["leave7"];
			((Range)theSheet.Cells[i + 4, 51]).Value2 = row["leave8"];
			((Range)theSheet.Cells[i + 4, 52]).Value2 = row["leave9"];
			((Range)theSheet.Cells[i + 4, 53]).Value2 = row["leave10"];
		}
		//末尾标注
		theCell = theSheet.Range[theSheet.Cells[table.Rows.Count + 5, 4], theSheet.Cells[table.Rows.Count + 5, columnCount]];
		theCell.Merge();
		theCell.RowHeight = 26.25;
		theCell.Value2 = "出勤 √    入井 #   休假△   事假 ○   病假 □   工伤 ◈   年休 ◇   婚假 ☆   产假 ⊙   丧假▣   探亲◎   出差 *";

		//将生成的Excel报表存储到Export文件夹中
		theExcelBook.SaveCopyAs(Server.MapPath("../Export") + "\\" + fileName);
		theExcelBook.Close(false, null, null);
		theExcelApp.Quit();
		System.Runtime.InteropServices.Marshal.ReleaseComObject(theSheet);
		System.Runtime.InteropServices.Marshal.ReleaseComObject(theExcelBook);
		System.Runtime.InteropServices.Marshal.ReleaseComObject(theExcelApp);
		GC.Collect();

		return fileName;
	}

	private string ExportByDateRange(System.Data.DataTable table) {
		string fileName; //导出的Excel报表文件名
		fileName = "职工考勤报表_" + m_StartDate.ToString("yyMMdd") + "_" + m_EndDate.ToString("yyMMdd") + ".xls";

		Microsoft.Office.Interop.Excel.Application theExcelApp = new Microsoft.Office.Interop.Excel.Application();
		Workbook theExcelBook = theExcelApp.Workbooks.Add(true);
		Worksheet theSheet = (Worksheet)theExcelBook.ActiveSheet;
		Range theCell;

		int rowCount = 3 + table.Rows.Count; // 报表总行数
		int columnCount = 23; // 报表总列数

		//整体设置
		theSheet.Cells.Font.Name = "宋体";
		theSheet.Cells.Font.Size = 10;
		theSheet.Cells.RowHeight = 14.25;

		//标题
		theCell = theSheet.Range[theSheet.Cells[1, 1], theSheet.Cells[1, columnCount]];
		theCell.Merge();
		theCell.RowHeight = 30;
		theCell.Value2 = "职工考勤报表";
		theCell.Font.Name = "华文中宋";
		theCell.Font.Size = 16;
		theCell.Font.Bold = true;
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;

		//制表时间
		theCell = theSheet.Range[theSheet.Cells[2, 1], theSheet.Cells[2, 3]];
		theCell.Merge();
		theCell.Value2 = "时间: " + m_StartDate.ToString("yyyy-MM-dd") + " 至 " + m_EndDate.ToString("yyyy-MM-dd");
		theCell = theSheet.Range[theSheet.Cells[2, 18], theSheet.Cells[2, 23]];
		theCell.Merge();
		theCell.Value2 = "制表时间: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //2012-05-13 14:41:14

		//表头整体
		theCell = theSheet.Range[theSheet.Cells[3, 1], theSheet.Cells[3, columnCount]];
		theCell.RowHeight = 42; // height of 3 normal rows
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
		theCell.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);
		theCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.ColorTranslator.FromOle(0xFFFFCC));

		//表头各列
		((Range)theSheet.Cells[3, 1]).Value2 = "部门";
		((Range)theSheet.Cells[3, 1]).ColumnWidth = 12;
		((Range)theSheet.Cells[3, 2]).Value2 = "工号";
		((Range)theSheet.Cells[3, 2]).ColumnWidth = 9;
		((Range)theSheet.Cells[3, 3]).Value2 = "姓名";
		((Range)theSheet.Cells[3, 3]).ColumnWidth = 12;
		((Range)theSheet.Cells[3, 4]).Value2 = "应出勤";
		((Range)theSheet.Cells[3, 4]).ColumnWidth = 6;
		((Range)theSheet.Cells[3, 5]).Value2 = "日常出勤";
		((Range)theSheet.Cells[3, 5]).ColumnWidth = 6.5;
		((Range)theSheet.Cells[3, 6]).Value2 = "迟到";
		((Range)theSheet.Cells[3, 6]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 7]).Value2 = "早退";
		((Range)theSheet.Cells[3, 7]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 8]).Value2 = "旷工";
		((Range)theSheet.Cells[3, 8]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 9]).Value2 = "日常加班";
		((Range)theSheet.Cells[3, 9]).ColumnWidth = 6.5;
		((Range)theSheet.Cells[3, 10]).Value2 = "节假日\r\n加班";
		((Range)theSheet.Cells[3, 10]).ColumnWidth = 5.5;
		((Range)theSheet.Cells[3, 11]).Value2 = "前夜";
		((Range)theSheet.Cells[3, 11]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 12]).Value2 = "后夜";
		((Range)theSheet.Cells[3, 12]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 13]).Value2 = "入井";
		((Range)theSheet.Cells[3, 13]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 14]).Value2 = "休假";
		((Range)theSheet.Cells[3, 14]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 15]).Value2 = "事假";
		((Range)theSheet.Cells[3, 15]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 16]).Value2 = "病假";
		((Range)theSheet.Cells[3, 16]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 17]).Value2 = "工伤";
		((Range)theSheet.Cells[3, 17]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 18]).Value2 = "年休";
		((Range)theSheet.Cells[3, 18]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 19]).Value2 = "婚假";
		((Range)theSheet.Cells[3, 19]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 20]).Value2 = "产假";
		((Range)theSheet.Cells[3, 20]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 21]).Value2 = "丧假";
		((Range)theSheet.Cells[3, 21]).ColumnWidth = 4;
		((Range)theSheet.Cells[3, 22]).Value2 = "探亲假";
		((Range)theSheet.Cells[3, 22]).ColumnWidth = 4.5;
		((Range)theSheet.Cells[3, 23]).Value2 = "出差";
		((Range)theSheet.Cells[3, 23]).ColumnWidth = 4;

		//数据行整体
		theCell = theSheet.Range[theSheet.Cells[4, 1], theSheet.Cells[rowCount, columnCount]];
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
		theCell.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

		//填充数据行
		System.Data.DataRow row;
		for (int i = 0; i < table.Rows.Count; i++) {
			row = table.Rows[i];
			((Range)theSheet.Cells[i + 4, 1]).Value2 = row["deptName"].ToString().Trim();
			((Range)theSheet.Cells[i + 4, 2]).Value2 = "'" + row["userId"].ToString().Trim();
			((Range)theSheet.Cells[i + 4, 3]).Value2 = row["userName"];
			((Range)theSheet.Cells[i + 4, 4]).Value2 = row["must"];
			((Range)theSheet.Cells[i + 4, 5]).Value2 = row["normal"];
			((Range)theSheet.Cells[i + 4, 6]).Value2 = row["late"];
			((Range)theSheet.Cells[i + 4, 7]).Value2 = row["quit"];
			((Range)theSheet.Cells[i + 4, 8]).Value2 = row["kuang"];
			((Range)theSheet.Cells[i + 4, 9]).Value2 = row["overtime_non_holiday"];
			((Range)theSheet.Cells[i + 4, 10]).Value2 = row["overtime_holiday"];
			((Range)theSheet.Cells[i + 4, 11]).Value2 = row["front"];
			((Range)theSheet.Cells[i + 4, 12]).Value2 = row["back"];
			((Range)theSheet.Cells[i + 4, 13]).Value2 = row["well"];
			((Range)theSheet.Cells[i + 4, 14]).Value2 = row["leave1"];
			((Range)theSheet.Cells[i + 4, 15]).Value2 = row["leave2"];
			((Range)theSheet.Cells[i + 4, 16]).Value2 = row["leave3"];
			((Range)theSheet.Cells[i + 4, 17]).Value2 = row["leave4"];
			((Range)theSheet.Cells[i + 4, 18]).Value2 = row["leave5"];
			((Range)theSheet.Cells[i + 4, 19]).Value2 = row["leave6"];
			((Range)theSheet.Cells[i + 4, 20]).Value2 = row["leave7"];
			((Range)theSheet.Cells[i + 4, 21]).Value2 = row["leave8"];
			((Range)theSheet.Cells[i + 4, 22]).Value2 = row["leave9"];
			((Range)theSheet.Cells[i + 4, 23]).Value2 = row["leave10"];
		}

		//将生成的Excel报表存储到Export文件夹中
		theExcelBook.SaveCopyAs(Server.MapPath("../Export") + "\\" + fileName);
		theExcelBook.Close(false, null, null);
		theExcelApp.Quit();
		System.Runtime.InteropServices.Marshal.ReleaseComObject(theSheet);
		System.Runtime.InteropServices.Marshal.ReleaseComObject(theExcelBook);
		System.Runtime.InteropServices.Marshal.ReleaseComObject(theExcelApp);
		GC.Collect();

		return fileName;
	}
}