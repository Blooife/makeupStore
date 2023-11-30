FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7004

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["makeupStore.Services.OrderAPI/makeupStore.Services.OrderAPI.csproj", "makeupStore.Services.OrderAPI/"]
RUN dotnet restore "makeupStore.Services.OrderAPI/makeupStore.Services.OrderAPI.csproj"
COPY . .
WORKDIR "/src/makeupStore.Services.OrderAPI"
RUN dotnet build "makeupStore.Services.OrderAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "makeupStore.Services.OrderAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "makeupStore.Services.OrderAPI.dll"]
