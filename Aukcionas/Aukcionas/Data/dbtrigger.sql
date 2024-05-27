-- ================================================
-- Template generated from Template Explorer using:
-- Create Trigger (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- See additional Create Trigger templates for more
-- examples of different Trigger statements.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER TRIGGER NullifySavedUrlTrigger
ON Auctions
AFTER UPDATE
AS
BEGIN
    -- Check if SavedUrl column was updated
    IF UPDATE(SavedUrl)
    BEGIN
        -- Calculate the duration since SavedUrl was set
        DECLARE @CurrentTime DATETIME = GETUTCDATE()
        DECLARE @AuctionId INT

        SELECT @AuctionId = inserted.id
        FROM inserted

        DECLARE @SavedUrlSetTime DATETIME

        SELECT @SavedUrlSetTime = SavedUrlSetTime
        FROM (SELECT id, SavedUrl, COALESCE(SavedUrlSetTime, AuctionStartTime) AS SavedUrlSetTime FROM inserted) AS i
        JOIN deleted d ON i.id = d.id
        WHERE i.SavedUrl IS NULL AND d.SavedUrl IS NOT NULL

        -- Nullify SavedUrl if it's been set for more than 1 hour
        IF DATEDIFF(HOUR, @SavedUrlSetTime, @CurrentTime) >= 1
        BEGIN
            UPDATE Auctions
            SET SavedUrl = NULL
            WHERE id = @AuctionId
        END
    END
END

GO
