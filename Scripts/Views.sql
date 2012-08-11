IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AccessLogView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.AccessLogView
END
GO
CREATE VIEW dbo.AccessLogView
AS
SELECT A.*, U.deptId, U.rankId, usertype = U.type, userName = RTRIM(U.username), deptName = RTRIM(D.deptName)
	, rank = RTRIM(R.rank)
	, [stateName] = CASE CAST(A.state AS INT)
			WHEN 0 THEN N'正常'
			WHEN 1 THEN N'迟到'
			WHEN 2 THEN N'早退'
			WHEN 3 THEN N'班次外考勤'
			WHEN 4 THEN N'异常'
			WHEN 101 THEN N'签退时签到'
			WHEN -1 THEN N'未设置班次'
			ELSE '未知'
		END
	, device = CASE A.devNum
			WHEN 1 THEN N'井口入1通道'
			WHEN 2 THEN N'井口入2通道'
			WHEN 3 THEN N'井口出通道'
			WHEN 4 THEN N'井口无刷卡通道'
			WHEN 5 THEN N'联建楼'
			WHEN 6 THEN N'办公楼'
			WHEN 7 THEN N'生活区'
			WHEN 8 THEN N'出井刷卡通道'
			ELSE '未知'
		END
	, AccessFlagName = CASE A.accessFlag WHEN 0 THEN N'签到' WHEN 1 THEN N'签退' WHEN 99 THEN N'异常' ELSE '未知' END
FROM UserInfo U, DeptInfo D, RankInfo R, AccessLog A
WHERE U.deptid=D.deptid AND U.rankId=R.id AND U.userid=A.userid AND A.recResult=1
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AttendanceMngView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.AttendanceMngView
END
GO
CREATE VIEW dbo.AttendanceMngView
AS
SELECT A.*
	, DeptID = U.deptId
	, UserName = RTrim(U.userName)
	, DeptName = RTrim(D.deptName)
	, InWellName = (CASE WHEN ifIn = 1 THEN N'下井' ELSE N'地面' END)
	, NightWorkName = (CASE nightWork WHEN 0 THEN N'无' WHEN 1 THEN N'前夜' WHEN 2 THEN N'后夜' WHEN 3 THEN N'前&后夜' ELSE N'未知' END)
	, ApprovedName = (CASE WHEN A.state = 1 THEN N'有效' ELSE N'无效' END)
FROM AttendanceMng A
	INNER JOIN UserInfo U ON U.userId = A.userId
	INNER JOIN DeptInfo D ON D.deptId = U.deptId
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.DevInfoView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.DevInfoView
END
GO
CREATE VIEW dbo.DevInfoView
AS
SELECT D.*
	, DevTypeName = P.paraValue
	, AccessFlagName = (
		CASE D.accessFlag
			WHEN 0 THEN N'只入不出'
			WHEN 1 THEN N'只出不入'
			WHEN 2 THEN N'既入又出'
			ELSE N'未知类型'
		END)
	, ActiveName = (CASE WHEN flag = 1 THEN N'有效' ELSE N'无效' END)
FROM DevInfo D
	INNER JOIN [Param] P ON D.devType = P.ID AND P.paraType = 1
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.HolidayInfoView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.HolidayInfoView
END
GO
CREATE VIEW dbo.HolidayInfoView
AS
SELECT *
	, ActiveName = (CASE WHEN flag = 1 THEN N'有效' ELSE N'无效' END)
FROM HolidayInfo
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.LeaveInfoView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.LeaveInfoView
END
GO
CREATE VIEW dbo.LeaveInfoView
AS
SELECT L.*
	, DeptID = U.deptId
	, UserName = RTrim(U.userName)
	, DeptName = RTrim(D.deptName)
	, TypeName = RTrim(P.paraValue)
	, ApprovedName = (CASE WHEN L.flag = 1 THEN N'有效' ELSE N'无效' END)
FROM leaveInfo L
	INNER JOIN UserInfo U ON U.userId = L.userId
	INNER JOIN DeptInfo D ON D.deptId = U.deptId
	INNER JOIN [Param] P ON L.[type] = P.ID AND P.paraType = '3'
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.LogonUserView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.LogonUserView
END
GO
CREATE VIEW dbo.LogonUserView
AS
SELECT U.*
	, RoleName = P.paraValue
	, DeptName = ISNULL(D.deptName, '')
	, ActiveName = (CASE WHEN flag = 1 THEN N'有效' ELSE N'无效' END)
FROM LogonUser U
	INNER JOIN Param P ON U.roleType = P.ID AND P.paraType = '2'
	LEFT JOIN DeptInfo D ON D.deptId = U.depId
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PreRosteringView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.PreRosteringView
END
GO
CREATE VIEW dbo.PreRosteringView
AS
SELECT P.*
	, UserName = (SELECT RTrim(U.userName) FROM UserInfo U WHERE U.userId = P.userId)
	, d1Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d1)
	, d2Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d2)
	, d3Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d3)
	, d4Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d4)
	, d5Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d5)
	, d6Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d6)
	, d7Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d7)
	, d8Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d8)
	, d9Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d9)
	, d10Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d10)
	, d11Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d11)
	, d12Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d12)
	, d13Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d13)
	, d14Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d14)
	, d15Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d15)
	, d16Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d16)
	, d17Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d17)
	, d18Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d18)
	, d19Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d19)
	, d20Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d20)
	, d21Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d21)
	, d22Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d22)
	, d23Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d23)
	, d24Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d24)
	, d25Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d25)
	, d26Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d26)
	, d27Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d27)
	, d28Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d28)
	, d29Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d29)
	, d30Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d30)
	, d31Name = (SELECT RTrim(bcName) FROM Rostering R WHERE R.ID = P.d31)
FROM PreRostering P
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RosteringView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.RosteringView
END
GO
CREATE VIEW dbo.RosteringView
AS
SELECT *
	, MultipleTypeName = (CASE multType WHEN 1 THEN N'单时段' WHEN 2 THEN N'多时段' ELSE N'' END)
	, NightWorkName = (CASE nightWork WHEN 0 THEN N'无' WHEN 1 THEN N'前夜' WHEN 2 THEN N'后夜' WHEN 3 THEN N'前&后夜' ELSE N'未知' END)
	, ActiveName = (CASE WHEN flag = 1 THEN N'有效' ELSE N'无效' END)
FROM Rostering
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UserInfoView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.UserInfoView
END
GO
CREATE VIEW dbo.UserInfoView
AS
SELECT U.*
	, DeptName = RTrim(D.deptName)
	, RankName = RTrim(R.Rank)
	, TypeName = (CASE [type] WHEN 0 THEN N'正式工' WHEN 1 THEN N'劳务工' WHEN 2 THEN N'中煤中宇' END)
	, CopyTypeName = (CASE [copyType]
			WHEN 1 THEN N'下井员工'
			WHEN 2 THEN N'联建楼(地面工种)'
			WHEN 3 THEN N'联建楼(地面工种,需要下井)'
			WHEN 4 THEN N'队长,副队长,技术员'
			WHEN 5 THEN N'办公楼(不下井)'
			WHEN 6 THEN N'办公楼(需要下井)'
			WHEN 7 THEN N'生活区'
			WHEN 8 THEN N'下井员工(出井刷卡)'
			WHEN 9 THEN N'联建楼(出井刷卡)'
			ELSE N'未知'
		END)
FROM UserInfo U
	INNER JOIN DeptInfo D ON D.deptId = U.deptId
	INNER JOIN RankInfo R ON R.ID = U.rankId
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.WorkDurationView') AND OBJECTPROPERTY(id, N'IsView') = 1) BEGIN
	DROP VIEW dbo.WorkDurationView
END
GO
CREATE VIEW dbo.WorkDurationView
AS
SELECT W.*, U.deptId
	, userType = U.type
	, userName = RTRIM(U.username)
	, deptName = RTRIM(D.deptName)
	, workType = CASE WHEN devnum<5 THEN N'入井' ELSE N'地面' END
	, comment = CASE
			WHEN duration > 0 THEN N'正常'
			WHEN duration = 0 THEN N'尚未签退'
			WHEN duration =-1 THEN N'超时未签退,不记工'
			WHEN duration =-2 THEN N'不足时或未下井!'
			ELSE N'未知'
		END
	, NightWorkName = (CASE nightWork WHEN 0 THEN N'无' WHEN 1 THEN N'前夜' WHEN 2 THEN N'后夜' WHEN 3 THEN N'前&后夜' ELSE N'未知' END)
	, bak1Name = CASE W.bak1 WHEN 0 THEN N'正常' WHEN 1 THEN N'迟到' WHEN 2 THEN N'早退' WHEN 3 THEN N'迟到及早退' ELSE '未知' END
FROM UserInfo U, DeptInfo D, WorkDuration W
WHERE D.deptid=U.deptid AND U.userid=W.userid
GO