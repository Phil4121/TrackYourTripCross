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

CREATE TABLE GenerallSettings (
	ID					TEXT		PRIMARY KEY,
	SettingKey			TEXT		NOT NULL,
	SettingValue		TEXT		NOT NULL,
	SortOrder			INTEGER		NOT NULL
);

CREATE TABLE BackgroundTasks (
	ID					TEXT		PRIMARY KEY,
	ID_ElementReference	TEXT		NOT NULL,
	ID_TaskType			INTEGER		NOT NULL,
	CreationDateTime	TEXT		NOT NULL,
	TaskData			TEXT		NOT NULL,
	TaskResponse		TEXT
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

CREATE TABLE FishedSpots (
	ID				TEXT		PRIMARY KEY,
	ID_Trip			TEXT,
	ID_FishingArea	TEXT,
	ID_Spot			TEXT,
	ID_FishedSpotWeather TEXT,
	StartDateTime	DATETIME,
	EndDateTime		DATETIME
);

CREATE TABLE FishedSpotsWheater (
	ID					TEXT		PRIMARY KEY,
	IsOverwritten		BIT,
	Temperature			FLOAT,
	WeatherSituation	INT
);

CREATE TABLE FishedSpotsWater (
	ID					TEXT		PRIMARY KEY,
	IsOverwritten		BIT,
	Temperature			FLOAT
);

INSERT INTO WaterModels (ID, Water) VALUES ('2a3eeecf-472c-4b0f-9df0-73386cb3b3f7', 'River');
INSERT INTO WaterModels (ID, Water) VALUES ('2f2f88a6-69ef-4096-8f8c-108aa8f628d5', 'Lake');
INSERT INTO WaterModels (ID, Water) VALUES ('8978b6c9-af19-403f-b6d3-dba4eb1be135', 'Barrier lake');
INSERT INTO WaterModels (ID, Water) VALUES ('d95694ef-1d6c-4ab4-9f9b-587fac74298e', 'Tidal water');

INSERT INTO SpotTypes (ID, SpotType) VALUES ('1fb8243b-a672-496b-955a-5930cb706250', 'Spot');
INSERT INTO SpotTypes (ID, SpotType) VALUES ('6b103efc-75c9-45ea-8fad-1b871cbbd391', 'Stretch');

INSERT INTO Turbities (ID, Turbidity, SortOrder) VALUES ('e114fd49-ef4a-4126-8d90-effe1d1bc3d5','klar', 1);
INSERT INTO Turbities (ID, Turbidity, SortOrder) VALUES ('c7c1a293-432a-4b96-98ad-5a1ad22bc3e8','leicht eingetrübt', 2);
INSERT INTO Turbities (ID, Turbidity, SortOrder) VALUES ('9f7282a1-4f6a-41ff-b055-32af324b1b1a','trüb', 3);
INSERT INTO Turbities (ID, Turbidity, SortOrder) VALUES ('359bc3c8-47b1-42b3-8f92-071d2fa09587','stark eingetrübt', 4);

INSERT INTO WaterColors (ID, WaterColor, SortOrder) VALUES ('e114fd49-ef4a-4126-8d90-effe1d1bc3d6','grün', 1);
INSERT INTO WaterColors (ID, WaterColor, SortOrder) VALUES ('c7c1a293-432a-4b96-98ad-5a1ad22bc3e9','braun', 2);
INSERT INTO WaterColors (ID, WaterColor, SortOrder) VALUES ('9f7282a1-4f6a-41ff-b055-32af324b1b2a','grau', 3);

INSERT INTO Currents (ID, [Current], SortOrder) VALUES ('e114fd49-ef4a-4126-8d90-effe2d1bc3d6','keine', 1);
INSERT INTO Currents (ID, [Current], SortOrder) VALUES ('c7c1a293-432a-4b96-98ad-5a1ad12bc3e9','wenig', 2);
INSERT INTO Currents (ID, [Current], SortOrder) VALUES ('9f7282a1-4f6a-41ff-b055-32af344b1b2a','normal', 3);
INSERT INTO Currents (ID, [Current], SortOrder) VALUES ('9f7282a1-4f4a-41ff-b055-32af344b1b2a','stark', 4);
INSERT INTO Currents (ID, [Current], SortOrder) VALUES ('9f7282a1-4f6a-41ff-b065-32af344b1b2a','sehr stark', 5);

INSERT INTO BaitColors (ID, BaitColor, SortOrder) VALUES ('e114fd49-ef4a-4126-8d90-effe1d2bc3d6','hell', 1);
INSERT INTO BaitColors (ID, BaitColor, SortOrder) VALUES ('c7c1a293-432a-4b96-98ad-5a1ad22bc4e9','natürlich', 2);
INSERT INTO BaitColors (ID, BaitColor, SortOrder) VALUES ('9f7282a1-4f6a-41ff-b055-33af324b1b2a','dunkel', 3);
INSERT INTO BaitColors (ID, BaitColor, SortOrder) VALUES ('9f7282a1-4f6a-41ff-b055-33af424b1b2a','neon', 4);

INSERT INTO BaitTypes (ID, BaitType, SortOrder) VALUES ('e114fd48-ef4a-4126-8d90-effe1d2bc3d6','Gummiköder', 1);

INSERT INTO Fishes (ID, FishName, SortOrder) VALUES ('8cd19c21-bf09-43ea-b799-dbcf30183e08','Zander', 1);

INSERT INTO Settings (ID, Setting, LandingPage, SortOrder) VALUES ('21d269b9-62ee-4104-8d32-cf92534ccba3','Allgemeine Einstellungen', 'GenerallSettingPage', 1);
INSERT INTO Settings (ID, Setting, LandingPage, SortOrder) VALUES ('21d269b9-62ee-4104-8d32-cf92534aaba3','Reviere', 'FishingAreasPage', 2);

INSERT INTO GenerallSettings (ID, SettingKey, SettingValue, SortOrder) VALUES ('21d269b9-62ee-4104-8d32-cf98534ccba3','DefaultTemperatureUnit', '1', 1);
INSERT INTO GenerallSettings (ID, SettingKey, SettingValue, SortOrder) VALUES ('21d269b9-62ee-4104-8f42-cf98534ccba3','DefaultLengthUnit', '1', 2);

-- *************************************************
-- ****************** MOCKED DATA ******************
-- *************************************************

INSERT INTO FishingAreas (ID, FishingArea, Lat, Lng, ID_WaterModel) VALUES ('0c46a2fd-9a56-4663-9ead-0d92ff39db7c', 'Donau Obermühl', 48.45, 13.9167, '2a3eeecf-472c-4b0f-9df0-73386cb3b3f7');
INSERT INTO Spots (ID, Spot, ID_FishingArea, ID_SpotType) VALUES ('610b3ec0-0d70-40a6-9d9f-5167a8135410', 'Zanderfelsen', '0c46a2fd-9a56-4663-9ead-0d92ff39db7c', '1fb8243b-a672-496b-955a-5930cb706250');
INSERT INTO SpotMarkers (ID, SpotMarker, Lat, Lng, ID_Spot) VALUES ('6cd22c21-bf09-43ea-b799-dbcf30114e09', '6cd22c21-bf09-43ea-b799-dbcf30114e09',  48.46, 13.9267, '610b3ec0-0d70-40a6-9d9f-5167a8135410');