<%@ Page Title="模板录入" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NewUserForm.aspx.cs" Inherits="Attendance_NewUserForm" EnableViewState="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
	var m_Timer;
	var m_count = 0;
	var m_Token = "";

	$(function () {
		$("#btnSearch").click(function () {
			if (isEmpty($("#txtName").val()) && isEmpty($("#txtUserId").val())) {
				alert("请输入工号或者姓名");
				$("#txtUserId").focus();
				return;
			}

			$.get('NewUserAjax.aspx?type=get', { id: $("#txtUserId").val(), name: $("#txtName").val() }, function (response) {
				eval("var user = " + response);
				if (user.id) {
					$("#txtUserId").val(user.id);
					$("#txtName").val(user.name);
					$("#txtCardNo").val(user.cardId);
					$("#selDept").val(user.deptId);
					$("#selRank").val(user.rankId);
					$("#selCopyType").val(user.copyType);
				}
				else {
					alert("没有找到用户");
					if (!isEmpty($("#txtUserId").val())) {
						$("#txtUserId").focus().select();
					}
					else {
						$("#txtName").focus().select();
					}
				}
			});
		});

		$("#txtUserId,#txtName").keypress(function (event) {
			if (event.keyCode == 13) {
				$("#btnSearch").click();
			}
		});

		$("#rdoInputIp").parent("label").click(function () {
			$("#txtIP").focus();
		});

		$("#chkRecordTemplate").click(function () {
			$("#fldTemplate").find("input,select").attr("disabled", !this.checked);
		});

		$("#btnSave").click(function () {
			if (isEmpty($("#txtUserId").val())) {
				alert("工号无效");
				$("#txtUserId").focus();
				return;
			}
			else if ($("#selCopyType").val() == "0") {
				alert("请选择同步类型");
				$("#selCopyType").focus();
				return;
			}

			var ip;
			if ($("#chkRecordTemplate").attr("checked")) { // Check if any IP selected or input.
				if ($("#rdoSelectIp").attr("checked")) {
					if (isEmpty($("#selIP").val())) {
						alert("请选择IP");
						$("#selIP").focus();
						return;
					}
					ip = $("#selIP").val();
				}
				else if ($("#rdoInputIp").attr("checked")) {
					if (isEmpty($("#txtIP").val())) {
						alert("请输入IP");
						$("#txtIP").focus();
						return;
					}
					ip = $("#txtIP").val();
				}
			}

			//Update Card No.
			var success = false;
			$.post("NewUserAjax.aspx?type=save"
				, { userId: $("#txtUserId").val()
					, cardNo: $("#txtCardNo").val()
					, copyType: $("#selCopyType").val()
				}
				, function (response) {
					if (response == "true") {
						success = true;
						if (!$("#chkRecordTemplate").attr("checked")) {
							alert("卡号更新完毕");
						}
					}
					else {
						alert("卡号更新失败：" + response);
					}
				}
			);
			if (!success) {
				return;
			}

			// Input feature
			if ($("#chkRecordTemplate").attr("checked")) {
				var devNum = "0";

				m_count = 0;
				m_Token = generateToken($("#txtUserId").val());

				if (ip.indexOf("(") >= 0) {
					ip = ip.replace(")", "");
					var arrIP = ip.split("(");
					ip = arrIP[1];
					devNum = arrIP[0];
				}
				alert("卡号更新完毕。按确定按钮，开始录入模板……");
				$.post("NewUserFeature.aspx?type=send"
					, { token: m_Token
						, userId: $("#txtUserId").val()
						, ip: ip
						, devNum: devNum
					}
					, function (response) {
						if (response != "true") {
							alert("访问模板录入的Ajax失败");
							return;
						}
						$("#imgUser").show();
						m_Timer = window.setInterval("checkFeature()", 1000);
					}
				);
			}
		});

		$("#btnCancel").click(function () {
			location.href = location.href;
		});

		$.ajaxSettings.async = false;
		$("#txtUserId").focus();
	});

	// 检查模板录入返回结果
	function checkFeature() {
		if (m_count < 0) { // Tell system to stop checking if we fail to stop the timer.
			return;
		}
		m_count++;

		$.get('NewUserFeature.aspx?type=get&unique=' + m_count.toString(), { token: m_Token }, function (response) {
			eval("var result = " + response);
			if (result && result.id != null) {
				if (result.status == 1) {
					alert("模板录入成功！");
				}
				else {
					var msg = "模板录入失败！";
					if (result.message) {
						msg += "\n" + result.message;
					}
					alert(msg);
				}
				stopTimer();
			}
			else {
				var maxWaitingTime = 5;
				if (m_count > maxWaitingTime * 60) {
					stopTimer();
					alert("等待时间超过" + maxWaitingTime.toString() + "分钟，停止等待模板录入服务器返回结果。");
				}
			}
		});
	}

	function stopTimer() {
		$("#imgUser").hide();
		window.clearInterval(m_Timer);
		m_count = -1; // Set this variable to an invalid value to tell system to stop checking if fail to stop the timer.
	}

	function generateToken(userId) {
		var dt = new Date();
		var dateNumber = dt.getDate() + (dt.getMonth()+1)*100 + dt.getFullYear()*10000;
		var timeNumber = dt.getSeconds() + dt.getMinutes()*100 + dt.getHours() * 10000;
		var token = dateNumber.toString() + timeNumber.toString() + $.trim(userId);
		return token;
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<fieldset style="padding-left: 30px;">
		<legend>查询条件</legend>
		<div style="float:left; line-height:25px; height:25px; width:50px;">工号</div>
		<div style="float:left; line-height:25px; height:25px; width:160px;"><input type="text" class="textbox" id="txtUserId" maxlength="6" /></div>
		<div style="float:left; line-height:25px; height:25px; width:60px; text-align:center;">姓名</div>
		<div style="float:left; line-height:25px; height:25px; width:160px;"><input type="text" class="textbox" id="txtName" maxlength="6" /></div>
		<div style="float:left; line-height:25px; height:25px;"><input type="button" class="button2" id="btnSearch" value="查询" /></div>
		<div style="clear:both;"></div>
	</fieldset>
	<fieldset style="margin-top:10px; padding-left: 30px;">
		<legend>相关信息</legend>
		<div>
			<div style="float:left; line-height:25px; height:25px; width:50px;">卡号</div>
			<div style="float:left; line-height:25px; height:25px; width:160px;"><input type="text" class="textbox" id="txtCardNo" maxlength="50" /></div>
			<div style="float:left; line-height:25px; height:25px; width:60px;">同步类型</div>
			<div style="float:left; line-height:25px; height:25px; width:160px;"><select id="selCopyType">
					<option value="0">-选择类型-</option>
					<option value="1">下井员工</option>
					<option value="2">联建楼(地面工种)</option>
					<option value="3">联建楼(地面工种,需要下井)</option>
					<option value="4">队长,副队长,技术员</option>
					<option value="5">办公楼(不下井)</option>
					<option value="6">办公楼(需要下井)</option>
					<option value="7">生活区</option>
					<option value="8">下井员工(出井刷卡)</option>
					<option value="9">联建楼(出井刷卡)</option>
				</select>
			</div>
			<div style="clear:both;"></div>
		</div>
		<div style="margin-top:5px;">
			<div style="float:left; line-height:25px; height:25px; width:50px;">部门</div>
			<div style="float:left; line-height:25px; height:25px; width:160px;"><select id="selDept" class="default">
				<option value="-1">-选择部门-</option>
				<asp:Repeater ID="repeaterDept" runat="server">
				<ItemTemplate>
				<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
				</ItemTemplate>
				</asp:Repeater>
			</select></div>
			<div style="float:left; line-height:25px; height:25px; width:60px;">职位</div>
			<div style="float:left; line-height:25px; height:25px; width:160px;"><select id="selRank" class="default">
				<option value="-1">-选择职位-</option>
				<asp:Repeater ID="repeaterRank" runat="server">
				<ItemTemplate>
				<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
				</ItemTemplate>
				</asp:Repeater>
			</select></div>
			<div style="clear:both;"></div>
		</div>
	</fieldset>
	<div style="margin-top:20px;"><label><input type="checkbox" id="chkRecordTemplate" checked="checked" />录入模板</label></div>
	<fieldset style="position:relative; margin-top:10px; padding-left: 30px;" id="fldTemplate">
		<legend>模板录入机器</legend>
		<div style="float:left; line-height:25px; height:25px; width:60px;"><label><input type="radio" name="rdIp" id="rdoSelectIp" checked="checked" />选择</label></div>
		<div style="float:left; line-height:25px; height:25px; width:160px;"><select id="selIP">
				<option value="">-选择机器-</option>
				<asp:Repeater ID="repeaterDevice" runat="server">
				<ItemTemplate>
				<option value="<%#GetDeviceName(Eval("antNo"), Eval("devIp")) %>"><%#GetDeviceName(Eval("antNo"), Eval("devIp"))%></option>
				</ItemTemplate>
				</asp:Repeater>
			</select>
		</div>
		<div style="clear:both;"></div>
		<div style="float:left; line-height:25px; height:25px; width:60px;"><label><input type="radio" name="rdIp" id="rdoInputIp" />输入</label></div>
		<div style="float:left; line-height:25px; height:25px; width:160px;"><input type="text" class="textbox" id="txtIP" maxlength="15" /></div>
		<img id="imgUser" style="position:absolute; top: 30px; left: 250px; display:none;" src="../Image/ajax-loader.gif" alt="" />
		<div style="clear:both;"></div>
	</fieldset>
	<div style="margin:10px 0px 0px 200px;">
		<input type="button" class="button2" id="btnSave" value="确定" />
		<input type="button" class="button2" id="btnCancel" value="取消" style="margin-left: 20px;" />
	</div>
</asp:Content>
