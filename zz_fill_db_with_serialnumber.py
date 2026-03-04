import pyodbc
import random
import string

server = 'serverpavlemvc.database.windows.net'
database = 'free-sql-db-0425609'
username = 'pavle'
password = 'lucyskye0405!'
driver = '{ODBC Driver 18 for SQL Server}'

conn = pyodbc.connect(
    f'DRIVER={driver};SERVER={server};DATABASE={database};UID={username};PWD={password};Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;'
)

cursor = conn.cursor()

# ── 1. Add SerialNumberId column to Items if it doesn't exist ──────────────────
cursor.execute("""
    IF NOT EXISTS (
        SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
        WHERE TABLE_NAME='Items' AND COLUMN_NAME='SerialNumberId'
    )
    BEGIN
        ALTER TABLE dbo.Items ADD SerialNumberId INT NULL
    END
""")

# ── 2. Create SerialNumbers table if it doesn't exist ─────────────────────────
cursor.execute("""
    IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='SerialNumbers')
    BEGIN
        CREATE TABLE dbo.SerialNumbers (
            Id     INT PRIMARY KEY IDENTITY(1,1),
            Name   NVARCHAR(255) NOT NULL,
            ItemId INT NULL
        )
    END
""")

conn.commit()

# ── 3. Helper to generate serial number strings e.g. "MCH-A3F9B2" ─────────────
def gen_serial(prefix):
    suffix = ''.join(random.choices(string.ascii_uppercase + string.digits, k=6))
    return f"{prefix}-{suffix}"

# ── 4. Items with matching serial number prefixes ─────────────────────────────
items = [
    ('Wireless Mouse',              29.99,  'MSE'),
    ('Mechanical Keyboard',         89.95,  'KBD'),
    ('USB-C Hub',                   45.00,  'USB'),
    ('Monitor Stand',               35.50,  'MNT'),
    ('Webcam HD 1080p',             79.99,  'CAM'),
    ('Noise Cancelling Headphones', 149.95, 'AUD'),
    ('Laptop Cooling Pad',          25.00,  'CLR'),
    ('External SSD 512GB',         109.99,  'SSD'),
    ('LED Desk Lamp',               22.50,  'LMP'),
    ('Ergonomic Chair',            299.00,  'CHR'),
]

for name, price, prefix in items:
    # Insert item (without SerialNumberId for now)
    cursor.execute(
        "INSERT INTO dbo.Items (Name, Price) OUTPUT INSERTED.Id VALUES (?, ?)",
        name, price
    )
    item_id = cursor.fetchone()[0]

    # Insert matching serial number
    serial_name = gen_serial(prefix)
    cursor.execute(
        "INSERT INTO dbo.SerialNumbers (Name, ItemId) OUTPUT INSERTED.Id VALUES (?, ?)",
        serial_name, item_id
    )
    serial_id = cursor.fetchone()[0]

    # Link serial number back to item
    cursor.execute(
        "UPDATE dbo.Items SET SerialNumberId = ? WHERE Id = ?",
        serial_id, item_id
    )

conn.commit()
print(f"Inserted {len(items)} items with serial numbers successfully.")

cursor.close()
conn.close()