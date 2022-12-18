FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TelephoneDirectory.ContactAPI/TelephoneDirectory.ContactAPI.csproj", "TelephoneDirectory.ContactAPI/"]
RUN dotnet restore "TelephoneDirectory.ContactAPI/TelephoneDirectory.ContactAPI.csproj"
COPY . .
WORKDIR "/src/TelephoneDirectory.ContactAPI"
RUN dotnet build "TelephoneDirectory.ContactAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelephoneDirectory.ContactAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelephoneDirectory.ContactAPI.dll"]
