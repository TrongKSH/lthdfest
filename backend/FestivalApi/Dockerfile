# Build and run ASP.NET Core 8 on Render (Docker Web Service).
# Render does not offer a native .NET runtime; Docker is required.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FestivalApi.csproj", "./"]
RUN dotnet restore "FestivalApi.csproj"

COPY . .
RUN dotnet publish "FestivalApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

RUN addgroup --system --gid 1001 appgroup \
    && adduser --system --uid 1001 --ingroup appgroup appuser
USER appuser

# Render injects PORT; app must listen on 0.0.0.0 (not localhost).
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
EXPOSE 8080
CMD ["sh", "-c", "exec dotnet FestivalApi.dll --urls \"http://0.0.0.0:${PORT:-8080}\""]
