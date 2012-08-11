<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DeptForm.aspx.cs" Inherits="System_DeptForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
	var deptId = "<%=GetQSInteger("id") %>";
	$.ajaxSettings.async = false;

	$(function () {
		$("#btnSave").click(function () {
			if (isEmpty($("#txtId").val())) {
				alert("请输入部门编号");
				$("#txtId").focus();
				return;
			}
			if (isEmpty($("#txtName").val())) {
				alert("请输入部门名称");
				$("#txtName").focus();
				return;
			}
			var pass = true;
			if (deptId < 0) { // Adding a new dept, so, check duplication on both id and name
				$.get("DeptAjax.aspx?type=duplicate_new"
					, { id: $("#txtId").val(), name: $("#txtName").val() }
					, function (response) {
						if (response == "id") {
							alert("编号为\"" + $("#txtId").val() + "\"的部门已经存在！ 不能新增重复的部门。");
							$("#txtId").focus();
							pass = false;
						}
						else if (response == "name") {
							alert("名称为\"" + $("#txtName").val() + "\"的部门已经存在！ 不能新增重复的部门。");
							$("#txtName").focus();
							pass = false;
						}
					}
				);
			}
			else { // Updating an existing dept, so, check duplication on the name
				$.get("DeptAjax.aspx?type=duplicate_exist"
					, { id: $("#txtId").val(), name: $("#txtName").val() }
					, function (response) {
						if (response == "name") {
							alert("名称为\"" + $("#txtName").val() + "\"的部门已经存在！ 不能再使用这个名称。");
							$("#txtName").focus();
							pass = false;
						}
					}
				);
			}

			if (pass) {
				$.post("DeptAjax.aspx?type=save"
					, { id: $("#txtId").val(), name: $("#txtName").val() }
					, function (response) {
						$("#btnBack").click();
					}
				);
			}
		});

		$("#btnBack").click(function () {
			location.href = "DeptList.aspx";
		});

		setIdState();
		loadDept(deptId);
	});

	function loadDept(id) {
		if (id == null || id < 0) {
			return;
		}

		$.get("DeptAjax.aspx?type=get", {id: id}, function(response) {
			eval("var dept = " + response);
			$("#txtId").val(dept.id);
			$("#txtName").val(dept.name);
		});
	}

	function setIdState() {
		if (deptId >= 0) {
			$("#txtId").attr("disabled", true);
		}
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=PageTitle%></div>
	<table style="margin-left: 40px;">
		<tr>
			<td style="width:80px;">部门编号</td>
			<td><input type="text" id="txtId" class="textbox" /></td>
		</tr>
		<tr>
			<td>部门名称</td>
			<td><input type="text" id="txtName" class="textbox" maxlength="50" /></td>
		</tr>
	</table>
	<div style="margin:10px 0px 0px 80px;">
		<input type="button" class="button2" id="btnSave" value="保存" />
		<input type="button" class="button2" id="btnBack" value="返回" />
	</div>
</asp:Content>
