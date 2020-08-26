<?php

/*
2) In the php.ini added lines:
[PHP]
extension_dir = "ext"
extension=pdo_mysql
[Date]
date.timezone = Europe/Prague

*/

$db_driver = "mysql";

$db_host = "localhost";
$db_port = 3306;
$db_name = "testdb";
$db_username = "testuser";
$db_password = "T3stUs3r!";

$availableDrivers = PDO::getAvailableDrivers();

echo "Available PDO drivers ";
print_r($availableDrivers);
echo PHP_EOL;

if (!in_array($db_driver, $availableDrivers))
  {
   echo "PDO driver ".$db_driver." or it's subdriver is not available or has not been enabled in php.ini".PHP_EOL;
   exit;
  }

try
  {
   // Build the connection string and connect the database
   $dsn = $db_driver.":host=".$db_host.":".$db_port.";dbname=".$db_name;
   $conn = new PDO($dsn, $db_username, $db_password);

   echo "ATTR_CLIENT_VERSION = ".$conn->getAttribute(PDO::ATTR_CLIENT_VERSION).PHP_EOL;
   echo "ATTR_CONNECTION_STATUS = ".$conn->getAttribute(PDO::ATTR_CONNECTION_STATUS).PHP_EOL;
   echo "ATTR_DRIVER_NAME = ".$conn->getAttribute(PDO::ATTR_DRIVER_NAME).PHP_EOL;
   echo "ATTR_SERVER_INFO = ".$conn->getAttribute(PDO::ATTR_SERVER_INFO).PHP_EOL;
   echo "ATTR_SERVER_VERSION = ".$conn->getAttribute(PDO::ATTR_SERVER_VERSION).PHP_EOL;
   echo PHP_EOL;

   // Disconnect the database
   $conn = null;
  }
catch (PDOException $e)
  {
   echo $e->getMessage().PHP_EOL;
  }

?>
