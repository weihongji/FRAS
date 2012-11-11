/* jQuery Extensions
----------------------------------------------------------*/
(function ($) {
	$.fn.emptySelect = function (reserveFirst) {
		return this.each(function () {
			if (this.tagName == 'SELECT') {
				if (reserveFirst) {
					if (this.options.length == 1) {
						return;
					}
					else if (this.options.length > 1) {
						var option = new Option(this.options[0].text, this.options[0].value);
						this.options.length = 0;
						if ($.browser.msie) {
							this.add(option);
						}
						else {
							this.add(option, null);
						}
					}
				}
				else {
					this.options.length = 0;
				}
			}
		});
	}

	$.fn.loadSelect = function (optionsDataArray, reserveFirst) {
		return this.emptySelect(reserveFirst).each(function () {
			if (this.tagName == 'SELECT') {
				var selectElement = this;
				$.each(optionsDataArray, function (index, optionData) {
					var option = new Option(optionData.name, optionData.id);
					if ($.browser.msie) {
						selectElement.add(option);
					}
					else {
						selectElement.add(option, null);
					}
				});
			}
		});
	}

	$.fn.outerHTML = function () {
		return $('<div>').append(this.eq(0).clone()).html();
	}

	$.fn.replace = function (content) {
		return this.each(function () {
			$(this).after(content).remove();
		});
	}

	//insertAt = {index:i, option:o}
	$.fn.addOption = function (value, text, insertAt) {
		return this.each(function () {
			if (value.tagName && value.tagName == "OPTION") {
				insertAt = text;
				text = value.text;
				value = value.value;
			}
			var options = this.options;
			insertAt = insertAt || { index: options.length, option: null };
			var option = new Option(text, value);
			if ($.browser.msie) {
				this.add(option, insertAt.index);
			}
			else {
				this.add(option, insertAt.option);
			}
		});
	}

	$.fn.removeOption = function (value) {
		return this.each(function () {
			for (var i = this.options.length - 1; i >= 0; i--) {
				if (this.options[i].value == value) {
					this.remove(i); return;
				}
			}
		});
	}

	$.showModalDialog = function (url, arguments, features) {
		var result;
		if (navigator.userAgent.toLowerCase().indexOf("chrome") >= 0) { // To fix the Google Chrome bug
			// Removed "px" from dialogWidth and dialogHeight
			if (features) {
				var match = features.match(/dialogWidth\s*:\s*\d+px/);
				if (match) {
					features = features.replace(match[0], match[0].replace("px", ""));
				}
				match = features.match(/dialogHeight\s*:\s*\d+px/);
				if (match) {
					features = features.replace(match[0], match[0].replace("px", ""));
				}
			}
			// Handle returnValue
			var prevReturnValue = window.returnValue; // Save the current returnValue
			window.returnValue = undefined;
			result = window.showModalDialog(url, arguments, features);
			if (result == undefined) { // We don't know here if undefined is the real result...
				// So we take no chance, in case this is the Google Chrome bug
				result = window.returnValue;
			}
			window.returnValue = prevReturnValue; // Restore the original returnValue
		}
		else { // Browsers other than Chrome
			result = window.showModalDialog(url, arguments, features);
		}
		return result;
	}
})(jQuery);

/* Layout Adjustment
----------------------------------------------------------*/
function extendModule(module) {
	//Collapse all menu folders
	$("#left>ul").hide();
	//Extend the specifed one
	$("#left>img[alt='" + module + "']").next("ul").show();
}

function highlightMenu(menu) {
	$("#left a").filter(function () { return this.innerHTML == menu; }).addClass("highlight");
}

function setNavigationBar(module) {
	var localTitle = $("div.title").text();
	var moduleName = "";
	if (localTitle.length > 0 && localTitle.indexOf(">") < 0) {
		switch (module.toLowerCase()) {
			case "query":
				moduleName = "查询报表";
				break;
			case "attendance":
				moduleName = "考勤管理";
				break;
			case "system":
				moduleName = "系统管理";
				break;
		}
		$("div.title").text(moduleName + " > " + localTitle);
	};
}

function setFrameWidth(width) {
	if (!isNumeric(width) || width == 0) {
		return;
	}
	$("div.page").css("width", width.toString() + "px");
}

function increaseContentWidth(amount) {
	if (!isNumeric(amount) || amount == 0) {
		return;
	}
	$("#right>div").each(function () {
		var width = parseInt($(this).css("width"), 10);
		if (!isNaN(width)) {
			width += amount;
			$(this).css("width", width.toString() + "px");
		}
	});
}

function getWindowWidth() {
	var width;
	if (document.body && document.body.offsetWidth) {
		width = document.body.offsetWidth;
	}
	else if (document.compatMode == 'CSS1Compat' && document.documentElement && document.documentElement.offsetWidth) {
		width = document.documentElement.offsetWidth;
	}
	else if (window.innerWidth && window.innerHeight) {
		width = window.innerWidth;
	}
	return width;
}

function bindMenuCollapseEvents(callback) {
	$("#left_collapsed_holder").show(); //启用菜单折叠功能

	$("#left_collapsed_holder").mouseover(function () {
		$("#left_collapsed_holder").hide();
		$("#left_collapsed").show(100);
	});

	$("#left_collapsed").mouseout(function () {
		//当菜单显示时，自动隐藏折叠条
		if ($("#left_collapsed").text().indexOf("隐藏菜单") >= 0) {
			$("#left_collapsed_holder").show();
			$("#left_collapsed").hide(500);
		}
	});

	$("#left_collapsed").click(function () {
		if ($("#left_collapsed").text().indexOf("显示菜单") >= 0) {
			if (typeof (callback) == "function") {
				callback(false);
			}
			$("#left").show();
			$("#left_collapsed").text("隐藏菜单<<");
		}
		else {
			$("#left").hide();
			$("#left_collapsed").text("显示菜单>>");
			if (typeof (callback) == "function") {
				callback(true);
			}
		}
	});
}

function collapseMenu() {
	$("#left_collapsed_holder").mouseover();
	$("#left_collapsed").click();
}

function stopDeviceMonitor() {
	$("#device_monitor").hide();
	window.clearInterval(m_deviceMonitorTimer);
}

function refreshDeviceMonitor() {
	$.ajax({
			async: true
		, type: "GET"
		, url: "../System/DeviceAjax.aspx?type=monitor&stamp=" + (new Date()).toString()
		, success: function(response) {
			eval("var devices = " + response);
			var html = "", dev;
			for(var i=0; i<devices.length; i++) {
				dev = devices[i];
				if (i == 0) {
					html += "<tr><td style='padding-bottom:10px;'>" + dev.text + ":</td><td style='padding-bottom:10px;" + (dev.value.indexOf("异常") < 0 ? " color:Green;" : " color:Red;") + "'>" + dev.value + "</td></tr>";
				}
				else {
					html += "<tr><td>" + dev.text + ":</td><td" + (dev.value.indexOf("异常") < 0 ? " style='color:Green;'" : " style='color:Red;'") + ">" + dev.value + "</td></tr>";
				}
			}
			$("#device_monitor>table:first").html(html);

			if ($("#device_monitor").css("display") == "none") {
				$("#device_monitor").show();
			}
		}
	});
}

/* Number Utilities
----------------------------------------------------------*/
//format "12345" to "12,345.00"
function formatMoney(val) {
	if (val == null || isNaN(val)) {
		return "";
	}
	val = val.toString().replace(/\,/g, "");
	val = parseFloat(val);
	if (isNaN(val)) {
		return "";
	}
	var decimal = Math.round((val - Math.floor(val)) * 100).toString();
	if (decimal.length == 1) {
		decimal = "0" + decimal;
	}
	var money = "." + decimal;
	val = Math.floor(val).toString();
	while (val.length > 3) {
		money = "," + val.substr(val.length - 3) + money;
		val = val.substr(0, val.length - 3);
	}
	money = val + money;
	return money;
}

/* Date Utilities
----------------------------------------------------------*/
//format a date to format "2012-05-16"
function formatDate(val) {
	var dt = toDate(val);
	var y = dt.getFullYear().toString();
	var m = "0" + (dt.getMonth() + 1).toString();
	var d = "0" + dt.getDate().toString();
	if (m.length>2) { m = m.substring(1);}
	if (d.length>2) { d = d.substring(1);}
	return y + "-" + m + "-" + d;
}

/* String Utilities
----------------------------------------------------------*/
/*
Description:
Encode a query string item to make it requested correctly at server side.
Examples:
location.href="Save.asp?descript=" + sDescription.convertQS();
*/
String.prototype.encodeQS = function () {
	var s = this;
	s = escape(s).replace(/\+/g, "%2B");
	return s;
}

/*
Description:
Add an item to a string of items. Items are separated by commas.
This function can avoid item duplication in the string.
Parameters:
stringItems		- The string that a new item will be added to.
item			- The item to be added.
isRefreshOrder	- (Optional) Flag to indicates if to refresh order of items in the string after add the new item. See examples below to get more understanding.
Examples:
addStringItem("a,b,c", "b")			-> "a,b,c" (not "a,b,c,b")
addStringItem("a,b,c", "b", true)	-> "a,c,b" ("b" will be moved to be the last one.)
*/
function addStringItem(stringItems, item, isRefreshOrder) {
	item = item.toString();

	if (isRefreshOrder) {
		// Remove the item if exists
		stringItems = removeStringItem(stringItems, item);
	}

	var s = "," + stringItems + ",";
	if (s.indexOf("," + item + ",") == -1) {
		stringItems += (stringItems.length == 0 ? "" : ",") + item;
	}
	return stringItems;
}

/*
Description:
Remove an item from a string of items. Items are separated by commas.
Parameters:
stringItems		- The string that a new item will be added to.
item			- The item to be added.
Examples:
removeStringItem("ab,b,c", "b") will get "ab,c"
*/
function removeStringItem(stringItems, item) {
	var s = "," + stringItems + ",";
	s = s.replace("," + item.toString() + ",", ",");
	if (s.length > 2) {
		s = s.substring(1, s.length - 1);
	}
	else {
		s = "";
	}
	return s;
}

/*
Description:
	var qs = "http://localhost/FRAS/System/UserList.aspx?item1=a&item2=b&item3=c&item4="
	setQSItem(qs, "item2", "x") will returns:
		"http://localhost/FRAS/System/UserList.aspx?item1=a&item2=x&item3=c&item4="
*/
function setQSItem(qs, name, value) {
	var item = name + "=" + value;
	qs == qs || "";
	// No query string item
	if (qs.indexOf("=") < 0) {
		return qs + "?" + item;
	}
	// One or more query string items
	var exp = new RegExp("[\?&]?" + name + "(=[^&]*)");
	var result = exp.exec(qs);
	if (result) {
		var match = result[0];
		var prefix = match[0];
		if (prefix == "?" || prefix == "&") {
			item = prefix + item;
		}
		return qs.replace(match, item);
	}
	else {
		return qs + (qs.indexOf("?") < 0 ? "?" : "&") + item;
	}
}

function removeQSItem(qs, name) {
	qs == qs || "";
	// No query string item
	if (qs.indexOf("=") < 0) {
		return qs;
	}
	// One or more query string items
	var exp = new RegExp("[\?&]?" + name + "(=[^&]*)");
	var result = exp.exec(qs);
	if (result) {
		var match = result[0];
		if (match.indexOf("?") >= 0) {
			qs = qs.replace(match, "?");
		}
		else {
			qs = qs.replace(match, "");
		}
		qs = qs.replace("?&", "?");
		if (qs.substring(qs.length - 1) == "?") {
			qs = qs.substring(0, qs.length - 1);
		}
	}
	return qs;
}

//Notes: Characters in the extension will be converted into lower case.
function getFileExtension(fileName) {
	if (fileName == undefined || fileName.indexOf(".") < 0) {
		return "";
	}
	var arrPart = fileName.split(".");
	return arrPart[arrPart.length-1].toLowerCase();
}

/* Validations
----------------------------------------------------------*/
function isEmpty(val) {
	return val == null || val.toString().replace(/^\s+/g, "").replace(/\s+$/g, "").length == 0;
}

function isNumeric(val) {
	if (typeof (val) == "number") {
		return true;
	}
	return (val - 0) == val && val.length > 0;
}

function isDate(val) {
	if (isEmpty(val)) return false;
	var arrPortion = val.toString().split("-");
	if (arrPortion.length != 3) {
		return false;
	}
	var dt = toDate(val);
	return dt.getFullYear() == parseInt(arrPortion[0], 10) && dt.getMonth()+1 == parseInt(arrPortion[1], 10) && dt.getDate() == parseInt(arrPortion[2], 10);
}

function toDate(val) {
	var dt = new Date(val);
	if (isNaN(dt)) { // IE & FF only accept format of "2012/05/16"
		dt = new Date(val.replace(/-/g, "/"));
	}
	return dt;
}

function isTime(val) {
	if (isEmpty(val)) return false;
	return isDate("2000-1-1 " + val);
}
