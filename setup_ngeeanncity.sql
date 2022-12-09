DROP DATABASE IF EXISTS ngeeanncity;
CREATE DATABASE IF NOT EXISTS ngeeanncity;
USE ngeeanncity;

/*--------------------------------------
              CREATE TABLE
--------------------------------------*/

CREATE TABLE user (
  userid int(11) NOT NULL AUTO_INCREMENT,
  email varchar(320) NOT NULL,
  password varchar(128) NOT NULL,
  username varchar(50) NOT NULL,
  savedgamedata mediumtext NOT NULL DEFAULT '{}' COMMENT 'JSON array',
  PRIMARY KEY (userid),
  UNIQUE(email),
  UNIQUE(username)
)
ENGINE = INNODB;


CREATE TABLE scores (
  id int(11) NOT NULL AUTO_INCREMENT,
  score int(11) NOT NULL,
  userid int(11) NOT NULL,
  timestamp datetime NOT NULL DEFAULT current_timestamp ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (id),
  CONSTRAINT FOREIGN KEY (userid) REFERENCES user(userid)
)
ENGINE = INNODB;

/*--------------------------------------
              CREATE VIEW
--------------------------------------*/

-- View top 10 highscore
CREATE VIEW highscore
  AS 
    SELECT u.username, s.score
    FROM  scores s 
    INNER JOIN user u ON s.userid = u.userid 
    ORDER BY s.score DESC, s.timestamp ASC 
    LIMIT 0,10
;



/*--------------------------------------
                INSERT
--------------------------------------*/

-- Dumping data for table user
INSERT INTO user (email, password, username) VALUES
('n@l.com', 'Qwer1234', 'nathen'),
('j@w.com', 'Qwer1234', 'joseph'),
('s@j.com', 'Qwer1234', 'daniel'),
('y@c.com', 'Qwer1234', 'yuchen'),
('z@y.com', 'Qwer1234', 'zhiyan');

-- Dumping data for highscore
INSERT INTO scores (score, userid) VALUES
(15, 1),
(13, 2),
(2, 3),
(7, 4),
(11, 5);
