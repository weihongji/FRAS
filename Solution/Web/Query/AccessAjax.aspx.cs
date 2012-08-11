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

public partial class Query_AccessAjax : PrivilegePage
{
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

	private string ExportExcel() {
		string userName = Request.QueryString["name"];
		int userType = GetQSInteger("usertype", -1);
		int dept = GetQSInteger("dept", -1);
		int device = GetQSInteger("device");
		int state = GetQSInteger("state", -1);
		DateTime start = Convert.ToDateTime(Request.QueryString["start"]);
		DateTime end = Convert.ToDateTime(Request.QueryString["end"]);
		System.Data.DataTable table;
		string fileName; //导出的Excel报表文件名

		table = AccessBiz.GetList(userName, userType, dept, device, state, start, end);
		fileName = "流水记录报表_" + start.ToString("yyMMdd") + "_" + end.ToString("yyMMdd") + ".xls";

		Microsoft.Office.Interop.Excel.Application theExcelApp = new Microsoft.Office.Interop.Excel.Application();
		Workbook theExcelBook = theExcelApp.Workbooks.Add(true);
		Worksheet theSheet = (Worksheet)theExcelBook.ActiveSheet;
		Range theCell;

		int rowCount = 3 + table.Rows.Count; // 报表总行数
		int columnCount = 7; // 报表总列数

		//整体设置
		theSheet.Cells.Font.Name = "宋体";
		theSheet.Cells.Font.Size = 10;
		theSheet.Cells.RowHeight = 14.25;

		//标题
		theCell = theSheet.Range[theSheet.Cells[1, 1], theSheet.Cells[1, columnCount]];
		theCell.Merge();
		theCell.RowHeight = 30;
		theCell.Value2 = "职工考勤流水报表";
		theCell.Font.Name = "华文中宋";
		theCell.Font.Size = 16;
		theCell.Font.Bold = true;
		theCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;

		//制表时间
		theCell = theSheet.Range[theSheet.Cells[2, 1], theSheet.Cells[2, 3]];
		theCell.Merge();
		theCell.Value2 = "时间: " + start.ToString("yyyy-MM-dd") + " 至 " + end.ToString("yyyy-MM-dd");
		theCell = theSheet.Range[theSheet.Cells[2, 6], theSheet.Cells[2, 7]];
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
		((Range)theSheet.Cells[3, 4]).Value2 = "考勤状态";
		((Range)theSheet.Cells[3, 4]).ColumnWidth = 13;
		((Range)theSheet.Cells[3, 5]).Value2 = "考勤设备";
		((Range)theSheet.Cells[3, 5]).ColumnWidth = 15;
		((Range)theSheet.Cells[3, 6]).Value2 = "出入时间";
		((Range)theSheet.Cells[3, 6]).ColumnWidth = 20;
		((Range)theSheet.Cells[3, 7]).Value2 = "签到/签退";
		((Range)theSheet.Cells[3, 7]).ColumnWidth = 9;

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
			((Range)theSheet.Cells[i + 4, 4]).Value2 = row["stateName"];
			((Range)theSheet.Cells[i + 4, 5]).Value2 = row["device"];
			((Range)theSheet.Cells[i + 4, 6]).Value2 = row["datetime"];
			((Range)theSheet.Cells[i + 4, 7]).Value2 = row["AccessFlagName"];
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