using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class Query_AttendanceOverview : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		ShowDeptTree();
	}

	private void ShowDeptTree() {
		DataTable table = DeptBiz.GetBriefList(LoginUserDeptID);
		TreeNode node, childNode;
		node = new TreeNode("部门选择|-1");
		node.SelectAction = TreeNodeSelectAction.None;
		foreach (DataRow row in table.Rows) {
			childNode = new TreeNode(row["Name"].ToString() + "|" + row["ID"].ToString());
			childNode.SelectAction = TreeNodeSelectAction.None;
			node.ChildNodes.Add(childNode);
			
		}
		deptTree.Nodes.Add(node);
	}
}