FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:$PORT

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Websockets.csproj", "./"]
RUN dotnet restore "Websockets.csproj"
COPY . .
RUN dotnet build "Websockets.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Websockets.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Websockets.dll"]
