FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["KickAss-Music-Player/KickAss-Music-Player.csproj", "KickAss-Music-Player/"]
RUN dotnet restore "KickAss-Music-Player/KickAss-Music-Player.csproj"
COPY . .
WORKDIR "/src/KickAss-Music-Player"
RUN dotnet build "KickAss-Music-Player.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KickAss-Music-Player.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KickAss-Music-Player.dll"]