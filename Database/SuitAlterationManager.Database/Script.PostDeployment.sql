/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
PRINT('Start Post Deployment Script')

GO
-- Initial data
:r ./_Data/01_init_data.sql

/* COMMENTATO: DA VALUTARE COME GESTIRE IN FUTURO

GO
IF ('$(Target)'='Dev' OR '$(Target)'='Local' OR '$(Target)'='Test')
BEGIN
    PRINT('Deploying test data')
    :r .\_Data\02_test_data.sql
END
GO
IF '$(Target)'='Arval'
BEGIN
    PRINT('Deploying Arval Data')
    :r .\_Data\03_arval_test_data.sql
END
GO

*/
PRINT('End Post Deployment Script')