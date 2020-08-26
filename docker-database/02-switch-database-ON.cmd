@echo off

docker container start mysql_adminer_mysql-db_1
docker container start mysql_adminer_mysql-admin_1
echo.

docker container ls
echo.

pause
