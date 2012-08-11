<%@ Page Title="综合查询" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DurationQuery.aspx.cs" Inherits="Query_DurationQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link type="text/css" rel="stylesheet" href="../Style/jquery/ui.css" />
<script type="text/javascript" src="../Script/jquery-ui.js"></script>
<script type="text/javascript" src="../Script/jquery-ui.datepicker-zh-CN.js"></script>
<script type="text/javascript">
	$(function () {
		$("#btnSearch").click(function () {
			if (!checkCriteria()) {
				return;
			}

			var url = location.href;
			url = removeQSItem(url, "name");
			url = removeQSItem(url, "usertype");
			url = removeQSItem(url, "dept");
			url = removeQSItem(url, "device");
			url = removeQSItem(url, "start");
			url = removeQSItem(url, "end");

			if (!isEmpty($("#txtName").val())) {
				url = setQSItem(url, "name", $("#txtName").val());
			}
			if ($("#selUserType").val() >= 0) {
				url = setQSItem(url, "usertype", $("#selUserType").val());
			}
			if ($("#selDept").val() >= 0) {
				url = setQSItem(url, "dept", $("#selDept").val());
			}
			if ($("#selDevice").val() > 0) {
				url = setQSItem(url, "device", $("#selDevice").val());
			}
			url = setQSItem(url, "start", $("#txtStart").val());
			url = setQSItem(url, "end", $("#txtEnd").val());
			url = setQSItem(url, "show", "1");
			location.href = url;
		});

		$("#txtStart").datepicker($.datepicker.regional['zh-CN']);
		$("#txtEnd").datepicker($.datepicker.regional['zh-CN']);

		$("#btnExport").click(function () {
			if (!checkCriteria()) {
				return;
			}

			$.ajax({
				  async: true
				, type: 'GET'
				, url: 'DurationAjax.aspx?type=export'
				, data: {
					  name: $("#txtName").val()
					, usertype: $("#selUserType").val()
					, dept: $("#selDept").val()
					, device: $("#selDevice").val()
					, start: $("#txtStart").val()
					, end: $("#txtEnd").val()
				}
				, beforeSend: function() {
					$("#lblDownload").html("正在生成Excel文件……");
					$("#imgDownload").show();
				}
				, success: function(response) {
					if (response && response.indexOf(".xls")>0) {
						$("#lblDownload").html("<a href='../Export/" + response + "'>" + response + "</a>");
					}
					else {
						if (response.length>100) {
							var msg = "生成Excel文件时报错！";
						}
						else {
							var msg = "生成Excel文件时报错：" + response;
						}
						alert(msg);
						$("#lblDownload").html("<span style='color:red; font-weight:bold;'>" + msg + "</span>");
					}
				}
				, complete: function() {
					$("#imgDownload").hide();
				}
			});
		});

		initValues();
	});

	function checkCriteria() {
		if (isEmpty($("#txtStart").val())) {
			alert("请输入起始日期");
			$("#txtStart").focus();
			return false;
		}
		else if (!isDate($("#txtStart").val())) {
			alert("起始日期无效");
			$("#txtStart").focus();
			return false;
		}
		if (isEmpty($("#txtEnd").val())) {
			alert("请输入结束日期");
			$("#txtEnd").focus();
			return false;
		}
		else if (!isDate($("#txtEnd").val())) {
			alert("结束日期无效");
			$("#txtEnd").focus();
			return false;
		}
		if (new Date($("#txtStart").val())>new Date($("#txtEnd").val())) {
			alert("起始时间不得小于终止时间");
			$("#txtEnd").focus();
			return false;
		}
		return true;
	}

	function initValues() {
		$("#txtName").val("<%=Request.QueryString["name"] %>");
		$("#selUserType").val("<%=GetQSInteger("usertype", -1) %>");
		if($("#selDept option").length == 2) {
			$("#selDept option:first").remove();
		}
		else {
			$("#selDept").val("<%=GetQSInteger("dept", -1) %>");
		}
		$("#selDevice").val("<%=GetQSInteger("device") %>");
		var now = (new Date()).getFullYear() + "-" + ((new Date()).getMonth()+1) + "-" + (new Date()).getDate();
		$("#txtStart").val("<%=Request.QueryString["start"] %>");
		if (!isDate($("#txtStart").val())) {
			$("#txtStart").val(formatDate(now));
		}
		$("#txtEnd").val("<%=Request.QueryString["end"] %>");
		if (!isDate($("#txtEnd").val())) {
			$("#txtEnd").val(formatDate(now));
		}
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<fieldset style="margin-bottom:10px;">
		<legend>检索</legend>
		<table style="width:100%;" cellpadding="3" cellspacing="0">
			<tr>
				<td style="width:120px;">姓名</td>
				<td style="width:160px;"><input type="text" class="textbox" id="txtName" /></td>
				<td style="width:120px;">员工类别</td>
				<td><select id="selUserType" class="default">
					<option value="-1">-全部-</option>
					<option value="0">正式工</option>
					<option value="1">劳务工</option>
					<option value="2">中煤中宇</option>
				</select></td>

			</tr>
			<tr>
				<td>部门</td>
				<td><select id="selDept" class="default">
					<option value="-1">-全部-</option>
					<asp:Repeater ID="repeaterDept" runat="server">
					<ItemTemplate>
					<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
					</ItemTemplate>
					</asp:Repeater>
				</select></td>
				<td>考勤设备</td>
				<td><select id="selDevice" class="default">
					<option value="0">-全部-</option>
					<option value="1">井口3号通道</option>
					<option value="2">井口2号通道</option>
					<option value="3">井口4号通道</option>
					<option value="4">井口1号通道</option>
					<option value="5">联建楼</option>
					<option value="6">办公楼</option>
					<option value="7">生活区</option>
					<option value="8">井口5号通道</option>
				</select></td>
			</tr>
			<tr>
				<td>起始日期</td>
				<td><input type="text" id="txtStart" class="textbox" maxlength="10" /></td>
				<td>结束日期</td>
				<td><input type="text" id="txtEnd" class="textbox" maxlength="10" /></td>
			</tr>
		</table>
	</fieldset>
	<div style="margin-bottom:10px;">
		<input type="button" class="button4" id="btnSearch" value="查询" />
		<input type="button" class="button4" id="btnExport" value="导出Excel" style="margin-left:20px;" />
		<span id="lblDownload" style="margin-left:20px;"></span>
		<img id="imgDownload" style="margin-left:10px; display:none;" src="../Image/ajax-loader.gif" alt="loading" />
	</div>
	<table class="list">
		<thead>
			<tr>
				<th style="width: 100px;">部门</th>
				<th style="width: 60px;">工号</th>
				<th style="width: 70px;">姓名</th>
				<th style="width: 130px;">签到时间</th>
				<th style="width: 130px;">签退时间</th>
				<th style="width: 60px;">时长</th>
				<th style="width: 40px;">工时</th>
				<th style="width: 80px;">考勤类型</th>
				<th style="width: 130px;">备注</th>
				<th style="width: 90px;">前后夜</th>
			</tr>
		</thead>
		<tbody>
			<asp:Repeater ID="repeaterDuration" EnableViewState="false" runat="server">
			<ItemTemplate>
			<tr>
				<td class="c"><%#Eval("DeptName")%></td>
				<td class="c"><%#Eval("userId")%></td>
				<td class="c"><%#Eval("UserName")%></td>
				<td class="c"><%#Eval("bak2")%></td>
				<td class="c"><%#Eval("bak3")%></td>
				<td class="c"><%#Eval("durations")%></td>
				<td class="c"><%#Eval("muldur")%></td>
				<td class="c"><%#Eval("workType")%></td>
				<td class="c"><%#Eval("comment")%></td>
				<td class="c"><%#Eval("NightWorkName")%></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
	<div style="margin-top:10px;">
		<asp:Literal ID="litRowCount" runat="server"></asp:Literal>
	</div>
</asp:Content>
