FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TestProjectRules/TestProjectRules.csproj", "TestProjectRules/"]
RUN dotnet restore "TestProjectRules/TestProjectRules.csproj"
COPY . .
WORKDIR "/src/TestProjectRules"
RUN dotnet build "TestProjectRules.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TestProjectRules.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TestProjectRules.dll"]