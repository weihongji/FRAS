<%@ Page Title="部门信息管理" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DeptList.aspx.cs" Inherits="System_DeptList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
	$(function () {
		$.ajaxSettings.async = false;
		setButtonState();

		$("#btnSelectAll").click(function () {
			if ($("#btnSelectAll").val() == "全选") {
				$(".list :checkbox").attr("checked", true);
				$("#btnSelectAll").val("取消");
			}
			else {
				$(".list :checkbox").attr("checked", false);
				$("#btnSelectAll").val("全选");
			}
			setButtonState();
		});

		$("#btnAdd").click(function () {
			location.href = "DeptForm.aspx?id=-1";
		});

		$("#btnDelete").click(function () {
			var selectedCount = $(".list :checkbox:checked").length;
			var msg = "你确定要删除选中的部门吗？";
			if (selectedCount > 1) {
				msg = "你确定要删除选中的（" + $(".list :checkbox:checked").length + "个）部门吗？"
			}
			if (!confirm(msg)) {
				return;
			}
			$(".list :checkbox:checked").each(function () {
				var id = this.value;
				$.post("DeptAjax.aspx?type=delete", { id: id }, function (response) {
					if (response != "true") {
						alert("删除失败！没有找到编号为" + id + "的部门。");
					}
				});
			});
			document.location.reload();
		});

		$(".list :checkbox").click(function () {
			if (this.checked) {
				if ($(".list :checkbox").not(":checked").length == 0) {
					$("#btnSelectAll").val("取消");
				}
			}
			else {
				if ($("#btnSelectAll").val() == "取消") {
					$("#btnSelectAll").val("全选");
				}
			}
			setButtonState();
		});
	});

	function setButtonState() {
		$("#btnDelete").attr("disabled", $(".list :checkbox:checked").length == 0);
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<div style="margin-bottom:10px;">
		<input type="button" class="button2" id="btnSelectAll" value="全选" />
		<input type="button" class="button2" id="btnAdd" value="新增" />
		<input type="button" class="button2" id="btnDelete" value="删除" />
	</div>
	<table class="list">
		<thead>
			<tr>
				<th style="width: 50px;">选择</th>
				<th style="width: 80px;">部门编号</th>
				<th style="width: 150px;">部门名称</th>
				<th style="width: 50px;">修改</th>
			</tr>
		</thead>
		<tbody>
			<asp:Repeater ID="repeaterDept" EnableViewState="false" runat="server">
			<ItemTemplate>
			<tr>
				<td class="c"><input type="checkbox" class="p" id="chk<%#Eval("deptId") %>" value="<%#Eval("deptId") %>" /></td>
				<td class="c"><%#Eval("deptId") %></td>
				<td><%#Eval("deptName")%></td>
				<td class="c"><a href="DeptForm.aspx?id=<%#Eval("deptId") %>">修改</a></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
</asp:Content>
