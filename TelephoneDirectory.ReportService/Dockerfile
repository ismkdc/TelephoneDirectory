FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TelephoneDirectory.ReportService/TelephoneDirectory.ReportService.csproj", "TelephoneDirectory.ReportService/"]
RUN dotnet restore "TelephoneDirectory.ReportService/TelephoneDirectory.ReportService.csproj"
COPY . .
WORKDIR "/src/TelephoneDirectory.ReportService"
RUN dotnet build "TelephoneDirectory.ReportService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelephoneDirectory.ReportService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelephoneDirectory.ReportService.dll"]
