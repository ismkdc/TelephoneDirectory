FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TelephoneDirectory.WebAPI/TelephoneDirectory.WebAPI.csproj", "TelephoneDirectory.WebAPI/"]
RUN dotnet restore "TelephoneDirectory.WebAPI/TelephoneDirectory.WebAPI.csproj"
COPY . .
WORKDIR "/src/TelephoneDirectory.WebAPI"
RUN dotnet build "TelephoneDirectory.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelephoneDirectory.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelephoneDirectory.WebAPI.dll"]
