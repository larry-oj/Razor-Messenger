# docker build -t razormessenger:1.0 -f .\Razor-Messenger\Dockerfile .
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5030
EXPOSE 5031

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN dotnet dev-certs https
WORKDIR /src
COPY ["../Razor-Messenger/Razor-Messenger.csproj", "Razor-Messenger/"]
COPY ["../Razor-Messenger.Data/Razor-Messenger.Data.csproj", "Razor-Messenger.Data/"]
COPY ["../Razor-Messenger.Services/Razor-Messenger.Services.csproj", "Razor-Messenger.Services/"]
RUN dotnet restore "Razor-Messenger/Razor-Messenger.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Razor-Messenger/Razor-Messenger.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Razor-Messenger/Razor-Messenger.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Razor-Messenger.dll", "--urls", "http://0.0.0.0:5030;https://0.0.0.0:5031"]
