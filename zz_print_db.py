import pyodbc

# -----------------------------
# CONNECTION CONFIG
# -----------------------------
server   = 'serverpavlemvc.database.windows.net'
database = 'free-sql-db-0425609'
username = 'pavle'
password = 'lucyskye0405!'
driver   = '{ODBC Driver 18 for SQL Server}'

# -----------------------------
# CONNECT
# -----------------------------
conn = pyodbc.connect(
    f'DRIVER={driver};SERVER={server};DATABASE={database};'
    f'UID={username};PWD={password};'
    f'Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;'
)

cursor = conn.cursor()

# -----------------------------
# PRINT ALL SerialNumbers ENTRIES
# -----------------------------
print("SerialNumbers table entries:")
print("-" * 50)

cursor.execute("SELECT * FROM SerialNumbers")

# Get column names from cursor description
columns = [column[0] for column in cursor.description]
print("Columns:", columns)
print("-" * 50)

rows = cursor.fetchall()

if not rows:
    print("No entries found in SerialNumbers table.")
else:
    for row in rows:
        for col_name, value in zip(columns, row):
            print(f"  {col_name}: {value}")
        print("-" * 50)

print(f"Total entries: {len(rows)}")

# -----------------------------
# CLOSE
# -----------------------------
cursor.close()
conn.close()