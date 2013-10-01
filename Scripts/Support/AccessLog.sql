/*
录入考勤记录（不含人员定位信息）

SELECT * FROM UserInfo WHERE userId = '0050'

SELECT TOP 100 * FROM AccessLog WHERE userId = '0050' and [date] between '2013-09-28' and '2013-09-30' order by [date], ID
SELECT TOP 100 * FROM WorkDuration WHERE userId = '0050' and [date] between '2013-09-20' and '2013-09-28' order by [date], ID
*/
BEGIN TRAN
	DECLARE @UserId varchar(6), @Date varchar(10), @DateStart varchar(20), @DateEnd varchar(20), @DateStarX varchar(20), @DateEnX varchar(20), @path varchar(255), @devNum INT, @bak4 varchar(255)
	SET @UserId = '0050'
	SET @path = ''
	SET @devNum = 6
	SET @bak4 = '17'
	SET @Date = '2013-09-28'
	SET @DateStart = @Date + ' 07:19:26'
	SET @DateStarX = @Date + ' 07:17:26'
	SET @DateEnd = @Date + ' 11:30:27'
	SET @DateEnX = @Date + ' 11:32:27'


	INSERT INTO AccessLog (userId, [datetime], [date], accessFlag, recResult, state, devNum, recPhotoPath)
	VALUES (@UserId, @DateStart, SUBSTRING(@DateStart, 1, 10), 0, 1, 0, @devNum, @path)

	INSERT INTO AccessLog (userId, [datetime], [date], accessFlag, recResult, state, devNum, recPhotoPath)
	VALUES (@UserId, @DateEnd, SUBSTRING(@DateEnd, 1, 10), 1, 1, 0, @devNum, @path)

	INSERT INTO WorkDuration
	VALUES (@UserId, @Date, 0, '4:15:01', DATEDIFF(second, @DateStarX, @DateEnX), @devNum, '0.5', 0, 0, @DateStarX,  @DateEnX, @bak4)

	--afternoon

	SET @DateStart = @Date + ' 13:47:26'
	SET @DateStarX = @Date + ' 13:49:26'
	SET @DateEnd = @Date + ' 17:51:27'
	SET @DateEnX = @Date + ' 17:53:27'


	INSERT INTO AccessLog (userId, [datetime], [date], accessFlag, recResult, state, devNum, recPhotoPath)
	VALUES (@UserId, @DateStart, SUBSTRING(@DateStart, 1, 10), 0, 1, 0, @devNum, @path)

	INSERT INTO AccessLog (userId, [datetime], [date], accessFlag, recResult, state, devNum, recPhotoPath)
	VALUES (@UserId, @DateEnd, SUBSTRING(@DateEnd, 1, 10), 1, 1, 0, @devNum, @path)

	INSERT INTO WorkDuration
	VALUES (@UserId, @Date, 0, '4:14:01', DATEDIFF(second, @DateStarX, @DateEnX), @devNum, '0.5', 0, 0, @DateStarX,  @DateEnX, @bak4)
COMMIT TRAN
