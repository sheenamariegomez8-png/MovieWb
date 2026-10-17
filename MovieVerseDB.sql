

USE master;
GO


IF DB_ID(N'MovieVerseDB') IS NOT NULL
BEGIN
ALTER DATABASE MovieVerseDB
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

DROP DATABASE MovieVerseDB;

END;
GO

-- Create the database
CREATE DATABASE MovieVerseDB;
GO

USE MovieVerseDB;
GO

-- Create the Movies table
CREATE TABLE dbo.Movies
(
MovieID INT IDENTITY(1,1) PRIMARY KEY,
MovieTitle NVARCHAR(200) NOT NULL,
Genre NVARCHAR(100) NOT NULL,
DatePublished DATE NOT NULL,
Director NVARCHAR(150) NOT NULL,
Duration INT NOT NULL,
Rating DECIMAL(3,1) NOT NULL,
Synopsis NVARCHAR(MAX) NOT NULL,

CONSTRAINT CK_Movies_Duration
    CHECK (Duration > 0),

CONSTRAINT CK_Movies_Rating
    CHECK (Rating >= 0 AND Rating <= 10)

);
GO

-- Insert sample movies
INSERT INTO dbo.Movies
(MovieTitle, Genre, DatePublished, Director,
Duration, Rating, Synopsis)
VALUES
(N'Midnight Sun', N'Romance, Drama', '2018-03-23',
N'Scott Speer', 91, 6.6,
N'A young woman with a rare condition experiences love while avoiding sunlight.'),

(N'Alive', N'Horror, Thriller', '2020-06-24',
N'Cho Il-hyung', 98, 6.3,
N'A young man struggles to survive during a zombie outbreak.'),

(N'People We Meet on Vacation', N'Romance, Comedy', '2026-01-09',
N'Brett Haley', 117, 6.6,
N'Two longtime friends travel together and explore their complicated relationship.'),

(N'Love Untangled', N'Romance, Comedy', '2025-08-29',
N'Sun Namkoong', 118, 7.1,
N'A Korean romantic comedy about young love and relationships.'),

(N'Pawn', N'Drama', '2020-09-29',
N'Kang Dae-gyu', 113, 7.0,
N'Two debt collectors unexpectedly become guardians to a young girl.'),

(N'Titanic', N'Romance, Drama', '1997-12-19',
N'James Cameron', 195, 7.9,
N'A romance unfolds aboard the ill-fated RMS Titanic.'),

(N'The Notebook', N'Romance, Drama', '2004-06-25',
N'Nick Cassavetes', 123, 7.8,
N'A couple faces challenges as their love story unfolds over the years.'),

(N'Five Feet Apart', N'Romance, Drama', '2019-03-15',
N'Justin Baldoni', 116, 7.2,
N'Two teenagers develop a relationship while navigating medical restrictions.'),

(N'Me Before You', N'Romance, Drama', '2016-06-03',
N'Thea Sharrock', 110, 7.4,
N'A young woman forms a meaningful connection with a man she cares for.'),

(N'The Fault in Our Stars', N'Romance, Drama', '2014-06-06',
N'Josh Boone', 126, 7.7,
N'Two teenagers meet at a support group and develop a close relationship.');
GO

-- Verify the inserted records
SELECT
MovieID,
MovieTitle,
Genre,
DatePublished,
Director,
Duration,
Rating,
Synopsis
FROM dbo.Movies
ORDER BY MovieID;
GO
