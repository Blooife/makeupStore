FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["makeupStore.Services.StorageAPI/makeupStore.Services.StorageAPI.csproj", "makeupStore.Services.StorageAPI/"]
RUN dotnet restore "makeupStore.Services.StorageAPI/makeupStore.Services.StorageAPI.csproj"
COPY . .
WORKDIR "/src/makeupStore.Services.StorageAPI"
RUN dotnet build "makeupStore.Services.StorageAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "makeupStore.Services.StorageAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "makeupStore.Services.StorageAPI.dll"]
