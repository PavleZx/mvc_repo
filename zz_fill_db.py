import pyodbc

# Connection info (from your original script)
server = 'serverpavlemvc.database.windows.net'
database = 'free-sql-db-0425609'
username = 'pavle'
password = 'lucyskye0405!'
driver = '{ODBC Driver 18 for SQL Server}'

conn = pyodbc.connect(
    f'DRIVER={driver};SERVER={server};DATABASE={database};UID={username};PWD={password};Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;'
)

cursor = conn.cursor()

create_table_sql = """
IF NOT EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Items'
)
BEGIN
    CREATE TABLE dbo.Items (
        Id    INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Name  NVARCHAR(255)     NOT NULL,
        Price FLOAT             NOT NULL
    )
    PRINT 'Table Items created.'
END
ELSE
BEGIN
    PRINT 'Table Items already exists.'
END
"""

cursor.execute(create_table_sql)
conn.commit()

print("Done. Table 'dbo.Items' is ready.")

cursor.close()
conn.close()