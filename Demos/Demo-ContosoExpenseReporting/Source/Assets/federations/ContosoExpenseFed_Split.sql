/* First, let's go to the ROOT */
USE FEDERATION ROOT WITH RESET
GO

/* Next, let's look at the metadata using some of the built-in system DMVs */
SELECT db_name() [db_name]
SELECT * FROM sys.federations
SELECT * FROM sys.federation_distributions
SELECT * FROM sys.federation_member_distributions ORDER BY federation_id, range_low;
GO


/* Query the Federation */ 
SELECT * FROM UserProfile
SELECT * FROM ExpenseReport

/* Let's go to the Federation Member */
USE FEDERATION UserExpense_Federation (UserId = 0)  WITH RESET, FILTERING = OFF
GO

/* Query the Federation */
SELECT * FROM UserProfile
SELECT * FROM ExpenseReport

/*--------------- LET'S SPLIT THIS THING!! ------------------*/
/* Go to the ROOT to Split */
USE FEDERATION ROOT WITH RESET
GO

/* Now Split it */
ALTER FEDERATION UserExpense_Federation SPLIT AT (UserID = 40)
GO

/* Let's look at the meta data again */
SELECT * FROM sys.federations
SELECT * FROM sys.federation_distributions
SELECT * FROM sys.federation_member_distributions ORDER BY federation_id, range_low;
GO

-- WITH REST = This is a required keyword that makes the connection reset explicit


USE FEDERATION UserExpense_Federation (UserId = 10)  WITH RESET, FILTERING = OFF
GO

SELECT * FROM UserProfile

USE FEDERATION UserExpense_Federation (UserId = 41)  WITH RESET, FILTERING = OFF
GO

SELECT * FROM UserProfile


USE FEDERATION UserExpense_Federation (UserId = 10)  WITH RESET, FILTERING = ON
GO

SELECT * FROM UserProfile
SELECT * FROM ExpenseReport












--http://msdn.microsoft.com/en-us/library/windowsazure/hh778416.aspx#BKMK_PerformSplit

--http://msdn.microsoft.com/en-us/hh532132



--select max(UserProfileID) as UserProfileID, quartile 
--from (select UserProfileID, ntile(4) over (order by UserProfileID) as [quartile] 
--from ExpenseReportDetail) i 
----where quartile = 2 
--group by quartile
--s


--select count(*) from ExpenseReportDetail where userprofileid<'C09CCD60-A191-C361-E84B-788913F43F70' 
--select count(*) from ExpenseReportDetail where userprofileid>'C09CCD60-A191-C361-E84B-788913F43F70'


--CREATE TABLE #tmp 
--(
--    Id int,
--    CustomerId uniqueidentifier
--)

--INSERT INTO #tmp (id, CustomerId)
--SELECT ROW_NUMBER() OVER(ORDER BY customerid), customerid FROM Customers ORDER BY CustomerId

--DECLARE @count int
--SELECT @count = COUNT(*) FROM #tmp

--SELECT CustomerId FROM #tmp WHERE Id = @count / 2
