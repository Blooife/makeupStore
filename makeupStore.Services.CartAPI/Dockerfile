FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["makeupStore.Services.CartAPI/makeupStore.Services.CartAPI.csproj", "makeupStore.Services.CartAPI/"]
RUN dotnet restore "makeupStore.Services.CartAPI/makeupStore.Services.CartAPI.csproj"
COPY . .
WORKDIR "/src/makeupStore.Services.CartAPI"
RUN dotnet build "makeupStore.Services.CartAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "makeupStore.Services.CartAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "makeupStore.Services.CartAPI.dll"]
