FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/FileStorage.Api/FileStorage.Api.csproj", "src/FileStorage.Api/"]
RUN dotnet restore "src/FileStorage.Api/FileStorage.Api.csproj"
COPY . .
WORKDIR "/src/src/FileStorage.Api"
RUN dotnet build "FileStorage.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FileStorage.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileStorage.Api.dll"]