-- *************************************************
-- ************ INITIAL BUILDING SKRIPT ************
-- *************************************************
-- CreationDate: 24.09.2019;
-- DO NOT MARK FOREIGN KEYS AS 'NOT NULL' BECAUSE
-- this causes a failiure in SQLLiteNetExtensions!
-- https://bitbucket.org/twincoders/sqlite-net-extensions/issues/108/insertorreplacewithchildren-throws-an

CREATE TABLE Fishes (
	ID			TEXT		PRIMARY KEY,
	FishName	TEXT		NOT NULL,
	SortOrder	INTEGER		NOT NULL
);

CREATE TABLE BaitColors (
	ID			TEXT		PRIMARY KEY,
	BaitColor	TEXT		NOT NULL,
	SortOrder	INTEGER		NOT NULL
);

CREATE TABLE BaitTypes (
	ID			TEXT		PRIMARY KEY,
	BaitType	TEXT		NOT NULL,
	SortOrder	INTEGER		NOT NULL
);

CREATE TABLE Currents (
	ID				TEXT		PRIMARY KEY,
	[Current]		TEXT		NOT NULL,
	SortOrder		INTEGER		NOT NULL
);

CREATE TABLE Turbities (
	ID			TEXT		PRIMARY KEY,
	Turbidity	TEXT		NOT NULL,
	SortOrder	INTEGER		NOT NULL
);

CREATE TABLE WaterColors (
	ID			TEXT		PRIMARY KEY,
	WaterColor	TEXT		NOT NULL,
	SortOrder	INTEGER		NOT NULL
);

CREATE TABLE PhasesOfTheMoon (
	ID				TEXT		PRIMARY KEY,
	PhaseOfTheMoon	TEXT		NOT NULL,
	SortOrder		INTEGER		NOT NULL
);

CREATE TABLE WaterModels (
	ID			TEXT		NOT NULL PRIMARY KEY,
	Water		TEXT		NOT NULL
);

CREATE TABLE WindDirections (
	ID				TEXT		PRIMARY KEY,
	WindDirection	TEXT		NOT NULL,
	SortOrder		INTEGER		NOT NULL
);

CREATE TABLE WheaterSituations (
	ID					TEXT		PRIMARY KEY,
	WeatherSituation	TEXT		NOT NULL,
	SortOrder			INTEGER		NOT NULL
);

CREATE TABLE Settings (
	ID					TEXT		PRIMARY KEY,
	Setting				TEXT		NOT NULL,
	LandingPage			TEXT		NOT NULL,
	SortOrder			INTEGER		NOT NULL
);

CREATE TABLE FishingAreas (
	ID					TEXT		PRIMARY KEY,
	FishingArea			TEXT		NOT NULL,
	Lat					FLOAT		NOT NULL,
	Lng					FLOAT		NOT NULL,
	ID_WaterModel		TEXT		
);

CREATE TABLE Spots (
	ID					TEXT		PRIMARY KEY,
	Spot				TEXT		NOT NULL,
	ID_FishingArea		TEXT		,
	ID_SpotType			TEXT		
);

CREATE TABLE SpotTypes (
	ID			TEXT		PRIMARY KEY,
	SpotType	TEXT		NOT NULL
);

CREATE TABLE SpotMarkers (
	ID			TEXT		PRIMARY KEY,
	SpotMarker	TEXT		NOT NULL,
	Lat			FLOAT		NOT NULL,
	Lng			FLOAT		NOT NULL,
	ID_Spot		TEXT
);

CREATE TABLE Trips (
	ID				TEXT		PRIMARY KEY,
	ID_FishingArea	TEXT,
	TripDateTime	DATETIME		
);

INSERT INTO WaterModels (ID, Water) VALUES ('2a3eeecf-472c-4b0f-9df0-73386cb3b3f7', 'River');
INSERT INTO WaterModels (ID, Water) VALUES ('2f2f88a6-69ef-4096-8f8c-108aa8f628d5', 'Lake');
INSERT INTO WaterModels (ID, Water) VALUES ('8978b6c9-af19-403f-b6d3-dba4eb1be135', 'Barrier lake');
INSERT INTO WaterModels (ID, Water) VALUES ('d95694ef-1d6c-4ab4-9f9b-587fac74298e', 'Tidal water');

INSERT INTO SpotTypes (ID, SpotType) VALUES ('1fb8243b-a672-496b-955a-5930cb706250', 'Spot');
INSERT INTO SpotTypes (ID, SpotType) VALUES ('6b103efc-75c9-45ea-8fad-1b871cbbd391', 'Stretch');

INSERT INTO Fishes (ID, FishName, SortOrder) VALUES ('8cd19c21-bf09-43ea-b799-dbcf30183e08','Zander', 1);

INSERT INTO Settings (ID, Setting, LandingPage, SortOrder) VALUES ('21d269b9-62ee-4104-8d32-cf92534aaba3','Reviere', 'FishingAreasPage', 1);

-- *************************************************
-- ****************** MOCKED DATA ******************
-- *************************************************

INSERT INTO FishingAreas (ID, FishingArea, Lat, Lng, ID_WaterModel) VALUES ('0c46a2fd-9a56-4663-9ead-0d92ff39db7c', 'Donau Obermühl', 48.45, 13.9167, '2a3eeecf-472c-4b0f-9df0-73386cb3b3f7');
INSERT INTO Spots (ID, Spot, ID_FishingArea, ID_SpotType) VALUES ('610b3ec0-0d70-40a6-9d9f-5167a8135410', 'Zanderfelsen', '0c46a2fd-9a56-4663-9ead-0d92ff39db7c', '1fb8243b-a672-496b-955a-5930cb706250');
INSERT INTO SpotMarkers (ID, SpotMarker, Lat, Lng, ID_Spot) VALUES ('6cd22c21-bf09-43ea-b799-dbcf30114e09', '6cd22c21-bf09-43ea-b799-dbcf30114e09',  48.46, 13.9267, '610b3ec0-0d70-40a6-9d9f-5167a8135410');