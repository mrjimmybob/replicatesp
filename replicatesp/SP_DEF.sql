ALTER PROCEDURE [dbo].[ListPeople] 
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM People
END

CREATE PROCEDURE [dbo].[ListMorePeople] 
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM People
END

CREATE PROCEDURE [dbo].[ListOtherPeople] 
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM People
END
 
CREATE PROCEDURE ListEvenOtherPeople
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM People
END