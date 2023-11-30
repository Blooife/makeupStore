FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7298

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["makeupStore.Web/makeupStore.Web.csproj", "makeupStore.Web/"]
RUN dotnet restore "makeupStore.Web/makeupStore.Web.csproj"
COPY . .
WORKDIR "/src/makeupStore.Web"
RUN dotnet build "makeupStore.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "makeupStore.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "makeupStore.Web.dll"]
