IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[sfTableFromIntegerList]') AND xtype IN (N'FN', N'IF', N'TF'))
	DROP FUNCTION dbo.sfTableFromIntegerList
GO
CREATE FUNCTION dbo.sfTableFromIntegerList(@list ntext)
RETURNS @tbl TABLE (
	[index] int IDENTITY(1,1),
	value int NULL
)
AS 
BEGIN
	DECLARE @pos      int,
		@textpos  int,
		@chunklen smallint,
		@str      nvarchar(4000),
		@tmpstr   nvarchar(4000),
		@leftover nvarchar(4000)

	SET @textpos = 1
	SET @leftover = ''
	WHILE @textpos <= datalength(@list) / 2 BEGIN
		SET @chunklen = 4000 - datalength(@leftover) / 2
		SET @tmpstr = ltrim(@leftover + substring(@list, @textpos, @chunklen))
		SET @textpos = @textpos + @chunklen

		SET @pos = charindex(',', @tmpstr)
		WHILE @pos > 0 BEGIN
			SET @str = substring(@tmpstr, 1, @pos - 1)
			INSERT @tbl (value) VALUES(convert(int, @str))
			SET @tmpstr = ltrim(substring(@tmpstr, @pos + 1, len(@tmpstr)))
			SET @pos = charindex(',', @tmpstr)
		END

		SET @leftover = @tmpstr
	END

	IF ltrim(rtrim(@leftover)) <> '' BEGIN
		INSERT @tbl (value) VALUES(convert(int, @leftover))
	END

	RETURN
END
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[sfTableFromItemList]') AND xtype IN (N'FN', N'IF', N'TF'))
	DROP FUNCTION dbo.sfTableFromItemList
GO
CREATE FUNCTION dbo.sfTableFromItemList(@list ntext)
RETURNS @tbl TABLE (
	[index] int IDENTITY(1,1),
	value varchar(1000) NULL
)
AS 
BEGIN
	DECLARE @pos      int,
		@textpos  int,
		@chunklen smallint,
		@str      nvarchar(4000),
		@tmpstr   nvarchar(4000),
		@leftover nvarchar(4000)

	SET @textpos = 1
	SET @leftover = ''
	WHILE @textpos <= datalength(@list) / 2 BEGIN
		SET @chunklen = 4000 - datalength(@leftover) / 2
		SET @tmpstr = ltrim(@leftover + substring(@list, @textpos, @chunklen))
		SET @textpos = @textpos + @chunklen

		SET @pos = charindex(',', @tmpstr)
		WHILE @pos > 0 BEGIN
			SET @str = substring(@tmpstr, 1, @pos - 1)
			INSERT @tbl (value) VALUES(ltrim(rtrim(@str)))
			SET @tmpstr = ltrim(substring(@tmpstr, @pos + 1, len(@tmpstr)))
			SET @pos = charindex(',', @tmpstr)
		END

		SET @leftover = @tmpstr
	END

	IF ltrim(rtrim(@leftover)) <> '' BEGIN
		INSERT @tbl (value) VALUES(ltrim(rtrim(@leftover)))
	END

	RETURN
END
GO