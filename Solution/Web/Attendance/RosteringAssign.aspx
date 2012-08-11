<%@ Page Title="排班预设" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RosteringAssign.aspx.cs" Inherits="Attendance_RosteringAssign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<style type="text/css">
	#assignment {
		border-collapse: collapse;
		table-layout: fixed;
	}

	#assignment td {
		margin: 0px;
		padding: 3px;
		width: 70px;
		border: 1px solid #aaa;
		text-align: center;
		overflow: hidden;
	}

	#assignment tr.head td {
		color: #4B4B4B;
		background-image: url(../Image/table_bg01.gif);
	}
</style>
<script type="text/javascript">
	var m_Users = new Array();

	$(function () {
		$("#selDept").change(function () {
			$("#selUser,#selUserSelected").emptySelect(); // clear user list

			if ($("#selDept").val() >= 0) {
				$.ajax({
					async: true
					, type: 'GET'
					, url: '../System/UserAjax.aspx?type=rostering_list'
					, data: { deptId: $("#selDept").val() }
					, beforeSend: function () {
						$("#imgUser").show();
					}
					, success: function (response) {
						eval("m_Users = " + response);
						$("#selUser").loadSelect(m_Users);
					}
					, complete: function () {
						$("#imgUser").hide();
					}
				});
			}

			$("#selUser").click().change(); //Trigger click event to make the change event be fired at the first user selection in IE.
		});

		$("#selUser").change(function () {
			$("#assignment tr").filter(function () { return !$(this).hasClass("head"); }).children().html("&nbsp;");
			var id = $("#selUser").val();
			if (id == null) {
				return;
			}
			$.get("RosteringAjax.aspx?type=get_pr", { userId: id[0] }, function (response) {
				eval("var a = " + response);
				if (a.names) {
					var arrTD = $("#assignment tr").filter(function () { return !$(this).hasClass("head"); }).children().get();
					for (var i = 0; i < 31; i++) {
						arrTD[i].innerHTML = a.names[i];
					}
				}
			});
		});

		$("#btnRight").click(function () {
			$("#selUser option:selected").each(function () {
				var insertAt = getOptionIndex($("#selUserSelected"), this.value);
				$("#selUserSelected").addOption(this.value, this.text.substring(0, this.text.indexOf(" ")), insertAt);
				$("#selUser").removeOption(this.value);
			});
			setButtonState();
		});

		$("#btnRightAll").click(function () {
			$("#selUser").emptySelect();
			$("#selUserSelected").emptySelect();
			for (var i = 0; i < m_Users.length; i++) {
				$("#selUserSelected").addOption(m_Users[i].id, m_Users[i].name.substring(0, m_Users[i].name.indexOf(" ")));
			}
			setButtonState();
		});

		$("#btnLeft").click(function () {
			$("#selUserSelected option:selected").each(function () {
				var insertAt = getOptionIndex($("#selUser"), this.value);
				var indexInAll = inUserArray(this.value);
				$("#selUser").addOption(m_Users[indexInAll].id, m_Users[indexInAll].name, insertAt);
				$("#selUserSelected").removeOption(this.value);
			});
			setButtonState();
		});

		$("#btnLeftAll").click(function () {
			$("#selUser").emptySelect();
			$("#selUserSelected").emptySelect();
			for (var i = 0; i < m_Users.length; i++) {
				$("#selUser").addOption(m_Users[i].id, m_Users[i].name);
			}
			setButtonState();
		});

		$("#btnAssign").click(function () {
			if ($("#selStart").val() > $("#selEnd").val()) {
				alert("结束日期不能小于结束日期");
				$("#selStart").focus();
				return;
			}
			var arrUserId = new Array();
			$("#selUserSelected option").each(function() {
				arrUserId.push(this.value);
			});
			if (arrUserId.length == 0) {
				alert("请选择员工到预设区");
				return;
			}
			var data = { userIds: arrUserId.join(","), rosteringId: <%=GetQSInteger("id") %>, start: $("#selStart").val(), end: $("#selEnd").val()};
			$.post("RosteringAjax.aspx?type=save_pr", data, function (response) {
				if (response == "true") {
					alert("预设成功！");
					$("#btnLeftAll").click();
					$("#selDept").change();
				}
				else {
					alert("预设失败：" + response);
				}
			});
		});

		$("#btnBack").click(function () {
			var qs = removeQSItem(location.search, "id");
			location.href = "RosteringList.aspx" + qs;
		});

		$.ajaxSettings.async = false;
		initDateList();
		setButtonState();
		if($("#selDept option").length == 2) {
			$("#selDept option:first").remove();
		}
		$("#selDept").change();
	});

	// Return a json object as {index:i, option:o}
	function getOptionIndex(targetSelect, value) {
		var myIndex = inUserArray(value);
		var selectedOptions = targetSelect.attr("options");
		for (var i = 0; i < selectedOptions.length; i++) {
			if (inUserArray(selectedOptions[i].value) > myIndex) {
				return { index: i, option: selectedOptions[i] };
			}
		}
		return null;
	}

	function inUserArray(value) {
		for (var i = 0; i < m_Users.length; i++) {
			if (m_Users[i].id == value) {
				return i;
			}
		}
		return -1;
	}

	function setButtonState() {
		$("#btnAssign").attr("disabled", $("#selUserSelected").attr("options").length == 0);
	}

	function initDateList() {
		var arrDate = new Array();
		for(var i=0; i<31; i++) {
			arrDate.push("<option value='" + (i + 1).toString() + "'>" + (i + 1).toString() + "</option>");
		}
		$("#selStart,#selEnd").each(function () {
			$(this).html(arrDate.join("\r\n"));
		});
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<div style="height:260px">
		<div style="float:left; width:360px; height:100%;">
			<fieldset style="height:90%">
				<legend>选择区</legend>
				<table cellpadding="3" cellspacing="0">
					<tr>
						<td style="width:30px;">部门</td>
						<td><select id="selDept" class="default">
							<option value="-1">-选择部门-</option>
							<asp:Repeater ID="repeaterDept" runat="server">
							<ItemTemplate>
							<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
							</ItemTemplate>
							</asp:Repeater>
						</select></td>
					</tr>
					<tr>
						<td valign="top">员工</td>
						<td style="position:relative;"><select id="selUser" style="width:290px;" multiple="multiple" size="10"></select>
							<img id="imgUser" style="position:absolute; top: 30px; left: 100px; display:none;" src="../Image/ajax-loader.gif" alt="" />
						</td>
					</tr>
				</table>
			</fieldset>
		</div>
		<div style="float:left; width:60px; height:100%; text-align:center;">
			<div style="margin-top:60px;"><input type="button" class="button2" id="btnRight" value="&gt;" /></div>
			<div style="margin-top:10px;"><input type="button" class="button2" id="btnRightAll" value="&gt;&gt;" /></div>
			<div style="margin-top:10px;"><input type="button" class="button2" id="btnLeft" value="&lt;" /></div>
			<div style="margin-top:10px;"><input type="button" class="button2" id="btnLeftAll" value="&lt;&lt;" /></div>
		</div>
		<div style="float:left; width:330px; height:100%;">
			<fieldset style="height:90%">
				<legend>预设区</legend>
				<div style="margin:10px 0px;"><%=Rostering.Name%>，签到时间：<%=Rostering.StartTime%>，签退时间：<%=Rostering.EndTime%></div>
				<div>
					<div style="float:left;"><select id="selUserSelected" style="width:120px;" multiple="multiple" size="10"></select></div>
					<div style="float:left; margin-left: 10px;">
						<div style="margin:40px 0px 10px;">预设日期区间</div>
						<div>从
							<select id="selStart"></select>
							日至
							<select id="selEnd"></select>
							日
						</div>
						<div style="clear:both"></div>
						<div style="text-align:right; margin-top:40px; margin-right:20px;"><input type="button" class="button2" id="btnAssign" value="预设" /></div>
					</div>
					<div style="clear:both"></div>
				</div>
			</fieldset>
		</div>
		<div style="clear:both"></div>
	</div>
	<div>预设班次状态：</div>
	<div style="position:relative;">
		<table id="assignment">
			<tr class="head">
				<td>1</td><td>2</td><td>3</td><td>4</td><td>5</td><td>6</td><td>7</td><td>8</td><td>9</td><td>10</td>
			</tr>
			<tr>
				<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>
			</tr>
			<tr class="head">
				<td>11</td><td>12</td><td>13</td><td>14</td><td>15</td><td>16</td><td>17</td><td>18</td><td>19</td><td>20</td>
			</tr>
			<tr>
				<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>
			</tr>
			<tr class="head">
				<td>21</td><td>22</td><td>23</td><td>24</td><td>25</td><td>26</td><td>27</td><td>28</td><td>29</td><td>30</td>
			</tr>
			<tr>
				<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>
			</tr>
			<tr class="head">
				<td>31</td>
			</tr>
			<tr>
				<td></td>
			</tr>
		</table>
		<img id="imgAssignment" style="position:absolute; top: 20px; left: 320px; display:none;" src="../Image/ajax-loader.gif" alt="" />
	</div>
	<div style="margin-top:10px; text-align:center;"><input type="button" class="button2" id="btnBack" value="返回" /></div>
</asp:Content>
