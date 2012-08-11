using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
	public class StringUtility
	{
		public static bool IsNumeric(string val) {
			int int_val;
			return int.TryParse(val, out int_val);
		}

		public static String AddStringItem(String stringItems, String item) {
			return AddStringItem(stringItems, item, false);
		}

		/// <summary>
		/// Add an item to a string of items. Items are separated by commas.
		/// This function can avoid item duplication in the string.
		/// See examples below to get more understanding.
		/// Since comma(,) is defined as delimiter in the string, this character can not be included in item value.
		/// </summary>
		/// <param name="stringItems">The string that a new item will be added to.</param>
		/// <param name="item">The item to be added.</param>
		/// <param name="isRefreshOrder">Flag to indicates if to refresh order of items in the string after add the new item.</param>
		/// <returns></returns>
		/// <example>
		///		AddStringItem("a,b,c", "b")			-> "a,b,c" (not "a,b,c,b")
		///		AddStringItem("a,b,c", "b", true)	-> "a,c,b" ("b" will be moved to be the last one.)
		/// </example>
		public static String AddStringItem(String stringItems, String item, bool isRefreshOrder) {
			if (stringItems == null) {
				stringItems = "";
			}
			if (String.IsNullOrWhiteSpace(item)) {
				return stringItems;
			}

			String s = "," + stringItems + ",";
			if (s.IndexOf("," + item + ",") == -1) {
				stringItems += (stringItems.Length == 0 ? "" : ",") + item;
			}
			return stringItems;
		}

		/// <summary>
		/// Remove an item from a string of items. Items are separated by commas.
		/// Since comma(,) is defined as delimiter in the string, this character can not be included in item value.
		/// </summary>
		/// <param name="stringItems"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		/// <example>
		///		RemoveStringItem("ab,b,c", "b") will get "ab,c"
		///		RemoveStringItem("ab,,c", "") will get "ab,c"
		/// </example>
		public static String RemoveStringItem(String stringItems, String item) {
			if (stringItems == null) {
				stringItems = "";
			}
			if (String.IsNullOrWhiteSpace(item)) {
				return stringItems;
			}

			String s = "," + stringItems + ",";
			s = s.Replace("," + item + ",", ",");
			if (s.Length > 2) {
				s = s.Substring(1, s.Length - 2);
			}
			else {
				s = "";
			}
			return s;
		}

		/// <summary>
		/// Remove each items in an item string from another item string. Items are separated by commas.
		/// Since comma(,) is defined as delimiter in the string, this character can not be included in item value.
		/// </summary>
		/// <param name="stringItems"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		/// <example>
		/// RemoveStringItems("ab,b,c", "ab,c") will get "b"
		/// RemoveStringItems("ab,b,c", "ab,c,d") will still get "b"
		/// </example>
		public static String RemoveStringItems(String stringItems, String items) {
			if (stringItems == null) {
				stringItems = "";
			}
			if (String.IsNullOrWhiteSpace(items)) {
				return stringItems;
			}

			String[] arrItem = items.Split(',');
			foreach (String item in arrItem) {
				stringItems = RemoveStringItem(stringItems, item);
			}

			return stringItems;
		}
	}
}
