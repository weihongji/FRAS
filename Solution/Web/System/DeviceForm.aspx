<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DeviceForm.aspx.cs" Inherits="System_DeviceForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
	var deviceId = "<%=GetQSInteger("id") %>";
	$.ajaxSettings.async = false;

	$(function () {
		$("#btnSave").click(function () {
			if (isEmpty($("#txtIP").val())) {
				alert("请输入设备IP");
				$("#txtIP").focus();
				return;
			}
			if (isEmpty($("#txtPort").val())) {
				alert("请输入端口号");
				$("#txtPort").focus();
				return;
			}
			if (isEmpty($("#txtUserName").val())) {
				alert("请输入用户名");
				$("#txtUserName").focus();
				return;
			}
			if (isEmpty($("#txtPassword").val())) {
				alert("请输入密码");
				$("#txtPassword").focus();
				return;
			}
			if (isEmpty($("#txtAntNo").val())) {
				alert("请输入通道号");
				$("#txtAntNo").focus();
				return;
			}
			if ($("#selDeviceType").val()<0) {
				alert("请选择设备类型");
				$("#selDeviceType").focus();
				return;
			}
			if ($("#selAccessFlag").val()<0) {
				alert("请选择出入类型");
				$("#selAccessFlag").focus();
				return;
			}
			if (isEmpty($("#txtLocation").val())) {
				alert("请输入设备位置");
				$("#txtLocation").focus();
				return;
			}

			$.post("DeviceAjax.aspx?type=save"
				, { id: deviceId
					, ip: $("#txtIP").val()
					, port: $("#txtPort").val()
					, userName: $("#txtUserName").val()
					, password: $("#txtPassword").val()
					, antNo: $("#txtAntNo").val()
					, deviceType: $("#selDeviceType").val()
					, accessFlag: $("#selAccessFlag").val()
					, location: $("#txtLocation").val()
					, active: $("#chkActive").attr("checked")
				}
				, function (response) {
					$("#btnBack").click();
				}
			);
		});

		$("#btnBack").click(function () {
			location.href = "DeviceList.aspx";
		});

		loadDevice(deviceId);
	});

	function loadDevice(id) {
		if (id == null || id == 0) {
			return;
		}

		$.get("DeviceAjax.aspx?type=get", {id: id}, function(response) {
			eval("var device = " + response);
			$("#txtIP").val(device.ip);
			$("#txtPort").val(device.port);
			$("#txtUserName").val(device.userName).change();
			$("#txtPassword").val(device.password);
			$("#txtAntNo").val(device.antNo);
			$("#selDeviceType").val(device.deviceType);
			$("#selAccessFlag").val(device.accessFlag);
			$("#txtLocation").val(device.location);
			$("#chkActive").attr("checked", device.active);
		});
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=PageTitle%></div>
	<table style="margin-left: 40px;">
		<tr>
			<td style="width:70px;">设备IP</td>
			<td><input type="text" id="txtIP" class="textbox" maxlength="15" /></td>
		</tr>
		<tr>
			<td>端口</td>
			<td><input type="text" id="txtPort" class="textbox" maxlength="6" /></td>
		</tr>
		<tr>
			<td>用户名</td>
			<td><input type="text" id="txtUserName" class="textbox" maxlength="50" /></td>
		</tr>
		<tr>
			<td>密码</td>
			<td><input type="text" id="txtPassword" class="textbox" maxlength="50" /></td>
		</tr>
		<tr>
			<td>通道号</td>
			<td><input type="text" id="txtAntNo" class="textbox" maxlength="4" /></td>
		</tr>
		<tr>
			<td>设备类型</td>
			<td><select id="selDeviceType" class="w180">
				<option value="-1">-选择设备类型-</option>
				<asp:Repeater ID="repeaterDeviceType" runat="server">
				<ItemTemplate>
				<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
				</ItemTemplate>
				</asp:Repeater>
			</select></td>
		</tr>
		<tr>
			<td>出入类型</td>
			<td><select id="selAccessFlag" class="default">
				<option value="-1">-选择出入类型-</option>
				<option value="0">只入不出</option>
				<option value="1">只出不入</option>
				<option value="2">既入又出</option>
			</select></td>
		</tr>
		<tr>
			<td>设备位置</td>
			<td><input type="text" id="txtLocation" class="textbox" maxlength="50" /></td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td><label><input type="checkbox" id="chkActive" checked="checked" class="p" style="margin: 10px 5px 0px 0px;" />是否有效</label></td>
		</tr>
	</table>
	<div style="margin:10px 0px 0px 80px;">
		<input type="button" class="button2" id="btnSave" value="保存" />
		<input type="button" class="button2" id="btnBack" value="返回" />
	</div>
</asp:Content>

