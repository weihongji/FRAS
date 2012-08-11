IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE name = N'IX_AttendanceMng_Date' AND id = object_id(N'[dbo].[AttendanceMng]')) BEGIN
	CREATE NONCLUSTERED INDEX [IX_AttendanceMng_Date] ON [dbo].[AttendanceMng] ([date] ASC)
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE name = N'IX_AttendanceMng_UserId' AND id = object_id(N'[dbo].[AttendanceMng]')) BEGIN
	CREATE NONCLUSTERED INDEX [IX_AttendanceMng_UserId] ON [dbo].[AttendanceMng] ([UserId] ASC)
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE name = N'IX_WorkDuration_Date' AND id = object_id(N'[dbo].[WorkDuration]')) BEGIN
	CREATE NONCLUSTERED INDEX [IX_WorkDuration_Date] ON [dbo].[WorkDuration] ([date] ASC)
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE name = N'IX_WorkDuration_UserId' AND id = object_id(N'[dbo].[WorkDuration]')) BEGIN
	CREATE NONCLUSTERED INDEX [IX_WorkDuration_UserId] ON [dbo].[WorkDuration] ([UserId] ASC)
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE name = N'IX_UserInfo_deptId' AND id = object_id(N'[dbo].[UserInfo]')) BEGIN
	CREATE NONCLUSTERED INDEX [IX_UserInfo_deptId] ON [dbo].[UserInfo] ([deptId] ASC)
END
GO
IF NOT EXISTS (SELECT * FROM dbo.sysindexes WHERE name = N'IX_UserInfo_featureId' AND id = object_id(N'[dbo].[UserInfo]')) BEGIN
	CREATE NONCLUSTERED INDEX [IX_UserInfo_featureId] ON [dbo].[UserInfo] ([featureId] ASC)
END
