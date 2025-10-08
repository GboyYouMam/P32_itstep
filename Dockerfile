FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "WebApplication1.csproj"
RUN dotnet publish "WebApplication1.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app


ENV ASPNETCORE_URLS=http://*:8080

COPY --from=build /app/publish .

EXPOSE 8080

VOLUME ["/app/notes_data"]

ENV ConnectionStrings__DefaultConnection="Data Source=/app/notes_data/notes.db"

ENTRYPOINT ["dotnet", "WebApplication1.dll"]
