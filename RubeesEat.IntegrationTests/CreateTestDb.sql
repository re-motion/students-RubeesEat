﻿DROP TABLE IF EXISTS EntryLines;
DROP TABLE IF EXISTS Bills;
DROP TABLE IF EXISTS Persons;

CREATE TABLE Persons
(
    PersonID UUID DEFAULT UUID() PRIMARY KEY,
    FirstName VARCHAR(64) NOT NULL,
    LastName VARCHAR(64) NOT NULL
);

CREATE TABLE Bills
(
    BillID UUID DEFAULT UUID() PRIMARY KEY,
    Description VARCHAR(256) NOT NULL,
    Date DATETIME NOT NULL
);
CREATE TABLE EntryLines
(
    EntryLineID INT AUTO_INCREMENT PRIMARY KEY,
    Amount DECIMAL(15,2) NOT NULL,
    PersonID UUID NOT NULL,
    BillID UUID NOT NULL,
    FOREIGN KEY (PersonID) REFERENCES Persons(PersonID),
    FOREIGN KEY (BillID) REFERENCES Bills(BillID)
);

INSERT INTO Persons (PersonID, FirstName, LastName)
VALUES ('A05764E0-C2F5-4A3F-8F04-746AEE8B355B', 'Item', 'Arslan');
INSERT INTO Persons (PersonID, FirstName, LastName)
VALUES ('883703F3-EEA8-4BCE-BACD-4A77FFE0C294', 'Patrick', 'Widener');
INSERT INTO Persons (PersonID, FirstName, LastName)
VALUES ('544FEBD0-05F8-471A-BAFC-CA7135538031', 'Lilli', 'Grubber');
INSERT INTO Persons (PersonID, FirstName, LastName)
VALUES ('7DBF157B-CBFF-43CE-BFDD-B367611BB1A5', 'Mich', 'Ludwig');

INSERT INTO Bills(BillID, Description, Date)
VALUES ('91CD57E6-418E-4628-82CC-09D471153CF6', 'Mittagessen auf Patrick sein Nacken', '2024-09-09 10:34:09');

INSERT INTO EntryLines(Amount, PersonID, BillID)
VALUES (1000020, '883703F3-EEA8-4BCE-BACD-4A77FFE0C294', '91CD57E6-418E-4628-82CC-09D471153CF6');

INSERT INTO EntryLines(Amount, PersonID, BillID)
VALUES (-20, '883703F3-EEA8-4BCE-BACD-4A77FFE0C294', '91CD57E6-418E-4628-82CC-09D471153CF6');

INSERT INTO EntryLines(Amount, PersonID, BillID)
VALUES (-500000, 'A05764E0-C2F5-4A3F-8F04-746AEE8B355B', '91CD57E6-418E-4628-82CC-09D471153CF6');

INSERT INTO EntryLines(Amount, PersonID, BillID)
VALUES (-500000, '544FEBD0-05F8-471A-BAFC-CA7135538031','91CD57E6-418E-4628-82CC-09D471153CF6');

INSERT INTO Bills(BillID, Description, Date)
VALUES ('01CD57E6-418E-4628-82CC-09D471153CF6', 'Mittagessen auf Michi sein Nacken', '2021-11-09 10:34:09');

INSERT INTO EntryLines(Amount, PersonID, BillID)
VALUES (1000020, '7DBF157B-CBFF-43CE-BFDD-B367611BB1A5', '01CD57E6-418E-4628-82CC-09D471153CF6');

INSERT INTO EntryLines(Amount, PersonID, BillID)
VALUES (-20, '883703F3-EEA8-4BCE-BACD-4A77FFE0C294', '01CD57E6-418E-4628-82CC-09D471153CF6');

INSERT INTO EntryLines(Amount, PersonID, BillID)
VALUES (-500000, 'A05764E0-C2F5-4A3F-8F04-746AEE8B355B', '01CD57E6-418E-4628-82CC-09D471153CF6');

INSERT INTO EntryLines(Amount, PersonID, BillID)
VALUES (-500000, '544FEBD0-05F8-471A-BAFC-CA7135538031','01CD57E6-418E-4628-82CC-09D471153CF6');