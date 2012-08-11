using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace Utility
{
	public class LogUtility
	{
		public static bool LogError(string text) {
			string fileName = "Error_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
			return LogBlock(fileName, text);
		}

		public static bool LogLine(string fileName, string singleLineText) {
			StreamWriter writer = null;
			try {
				string filePath = GetLogPath() + fileName;
				writer = new StreamWriter(filePath, true);
				if (singleLineText == "") { // Generate a blank line with no timestamp.
					writer.WriteLine();
				}
				else {
					string[] lines = singleLineText.Replace("\r", "").Split('\n');
					foreach (string s in lines) {
						writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss - ") + s);
					}
				}
				writer.Close();
				return true;
			}
			catch {
				if (writer != null) writer.Close();
				return false;
			}
		}

		public static bool LogBlock(string fileName, string blockText) {
			StreamWriter writer = null;
			try {
				string filePath = GetLogPath() + fileName;
				writer = new StreamWriter(filePath, true);

				if (blockText == "") { // Generate a blank line with no timestamp.
					writer.WriteLine();
				}
				else {
					writer.WriteLine("============================================================");
					writer.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
					writer.WriteLine("============================================================");
					writer.WriteLine(blockText);
				}
				writer.Close();
				return true;
			}
			catch {
				if (writer != null) writer.Close();
				return false;
			}
		}

		public static string GetLogPath() {
			return System.Web.HttpContext.Current.Request.PhysicalApplicationPath + @"\Log\";
		}
	}
}
