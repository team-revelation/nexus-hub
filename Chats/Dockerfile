FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:$PORT

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Chats.csproj", "./"]
RUN dotnet restore "Chats.csproj"
COPY . .
RUN dotnet build "Chats.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chats.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chats.dll"]
