import pyodbc

# -----------------------------
# REPLACE THESE PLACEHOLDERS
# -----------------------------
server = 'serverpavlemvc.database.windows.net'  # e.g., myserver.database.windows.net
database = 'free-sql-db-0425609'                  # e.g., MyDb
username = 'pavle'                       # e.g., adminuser
password = 'lucyskye0405!'                       # e.g., MyPassword123
driver = '{ODBC Driver 18 for SQL Server}'       # Make sure you have this installed
# -----------------------------

# Create connection
conn = pyodbc.connect(
    f'DRIVER={driver};SERVER={server};DATABASE={database};UID={username};PWD={password};Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;'
)

cursor = conn.cursor()

# Get all tables
cursor.execute("""
SELECT TABLE_SCHEMA, TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME
""")
tables = cursor.fetchall()

print("Tables and their columns:")
print("-" * 50)

# Loop through each table and get columns
for schema, table in tables:
    print(f"Table: {schema}.{table}")
    cursor.execute("""
    SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = ? AND TABLE_NAME = ?
    ORDER BY ORDINAL_POSITION
    """, (schema, table))
    columns = cursor.fetchall()
    for col in columns:
        name, datatype, nullable, length = col
        length_str = f"({length})" if length else ""
        print(f"  {name} - {datatype}{length_str} - Nullable: {nullable}")
    print("-" * 50)

# Close connection
cursor.close()
conn.close()
