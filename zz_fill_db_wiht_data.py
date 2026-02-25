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

items = [
    ('Wireless Mouse',        29.99),
    ('Mechanical Keyboard',   89.95),
    ('USB-C Hub',             45.00),
    ('Monitor Stand',         35.50),
    ('Webcam HD 1080p',       79.99),
    ('Noise Cancelling Headphones', 149.95),
    ('Laptop Cooling Pad',    25.00),
    ('External SSD 512GB',   109.99),
    ('LED Desk Lamp',         22.50),
    ('Ergonomic Chair',      299.00),
]

cursor.executemany(
    "INSERT INTO dbo.Items (Name, Price) VALUES (?, ?)",
    items
)

conn.commit()
print(f"Inserted {len(items)} items successfully.")

cursor.close()
conn.close()