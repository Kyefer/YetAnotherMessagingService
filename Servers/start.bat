@echo off

SET "proxyCmd=dotnet run proxy"
SET "slaveCmd=dotnet run slave"
:: The proxy port cannot be changed as it is used in the client
SET proxyPort=8000
:: Change 
SET slavePorts=8001 8002 8003 8004

start cmd /k %proxyCmd% %proxyPort% %slavePorts%

for %%a in (%slavePorts%) do (
    TIMEOUT 3 > NUL
    start cmd /k %slaveCmd% %%a %slavePorts%
)