FROM mcr.microsoft.com/mssql/server:2019-CU10-ubuntu-20.04

ENV ACCEPT_EULA="Y"
ENV SA_PASSWORD="SqlServer2019"
ENV MSSQL_PID="Express"

ENV PATH=$PATH:/opt/mssql-tools/bin


########################
# Setup Northwind Data #
########################

ARG NORTHWIND_SQL_GITHUB="https://raw.githubusercontent.com/Microsoft/sql-server-samples/master/samples/databases/northwind-pubs/instnwnd.sql"
ARG NORTHWIND_SQL="/usr/share/instnwnd.sql"
RUN wget "${NORTHWIND_SQL_GITHUB}" -O "${NORTHWIND_SQL}"
COPY entrypoint.sh .
ENTRYPOINT ["./entrypoint.sh"]
