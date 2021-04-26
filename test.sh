#!/bin/sh


rm -rf Tests/Ckode.Dapper.Repository.IntegrationTests/TestResults
rm -rf Tests/Ckode.Dapper.Repository.UnitTests/TestResults
dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings
