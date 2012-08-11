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

public partial class Query_DurationAjax : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		string result;
		switch (queryType) {
			case "export":
				result = ExportExcel();
				Response.Write(result);
				break;
			case "export_daily":
				result = ExportDailyExcel();
				Response.Write(result);
				break;
			default:
				Response.Write("Unknown query type: " + queryType);
				break;
		}
	}

	private string ExportExcel() {
		string userName = Request.QueryString["name"];
		int userType = GetQSInteger("usertype", -1);
		int dept = GetQSInteger("dept", -1);
		int device = GetQSInteger("device");
		DateTime start = Convert.ToDateTime(Request.QueryString["start"]);
		DateTime end = Convert.ToDateTime(Request.QueryString["end"]);
		System.Data.DataTable table;
		string fileName; //导出的Excel报表文件名

		table = WorkDurationBiz.GetList(userName, userType, dept, device, start, end);
		fileName = "综合查询报表_" + start.ToString("yyMMdd") + "_" + end.ToString("yyMMdd") + ".xls";

		Microsoft.Office.Interop.Excel.Application theExcelApp = new Microsoft.Office.Interop.Excel.Application();
		Workbook theExcelBook = theExcelApp.Workbooks.Add(true);
		Worksheet theSheet = (Worksheet)theExcelBook.ActiveSheet;
		Range theCell;

		int rowCount = 3 + table.Rows.Count; // 报表总行数
		int columnCount = 10; // 报表总列数

		//整体设置
		theSheet.Cells.Font.Name = "宋体";
		theSheet.Cells.Font.Size = 10;
		theSheet.Cells.RowHeight = 14.25;

		//标题
		theCell = theSheet.Range[theSheet.Cells[1, 1], theSheet.Cells[1, columnCount]];
		theCell.Merge();
		theCell.RowHeight = 30;
		theCell.Value2 = "职工考勤流水综合报表";
		theCell.Font.Name = "华文中宋";
		theCell.Font.Size = 16;
		theCell.Font.Bold = true;
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;

		//制表时间
		theCell = theSheet.Range[theSheet.Cells[2, 1], theSheet.Cells[2, 3]];
		theCell.Merge();
		theCell.Value2 = "时间: " + start.ToString("yyyy-MM-dd") + " 至 " + end.ToString("yyyy-MM-dd");
		theCell = theSheet.Range[theSheet.Cells[2, 9], theSheet.Cells[2, 10]];
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
		((Range)theSheet.Cells[3, 4]).Value2 = "签到时间";
		((Range)theSheet.Cells[3, 4]).ColumnWidth = 20;
		((Range)theSheet.Cells[3, 5]).Value2 = "签退时间";
		((Range)theSheet.Cells[3, 5]).ColumnWidth = 20;
		((Range)theSheet.Cells[3, 6]).Value2 = "工作时长";
		((Range)theSheet.Cells[3, 6]).ColumnWidth = 10;
		((Range)theSheet.Cells[3, 7]).Value2 = "工时";
		((Range)theSheet.Cells[3, 7]).ColumnWidth = 7;
		((Range)theSheet.Cells[3, 8]).Value2 = "考勤类型";
		((Range)theSheet.Cells[3, 8]).ColumnWidth = 7.5;
		((Range)theSheet.Cells[3, 9]).Value2 = "备注";
		((Range)theSheet.Cells[3, 9]).ColumnWidth = 18;
		((Range)theSheet.Cells[3, 10]).Value2 = "前后夜";
		((Range)theSheet.Cells[3, 10]).ColumnWidth = 8;

		//数据行整体
		theCell = theSheet.Range[theSheet.Cells[4, 1], theSheet.Cells[rowCount, columnCount]];
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
		theCell.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

		//填充数据行
		System.Data.DataRow row;
		for (int i = 0; i < table.Rows.Count; i++ ) {
			row = table.Rows[i];
			((Range)theSheet.Cells[i + 4, 1]).Value2 = row["DeptName"].ToString().Trim();
			((Range)theSheet.Cells[i + 4, 2]).Value2 = "'" + row["userId"].ToString().Trim();
			((Range)theSheet.Cells[i + 4, 3]).Value2 = row["UserName"];
			((Range)theSheet.Cells[i + 4, 4]).Value2 = row["bak2"];
			((Range)theSheet.Cells[i + 4, 5]).Value2 = row["bak3"];
			((Range)theSheet.Cells[i + 4, 6]).Value2 = row["durations"];
			((Range)theSheet.Cells[i + 4, 7]).Value2 = row["muldur"];
			((Range)theSheet.Cells[i + 4, 8]).Value2 = row["workType"];
			((Range)theSheet.Cells[i + 4, 9]).Value2 = row["comment"];
			((Range)theSheet.Cells[i + 4, 10]).Value2 = row["NightWorkName"];
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

	private string ExportDailyExcel() {
		DateTime date = Convert.ToDateTime(Request.QueryString["start"]);
		System.Data.DataTable table;
		string fileName; //导出的Excel报表文件名

		table = WorkDurationBiz.GetDailyAttendance(date);
		fileName = "考勤汇总_" + date.ToString("yyMMdd") + ".xls";

		Microsoft.Office.Interop.Excel.Application theExcelApp = new Microsoft.Office.Interop.Excel.Application();
		Workbook theExcelBook = theExcelApp.Workbooks.Add(true);
		Worksheet theSheet = (Worksheet)theExcelBook.ActiveSheet;
		Range theCell;

		int rowCount = 3 + table.Rows.Count; // 报表总行数
		int columnCount = 13; // 报表总列数

		//整体设置
		theSheet.Cells.Font.Name = "宋体";
		theSheet.Cells.Font.Size = 12;
		theSheet.Cells.RowHeight = 14.25;

		//标题
		theCell = theSheet.Range[theSheet.Cells[1, 1], theSheet.Cells[1, columnCount]];
		theCell.Merge();
		theCell.RowHeight = 21.75;
		theCell.Value2 = date.Month.ToString() + "月" + date.Day.ToString() + "日 考勤汇总";
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;

		//表头整体
		theCell = theSheet.Range[theSheet.Cells[2, 1], theSheet.Cells[3, columnCount]];
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
		theCell.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

		//表头各列
		theCell = theSheet.Range[theSheet.Cells[2, 1], theSheet.Cells[3, 1]];
		theCell.Merge();
		theCell.Value2 = "单位";
		theCell.ColumnWidth = 14.38;
		theCell = theSheet.Range[theSheet.Cells[2, 2], theSheet.Cells[3, 2]];
		theCell.Merge();
		theCell.Value2 = "在册人数";
		theCell.ColumnWidth = 8.38;
		theCell = theSheet.Range[theSheet.Cells[2, 3], theSheet.Cells[2, 4]];
		theCell.Merge();
		theCell.Value2 = "8点班";
		theCell = theSheet.Range[theSheet.Cells[2, 5], theSheet.Cells[2, 6]];
		theCell.Merge();
		theCell.Value2 = "4点班";
		theCell = theSheet.Range[theSheet.Cells[2, 7], theSheet.Cells[2, 8]];
		theCell.Merge();
		theCell.Value2 = "0点班";
		theCell = theSheet.Range[theSheet.Cells[2, 9], theSheet.Cells[2, 10]];
		theCell.Merge();
		theCell.Value2 = "白班";
		theCell = theSheet.Range[theSheet.Cells[2, 11], theSheet.Cells[3, 11]];
		theCell.Merge();
		theCell.Value2 = "休假";
		theCell = theSheet.Range[theSheet.Cells[2, 12], theSheet.Cells[3, 12]];
		theCell.Merge();
		theCell.Value2 = "请假";
		theCell = theSheet.Range[theSheet.Cells[2, 13], theSheet.Cells[3, 13]];
		theCell.Merge();
		theCell.Value2 = "缺勤";

		((Range)theSheet.Cells[3, 3]).Value2 = "井下";
		((Range)theSheet.Cells[3, 3]).ColumnWidth = 8.38;
		((Range)theSheet.Cells[3, 4]).Value2 = "地面";
		((Range)theSheet.Cells[3, 4]).ColumnWidth = 8.38;
		((Range)theSheet.Cells[3, 5]).Value2 = "井下";
		((Range)theSheet.Cells[3, 5]).ColumnWidth = 8.38;
		((Range)theSheet.Cells[3, 6]).Value2 = "地面";
		((Range)theSheet.Cells[3, 6]).ColumnWidth = 8.38;
		((Range)theSheet.Cells[3, 7]).Value2 = "井下";
		((Range)theSheet.Cells[3, 7]).ColumnWidth = 8.38;
		((Range)theSheet.Cells[3, 8]).Value2 = "地面";
		((Range)theSheet.Cells[3, 8]).ColumnWidth = 8.38;
		((Range)theSheet.Cells[3, 9]).Value2 = "井下";
		((Range)theSheet.Cells[3, 9]).ColumnWidth = 8.38;
		((Range)theSheet.Cells[3, 10]).Value2 = "地面";
		((Range)theSheet.Cells[3, 10]).ColumnWidth = 8.38;


		//数据行整体
		theCell = theSheet.Range[theSheet.Cells[4, 1], theSheet.Cells[rowCount, columnCount]];
		theCell.Borders.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black);

		theCell = theSheet.Range[theSheet.Cells[4, 2], theSheet.Cells[rowCount, columnCount]];
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;

		//填充数据行
		System.Data.DataRow row;
		for (int i = 0; i < table.Rows.Count; i++) {
			row = table.Rows[i];
			((Range)theSheet.Cells[i + 4, 1]).Value2 = row["DeptName"].ToString();
			((Range)theSheet.Cells[i + 4, 2]).Value2 = row["UserCount"];
			((Range)theSheet.Cells[i + 4, 3]).Value2 = row["C_8_W"];
			((Range)theSheet.Cells[i + 4, 4]).Value2 = row["C_8_G"];
			((Range)theSheet.Cells[i + 4, 5]).Value2 = row["C_4_W"];
			((Range)theSheet.Cells[i + 4, 6]).Value2 = row["C_4_G"];
			((Range)theSheet.Cells[i + 4, 7]).Value2 = row["C_0_W"];
			((Range)theSheet.Cells[i + 4, 8]).Value2 = row["C_0_G"];
			((Range)theSheet.Cells[i + 4, 9]).Value2 = row["C_D_W"];
			((Range)theSheet.Cells[i + 4, 10]).Value2 = row["C_D_G"];
			((Range)theSheet.Cells[i + 4, 11]).Value2 = row["XiuJia"];
			((Range)theSheet.Cells[i + 4, 12]).Value2 = row["QingJia"];
			((Range)theSheet.Cells[i + 4, 13]).Value2 = row["QueQin"];
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