FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:$PORT

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Exploring.csproj", "./"]
RUN dotnet restore "Exploring.csproj"
COPY . .
RUN dotnet build "Exploring.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exploring.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exploring.dll"]
