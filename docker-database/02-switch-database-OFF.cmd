@echo off

docker container stop mysql_adminer_mysql-admin_1
docker container stop mysql_adminer_mysql-db_1
echo.

docker container ls
echo.

pause
