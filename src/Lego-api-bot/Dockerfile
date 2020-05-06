FROM mcr.microsoft.com/dotnet/core/runtime:3.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/Lego-api-bot/Lego-api-bot.csproj", "src/Lego-api-bot/"]
COPY ["src/Lego-api-data/Lego-api-data.csproj", "src/Lego-api-data/"]
RUN dotnet restore "src/Lego-api-bot/Lego-api-bot.csproj"
COPY . .
WORKDIR "/src/src/Lego-api-bot"
RUN dotnet build "Lego-api-bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lego-api-bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lego-api-bot.dll"]