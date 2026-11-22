-- Fancy dress hire system sql server database

--Drop tables

DROP TABLE IF EXISTS Rentals;
DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS Costumes;
DROP TABLE IF EXISTS Users;

--Create Users table for staff who will log in
CREATE TABLE Users (
	UserId NVARCHAR(10) NOT NULL PRIMARY KEY,
	UserName NVARCHAR(20) NOT NULL UNIQUE,
	PasswordHash NVARCHAR(256) NOT NULL,
	PasswordSalt NVARCHAR(64) NOT NULL,
	UserRole NVARCHAR(15) NOT NULL,
	CONSTRAINT check_userName_length CHECK (LEN(UserName) BETWEEN 3 and 20),
	CONSTRAINT check_userRole CHECK (UserRole IN ('Admin', 'Staff'))
);

--Create costumes table
CREATE Table Costumes (
	CostumeId NVARCHAR(10) NOT NULL PRIMARY KEY,
	CostumeName NVARCHAR(120) NOT NULL,
	Category NVARCHAR(30) NOT NULL,
	Era NVARCHAR(40) NOT NULL,
	Size NVARCHAR(10) NOT NULL,
	DailyHirePrice DECIMAL(8,2) NOT NULL,
	TotalQuantity INT NOT NULL,
	AvailableQuantity INT NOT NULL,
	CONSTRAINT check_costume_category CHECK (Category IN ('Royal','Historical','Masquerade')),
	CONSTRAINT check_costume_size CHECK (Size IN ('XS','S','M','L','XL','XXL')),
	CONSTRAINT check_costume_price CHECK (DailyHirePrice > 0),
	CONSTRAINT check_costume_quantity CHECK (TotalQuantity >=0 AND AvailableQuantity BETWEEN 0 AND TotalQuantity)
);		

--Create customers table
CREATE TABLE Customers(
	CustomerId NVARCHAR(10) NOT NULL PRIMARY KEY,
	FullName NVARCHAR(100) NOT NULL,
	Email NVARCHAR(100) NOT NULL UNIQUE,
	Phone NVARCHAR(15) NOT NULL,
	CONSTRAINT check_customer_email CHECK (Email LIKE '%@%.%'),
	CONSTRAINT check_customer_phone CHECK (LEN(Phone) BETWEEN 7 AND 15 AND Phone NOT LIKE '%[^0-9]%')
);

--Rental Table
CREATE TABLE Rentals(
	RentalId NVARCHAR(10) NOT NULL PRIMARY KEY,
	CostumeId NVARCHAR(10) NOT NULL,
	CustomerId NVARCHAR(10) NOT NULL,
	StartDate DATE NOT NULL,
	DueDate DATE NOT NULL,
	ReturnDate DATE NULL,
	TotalCost DECIMAL(8,2) NOT NULL DEFAULT 0,
	LateFee DECIMAL(8,2) NOT NULL DEFAULT 0,
	LateFeePaid BIT NOT NULL DEFAULT 0,
	CONSTRAINT foreign_key_rentals_costume FOREIGN KEY (CostumeId) REFERENCES Costumes(CostumeId),
	CONSTRAINT foreign_key_customer FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
	CONSTRAINT check_rental_due_date CHECK (DueDate >= StartDate),
	CONSTRAINT check_rental_return_date CHECK (ReturnDate IS NULL OR ReturnDate >= StartDate),
	CONSTRAINT check_rental_costs CHECK (TotalCost >= 0 AND LateFee >=0)

);

--inserting sample data
INSERT INTO Users (UserId, UserName, PasswordHash, PasswordSalt, UserRole) VALUES
(N'U001', N'admin', N'l/27WJmxfpxD5I9MhGh7B4xGCUbHsxZIL718+O/Q2Ig=', N'iiMtPlVQRUjSmv1tcFJQsg==', N'Admin'),
(N'U002', N'staff', N'B4xf4jRm4S4xhs1cAsG1rrh0vBgT9SQAVaDRT2+x6/c=', N'Ci2/btgInoCRpNdzDNVs0A==', N'Staff');

--sample data for costumes (40 costumes, 3 categoreis, all sizes)
--the available quantity is the total quantity at the start
INSERT INTO Costumes (CostumeId, CostumeName, Category, Era, Size, DailyHirePrice, TotalQuantity, AvailableQuantity) VALUES
	(N'GA001', N'Versailles Duchess Gown', N'Royal', N'Baroque', N'M', 1200.00, 3, 3),
    (N'GA002', N'Imperial Court Tailcoat',        N'Historical', N'Regency',N'L',    950.00, 2, 2),
    (N'GA003', N'Venetian Midnight Ensemble',     N'Masquerade', N'Venetian',N'M',    800.00, 4, 4),
    (N'GA004', N'Renaissance Queen Gown',         N'Royal',      N'Renaissance',N'S',   1450.00, 2, 2),
    (N'GA005', N'Victorian Nobleman''s Suit',     N'Historical', N'Victorian',N'XL',  1100.00, 3, 3),
    (N'GA006', N'Golden Masquerade Gown',         N'Masquerade', N'Venetian',N'L',   1300.00, 2, 2),
    (N'GA007', N'Maharaja Court Robe',            N'Royal',      N'Mughal',N'L',   1250.00, 2, 2),
    (N'GA008', N'Edwardian Opera Cape',           N'Historical', N'Edwardian',N'M',700.00, 5, 5),
    (N'GA009', N'Tudor King''s Doublet',          N'Royal',      N'Tudor',N'XL',  1350.00, 1, 1),
    (N'GA010', N'Empress Josephine Gown',         N'Royal',      N'Napoleonic',N'XS',1150.00, 2, 2),
    (N'GA011', N'Roaring Twenties Flapper Dress', N'Historical', N'1920s',N'S',650.00, 4, 4),
    (N'GA012', N'Silver Moon Domino Cloak',       N'Masquerade', N'Venetian', N'XXL',  600.00, 3, 3),
    (N'GA013', N'Byzantine Empress Robe',         N'Royal',      N'Byzantine',N'M',   1400.00, 2, 2),
    (N'GA014', N'Georgian Officer Uniform',       N'Historical', N'Georgian',N'L',    900.00, 3, 3),
    (N'GA015', N'Phantom of the Opera Ensemble',  N'Masquerade', N'Victorian',N'M',   1000.00, 1, 1),
    (N'GA016', N'Tsarina Winter Gown',            N'Royal',      N'Imperial Russian', N'L',   1500.00, 2, 2),
    (N'GA017', N'Medieval Knight Tabard',         N'Historical', N'Medieval',N'XL',   750.00, 4, 4),
    (N'GA018', N'Crimson Carnival Gown',          N'Masquerade', N'Venetian',N'S',   1100.00, 2, 2),
    (N'GA019', N'Pharaoh Ceremonial Robe',        N'Royal',      N'Ancient Egyptian',N'XXL', 1300.00, 2, 2),
    (N'GA020', N'Regency Ballroom Gown',          N'Historical', N'Regency',N'XS',   850.00, 3, 3),
    (N'GA021', N'Harlequin Jester Costume',       N'Masquerade', N'Commedia dell''arte',N'M',   550.00, 4, 4),
    (N'GA022', N'Sun King Coronation Robe',       N'Royal',      N'Baroque',N'L',   1600.00, 1, 1),
    (N'GA023', N'Viking Chieftain Outfit',        N'Historical', N'Viking',N'XXL',  800.00, 2, 2),
    (N'GA024', N'Black Swan Masquerade Gown',     N'Masquerade', N'Baroque',N'XS',  1250.00, 2, 2),
    (N'GA025', N'Ottoman Sultan Kaftan',          N'Royal',      N'Ottoman',N'XL',  1350.00, 2, 2),
    (N'GA026', N'Roman Senator Toga',             N'Historical', N'Ancient Roman',N'M',    600.00, 5, 5),
    (N'GA027', N'Peacock Feather Ensemble',       N'Masquerade', N'Venetian',N'L',   1150.00, 2, 2),
    (N'GA028', N'Heian Court Kimono',             N'Royal',      N'Heian',N'S',   1450.00, 1, 1),
    (N'GA029', N'Elizabethan Lady''s Gown',       N'Historical', N'Elizabethan',N'M',   1050.00, 3, 3),
    (N'GA030', N'Emerald Plague Doctor Cloak',    N'Masquerade', N'Venetian',N'XL',700.00, 1, 1),
    (N'GA031', N'Habsburg Archduke Uniform',      N'Royal',      N'Habsburg',N'M',1250.00, 2, 2),
    (N'GA032', N'Wild West Saloon Dress',         N'Historical', N'Victorian',N'L',650.00, 3, 3),
    (N'GA033', N'Ivory Lace Venetian Gown',       N'Masquerade', N'Venetian',N'XXL',1200.00, 2, 2),
    (N'GA034', N'Celtic Queen Robe',              N'Royal',      N'Iron Age',N'XS', 1100.00, 2, 2),
    (N'GA035', N'Rock and Roll Swing Dress',      N'Historical', N'1950s',N'S',550.00, 4, 4),
    (N'GA036', N'Midnight Raven Ensemble',        N'Masquerade', N'Gothic',N'M',900.00, 2, 2),
    (N'GA037', N'Mughal Empress Lehenga',         N'Royal',      N'Mughal',N'XXL',1550.00, 2, 2),
    (N'GA038', N'Musketeer Captain Outfit',       N'Historical', N'Baroque',N'XL',800.00, 3, 3),
    (N'GA039', N'Rose Gold Carnival Suit',        N'Masquerade', N'Rococo',N'L',1050.00, 2, 2),
    (N'GA040', N'Rococo Marquise Gown',           N'Royal',      N'Rococo',N'S',1400.00, 1, 1);

	--sample customers( 10 customers)
INSERT INTO Customers (CustomerId, FullName, Email, Phone) VALUES
	(N'C001', N'John Doe', N'john.doe@email.com', N'51236574'),
	(N'C002', N'Jane Smith', N'jane.smith@email.com', N'51236575'),
	(N'C003', N'Robert Johnson', N'robert.johnson@email.com', N'51236576'),
	(N'C004', N'Emily Davis', N'emily.davis@email.com', N'51236577'),
	(N'C005', N'Michael Wilson', N'michael.wilson@email.com', N'51236578'),
	(N'C006', N'Sarah Brown', N'sarah.brown@email.com', N'51236579'),
	(N'C007', N'David Miller', N'david.miller@email.com', N'51236580'),
	(N'C008', N'Lisa Taylor', N'lisa.taylor@email.com', N'51236581'),
	(N'C009', N'James Anderson', N'james.anderson@email.com', N'51236582'),
	(N'C010', N'Jennifer Thomas', N'jennifer.thomas@email.com', N'51236583')

	-- sample rentals (15 rentals); 6 returned, 3 overdue, 6 active
	--Days: DueDate - StartDate
	--TotalCost: days* DailyHirePrice
	--offsetdays are days from today (so -5 means 5 days ago)
	
DECLARE @Today DATE = CAST(GETDATE() AS DATE);

INSERT INTO Rentals (RentalId, CostumeId, CustomerId, StartDate, DueDate, ReturnDate, TotalCost) VALUES
    -- Returned
    (N'R001', N'GA004', N'C006', DATEADD(DAY, -40, @Today), DATEADD(DAY, -35, @Today), DATEADD(DAY, -36, @Today), 7250.00),
    (N'R002', N'GA007', N'C004', DATEADD(DAY, -30, @Today), DATEADD(DAY, -26, @Today), DATEADD(DAY, -26, @Today), 5000.00),
    (N'R003', N'GA001', N'C008', DATEADD(DAY, -25, @Today), DATEADD(DAY, -21, @Today), DATEADD(DAY, -21, @Today), 4800.00),
    (N'R004', N'GA011', N'C002', DATEADD(DAY, -20, @Today), DATEADD(DAY, -17, @Today), DATEADD(DAY, -18, @Today), 1950.00),
    (N'R005', N'GA026', N'C010', DATEADD(DAY, -15, @Today), DATEADD(DAY, -13, @Today), DATEADD(DAY, -13, @Today), 1200.00),
    (N'R006', N'GA020', N'C001', DATEADD(DAY, -12, @Today), DATEADD(DAY, -9, @Today), DATEADD(DAY, -10, @Today), 2550.00),
    -- Overdue (not returned, due date has passed)
    (N'R007', N'GA001', N'C003', DATEADD(DAY, -14, @Today), DATEADD(DAY, -6, @Today), NULL, 9600.00),
    (N'R008', N'GA002', N'C009', DATEADD(DAY, -10, @Today), DATEADD(DAY, -4, @Today), NULL, 5700.00),
    (N'R009', N'GA015', N'C005', DATEADD(DAY, -8, @Today), DATEADD(DAY, -2, @Today), NULL, 6000.00),
    -- Active (not returned, not due yet)
    (N'R010', N'GA006', N'C007', DATEADD(DAY, -3, @Today), DATEADD(DAY, 4, @Today), NULL, 9100.00),
    (N'R011', N'GA009', N'C001', DATEADD(DAY, -2, @Today), DATEADD(DAY, 5, @Today), NULL, 9450.00),
    (N'R012', N'GA022', N'C002', DATEADD(DAY, -1, @Today), DATEADD(DAY, 3, @Today), NULL, 6400.00),
    (N'R013', N'GA030', N'C004', @Today,DATEADD(DAY,2, @Today), NULL, 1400.00),
    (N'R014', N'GA003', N'C006', DATEADD(DAY, -1, @Today), DATEADD(DAY,6, @Today), NULL, 5600.00),
    (N'R015', N'GA036', N'C010', @Today,DATEADD(DAY, 7, @Today), NULL, 6300.00);

-- updating costume quantity to update available quantity
UPDATE Costumes
SET AvailableQuantity = AvailableQuantity - 1
WHERE CostumeId IN (N'GA001', N'GA002', N'GA015', N'GA006', N'GA009', N'GA022', N'GA030', N'GA003', N'GA036');

--to check if everything is working
SELECT COUNT(*) AS Costumes FROM Costumes;
SELECT COUNT(*) AS Customers FROM Customers;
SELECT COUNT(*) AS Rentals FROM Rentals;
SELECT COUNT(*) AS Users FROM Users;
SELECT CostumeId, CostumeName FROM Costumes WHERE AvailableQuantity = 0;
SELECT RentalId, CustomerId, DueDate FROM Rentals WHERE ReturnDate IS NULL AND DueDate < @Today;





	


