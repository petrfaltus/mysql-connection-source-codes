# MySQL connection source codes
Small example console source codes how to connect to the MySQL, how to update rows and how to read the table.

## Running under Windows
1. clone this repository to your computer
2. install the **MySQL** (as a **Docker** container)
3. prepare the user, the table and rows in the database
4. build and run the example **Java** code
5. compile and run the example **.NET C#** code
6. run the example **PHP** code

### 1. Cloning to your computer
- install [GIT] on your computer
- clone this repository to your computer by the GIT command
  `git clone https://github.com/petrfaltus/mysql-connection-source-codes.git`

### 2. Installation of the MySQL (as a Docker container)
- install [docker desktop] on your computer
- refer the [MySQL image]
- refer the [Adminer image]

The subdirectory `docker-database` contains prepared Windows batches:
- `01-run-database.cmd` - pulls images and runs both containers (database + adminer) after the compose file **at the first time**
- `02-switch-database-OFF.cmd` - stops both already existing containers (adminer + database)
- `02-switch-database-ON.cmd` - starts both already existing containers (database + adminer)
- `03-inspect-database.cmd` - shows details for already existing database container
- `03-inspect-adminer.cmd` - shows details for already existing adminer container
- `04-exec-connection-to-database-root.cmd` - executes the **mysql cli** terminal into running database container (as the user *root*)
- `04-exec-connection-to-database-testuser.cmd` - executes the **mysql cli** terminal into running database container (as the user *testuser*)
- `containers.cmd` - lists currently running containers and list of all existing containers

### 3. Preparing the database
For the connection to the database use either the **mysql cli** terminal or the Adminer container on [http://localhost:8080](http://localhost:8080)

#### Connection using Adminer container
User *root* (default password *R00tPa33w0rd!*)

![user root configuration](adminer.root.png)

User *testuser* (default password *T3stUs3r!*)

![user testuser configuration](adminer.testuser.png)

#### SQL lines for root
```sql
SET NAMES utf8;

CREATE USER `testuser` IDENTIFIED BY 'T3stUs3r!';
CREATE DATABASE `testdb`;
GRANT ALL PRIVILEGES ON `testdb`.* TO `testuser`@`%`;
```

#### SQL lines for testuser
```sql
SET NAMES utf8;

USE `testdb`;

DROP TABLE IF EXISTS `people`;
CREATE TABLE `people` (
  `name` VARCHAR(40) NOT NULL,
  `surname` VARCHAR(60) NOT NULL,
  `age` TINYINT UNSIGNED NOT NULL,
  `created` DATETIME DEFAULT CURRENT_TIMESTAMP,
  `updated` DATETIME ON UPDATE CURRENT_TIMESTAMP,
  `remark` VARCHAR(80),
  `id` SMALLINT UNSIGNED NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`id`)
);

INSERT INTO `people` (`name`, `surname`, `age`) VALUES ('Robin', 'Shark', 35);
INSERT INTO `people` (`name`, `surname`, `age`) VALUES ('Linda', 'Morwin', 28);
INSERT INTO `people` (`name`, `surname`, `age`) VALUES ('Patrick', 'Woody', 51);
INSERT INTO `people` (`name`, `surname`, `age`) VALUES ('Aneta', 'White', 17);
INSERT INTO `people` (`name`, `surname`, `age`) VALUES ('Roger', 'Hover', 29);

DROP FUNCTION IF EXISTS `factorial`;
DELIMITER $$
CREATE FUNCTION `factorial`(`n` INT) RETURNS INT DETERMINISTIC
BEGIN
  DECLARE `result` INT DEFAULT 1;
  DECLARE `ijk` INT DEFAULT 2;
  IF `n` < 0 THEN
    RETURN -1;
  END IF;
  IF `n` < 2 THEN
    RETURN `result`;
  END IF;
  WHILE `ijk` <= `n` DO
    SET `result` = `result` * `ijk`;
    SET `ijk` = `ijk` + 1;
  END WHILE;
  RETURN `result`;
END$$
DELIMITER ;

DROP PROCEDURE IF EXISTS `add_and_subtract`;
DELIMITER $$
CREATE PROCEDURE `add_and_subtract`(`a` INT, `b` INT, OUT `x` INT, OUT `y` INT)
BEGIN
  SET `x` = `a` + `b`;
  SET `y` = `a` - `b`;
END$$
DELIMITER ;
```

#### optional SQL check lines for testuser
```sql
SET NAMES utf8;

USE `testdb`;

SELECT * FROM `people`;

SELECT count(*) FROM `people`;
SELECT count(*) FROM `people` WHERE `id`!=1;

SELECT `factorial`(2);
SELECT `factorial`(3);
SELECT `factorial`(4);

CALL `add_and_subtract`(12, 5, @env_var_x, @env_var_y);
select @env_var_x, @env_var_y;
```

### 4. The Java client source code
- install [Java JDK] on your computer
- set the OS environment `%JAVA_HOME%` variable (must exist `"%JAVA_HOME%\bin\java.exe"`)

#### 4.1. Apache Maven
- install [Apache Maven] on your computer
- add the Maven directory (where the batch `mvn.cmd` locates) to the OS environment `%PATH%` variable

The subdirectory `java-maven` contains prepared Windows batches:
- `01-build.cmd` - cleans, compiles and builds the Maven project
- `02-run.cmd` - runs the built Java archive (JAR)
- `03-clean.cmd` - cleans the Maven project

#### 4.2. Gradle Build Tool
- install [Gradle Build Tool] on your computer
- add the Gradle directory (where the batch `gradle.bat` locates) to the OS environment `%PATH%` variable

The subdirectory `java-gradle` contains prepared Windows batches:
- `01-build.cmd` - cleans, compiles and builds the Gradle project
- `02-run.cmd` - runs the built Java archive (JAR)
- `03-clean.cmd` - cleans the Gradle project

### 5. The .NET C# client source code
- use the `csc.exe` .NET C# compiler that is the part of Microsoft .NET Framework (part of OS)
- install [MySQL Connector/NET] on your computer
- check the right `MySql.Data.dll` from [MySQL Connector/NET] in the `csharp\bin` directory

The subdirectory `csharp` contains prepared Windows batches:
- `01-compile.cmd` - compiles the source code (contains the path definition to the `csc.exe` compiler)
- `02-run.cmd` - runs the Windows executable
- `03-clean.cmd` - deletes the Windows executable

### 6. The PHP client source code
- install [PHP] on your computer
- set the OS environment `%PHP_HOME%` variable (must exist `"%PHP_HOME%\php.exe"`)

To the `php.ini` in the PHP directory `%PHP_HOME%` add lines
```
[PHP]
extension_dir = "ext"
extension=pdo_mysql

[Date]
date.timezone = Europe/Prague
```

The subdirectory `php` contains prepared Windows batch:
- `01-run.cmd` - runs the code through the PHP interpreter

## Versions
Now in August 2020 I have the computer with **Windows 10 Pro 64bit**, **12GB RAM** and available **50GB free HDD space**

| Tool | Version | Setting |
| ------ | ------ | ------ |
| [GIT] | 2.26.0.windows.1 | |
| [docker desktop] | 2.3.0.4 (46911) stable | 2 CPUs, 3GB memory, 1GB swap, 48GB disc image size |
| [MySQL image] | 5.7.30 | default password for root: R00tPa33w0rd! |
| [Adminer image] | 4.7.7-standalone | |
| [Java JDK] | 14.0.1 | Java(TM) SE Runtime Environment (build 14.0.1+7) |
| [Apache Maven] | 3.6.3 | |
| [Gradle Build Tool] | 6.3 | |
| [MySQL Connector/NET] | 8.0.20.0 | |
| [PHP] | 7.4.4 | 7.4.4-nts-Win32-vc15-x64 |

## To do (my plans to the future)


[GIT]: <https://git-scm.com>
[docker desktop]: <https://docs.docker.com/desktop/>
[MySQL image]: <https://hub.docker.com/_/mysql>
[Adminer image]: <https://hub.docker.com/_/adminer>
[Java JDK]: <https://www.oracle.com/java/technologies/javase-downloads.html>
[Apache Maven]: <https://maven.apache.org/>
[Gradle Build Tool]: <https://gradle.org/>
[MySQL Connector/NET]: <https://dev.mysql.com/doc/connector-net/en/connector-net-introduction.html>
[PHP]: <https://www.php.net/>
