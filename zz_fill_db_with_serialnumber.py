import pyodbc

server = 'serverpavlemvc.database.windows.net'
database = 'free-sql-db-0425609'
username = 'pavle'
password = 'lucyskye0405!'
driver = '{ODBC Driver 18 for SQL Server}'

conn = pyodbc.connect(
    f'DRIVER={driver};SERVER={server};DATABASE={database};UID={username};PWD={password};Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;'
)

cursor = conn.cursor()

cursor.execute("UPDATE dbo.Items SET CategoryId = 1 WHERE CategoryId IS NULL")

conn.commit()
print(f"Updated {cursor.rowcount} rows.")

cursor.close()
conn.close()