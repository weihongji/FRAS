<%@ Page Title="请假管理" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LeaveList.aspx.cs" Inherits="Attendance_LeaveList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
	$(function () {
		$("#selDept").change(function() {
			$("#selUser").emptySelect(true); // clear user list

			if ($("#selDept").val() == "-1") {
				return;
			}

			$.get("../System/UserAjax.aspx?type=brief_list", {deptId: $("#selDept").val()}, function(response) {
				eval("var users = " + response);
				$("#selUser").loadSelect(users, true);
			});
		});

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
			location.href = "LeaveForm.aspx?id=0<%=ListQS%>";
		});

		$("#btnDelete").click(function () {
			var selectedCount = $(".list :checkbox:checked").length;
			var msg = "你确定要删除选中的请假记录吗？";
			if (selectedCount > 1) {
				msg = "你确定要删除选中的（" + $(".list :checkbox:checked").length + "个）请假记录吗？"
			}
			if (!confirm(msg)) {
				return;
			}
			$(".list :checkbox:checked").each(function () {
				var id = this.value;
				$.post("LeaveAjax.aspx?type=delete", { id: id }, function (response) {
					if (response != "true") {
						alert("删除失败！没有找到编号为" + id + "的请假记录。");
					}
				});
			});
			document.location.reload();
		});

		$("#btnApprove").click(function () {
			$(".list :checkbox:checked").each(function () {
				var id = this.value;
				$.post("LeaveAjax.aspx?type=approve", { id: id }, function (response) {
					if (response != "true") {
						alert("批准失败！没有找到编号为" + id + "的请假记录。");
					}
				});
			});
			document.location.reload();
		});

		$("#btnSearch").click(function () {
			var url = location.href;
			url = removeQSItem(url, "dept");
			url = removeQSItem(url, "user");
			url = removeQSItem(url, "state");
			if ($("#selDept").val() >= 0) {
				url = setQSItem(url, "dept", $("#selDept").val());
			}
			if ($("#selUser").val().length > 0) {
				url = setQSItem(url, "user", $("#selUser").val());
			}
			url = setQSItem(url, "state", $("#selState").val());
			url = setQSItem(url, "show", "1");
			location.href = url;
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

		$.ajaxSettings.async = false;
		initValues();
		setButtonState();
	});

	function setButtonState() {
		$("#btnDelete,#btnApprove").attr("disabled", $(".list :checkbox:checked").length == 0);
	}

	function initValues() {
		if($("#selDept option").length == 2) {
			$("#selDept option:first").remove();
			$("#selDept").change();
		}
		else {
			$("#selDept").val("<%=GetQSInteger("dept", -1) %>").change();
		}
		$("#selUser").val("<%=Request.QueryString["user"] %>");
		$("#selState").val("<%=GetQSInteger("state", 0) %>");
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<fieldset style="margin-bottom:20px;">
		<legend>检索</legend>
		<div>
			<div style="float:left;">部门：<select id="selDept" class="default">
				<option value="-1">-全部部门-</option>
				<asp:Repeater ID="repeaterDept" runat="server">
				<ItemTemplate>
				<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
				</ItemTemplate>
				</asp:Repeater>
			</select></div>
			<div style="float:left; margin-left:30px;">员工：<select id="selUser" class="default">
				<option value="">-全部员工-</option>
			</select></div>
			<div style="float:left; margin-left:30px;">审批状态：<select id="selState" class="default">
				<option value="0">无效</option>
				<option value="1">有效</option>
				<option value="-1">全部</option>
			</select></div>
			<div style="float:left; margin-left:30px;"><input type="button" class="button2" id="btnSearch" value="检索" /></div>
			<div style="clear:both;"></div>
		</div>
	</fieldset>
	<div style="margin-bottom:10px;">
		<input type="button" class="button2" id="btnSelectAll" value="全选" />
		<input type="button" class="button2" id="btnAdd" value="新增" />
		<input type="button" class="button2" id="btnDelete" value="删除" />
		<input type="button" class="button2" id="btnApprove" value="批准" />
	</div>
	<table class="list">
		<thead>
			<tr>
				<th style="width: 50px;">选择</th>
				<th style="width: 50px;">编号</th>
				<th style="width: 70px;">员工姓名</th>
				<th style="width: 100px;">部门</th>
				<th style="width: 80px;">起始日期</th>
				<th style="width: 80px;">终止日期</th>
				<th style="width: 70px;">请假类别</th>
				<th style="width: 50px;">天数</th>
				<th style="width: 70px;">是否有效</th>
				<th style="width: 50px;">修改</th>
			</tr>
		</thead>
		<tbody>
			<asp:Repeater ID="repeaterLeave" EnableViewState="false" runat="server">
			<ItemTemplate>
			<tr>
				<td class="c"><%#GetRecordCheckbox(Eval("ID"), Eval("flag"))%></td>
				<td class="c"><%#Eval("ID") %></td>
				<td class="c"><%#Eval("UserName")%></td>
				<td><%#Eval("DeptName")%></td>
				<td class="c"><%#Eval("startDate")%></td>
				<td class="c"><%#Eval("endDate")%></td>
				<td class="c"><%#Eval("TypeName")%></td>
				<td class="c"><%#Eval("days")%></td>
				<td class="c"><%#Eval("ApprovedName")%></td>
				<td class="c"><%#GetEditLink(Eval("ID"), Eval("flag"))%></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
</asp:Content>
