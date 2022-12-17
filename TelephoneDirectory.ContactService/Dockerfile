FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TelephoneDirectory.ContactService/TelephoneDirectory.ContactService.csproj", "TelephoneDirectory.ContactService/"]
RUN dotnet restore "TelephoneDirectory.ContactService/TelephoneDirectory.ContactService.csproj"
COPY . .
WORKDIR "/src/TelephoneDirectory.ContactService"
RUN dotnet build "TelephoneDirectory.ContactService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelephoneDirectory.ContactService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelephoneDirectory.ContactService.dll"]
