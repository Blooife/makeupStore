FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["makeUp.Services.AuthAPI/makeUp.Services.AuthAPI.csproj", "makeUp.Services.AuthAPI/"]
RUN dotnet restore "makeUp.Services.AuthAPI/makeUp.Services.AuthAPI.csproj"
COPY . .
WORKDIR "/src/makeUp.Services.AuthAPI"
RUN dotnet build "makeUp.Services.AuthAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "makeUp.Services.AuthAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "makeUp.Services.AuthAPI.dll"]
