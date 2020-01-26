FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY KickAss-Music-Player/* project/KickAss-Music-Player/
COPY KickAss-Music-Player-DB/* project/KickAss-Music-Player-DB/
COPY KickAss-Music-Player.BusinessLogic/* project/KickAss-Music-Player.BusinessLogic/
COPY KickAss-Music-Player.BusinessLogicTests/* project/KickAss-Music-Player.BusinessLogicTests/
COPY KickAss-Music-Player.DataModels/* project/KickAss-Music-Player.DataModels/
COPY KickAss-Music-Player.Test/* project/KickAss-Music-Player.Test/
COPY KickAss-Music-PlayerTests/* project/KickAss-Music-PlayerTests/
WORKDIR /src/project
RUN dotnet restore "KickAss-Music-Player/KickAss-Music-Player.csproj"
RUN dotnet build "KickAss-Music-Player/KickAss-Music-Player.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KickAss-Music-Player/KickAss-Music-Player.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KickAss-Music-Player.dll"]