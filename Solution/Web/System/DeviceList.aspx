<%@ Page Title="设备信息管理" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DeviceList.aspx.cs" Inherits="System_DeviceList" %>

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
			location.href = "DeviceForm.aspx?id=0";
		});

		$("#btnDelete").click(function () {
			var selectedCount = $(".list :checkbox:checked").length;
			var msg = "你确定要删除选中的设备信息吗？";
			if (selectedCount > 1) {
				msg = "你确定要删除选中的（" + $(".list :checkbox:checked").length + "个）设备信息吗？"
			}
			if (!confirm(msg)) {
				return;
			}
			$(".list :checkbox:checked").each(function () {
				var id = this.value;
				$.post("DeviceAjax.aspx?type=delete", { id: id }, function (response) {
					if (response != "true") {
						alert("删除失败！没有找到编号为" + id + "的设备信息。");
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
				<th style="width: 40px;">选择</th>
				<th style="width: 40px;">编号</th>
				<th style="width: 80px;">IP地址</th>
				<th style="width: 60px;">端口</th>
				<th style="width: 150px;">设备类型</th>
				<th style="width: 80px;">设备用户名</th>
				<th style="width: 60px;">设备密码</th>
				<th style="width: 50px;">通道号</th>
				<th style="width: 60px;">出入类型</th>
				<th style="width: 60px;">是否有效</th>
				<th style="width: 50px;">修改</th>
			</tr>
		</thead>
		<tbody>
			<asp:Repeater ID="repeaterDevice" EnableViewState="false" runat="server">
			<ItemTemplate>
			<tr>
				<td class="c"><input type="checkbox" class="p" id="chk<%#Eval("ID") %>" value="<%#Eval("ID") %>" /></td>
				<td class="c"><%#Eval("ID") %></td>
				<td><%#Eval("devIp")%></td>
				<td class="c"><%#Eval("devPort")%></td>
				<td><%#Eval("DevTypeName")%></td>
				<td><%#Eval("devUserName")%></td>
				<td><%#Eval("devPassword")%></td>
				<td class="c"><%#Eval("antNo")%></td>
				<td class="c"><%#Eval("AccessFlagName")%></td>
				<td class="c"><%#Eval("ActiveName")%></td>
				<td class="c"><a href="DeviceForm.aspx?id=<%#Eval("ID") %>">修改</a></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
</asp:Content>

