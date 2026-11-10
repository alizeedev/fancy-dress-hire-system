-- ============================================================
-- THE GRAND ATELIER | Royal & Historical Costume Hire System
-- MySQL 8.0.31 | Database schema + fictional demonstration data
-- Run this file in MySQL Workbench on a test database.
-- It does not delete existing tables or data.
-- Re-running will skip existing sample IDs without overwriting them.
-- ============================================================

-- 1. Create and select the database
SET NAMES utf8mb4;

CREATE DATABASE IF NOT EXISTS grand_atelier
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE grand_atelier;

-- 2. Table: Costumes
-- One row represents one costume style/size, with multiple units in stock.
CREATE TABLE IF NOT EXISTS Costumes (
    CostumeID VARCHAR(10) PRIMARY KEY,
    CostumeName VARCHAR(120) NOT NULL,
    Category VARCHAR(30) NOT NULL,
    Era VARCHAR(40) NOT NULL,
    Size VARCHAR(10) NOT NULL,
    DailyHirePrice DECIMAL(8,2) NOT NULL,
    TotalQuantity INT NOT NULL,
    AvailableQuantity INT NOT NULL,
    INDEX idx_costume_category_size (Category, Size),
    CONSTRAINT chk_costume_category
        CHECK (Category IN ('Royal', 'Historical', 'Masquerade')),
    CONSTRAINT chk_costume_price
        CHECK (DailyHirePrice > 0),
    CONSTRAINT chk_costume_quantity
        CHECK (TotalQuantity >= 0 AND AvailableQuantity BETWEEN 0 AND TotalQuantity)
) ENGINE=InnoDB;

-- 3. Table: Customers
CREATE TABLE IF NOT EXISTS Customers (
    CustomerID VARCHAR(10) PRIMARY KEY,
    FullName VARCHAR(100) NOT NULL
) ENGINE=InnoDB;

-- 4. Table: Rentals
-- Each rental row represents ONE unit of a costume being hired.
-- ReturnDate = NULL means the costume has not yet been returned.
CREATE TABLE IF NOT EXISTS Rentals (
    RentalID VARCHAR(10) PRIMARY KEY,
    CustomerID VARCHAR(10) NOT NULL,
    CostumeID VARCHAR(10) NOT NULL,
    HireDate DATE NOT NULL,
    DueDate DATE NOT NULL,
    ReturnDate DATE NULL,
    CONSTRAINT fk_rentals_customer
        FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT fk_rentals_costume
        FOREIGN KEY (CostumeID) REFERENCES Costumes(CostumeID)
        ON UPDATE CASCADE ON DELETE RESTRICT,
    CONSTRAINT chk_rental_due_date CHECK (DueDate >= HireDate),
    CONSTRAINT chk_rental_return_date
        CHECK (ReturnDate IS NULL OR ReturnDate >= HireDate)
) ENGINE=InnoDB;

-- 5. Add fictional costumes
-- AvailableQuantity reflects the active sample rentals inserted below.
INSERT INTO Costumes
    (CostumeID, CostumeName, Category, Era, Size,
     DailyHirePrice, TotalQuantity, AvailableQuantity)
VALUES
    ('GA001', 'Versailles Duchess Gown',    'Royal',      'Baroque',     'M',  1200.00, 3, 2),
    ('GA002', 'Imperial Court Tailcoat',    'Historical', 'Regency',    'L',   950.00, 2, 1),
    ('GA003', 'Venetian Midnight Ensemble','Masquerade', 'Venetian',   'M',   800.00, 4, 3),
    ('GA004', 'Renaissance Queen Gown',     'Royal',      'Renaissance','S',  1450.00, 2, 2),
    ('GA005', 'Victorian Nobleman''s Suit', 'Historical', 'Victorian',  'XL', 1100.00, 3, 3),
    ('GA006', 'Golden Masquerade Gown',     'Masquerade', 'Venetian',   'L',  1300.00, 2, 1),
    ('GA007', 'Maharaja Court Robe',        'Royal',      'Mughal',     'L',  1250.00, 2, 2),
    ('GA008', 'Edwardian Opera Cape',       'Historical', 'Edwardian',  'M',   700.00, 5, 5)
ON DUPLICATE KEY UPDATE CostumeID = CostumeID;

-- 6. Add fictional customers (Mauritian, French and international names)
INSERT INTO Customers (CustomerID, FullName)
VALUES
    ('CU001', 'Sahana Beeharry'),
    ('CU002', 'Yannick Chan'),
    ('CU003', 'Élodie Moreau'),
    ('CU004', 'Imran Abdool'),
    ('CU005', 'Maëlle Lefèvre'),
    ('CU006', 'Nathan Bissessur'),
    ('CU007', 'Aïcha Benali'),
    ('CU008', 'Keisha Ncube')
ON DUPLICATE KEY UPDATE CustomerID = CustomerID;

-- 7. Add fictional rentals
-- NULL ReturnDate = currently on hire; a date = already returned.
INSERT INTO Rentals
    (RentalID, CustomerID, CostumeID, HireDate, DueDate, ReturnDate)
VALUES
    ('R001', 'CU003', 'GA001', '2026-11-01', '2026-11-09', NULL),
    ('R002', 'CU001', 'GA002', '2026-11-05', '2026-11-12', NULL),
    ('R003', 'CU005', 'GA003', '2026-11-06', '2026-11-14', NULL),
    ('R004', 'CU006', 'GA004', '2026-10-25', '2026-10-30', '2026-10-29'),
    ('R005', 'CU007', 'GA006', '2026-11-07', '2026-11-15', NULL),
    ('R006', 'CU004', 'GA007', '2026-11-02', '2026-11-06', '2026-11-06'),
    ('R007', 'CU008', 'GA001', '2026-10-20', '2026-10-24', '2026-10-24')
ON DUPLICATE KEY UPDATE RentalID = RentalID;

-- ============================================================
-- 8. PRESENTATION QUERIES: safe to run repeatedly
-- ============================================================

-- Show every costume
SELECT * FROM Costumes ORDER BY Category, CostumeName;

-- Show every customer
SELECT * FROM Customers ORDER BY FullName;

-- Show raw rental records
SELECT * FROM Rentals ORDER BY HireDate DESC;

-- Search for available Royal costumes in size M
SELECT CostumeID, CostumeName, Size, DailyHirePrice, AvailableQuantity
FROM Costumes
WHERE Category = 'Royal' AND Size = 'M' AND AvailableQuantity > 0;

-- JOIN: show each rental together with customer and costume names
SELECT
    r.RentalID,
    c.FullName AS Customer,
    co.CostumeName AS Costume,
    r.HireDate,
    r.DueDate,
    r.ReturnDate,
    CASE WHEN r.ReturnDate IS NULL THEN 'On Hire'
         ELSE 'Returned' END AS RentalStatus
FROM Rentals AS r
INNER JOIN Customers AS c ON r.CustomerID = c.CustomerID
INNER JOIN Costumes AS co ON r.CostumeID = co.CostumeID
ORDER BY r.HireDate DESC;

-- Currently hired costumes
SELECT r.RentalID, c.FullName, co.CostumeName, r.DueDate
FROM Rentals AS r
INNER JOIN Customers AS c ON r.CustomerID = c.CustomerID
INNER JOIN Costumes AS co ON r.CostumeID = co.CostumeID
WHERE r.ReturnDate IS NULL
ORDER BY r.DueDate;

-- Check stock consistency: should return zero rows for the sample data.
-- Note: the application must keep AvailableQuantity in sync with rentals.
SELECT co.CostumeID, co.CostumeName, co.AvailableQuantity,
       co.TotalQuantity - COUNT(r.RentalID) AS ExpectedAvailable
FROM Costumes AS co
LEFT JOIN Rentals AS r
  ON r.CostumeID = co.CostumeID AND r.ReturnDate IS NULL
GROUP BY co.CostumeID, co.CostumeName, co.AvailableQuantity, co.TotalQuantity
HAVING co.AvailableQuantity <> ExpectedAvailable;

-- ============================================================
-- 9. OPTIONAL CRUD DEMO (do not run automatically)
-- Select these three statements in Workbench and execute together
-- if your lecturer asks to see INSERT, UPDATE and DELETE.
-- This temporary costume is not used in any rental.
-- ============================================================
-- INSERT INTO Costumes (CostumeID, CostumeName, Category, Era, Size,
--                       DailyHirePrice, TotalQuantity, AvailableQuantity)
-- VALUES ('GA999', 'Demonstration Court Cape', 'Royal', 'Regency', 'M',
--         500.00, 1, 1);
-- UPDATE Costumes SET DailyHirePrice = 550.00 WHERE CostumeID = 'GA999';
-- DELETE FROM Costumes WHERE CostumeID = 'GA999';
