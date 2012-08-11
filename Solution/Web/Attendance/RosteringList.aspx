<%@ Page Title="排班管理" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RosteringList.aspx.cs" Inherits="Attendance_RosteringList" %>

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
			var result = $.showModalDialog("RosteringDialog.htm", null, "dialogWidth:420px;dialogHeight:320px");
			if (result == "1" || result == "2") {
				location.href = "RosteringForm.aspx?id=0&type=" + result;
			}
		});

		$("#btnDelete").click(function () {
			var selectedCount = $(".list :checkbox:checked").length;
			var msg = "你确定要删除选中的排班记录吗？";
			if (selectedCount > 1) {
				msg = "你确定要删除选中的（" + $(".list :checkbox:checked").length + "个）排班记录吗？"
			}
			if (!confirm(msg)) {
				return;
			}
			$(".list :checkbox:checked").each(function () {
				var id = this.value;
				$.post("RosteringAjax.aspx?type=delete", { id: id }, function (response) {
					if (response != "true") {
						alert("删除失败！没有找到编号为" + id + "的排班记录。");
					}
				});
			});
			document.location.reload();
		});

		$("#btnAssign").click(function () {
			var checkedCount = $(".list :checkbox:checked").length;
			if (checkedCount == 1) {
				location.href = "RosteringAssign.aspx?id=" + $(".list :checkbox:checked").val();
			}
			else if (checkedCount == 0) {
				alert("请选择一个班次进行排班");
			}
			else {
				alert("每次只能选择一个班次进行排班");
			}
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
		<%if (LoginUserRole != (int)Entity.XEnum.LoginUserRoleType.Administrator && LoginUserRole != (int)Entity.XEnum.LoginUserRoleType.KaoQinYuan) {%>
		$("#btnSelectAll,#btnAdd,#btnDelete").attr("disabled", "true");
		<%} %>
		<%else { %>
		$("#btnDelete").attr("disabled", $(".list :checkbox:checked").length == 0);
		<%} %>
		$("#btnAssign").attr("disabled", $(".list :checkbox:checked").length != 1);
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<div style="margin-bottom:10px;">
		<input type="button" class="button2" id="btnSelectAll" value="全选" />
		<input type="button" class="button2" id="btnAdd" value="新增" />
		<input type="button" class="button2" id="btnDelete" value="删除" />
		<input type="button" class="button2" id="btnAssign" value="排班" />
	</div>
	<table class="list two_line_header">
		<thead>
			<tr>
				<th style="width: 50px;">选择</th>
				<th style="width: 50px;">编号</th>
				<th style="width: 70px;">班次名称</th>
				<th style="width: 70px;">起始时间</th>
				<th style="width: 70px;">终止时间</th>
				<th style="width: 70px;">提前签到<br />（分钟）</th>
				<th style="width: 70px;">延点签退<br />（分钟）</th>
				<th style="width: 70px;">是否有效</th>
				<th style="width: 70px;">多时段</th>
				<th style="width: 50px;">工长</th>
				<th style="width: 70px;">夜班类型</th>
				<th style="width: 50px;">修改</th>
			</tr>
		</thead>
		<tbody>
			<asp:Repeater ID="repeaterRostering" EnableViewState="false" runat="server">
			<ItemTemplate>
			<tr>
				<td class="c"><%#GetSelect((int)Eval("multType"), (int)Eval("ID"))%></td>
				<td class="c"><%#GetID((int)Eval("multType"), (int)Eval("ID"))%></td>
				<td><%#Eval("bcName")%></td>
				<td class="c"><%#Eval("startTime")%></td>
				<td class="c"><%#Eval("endTime")%></td>
				<td class="c"><%#Eval("earlyRange")%></td>
				<td class="c"><%#Eval("lateRange")%></td>
				<td class="c"><%#Eval("ActiveName")%></td>
				<td class="c"><%#GetMultiple((int)Eval("multType"), (string)Eval("MultipleTypeName"))%></td>
				<td class="c"><%#Eval("mulripleDur")%></td>
				<td class="c"><%#Eval("NightWorkName")%></td>
				<td class="c"><%#GetModify((int)Eval("multType"), (int)Eval("ID"))%></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
</asp:Content>
