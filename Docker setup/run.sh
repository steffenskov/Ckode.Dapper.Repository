#!/bin/sh

sudo docker run -p 1433:1433 --name dapper-repo-sql -h dapper-repo-sql -d dapper-repo-sql:latest
sudo docker exec -it dapper-repo-sql "bash"
