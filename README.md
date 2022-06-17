# Bachelorprosjekt

Korleis få til å virke i visual studio.
-importer prosjekt: Bachelorprosjekt3 og BachelorprosjektTesting (https://github.com/havtei/bachelorprosjektTesting)
-oppdater databasetilkopling
-nuget package manager console: update-database

akkurat no i utviklinga, må rolle leggast til manuelt. pga "[Authorize(Roles = "Fagansvarlig, Admin, Bedrift, Student")]" i HomeController og tilsvarande andre stader, må ein vere innlogga med rolle for å teste.

Eksempel for å opprette brukar i ei bestemt rolle:
USE [bachelorprosjekt]
GO

INSERT INTO [dbo].[AspNetRoles]
           ([Id]
           ,[Name]
           ,[NormalizedName]
           ,[ConcurrencyStamp])
     VALUES
           (1
           ,'Fagansvarlig'
           ,'FAGANSVARLIG'
           ,NULL)
GO

-- Og viss 'abcuserid...' er brukarid i dbo.AspNeuUsers

USE [bachelorprosjekt]
GO

INSERT INTO [dbo].[AspNetUserRoles]
           ([UserId]
           ,[RoleId])
     VALUES
           ('abcuserid...'
           ,3)
GO


