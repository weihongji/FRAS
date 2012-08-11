using System;
using System.Reflection; //Used for GetDescription() function
using System.ComponentModel; //Used for GetDescription() function

namespace Entity
{
	public class XEnum
	{
		#region "Enum Types"

		//Corresponding to table ParamType
		public enum ParamType
		{
			All = 0,
			Device = 1,
			Role = 2,
			Leave = 3
		}

		public enum RosteringType
		{
			Single = 1,
			FirstPart = 2,
			SecondPart = 0
		}

		public enum LoginUserRoleType
		{
			Administrator = 5,	//管理员
			JianKongYuan = 6,	//监控员
			KaoQinYuan = 7,		//考勤员
			BanShiYuan = 19		//办事员
		}
		#endregion

		#region "Methods"

		/// <summary>
		/// Get description that bound to a enum item
		/// </summary>
		/// <param name="value">Enum item</param>
		/// <returns>
		/// If the specified enum item has no description bound, its name will be returned.
		/// </returns>
		public static String GetDescription(Enum value) {
			FieldInfo field = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			if (attributes.Length > 0) {
				return attributes[0].Description;
			}
			else {
				return value.ToString();
			}
		}

		#endregion

		#region "Examples"
		/*
		 * enum Days { Saturday, Sunday, Monday, Tuesday, Wednesday, Thursday, Friday };
		 * Enum to integer:
		 *		(int) Days.Saturday
		 * Name to Enum:
		 *		Enum.Parse(typeof(Days), "Monday")
		 * Get Enum name:
		 *		Days.Monday.ToString()
		 * Get Enum Description:
		 *		GetDescription(Days.Monday)
		 */
		#endregion
	}
}
