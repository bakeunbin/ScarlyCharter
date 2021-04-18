﻿
CREATE TABLE CLIENT (
	Client_ID		INT				NOT NULL,
	Client_Name		VARCHAR (30),
	Payment_Info	VARCHAR (30),
	Username		VARCHAR (30)	NOT NULL,
	Password		VARCHAR (256)	NOT NULL
);

ALTER TABLE CLIENT ADD PRIMARY KEY (Client_ID);

CREATE TABLE SEASON (
	Season_ID		INT				NOT NULL CHECK (Season_ID >= 0 AND Season_ID < 4),
	Season_name		VARCHAR (30)	NOT NULL
);

ALTER TABLE SEASON ADD PRIMARY KEY (Season_ID);

CREATE TABLE REGION (
	Region_ID		INT				NOT NULL CHECK (Region_ID >= 0 AND Region_ID < 20),
	Region_Name		VARCHAR (30)	NOT NULL,
	Season_ID		INT
);

ALTER TABLE REGION ADD PRIMARY KEY (Region_ID);
ALTER TABLE REGION ADD FOREIGN KEY (Season_ID) REFERENCES SEASON (Season_ID);

CREATE TABLE LOCATION (
	Location_ID		INT				NOT NULL,
	Region_ID		INT				NOT NULL,
	Type			VARCHAR (30)	NOT NULL CHECK (Type IN ('River', 'Lake', 'Inshore', 'Offshore')),
	Availability	BIT,
);

ALTER TABLE LOCATION ADD PRIMARY KEY (Location_ID);
ALTER TABLE LOCATION ADD FOREIGN KEY (Region_ID) REFERENCES REGION (Region_ID) ON DELETE CASCADE ON UPDATE CASCADE;

CREATE TABLE FISH (
	Fish_ID			INT				NOT NULL,
	Fish_Name		VARCHAR (30)	NOT NULL,
	Region_ID		INT				NOT NULL,
	Season_ID		INT
);

ALTER TABLE FISH ADD PRIMARY KEY (Fish_ID);
ALTER TABLE FISH ADD FOREIGN KEY (Region_ID) REFERENCES REGION (Region_ID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE FISH ADD FOREIGN KEY (Season_ID) REFERENCES SEASON (Season_ID) ON DELETE SET NULL ON UPDATE CASCADE;

CREATE TABLE SCHEDULE (
	Schedule_ID		INT				NOT NULL,
	Date			DATE			NOT NULL,
	Start_Time		TIME			NOT NULL,
	End_Time		TIME			NOT NULL
);

ALTER TABLE SCHEDULE ADD PRIMARY KEY (Schedule_ID);

CREATE TABLE GUIDE (
	Guide_ID		INT				NOT NULL,
	Guide_Name		VARCHAR (30)	NOT NULL,
	Region_ID		INT,
	Season_ID		INT,
	Fishing_Style	VARCHAR (30),
	Overnight		BIT				-- 0 = no, 1 = will do over night.
);

ALTER TABLE GUIDE ADD PRIMARY KEY (Guide_ID);
ALTER TABLE GUIDE ADD FOREIGN KEY (Region_ID) REFERENCES REGION (Region_ID) ON DELETE SET NULL ON UPDATE CASCADE;
ALTER TABLE GUIDE ADD FOREIGN KEY (Season_ID) REFERENCES SEASON (Season_ID) ON DELETE SET NULL ON UPDATE CASCADE;

CREATE TABLE BOOKED_TRIP (
	Trip_ID			INT				NOT NULL,
	Client_ID		INT				NOT NULL,
	Schedule_ID		INT				NOT NULL,
	Location_ID		INT				NOT NULL,
	Guide_ID		INT				NOT NULL,
	Target_Fish_ID	INT,
	Party_Size		INT				NOT NULL CHECK (Party_Size > 0 AND Party_Size <= 10),
	Fishing_Style	VARCHAR (30)
);

ALTER TABLE BOOKED_TRIP ADD PRIMARY KEY (Trip_Id);
ALTER TABLE BOOKED_TRIP ADD FOREIGN KEY (Client_ID) REFERENCES CLIENT (Client_ID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE BOOKED_TRIP ADD FOREIGN KEY (Schedule_ID) REFERENCES SCHEDULE (Schedule_ID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE BOOKED_TRIP ADD FOREIGN KEY (Location_ID) REFERENCES Location (Location_ID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE BOOKED_TRIP ADD FOREIGN KEY (Guide_ID) REFERENCES GUIDE (Guide_ID) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE BOOKED_TRIP ADD FOREIGN KEY (Target_Fish_ID) REFERENCES FISH (Fish_ID) ON DELETE SET NULL ON UPDATE CASCADE;

GO

INSERT INTO CLIENT VALUES (0, 'James Charlie', '4564239499330319' ,' jcharlie212', '********')
INSERT INTO CLIENT VALUES (1, 'Laide Dineo', '4532333249302312' ,' dlaids229', '********')
INSERT INTO CLIENT VALUES (2, 'Koali Green', '4522548299391231' , 'koaGreen1212', '********')
INSERT INTO CLIENT VALUES (3, 'Holton Jeon', '4513425324623312' ,' jchhJeon4422', '********')

-- (Season_ID, Season_Name)
INSERT INTO SEASON VALUES (0, 'Spring') 
INSERT INTO SEASON VALUES (1, 'Summer') 
INSERT INTO SEASON VALUES (2, 'Fall') 
INSERT INTO SEASON VALUES (3, 'Winter') 

-- (Region_ID, Region_Name, Season_ID)
INSERT INTO REGION VALUES (0, 'SouthEast Georgia', 1)
INSERT INTO REGION VALUES (1, 'North Georgia', 3)
INSERT INTO REGION VALUES (2, 'Coastal Georgia', 2)

-- (Location_ID, Region_ID, Type, Availability)
INSERT INTO LOCATION VALUES (0, 0, 'River', 1)
INSERT INTO LOCATION VALUES (1, 1, 'Offshore', 1)
INSERT INTO LOCATION VALUES (2, 2, 'Lake', 0) 

-- (Fish_ID, Fish_Name, Region_ID, Season_ID)
INSERT INTO FISH VALUES (0, 'Redfish', 0, 2) 
INSERT INTO FISH VALUES (1, 'Bass', 1, 2) 
INSERT INTO FISH VALUES (2, 'Trout', 2, 3) 
INSERT INTO FISH VALUES (3, 'Snapper', 0, 3)

-- (Schedule_ID, Date, Start_Time, End_Time)
INSERT INTO SCHEDULE VALUES (0, '2021-03-04', '13:00:00', '14:15:00')
INSERT INTO SCHEDULE VALUES (1, '2021-07-06', '15:00:00', '15:30:00')
INSERT INTO SCHEDULE VALUES (2, '2021-09-10', '17:00:00', '16:45:00')

-- (Guide_ID, Guide_Name, Region_ID, Season_ID, Fishing_Style, Overnight)
INSERT INTO GUIDE VALUES (0, 'John J Cod', 0, 3, 'Fly Fishing', 1)
INSERT INTO GUIDE VALUES (1, 'Jill M Red', 1, 2, 'Spinning Reel', 0)
INSERT INTO GUIDE VALUES (2, 'Gary Y James', 2, 2, 'Offshore', 1)

-- (Trip_ID, Client_ID, Schedule_ID, Target_Fish_ID, Location_ID, Guide_ID, Party_Size, Fishing_Style)
INSERT INTO BOOKED_TRIP VALUES (0, 0, 1, 0, 1, 1, 1, 'Spinning Reel')
INSERT INTO BOOKED_TRIP VALUES (1, 1, 2, 0, 2, 2, 2, 'Fly Fishing')

GO

CREATE TRIGGER ON_CLIENT_REGISTER ON CLIENT AFTER INSERT
AS 
IF EXISTS (SELECT *
		   FROM CLIENT AS C
		   WHERE Username = C.Username)
BEGIN
	RAISERROR ('User already exists!', 1, 1);
	ROLLBACK TRANSACTION;
END;
