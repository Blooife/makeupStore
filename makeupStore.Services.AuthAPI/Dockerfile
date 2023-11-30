FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7002

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["makeupStore.Services.AuthAPI/makeupStore.Services.AuthAPI.csproj", "makeupStore.Services.AuthAPI/"]
RUN dotnet restore "makeupStore.Services.AuthAPI/makeupStore.Services.AuthAPI.csproj"
COPY . .
WORKDIR "/src/makeupStore.Services.AuthAPI"
RUN dotnet build "makeupStore.Services.AuthAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "makeupStore.Services.AuthAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "makeupStore.Services.AuthAPI.dll"]
