IF NOT EXISTS (
   SELECT 1
   FROM sys.tables AS t
   INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id
   WHERE s.name = N'dbo'
   AND t.name = N'SchemaVersions'
   AND t.type = N'U'
)
BEGIN
    SET ANSI_NULLS ON
    SET QUOTED_IDENTIFIER ON

    CREATE TABLE dbo.SchemaVersions (
        Id int IDENTITY(1,1) NOT NULL,
        ScriptName nvarchar(255) NOT NULL,
        Applied datetime NOT NULL,

        CONSTRAINT PK_SchemaVersions_Id PRIMARY KEY CLUSTERED (Id ASC)
            WITH (
                PAD_INDEX = OFF,
                STATISTICS_NORECOMPUTE = OFF,
                IGNORE_DUP_KEY = OFF,
                ALLOW_ROW_LOCKS = ON,
                ALLOW_PAGE_LOCKS = ON,
                OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
            ) ON [PRIMARY]
    ) ON [PRIMARY]
END;