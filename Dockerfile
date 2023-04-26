FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Server/Artisan.Server.csproj", "Server/"]
COPY ["Client/Artisan.Client.csproj", "Client/"]
COPY ["Components/Artisan.CommonComponents.csproj", "Components/"]
COPY ["Shared/Artisan.Shared.csproj", "Shared/"]
COPY ["Pages/DiceThrower/Artisan.Pages.DiceThrower.csproj", "Pages/DiceThrower/"]
COPY ["Pages/CharGen/Artisan.Pages.CharGen.csproj", "Pages/CharGen/"]
COPY ["ArkLens/ArkLens.Builders/ArkLens.Builders.csproj", "ArkLens/ArkLens.Builders/"]
COPY ["ArkLens/ArkLens.Entities/ArkLens.Entities.csproj", "ArkLens/ArkLens.Entities/"]
COPY ["ArkLens/ArkLens.Core/ArkLens.Core.csproj", "ArkLens/ArkLens.Core/"]
COPY ["ArkLens/ArkLens.Resources/ArkLens.Resources.csproj", "ArkLens/ArkLens.Resources/"]
COPY ["ArkLens/ArkLens.Snapshots/ArkLens.Snapshots.csproj", "ArkLens/ArkLens.Snapshots/"]
COPY ["Data/Artisan.Data.csproj", "Data/"]
RUN dotnet restore "Server/Artisan.Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Artisan.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Artisan.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Artisan.Server.dll"]
COPY ["Server/artisan.pfx", ""]
