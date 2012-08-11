IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'dbo.FeatureInputResult') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) BEGIN
	CREATE TABLE dbo.FeatureInputResult(
		ID int NOT NULL IDENTITY(1,1),
		Token varchar(50) NOT NULL,
		UserId varchar(6) NOT NULL,
		Status int NOT NULL,
		Message nvarchar(100) NULL,
		DTStamp datetime NOT NULL CONSTRAINT DF_FeatureInputResult_DTStamp  DEFAULT (getdate())
	)

	CREATE CLUSTERED INDEX IX_FeatureInputResult_Token ON dbo.FeatureInputResult 
	(
		Token DESC
	)
END
GO

IF NOT EXISTS(SELECT * FROM dbo.rostering WHERE bak1 = '1') BEGIN
	SET IDENTITY_INSERT dbo.rostering ON
	INSERT INTO dbo.rostering (ID, bcName, startTime, endTime, earlyRange, lateRange, realStartTime, realEndTime, flag, multType, mulripleDur, nightWork, bak1)
	VALUES (1, N'自由排班', '00:00', '23:59', 0, 0, '00:00', '23:59', 1, 1, 0, 0, '1')
	SET IDENTITY_INSERT dbo.rostering OFF
END