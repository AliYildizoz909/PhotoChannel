#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
ENV PATH="${PATH}:/root/.dotnet/tools"
WORKDIR /src
RUN dotnet tool install --global dotnet-ef && dotnet tool update --global dotnet-ef && dotnet ef
COPY ./PhotoChannelWebAPI/*.csproj PhotoChannelWebAPI/
COPY ./Business/*.csproj Business/
COPY ./DataAccess/*.csproj DataAccess/
COPY ./Common/*.csproj Common/
COPY ./Entities/*.csproj Entities/
COPY *.sln .
RUN dotnet restore
COPY . .

FROM restore AS migration
WORKDIR /src/DataAccess
RUN dotnet ef --startup-project ../PhotoChannelWebAPI/ migrations add InitialCreate
WORKDIR /src

FROM migration AS build
RUN dotnet build -c Release -o /app/build /p:UseAppHost=false

FROM migration AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN apt-get update 
RUN apt-get --yes install curl
HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --silent --fail http://localhost/health || exit 1
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhotoChannelWebAPI.dll"]