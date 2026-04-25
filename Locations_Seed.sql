SET NOCOUNT ON;
GO

IF OBJECT_ID(N'dbo.Locations', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Locations
    (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Province NVARCHAR(50) NOT NULL,
        Sector NVARCHAR(100) NOT NULL,
        CONSTRAINT UQ_Locations_Province_Sector UNIQUE (Province, Sector)
    );
END;
GO

IF COL_LENGTH('dbo.Locations', 'Province') IS NULL
BEGIN
    ALTER TABLE dbo.Locations ADD Province NVARCHAR(50) NULL;
END;

IF COL_LENGTH('dbo.Locations', 'Sector') IS NULL
BEGIN
    ALTER TABLE dbo.Locations ADD Sector NVARCHAR(100) NULL;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Locations')
      AND name = N'UQ_Locations_Province_Sector'
)
BEGIN
    CREATE UNIQUE INDEX UQ_Locations_Province_Sector ON dbo.Locations(Province, Sector);
END;
GO

IF COL_LENGTH('dbo.Locations', 'Village') IS NOT NULL
BEGIN
    MERGE dbo.Locations AS t
    USING (VALUES
        (N'KIGALI', N'Nyarugenge'),
        (N'KIGALI', N'Gasabo'),
        (N'KIGALI', N'Kicukiro'),
        (N'EAST', N'Bugesera'),
        (N'EAST', N'Gatsibo'),
        (N'EAST', N'Kayonza'),
        (N'EAST', N'Kirehe'),
        (N'EAST', N'Ngoma'),
        (N'EAST', N'Nyagatare'),
        (N'EAST', N'Rwamagana'),
        (N'WEST', N'Karongi'),
        (N'WEST', N'Ngororero'),
        (N'WEST', N'Nyabihu'),
        (N'WEST', N'Nyamasheke'),
        (N'WEST', N'Rubavu'),
        (N'WEST', N'Rusizi'),
        (N'WEST', N'Rutsiro'),
        (N'NORTH', N'Burera'),
        (N'NORTH', N'Gakenke'),
        (N'NORTH', N'Gicumbi'),
        (N'NORTH', N'Musanze'),
        (N'NORTH', N'Rulindo'),
        (N'SOUTH', N'Gisagara'),
        (N'SOUTH', N'Huye'),
        (N'SOUTH', N'Kamonyi'),
        (N'SOUTH', N'Muhanga'),
        (N'SOUTH', N'Nyamagabe'),
        (N'SOUTH', N'Nyanza'),
        (N'SOUTH', N'Nyaruguru'),
        (N'SOUTH', N'Ruhango')
    ) AS s(Province, Sector)
    ON t.Province = s.Province AND t.Sector = s.Sector
    WHEN NOT MATCHED THEN
        INSERT (Province, Sector, Village)
        VALUES (s.Province, s.Sector, s.Sector);
END
ELSE
BEGIN
    MERGE dbo.Locations AS t
    USING (VALUES
        (N'KIGALI', N'Nyarugenge'),
        (N'KIGALI', N'Gasabo'),
        (N'KIGALI', N'Kicukiro'),
        (N'EAST', N'Bugesera'),
        (N'EAST', N'Gatsibo'),
        (N'EAST', N'Kayonza'),
        (N'EAST', N'Kirehe'),
        (N'EAST', N'Ngoma'),
        (N'EAST', N'Nyagatare'),
        (N'EAST', N'Rwamagana'),
        (N'WEST', N'Karongi'),
        (N'WEST', N'Ngororero'),
        (N'WEST', N'Nyabihu'),
        (N'WEST', N'Nyamasheke'),
        (N'WEST', N'Rubavu'),
        (N'WEST', N'Rusizi'),
        (N'WEST', N'Rutsiro'),
        (N'NORTH', N'Burera'),
        (N'NORTH', N'Gakenke'),
        (N'NORTH', N'Gicumbi'),
        (N'NORTH', N'Musanze'),
        (N'NORTH', N'Rulindo'),
        (N'SOUTH', N'Gisagara'),
        (N'SOUTH', N'Huye'),
        (N'SOUTH', N'Kamonyi'),
        (N'SOUTH', N'Muhanga'),
        (N'SOUTH', N'Nyamagabe'),
        (N'SOUTH', N'Nyanza'),
        (N'SOUTH', N'Nyaruguru'),
        (N'SOUTH', N'Ruhango')
    ) AS s(Province, Sector)
    ON t.Province = s.Province AND t.Sector = s.Sector
    WHEN NOT MATCHED THEN
        INSERT (Province, Sector)
        VALUES (s.Province, s.Sector);
END;
GO

SELECT COUNT(*) AS TotalRows FROM dbo.Locations;
SELECT Province, Sector FROM dbo.Locations ORDER BY Province, Sector;
GO
