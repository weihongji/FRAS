using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace DAL
{
	public class SqlCondition
	{
		String m_ColumnName;
		Object[] m_ColumnValue;
		EnumConstraintType m_ConstraintType;

		public SqlCondition() { }

		public SqlCondition(String columnName, Object columnValue) {
			m_ColumnName = columnName;
			m_ColumnValue = new Object[] { columnValue };
			m_ConstraintType = EnumConstraintType.Equal;
		}

		public SqlCondition(String columnName, Object columnValue, EnumConstraintType ConstraintType) {
			m_ColumnName = columnName;
			m_ColumnValue = new Object[] { columnValue };
			m_ConstraintType = ConstraintType;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="columnName"></param>
		/// <param name="columnValue"></param>
		/// <param name="ConstraintType"></param>
		/// <example>
		/// SqlCondition condition = new SqlCondition("OrgType", new Object[] { XEnum.OrgType.County, XEnum.OrgType.Zone }, SqlCondition.EnumConstraintType.In)
		/// </example>
		public SqlCondition(String columnName, Object[] columnValue, EnumConstraintType ConstraintType) {
			m_ColumnName = columnName;
			m_ColumnValue = columnValue;
			m_ConstraintType = ConstraintType;
		}

		/// <summary>
		/// This name might not be a real column name. So, do not connect it in a sql statement string.
		/// For example, value of "UserID" may be used to represents Users.ID
		/// </summary>
		public String ColumnName {
			get { return m_ColumnName; }
			set { m_ColumnName = value; }
		}

		public Object[] ColumnValue {
			get { return m_ColumnValue; }
			set { m_ColumnValue = value; }
		}

		public EnumConstraintType ConstraintType {
			get { return m_ConstraintType; }
			set { m_ConstraintType = value; }
		}

		public enum EnumConstraintType
		{
			Equal = 1,
			Greater = 2,
			GreaterOrEqual = 3,
			Less = 4,
			LessOrEqual = 5,
			NoEqual = 6,
			Between = 7,
			In = 8,
			Like = 9
		}

		public String GetConditionSql() {
			return GetConditionSql(m_ColumnName);
		}

		/// <summary>
		/// This function can not protect sql statement from SQL injection.
		/// Function GetConditionSqlInParameter() can be used for reason of safty.
		/// </summary>
		/// <param name="realColumnName"></param>
		/// <returns></returns>
		public String GetConditionSql(String realColumnName) {
			String condition = "";
			Object[] values = new Object[m_ColumnValue.Length];
			for (int i = 0; i < values.Length; i++) {
				values[i] = m_ColumnValue[i].ToString().Replace("'", "''");
			}
			switch (m_ConstraintType) {
				case EnumConstraintType.Equal:
					condition = realColumnName + " = '" + values[0] + "'";
					break;
				case EnumConstraintType.Greater:
					condition = realColumnName + " > '" + values[0] + "'";
					break;
				case EnumConstraintType.GreaterOrEqual:
					condition = realColumnName + " >= '" + values[0] + "'";
					break;
				case EnumConstraintType.Less:
					condition = realColumnName + " < '" + values[0] + "'";
					break;
				case EnumConstraintType.LessOrEqual:
					condition = realColumnName + " <= '" + values[0] + "'";
					break;
				case EnumConstraintType.NoEqual:
					condition = realColumnName + " != '" + values[0] + "'";
					break;
				case EnumConstraintType.Between:
					if (values.Length >= 2) {
						condition = realColumnName + " BETWEEN '" + values[0] + "' AND '" + values[1] + "'";
					}
					break;
				case EnumConstraintType.In:
					for (int i = 0; i < values.Length - 1; i++) {
						condition += "'" + values[i] + "', ";
					}
					condition += "'" + values[values.Length - 1] + "'";
					condition = realColumnName + " IN (" + condition + ")";
					break;
				case EnumConstraintType.Like:
					break;
			}
			return condition;
		}

		public String GetConditionSqlInParameter(out SqlParameter[] parameters, SqlDbType dbType) {
			return GetConditionSqlInParameter(m_ColumnName, out parameters, dbType, 0);
		}

		public String GetConditionSqlInParameter(out SqlParameter[] parameters, SqlDbType dbType, int size) {
			return GetConditionSqlInParameter(m_ColumnName, out parameters, dbType, size);
		}

		/// <summary>
		/// This function is safer than GetConditionSql() in preventing SQL injection.
		/// </summary>
		/// <param name="realColumnName"></param>
		/// <returns></returns>
		public String GetConditionSqlInParameter(String realColumnName, out SqlParameter[] parameters, SqlDbType dbType, int size) {
			String condition = "";
			switch (m_ConstraintType) {
				case EnumConstraintType.Equal:
					condition = realColumnName + " = @" + m_ColumnName;
					break;
				case EnumConstraintType.Greater:
					condition = realColumnName + " > @" + m_ColumnName;
					break;
				case EnumConstraintType.GreaterOrEqual:
					condition = realColumnName + " >= @" + m_ColumnName;
					break;
				case EnumConstraintType.Less:
					condition = realColumnName + " < @" + m_ColumnName;
					break;
				case EnumConstraintType.LessOrEqual:
					condition = realColumnName + " <= @" + m_ColumnName;
					break;
				case EnumConstraintType.NoEqual:
					condition = realColumnName + " != @" + m_ColumnName;
					break;
				case EnumConstraintType.Between:
					condition = realColumnName + " BETWEEN @" + m_ColumnName + "0 AND @" + m_ColumnName + "1";
					break;
				case EnumConstraintType.In:
					for (int i = 0; i < m_ColumnValue.Length - 1; i++) {
						condition += "@" + m_ColumnName + i.ToString() + ", ";
					}
					condition += "@" + m_ColumnName + (m_ColumnValue.Length - 1).ToString();
					condition = realColumnName + " IN (" + condition + ")";
					break;
				case EnumConstraintType.Like:
					condition = realColumnName + " LIKE @" + m_ColumnName;
					break;
			}

			// Output SqlParameters
			switch (m_ConstraintType) {
				case EnumConstraintType.Between:
					parameters = new SqlParameter[2];
					if (size > 0) {
						parameters[0] = new SqlParameter("@" + m_ColumnName + "0", dbType, size);
						parameters[1] = new SqlParameter("@" + m_ColumnName + "1", dbType, size);
					}
					else {
						parameters[0] = new SqlParameter("@" + m_ColumnName + "0", dbType);
						parameters[1] = new SqlParameter("@" + m_ColumnName + "1", dbType);
					}
					parameters[0].Value = m_ColumnValue[0];
					parameters[1].Value = m_ColumnValue[1];
					break;
				case EnumConstraintType.In:
					parameters = new SqlParameter[m_ColumnValue.Length];
					for (int i = 0; i < m_ColumnValue.Length; i++) {
						if (size > 0) {
							parameters[i] = new SqlParameter("@" + m_ColumnName + i.ToString(), dbType, size);
						}
						else {
							parameters[i] = new SqlParameter("@" + m_ColumnName + i.ToString(), dbType);
						}
						parameters[i].Value = m_ColumnValue[i];
					}
					break;
				default:
					parameters = new SqlParameter[1];
					if (size > 0) {
						parameters[0] = new SqlParameter("@" + m_ColumnName, dbType, size);
					}
					else {
						parameters[0] = new SqlParameter("@" + m_ColumnName, dbType);
					}
					parameters[0].Value = m_ColumnValue[0];
					break;
			}

			return condition;
		}
	}
}
