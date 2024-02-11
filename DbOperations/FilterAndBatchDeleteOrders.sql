CREATE PROCEDURE FilterAndBatchDeleteOrders
    @Status NVARCHAR(50),
    @Delete BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @Delete=1
    BEGIN
        DELETE FROM [Order] WHERE Status = @Status;
    END
    ELSE
    BEGIN
        SELECT * FROM [Order] WHERE Status = @Status;
    END
END