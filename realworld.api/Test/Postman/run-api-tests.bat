@REM #!/usr/bin/env bash
@REM # set -x

@REM # SCRIPTDIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null && pwd )"

@REM # APIURL=${APIURL:-https://api.realworld.io/api}
@REM # USERNAME=${USERNAME:-u`date +%s`}
@REM # EMAIL=${EMAIL:-$USERNAME@mail.com}
@REM # PASSWORD=${PASSWORD:-password}

@REM # npx newman run $SCRIPTDIR/Conduit.postman_collection.json \
@REM #   --delay-request 500 \
@REM #   --global-var "APIURL=$APIURL" \
@REM #   --global-var "USERNAME=$USERNAME" \
@REM #   --global-var "EMAIL=$EMAIL" \
@REM #   --global-var "PASSWORD=$PASSWORD" \
@REM #   "$@"


@echo off
setlocal enabledelayedexpansion

set SCRIPTDIR=%~dp0
set APIURL=%1
set USERNAME=%2
set EMAIL=%3
set PASSWORD=%4

if "%APIURL%"=="" set APIURL=https://api.realworld.io/api
if "%USERNAME%"=="" set USERNAME=u%time:~6,5% 
@REM là comment
if "%EMAIL%"=="" set EMAIL=!USERNAME!@gmail.com
if "%PASSWORD%"=="" set PASSWORD=password
@REM if "%USERNAME%"=="" set USERNAME=string
@REM if "%EMAIL%"=="" set EMAIL=string@gmail.com
@REM if "%PASSWORD%"=="" set PASSWORD=string

echo %USERNAME%
echo %EMAIL%
echo %PASSWORD%

npx newman run "%SCRIPTDIR%\Conduit.postman_collection.json" ^
  --delay-request 500 ^
  --global-var "APIURL=%APIURL%" ^
  --global-var "USERNAME=%USERNAME%" ^
  --global-var "EMAIL=%EMAIL%" ^
  --global-var "PASSWORD=%PASSWORD%" ^
  %*