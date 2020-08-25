@echo off

rem mysql cli tool
docker exec -it mysql_adminer_mysql-db_1 mysql --user=testuser --password=T3stUs3r!
echo.

pause
