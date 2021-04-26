#!/bin/sh

dotnet test --collect:"XPlat Code Coverage" --settings coverlet.runsettings
