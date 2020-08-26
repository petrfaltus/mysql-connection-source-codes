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
$db_table = "people";

$db_update_column = "remark";
$db_update_column_variable = ":updatevar";

$db_column = "id";
$db_column_variable = ":var";
$db_column_value = 1;

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

   // UPDATE statement
   $new_comment = "PHP ".date("j.n.Y H:i:s");

   $stm0 = $conn->prepare("update ".$db_table." set ".$db_update_column."=".$db_update_column_variable." where ".$db_column."!=".$db_column_variable);
   $stm0->bindParam($db_update_column_variable, $new_comment, PDO::PARAM_STR);
   $stm0->bindParam($db_column_variable, $db_column_value, PDO::PARAM_INT);
   $stm0->execute();
   echo "Total updated rows: ".$stm0->rowCount().PHP_EOL;
   echo PHP_EOL;

   $stm0 = null;

   // Full SELECT statement
   $stm1 = $conn->prepare("select * from ".$db_table);
   $stm1->execute();
   echo "Total columns x rows: ".$stm1->columnCount()." x ".$stm1->rowCount().PHP_EOL;

   echo "Fetch all rows ";
   $lines1 = $stm1->fetchAll(PDO::FETCH_ASSOC);
   if ($lines1 == false)
     print_r($stm1->errorInfo());
   else
     print_r($lines1);
   echo PHP_EOL;

   $stm1 = null;

   // SELECT WHERE statement
   $stm2 = $conn->prepare("select count(*) from ".$db_table." where ".$db_column."!=".$db_column_variable);
   $stm2->bindParam($db_column_variable, $db_column_value, PDO::PARAM_INT);
   $stm2->execute();
   echo "Total columns x rows: ".$stm2->columnCount()." x ".$stm2->rowCount().PHP_EOL;

   echo "Fetch all rows ";
   $lines2 = $stm2->fetchAll(PDO::FETCH_ASSOC);
   if ($lines2 == false)
     print_r($stm2->errorInfo());
   else
     print_r($lines2);

   $stm2 = null;

   // Disconnect the database
   $conn = null;
  }
catch (PDOException $e)
  {
   echo $e->getMessage().PHP_EOL;
  }

?>
