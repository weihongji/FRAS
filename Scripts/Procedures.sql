IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.spAttendanceReport') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) BEGIN
	DROP PROCEDURE dbo.spAttendanceReport
END
GO
CREATE PROCEDURE dbo.spAttendanceReport
	@DeptId int,
	@UserType int,
	@DeviceId int,
	@StartDate smalldatetime = null,
	@EndDate smalldatetime = null,
	@Year int = null,
	@Month int = null
AS
BEGIN
	DECLARE @report_date table (date varchar(10))
	DECLARE @holiday table (date varchar(10))
	DECLARE @startDateStr varchar(20)
	DECLARE @endDateStr varchar(20)
	DECLARE @tempDate smalldatetime
	DECLARE @mustDays int
	DECLARE @monthModel bit /* In month model, attendance status of each day in the month is required to display. */

	SET @monthModel = 0
	/* Regulate parameters */
	IF @Year > 2000 AND @Month BETWEEN 1 AND 12 BEGIN
		SET @StartDate = CAST(CAST(@Year AS varchar) + RIGHT('0' + CAST(@Month AS varchar), 2) + '01' AS smalldatetime)
		SET @EndDate   = DATEADD(month, 1, @StartDate)-1
		SET @monthModel = 1
	END
	IF @StartDate IS NULL OR @EndDate IS NULL BEGIN
		RAISERROR(N'Invalid values for parameter @StartDate and @EndDate', 16, 1)
		RETURN
	END
	SET @startDateStr = CONVERT(varchar(10), @StartDate, 120)
	SET @endDateStr = CONVERT(varchar(10), @EndDate, 120)

	/*User*/
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#user_report')) BEGIN
		DROP TABLE #user_report
	END
	SELECT userId = rtrim(U.userId), userName = rtrim(U.userName), deptName = rtrim(D.deptName), U.copyType
	INTO #user_report
	FROM UserInfo U INNER JOIN DeptInfo D ON U.deptId=D.deptId
	WHERE (@DeptId < 0 OR U.deptId = @DeptId) AND (@UserType < 0 OR U.[type] = @UserType)

	/* Report Dates */
	SET @tempDate = @StartDate
	WHILE @tempDate<=@EndDate BEGIN
		INSERT INTO @report_date SELECT CONVERT(varchar(10), @tempDate, 120)
		SET @tempDate = DATEADD(day, 1, @tempDate)
	END

	/* Holiday Dates */
	INSERT INTO @holiday
	SELECT date FROM @report_date
	WHERE EXISTS(SELECT * FROM holidayInfo WHERE flag=1 AND date between startDate and endDate)

	/* Leave Dates */
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#leave_report')) BEGIN
		DROP TABLE #leave_report
	END
	SELECT L.userId, R.date, leaveType = RTRIM(P.paraName)
	INTO #leave_report
	FROM LeaveInfo L
		INNER JOIN Param P ON L.[type]=P.ID
		INNER JOIN @report_date R ON R.date BETWEEN L.startDate AND L.enddate
	WHERE L.flag=1

	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#leave_sum_report')) BEGIN
		DROP TABLE #leave_sum_report
	END
	SELECT userId
		, total = COUNT(*)
		, leave1 = SUM(CASE leaveType WHEN '1'  THEN 1 END)
		, leave2 = SUM(CASE leaveType WHEN '2'  THEN 1 END)
		, leave3 = SUM(CASE leaveType WHEN '3'  THEN 1 END)
		, leave4 = SUM(CASE leaveType WHEN '4'  THEN 1 END)
		, leave5 = SUM(CASE leaveType WHEN '5'  THEN 1 END)
		, leave6 = SUM(CASE leaveType WHEN '6'  THEN 1 END)
		, leave7 = SUM(CASE leaveType WHEN '7'  THEN 1 END)
		, leave8 = SUM(CASE leaveType WHEN '8'  THEN 1 END)
		, leave9 = SUM(CASE leaveType WHEN '9'  THEN 1 END)
		, leave10= SUM(CASE leaveType WHEN '10' THEN 1 END)
	INTO #leave_sum_report
	FROM #leave_report
	GROUP BY userId

	/* Attendance */
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#attendance_report')) BEGIN
		DROP TABLE #attendance_report
	END
	SELECT userId = RTRIM(userId)
		, total   = SUM(workDurs)															/*出勤数*/
		, holiday = SUM(CASE WHEN H.date IS NOT NULL THEN workDurs END)	/*节假日出勤数*/
		, front   = SUM(CASE WHEN nightWork IN (1, 3) THEN 1 END)							/*前夜出勤数*/
		, back    = SUM(CASE WHEN nightWork IN (2, 3) THEN 1 END)							/*后夜出勤数*/
		, well    = SUM(CASE WHEN ifIn=1 THEN 1 END)										/*入井出勤数*/
	INTO #attendance_report
	FROM AttendanceMng A
		LEFT JOIN @holiday H ON A.date = H.date
	WHERE state=1
		AND A.date BETWEEN @startDateStr AND @endDateStr
		AND NOT EXISTS(SELECT * FROM #leave_report L WHERE L.userId = A.userId AND L.date = A.date)
	GROUP BY userId

	/* WorkDuration */
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#workduration_report')) BEGIN
		DROP TABLE #workduration_report
	END
	SELECT W1.userId, W1.total, W1.holiday, W2.late, W2.quit, W2.front, W2.back, W2.well
	INTO #workduration_report
	FROM
		(
			/* WorkDuration 1 */
			SELECT userId
				, total    = SUM(
						CASE WHEN days_onground<1.0 THEN (CASE WHEN days_inwell<1.0 THEN days_onground ELSE 1.0 END) ELSE 1.0 END
					)
				, holiday = SUM(
						CASE WHEN is_holiday = 1 THEN
							CASE WHEN days_onground<1.0 THEN (CASE WHEN days_inwell<1.0 THEN days_onground ELSE 1.0 END) ELSE 1.0 END
						END
					)
			FROM (
					SELECT U.userId, date
						, days_onground = ISNULL(SUM(
							CASE
								WHEN U.copyType IN (3, 4) THEN
									CASE WHEN devNum = 5 THEN mulDur END
								WHEN U.copyType IN (6) THEN
									CASE WHEN devNum = 6 THEN mulDur END
								ELSE
									CASE WHEN devNum != 4 THEN mulDur END
							END), 0)
						, days_inwell   = ISNULL(SUM(
							CASE
								WHEN U.copyType IN (3, 4) THEN
									CASE WHEN devNum < 4 THEN mulDur END
								WHEN U.copyType IN (6) THEN
									CASE WHEN devNum = 4 THEN mulDur END
							END), 0)
						, is_holiday = CASE WHEN EXISTS(SELECT * FROM @holiday H WHERE H.date = workDuration.date) THEN 1 ELSE 0 END
					FROM workDuration
						INNER JOIN #user_report U ON workDuration.userId = U.userId
					WHERE duration>0
						AND (@DeviceId <= 0 OR devNum = @DeviceId)
						AND date BETWEEN @startDateStr AND @endDateStr
						AND NOT EXISTS(SELECT * FROM #leave_report L WHERE L.userId = workDuration.userId AND L.date = workDuration.date)
					GROUP BY U.userId, date
				) AS T
			GROUP BY userId
		) AS W1
		INNER JOIN
		(
			/* WorkDuration 2 */
			SELECT userId
				, late = SUM(CASE WHEN bak1='1' or bak1='3' THEN 1 END)		/*迟到*/
				, quit  = SUM(CASE WHEN bak1='2' or bak1='3' THEN 1 END)		/*早退*/
				, front = SUM(CASE WHEN nightWork IN (1, 3) THEN 1 END)	/*前夜*/
				, back  = SUM(CASE WHEN nightWork IN (2, 3) THEN 1 END)	/*后夜*/
				, well  = SUM(CASE WHEN devNum<5 THEN 1 END)					/*入井*/
			FROM workDuration
			WHERE duration>0
				AND (@DeviceId <= 0 OR devNum = @DeviceId)
				AND date BETWEEN @startDateStr AND @endDateStr
				AND NOT EXISTS(SELECT * FROM #leave_report L WHERE L.userId = workDuration.userId AND L.date = workDuration.date)
			GROUP BY userId
		) AS W2 ON W2.userId = W1.userId
	
	/* 应出勤 */
	SELECT @mustDays = COUNT(*) FROM @report_date
	WHERE date NOT IN(SELECT date FROM @holiday) AND DATEPART(weekday, date) NOT IN (1, 7)

	/* the final query */
	IF @monthModel = 0 BEGIN
		SELECT U.deptName, U.userId, U.userName
			, must   = @mustDays									/*应出勤*/
			, normal = ISNULL(A.total, 0) + ISNULL(W.total, 0) - (ISNULL(W.holiday, 0) + ISNULL(A.holiday, 0)) /*日常出勤*/
			, late   = ISNULL(W.late, 0)							/*迟到*/
			, quit   = ISNULL(W.quit, 0)							/*早退*/
			, kuang  = CASE WHEN ISNULL(A.total, 0) + ISNULL(W.total, 0) + ISNULL(L.total, 0) - @mustDays < 0 THEN ABS(ISNULL(A.total, 0) + ISNULL(W.total, 0) + ISNULL(L.total, 0) - @mustDays) ELSE 0 END /*旷工*/
			, overtime_non_holiday = CASE WHEN ISNULL(A.total, 0) + ISNULL(W.total, 0)- @mustDays - (ISNULL(W.holiday, 0) + ISNULL(A.holiday, 0)) + ISNULL(L.leave10, 0)>0 THEN ISNULL(A.total, 0) + ISNULL(W.total, 0)- @mustDays - (ISNULL(W.holiday, 0) + ISNULL(A.holiday, 0)) + ISNULL(L.leave10, 0) ELSE 0 END /*日常加班*/
			, overtime_holiday = ISNULL(W.holiday, 0) + ISNULL(A.holiday, 0)  /*节假日加班*/
			, front  = ISNULL(A.front, 0) + ISNULL(W.front, 0)		/*前夜*/
			, back   = ISNULL(A.back, 0) + ISNULL(W.back, 0)		/*后夜*/
			, well   = ISNULL(A.well, 0) + ISNULL(W.well, 0)		/*入井*/
			, leave1 = ISNULL(L.leave1, 0)							/*休假*/
			, leave2 = ISNULL(L.leave2, 0)							/*事假*/
			, leave3 = ISNULL(L.leave3, 0)							/*病假*/
			, leave4 = ISNULL(L.leave4, 0)							/*工伤*/
			, leave5 = ISNULL(L.leave5, 0)							/*年休*/
			, leave6 = ISNULL(L.leave6, 0)							/*婚假*/
			, leave7 = ISNULL(L.leave7, 0)							/*产假*/
			, leave8 = ISNULL(L.leave8, 0)							/*丧假*/
			, leave9 = ISNULL(L.leave9, 0)							/*探亲假*/
			, leave10= ISNULL(L.leave10, 0)							/*出差*/
		FROM #user_report AS U
			LEFT JOIN #attendance_report AS A ON A.userId = U.userId
			LEFT JOIN #workduration_report AS W ON W.userId = U.userId
			LEFT JOIN #leave_sum_report AS L ON L.userId = U.userId
		ORDER BY deptName, userName
	END
	ELSE BEGIN
		IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#daily_attendance')) BEGIN
			DROP TABLE #daily_attendance
		END
		SELECT userId, DATE, ifIn = MAX(ifIn)
		INTO #daily_attendance
		FROM (
				SELECT userId, DATE, ifIn = MAX(ifIn)
				FROM AttendanceMng
				WHERE state=1 AND DATE BETWEEN @startDateStr AND @endDateStr
				GROUP BY userId, DATE
				UNION
				SELECT userId, DATE, ifIn = MAX(CASE WHEN devNum<5 THEN 1 ELSE 0 END)
				FROM WorkDuration
				WHERE duration>0 AND DATE BETWEEN @startDateStr AND @endDateStr
				GROUP BY userId, DATE
			) W
		GROUP BY userId, DATE
		
		IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#daily_status')) BEGIN
			DROP TABLE #daily_status
		END
		SELECT userId, d1 = MIN(CASE WHEN theDay = 1 THEN ISNULL(leaveType, ifIn+11) END)
			, d2 = MIN(CASE WHEN theDay = 2 THEN ISNULL(leaveType, ifIn+11) END)
			, d3 = MIN(CASE WHEN theDay = 3 THEN ISNULL(leaveType, ifIn+11) END)
			, d4 = MIN(CASE WHEN theDay = 4 THEN ISNULL(leaveType, ifIn+11) END)
			, d5 = MIN(CASE WHEN theDay = 5 THEN ISNULL(leaveType, ifIn+11) END)
			, d6 = MIN(CASE WHEN theDay = 6 THEN ISNULL(leaveType, ifIn+11) END)
			, d7 = MIN(CASE WHEN theDay = 7 THEN ISNULL(leaveType, ifIn+11) END)
			, d8 = MIN(CASE WHEN theDay = 8 THEN ISNULL(leaveType, ifIn+11) END)
			, d9 = MIN(CASE WHEN theDay = 9 THEN ISNULL(leaveType, ifIn+11) END)
			, d10 = MIN(CASE WHEN theDay = 10 THEN ISNULL(leaveType, ifIn+11) END)
			, d11 = MIN(CASE WHEN theDay = 11 THEN ISNULL(leaveType, ifIn+11) END)
			, d12 = MIN(CASE WHEN theDay = 12 THEN ISNULL(leaveType, ifIn+11) END)
			, d13 = MIN(CASE WHEN theDay = 13 THEN ISNULL(leaveType, ifIn+11) END)
			, d14 = MIN(CASE WHEN theDay = 14 THEN ISNULL(leaveType, ifIn+11) END)
			, d15 = MIN(CASE WHEN theDay = 15 THEN ISNULL(leaveType, ifIn+11) END)
			, d16 = MIN(CASE WHEN theDay = 16 THEN ISNULL(leaveType, ifIn+11) END)
			, d17 = MIN(CASE WHEN theDay = 17 THEN ISNULL(leaveType, ifIn+11) END)
			, d18 = MIN(CASE WHEN theDay = 18 THEN ISNULL(leaveType, ifIn+11) END)
			, d19 = MIN(CASE WHEN theDay = 19 THEN ISNULL(leaveType, ifIn+11) END)
			, d20 = MIN(CASE WHEN theDay = 20 THEN ISNULL(leaveType, ifIn+11) END)
			, d21 = MIN(CASE WHEN theDay = 21 THEN ISNULL(leaveType, ifIn+11) END)
			, d22 = MIN(CASE WHEN theDay = 22 THEN ISNULL(leaveType, ifIn+11) END)
			, d23 = MIN(CASE WHEN theDay = 23 THEN ISNULL(leaveType, ifIn+11) END)
			, d24 = MIN(CASE WHEN theDay = 24 THEN ISNULL(leaveType, ifIn+11) END)
			, d25 = MIN(CASE WHEN theDay = 25 THEN ISNULL(leaveType, ifIn+11) END)
			, d26 = MIN(CASE WHEN theDay = 26 THEN ISNULL(leaveType, ifIn+11) END)
			, d27 = MIN(CASE WHEN theDay = 27 THEN ISNULL(leaveType, ifIn+11) END)
			, d28 = MIN(CASE WHEN theDay = 28 THEN ISNULL(leaveType, ifIn+11) END)
			, d29 = MIN(CASE WHEN theDay = 29 THEN ISNULL(leaveType, ifIn+11) END)
			, d30 = MIN(CASE WHEN theDay = 30 THEN ISNULL(leaveType, ifIn+11) END)
			, d31 = MIN(CASE WHEN theDay = 31 THEN ISNULL(leaveType, ifIn+11) END)
		INTO #daily_status
		FROM (
			SELECT userId, theDay = DATEPART(DAY, CAST(DATE AS SMALLDATETIME)), leaveType = MIN(leaveType), ifIn = MAX(ifIn)
			FROM (
				SELECT userId = ISNULL(A.userId, L.userId), DATE = ISNULL(A.DATE, L.DATE), L.leaveType, A.ifIn
				FROM #daily_attendance A
					FULL OUTER JOIN #leave_report L ON A.DATE = L.DATE AND A.userId = L.userId
			) T1
			GROUP BY userId, DATE
		) T2
		GROUP BY userId
		
		SELECT U.deptName, U.userId, U.userName
			, must   = @mustDays									/*应出勤*/
			, normal = ISNULL(A.total, 0) + ISNULL(W.total, 0) - (ISNULL(W.holiday, 0) + ISNULL(A.holiday, 0)) /*日常出勤*/
			, late   = ISNULL(W.late, 0)							/*迟到*/
			, quit   = ISNULL(W.quit, 0)							/*早退*/
			, kuang  = CASE WHEN ISNULL(A.total, 0) + ISNULL(W.total, 0) + ISNULL(L.total, 0) - @mustDays < 0 THEN ABS(ISNULL(A.total, 0) + ISNULL(W.total, 0) + ISNULL(L.total, 0) - @mustDays) ELSE 0 END /*旷工*/
			, overtime_non_holiday = CASE WHEN ISNULL(A.total, 0) + ISNULL(W.total, 0)- @mustDays - (ISNULL(W.holiday, 0) + ISNULL(A.holiday, 0)) + ISNULL(L.leave10, 0)>0 THEN ISNULL(A.total, 0) + ISNULL(W.total, 0)- @mustDays - (ISNULL(W.holiday, 0) + ISNULL(A.holiday, 0)) + ISNULL(L.leave10, 0) ELSE 0 END /*日常加班*/
			, overtime_holiday = ISNULL(W.holiday, 0) + ISNULL(A.holiday, 0)  /*节假日加班*/
			, front  = ISNULL(A.front, 0) + ISNULL(W.front, 0)		/*前夜*/
			, back   = ISNULL(A.back, 0) + ISNULL(W.back, 0)		/*后夜*/
			, well   = ISNULL(A.well, 0) + ISNULL(W.well, 0)		/*入井*/
			, leave1 = ISNULL(L.leave1, 0)							/*休假*/
			, leave2 = ISNULL(L.leave2, 0)							/*事假*/
			, leave3 = ISNULL(L.leave3, 0)							/*病假*/
			, leave4 = ISNULL(L.leave4, 0)							/*工伤*/
			, leave5 = ISNULL(L.leave5, 0)							/*年休*/
			, leave6 = ISNULL(L.leave6, 0)							/*婚假*/
			, leave7 = ISNULL(L.leave7, 0)							/*产假*/
			, leave8 = ISNULL(L.leave8, 0)							/*丧假*/
			, leave9 = ISNULL(L.leave9, 0)							/*探亲假*/
			, leave10= ISNULL(L.leave10, 0)							/*出差*/
			, d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16, d17, d18, d19, d20, d21, d22, d23, d24, d25, d26, d27, d28, d29, d30, d31
		FROM #user_report AS U
			LEFT JOIN #attendance_report AS A ON A.userId = U.userId
			LEFT JOIN #workduration_report AS W ON W.userId = U.userId
			LEFT JOIN #leave_sum_report AS L ON L.userId = U.userId
			LEFT JOIN #daily_status AS S ON S.userId = U.userId
		ORDER BY deptName, userName
	END

	/* Drop Temporary Tables */
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#user_report')) BEGIN
		DROP TABLE #user_report
	END
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#leave_report')) BEGIN
		DROP TABLE #leave_report
	END
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#leave_sum_report')) BEGIN
		DROP TABLE #leave_sum_report
	END
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#attendance_report')) BEGIN
		DROP TABLE #attendance_report
	END
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#workduration_report')) BEGIN
		DROP TABLE #workduration_report
	END
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#daily_attendance')) BEGIN
		DROP TABLE #daily_attendance
	END
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#daily_status')) BEGIN
		DROP TABLE #daily_status
	END
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.spDailyAttendance') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) BEGIN
	DROP PROCEDURE dbo.spDailyAttendance
END
GO
CREATE PROCEDURE dbo.spDailyAttendance
	@Date smalldatetime
AS
BEGIN
	DECLARE @dateStr varchar(20)

	SET @dateStr = CONVERT(varchar(10), @Date, 120)
	
	/*Dept*/
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#daily_attendance_dept')) BEGIN
		DROP TABLE #daily_attendance_dept
	END
	SELECT deptId, DeptName
		, UserCount = SUM(CASE WHEN userId IS NOT NULL THEN 1 ELSE 0 END)
		, XiuJia  = SUM(CASE WHEN XiuJia  > 0 THEN 1 ELSE 0 END)
		, QingJia = SUM(CASE WHEN QingJia > 0 THEN 1 ELSE 0 END)
		, QueQin  = SUM(CASE WHEN QueQin  > 0 THEN 1 ELSE 0 END)
	INTO #daily_attendance_dept
	FROM (
			SELECT D.deptId, DeptName = RTRIM(D.deptName), U.userId
				, XiuJia  = SUM(CASE WHEN L.type = 8 THEN 1 ELSE 0 END)
				, QingJia = SUM(CASE WHEN L.type > 8 THEN 1 ELSE 0 END)
				, QueQin  = SUM(CASE WHEN W.ID IS NULL AND L.ID IS NULL THEN 1 ELSE 0 END)
			FROM DeptInfo D
				LEFT JOIN UserInfo U ON U.deptId = D.deptId
				LEFT JOIN LeaveInfo L ON L.userId = U.userId AND @dateStr BETWEEN L.startDate AND L.endDate
				LEFT JOIN WorkDuration W ON W.userId = U.userId AND W.[date] = @dateStr
			GROUP BY D.deptId, D.deptName, U.userId
		) A
	GROUP BY deptId, deptName

	/*WorkDuration*/
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#daily_attendance_Work')) BEGIN
		DROP TABLE #daily_attendance_Work
	END
	SELECT deptId
		, C_8_W  = SUM(CASE WHEN C_8_W > 0 THEN 1 ELSE 0 END)
		, C_8_G  = SUM(CASE WHEN C_8_G > 0 THEN 1 ELSE 0 END)
		, C_4_W  = SUM(CASE WHEN C_4_W > 0 THEN 1 ELSE 0 END)
		, C_4_G  = SUM(CASE WHEN C_4_G > 0 THEN 1 ELSE 0 END)
		, C_0_W  = SUM(CASE WHEN C_0_W > 0 THEN 1 ELSE 0 END)
		, C_0_G  = SUM(CASE WHEN C_0_G > 0 THEN 1 ELSE 0 END)
		, C_D_W  = SUM(CASE WHEN C_D_W > 0 THEN 1 ELSE 0 END)
		, C_D_G  = SUM(CASE WHEN C_D_G > 0 THEN 1 ELSE 0 END)
	INTO #daily_attendance_Work
	FROM (
			SELECT U.deptId, W.userId
				, C_8_W = SUM(CASE WHEN (R.bcName = N'八点班' OR (R.bcName = N'自由班次' AND ABS(DATEPART(HOUR, W.bak2) - 8)<=4)) AND devnum<5 THEN 1 ELSE 0 END)
				, C_8_G = SUM(CASE WHEN (R.bcName = N'八点班' OR (R.bcName = N'自由班次' AND ABS(DATEPART(HOUR, W.bak2) - 8)<=4)) AND devnum>4 THEN 1 ELSE 0 END)
				, C_4_W = SUM(CASE WHEN (R.bcName = N'四点班' OR (R.bcName = N'自由班次' AND ABS(DATEPART(HOUR, W.bak2) - 16)<=4)) AND devnum<5 THEN 1 ELSE 0 END)
				, C_4_G = SUM(CASE WHEN (R.bcName = N'四点班' OR (R.bcName = N'自由班次' AND ABS(DATEPART(HOUR, W.bak2) - 16)<=4)) AND devnum>4 THEN 1 ELSE 0 END)
				, C_0_W = SUM(CASE WHEN (R.bcName = N'零点班' OR (R.bcName = N'自由班次' AND (DATEPART(HOUR, W.bak2)<=4 OR DATEPART(HOUR, W.bak2)>=20))) AND devnum<5 THEN 1 ELSE 0 END)
				, C_0_G = SUM(CASE WHEN (R.bcName = N'零点班' OR (R.bcName = N'自由班次' AND (DATEPART(HOUR, W.bak2)<=4 OR DATEPART(HOUR, W.bak2)>=20))) AND devnum>4 THEN 1 ELSE 0 END)
				, C_D_W = SUM(CASE WHEN R.bcName IN (N'联建楼', N'办公楼') AND devnum<5 THEN 1 ELSE 0 END)
				, C_D_G = SUM(CASE WHEN R.bcName IN (N'联建楼', N'办公楼') AND devnum>4 THEN 1 ELSE 0 END)
			FROM WorkDuration W
				INNER JOIN UserInfo U ON U.userId = W.userId
				INNER JOIN DeptInfo D ON D.deptId = U.deptId
				INNER JOIN Rostering R ON R.ID = (CASE WHEN ISNUMERIC(W.bak4) = 1 THEN CAST(W.bak4 AS int) ELSE NULL END) 
			WHERE [date] = @dateStr
			GROUP BY U.deptId, W.userId
		) W
	GROUP BY deptId
	/* the final query */
	SELECT D.*
		, C_8_W = ISNULL(W.C_8_W, 0)
		, C_8_G = ISNULL(W.C_8_G, 0)
		, C_4_W = ISNULL(W.C_4_W, 0)
		, C_4_G = ISNULL(W.C_4_G, 0)
		, C_0_W = ISNULL(W.C_0_W, 0)
		, C_0_G = ISNULL(W.C_0_G, 0)
		, C_D_W = ISNULL(W.C_D_W, 0)
		, C_D_G = ISNULL(W.C_D_G, 0)
	FROM #daily_attendance_dept D
		LEFT JOIN #daily_attendance_Work W ON W.deptId = D.deptId
	ORDER BY D.deptId

	/* Drop Temporary Tables */
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#daily_attendance_dept')) BEGIN
		DROP TABLE #daily_attendance_dept
	END
	IF EXISTS(SELECT * FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID('tempdb..#daily_attendance_Work')) BEGIN
		DROP TABLE #daily_attendance_Work
	END
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.spGetWorkingUser') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) BEGIN
	DROP PROCEDURE dbo.spGetWorkingUser
END
GO
CREATE PROCEDURE dbo.spGetWorkingUser
	@DeptId int,
	@Date smalldatetime
AS
BEGIN
	DECLARE @dateStr as varchar(10)
	SET @dateStr = CONVERT(varchar(10), CAST(@Date AS smalldatetime), 120)
	
	SELECT userId = RTRIM(U.userId)
		, userName = REPLACE(REPLACE(U.userName, '	', ''), ' ', '')
		, U.featureId
		, photo = SUBSTRING(RTRIM(F.photoPath), PATINDEX('%\Picture\%', RTRIM(F.photoPath)) + 9, 255)
		, durationId = ISNULL(W.ID, 0)
	FROM UserInfo U WITH (NOLOCK)
		LEFT JOIN FeatureInfo F WITH (NOLOCK) ON F.ID = U.featureId
		LEFT JOIN WorkDuration W WITH (NOLOCK) ON W.userId = U.userId AND W.duration = 0 AND W.date = @dateStr
	WHERE U.deptId = @deptId
	ORDER BY userName
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.spSavePreRostering') AND OBJECTPROPERTY(id, N'IsProcedure') = 1) BEGIN
	DROP PROCEDURE dbo.spSavePreRostering
END
GO
CREATE PROCEDURE dbo.spSavePreRostering
	@UserIDs varchar(8000),
	@RosteringID int,
	@Start int,
	@End int
AS
	DECLARE @sql nvarchar(4000)
	DECLARE @i int

	SET @sql = 'BEGIN TRAN' + char(13) + char(10)
		+ 'DECLARE @tbl table (UserId varchar(6))' + char(13) + char(10)
		+ 'INSERT INTO @tbl SELECT value FROM dbo.sfTableFromItemList(@IDs)' + char(13) + char(10)
		+ 'INSERT INTO PreRostering (userId, d1, d2, d3, d4, d5, d6, d7, d8, d9, d10, d11, d12, d13, d14, d15, d16, d17, d18, d19, d20, d21, d22, d23, d24, d25, d26, d27, d28, d29, d30, d31, UpdateTime)' + char(13) + char(10)
		+ 'SELECT UserId, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GETDATE()' + char(13) + char(10)
		+ 'FROM @tbl U' + char(13) + char(10)
		+ 'WHERE NOT EXISTS(SELECT * FROM PreRostering P WHERE P.userId = U.UserId)' + char(13) + char(10)
		+ '' + char(13) + char(10)
		+ 'UPDATE PreRostering SET UpdateTime = GETDATE()' + char(13) + char(10);
	SET @i = @Start
	WHILE @i <= @End BEGIN
		SET @sql = @sql + ', d' + CAST(@i AS varchar) + '=' + CAST(@RosteringID AS varchar) + char(13) + char(10)
		SET @i = @i + 1
	END
	SET @sql = @sql + 'WHERE userId IN (SELECT UserId FROM @tbl)' + char(13) + char(10)
	SET @sql = @sql + 'COMMIT TRAN'
	--PRINT @sql
	EXEC sp_executesql @sql, N'@IDs varchar(8000)', @IDs = @userIds
GO