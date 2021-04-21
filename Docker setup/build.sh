#!/bin/sh

sudo docker image rm dapper-repo-sql
sudo docker build -t dapper-repo-sql .

