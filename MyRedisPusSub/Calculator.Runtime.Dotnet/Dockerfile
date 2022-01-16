
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Calculator.Runtime.Dotnet.csproj", "."]
RUN dotnet restore "Calculator.Runtime.Dotnet.csproj"
COPY . .

WORKDIR "/src"
RUN dotnet build "Calculator.Runtime.Dotnet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Calculator.Runtime.Dotnet.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Calculator.Runtime.Dotnet.dll"]


