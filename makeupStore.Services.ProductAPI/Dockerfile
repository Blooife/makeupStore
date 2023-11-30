FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["makeupStore.Services.ProductAPI/makeupStore.Services.ProductAPI.csproj", "makeupStore.Services.ProductAPI/"]
RUN dotnet restore "makeupStore.Services.ProductAPI/makeupStore.Services.ProductAPI.csproj"
COPY . .
WORKDIR "/src/makeupStore.Services.ProductAPI"
RUN dotnet build "makeupStore.Services.ProductAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "makeupStore.Services.ProductAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "makeupStore.Services.ProductAPI.dll"]
