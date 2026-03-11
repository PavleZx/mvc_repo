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

# ── 1. Create Clients table ────────────────────────────────────────────────────
cursor.execute("""
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Clients')
    BEGIN
        CREATE TABLE dbo.Clients (
            Id   INT PRIMARY KEY IDENTITY(1,1),
            Name NVARCHAR(255) NOT NULL
        )
    END
""")

# ── 2. Create ItemClients helper table ────────────────────────────────────────
cursor.execute("""
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='ItemClients')
    BEGIN
        CREATE TABLE dbo.ItemClients (
            Id       INT PRIMARY KEY IDENTITY(1,1),
            ItemId   INT NOT NULL,
            ClientId INT NOT NULL,
            FOREIGN KEY (ItemId)   REFERENCES dbo.Items(Id),
            FOREIGN KEY (ClientId) REFERENCES dbo.Clients(Id)
        )
    END
""")

conn.commit()

# ── 3. Seed 3 clients ─────────────────────────────────────────────────────────
clients = ['Alice Johnson', 'Bob Smith', 'Carol White']

client_ids = []
for name in clients:
    cursor.execute("""
        IF NOT EXISTS (SELECT 1 FROM dbo.Clients WHERE Name = ?)
        INSERT INTO dbo.Clients (Name) VALUES (?)
    """, name, name)
    conn.commit()

    cursor.execute("SELECT Id FROM dbo.Clients WHERE Name = ?", name)
    client_ids.append(cursor.fetchone()[0])

print(f"Clients: {list(zip(clients, client_ids))}")

# ── 4. Fetch all item IDs ─────────────────────────────────────────────────────
cursor.execute("SELECT Id FROM dbo.Items")
item_ids = [row[0] for row in cursor.fetchall()]

# ── 5. Link every item to all 3 clients ───────────────────────────────────────
count = 0
for item_id in item_ids:
    for client_id in client_ids:
        # Avoid duplicates if script is run multiple times
        cursor.execute("""
            IF NOT EXISTS (
                SELECT 1 FROM dbo.ItemClients 
                WHERE ItemId = ? AND ClientId = ?
            )
            INSERT INTO dbo.ItemClients (ItemId, ClientId) VALUES (?, ?)
        """, item_id, client_id, item_id, client_id)
        count += 1

conn.commit()
print(f"Linked {len(item_ids)} items x 3 clients = {count} relationships created.")

cursor.close()
conn.close()