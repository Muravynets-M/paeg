FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PAEG.Client/PAEG.Client.csproj", "PAEG.Client/"]
RUN dotnet restore "PAEG.Client/PAEG.Client.csproj"
COPY . .
WORKDIR "/src/PAEG.Client"
RUN dotnet build "PAEG.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PAEG.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PAEG.Client.dll"]
