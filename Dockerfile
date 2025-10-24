# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0.415 AS build
WORKDIR /src

# Copiar toda la solución del backend
COPY ./BackCoding-Challenge/ ./BackCoding-Challenge/

# Restaurar dependencias
RUN dotnet restore ./BackCoding-Challenge/src/WebApi/BackCoding.Challenge.WebApi.csproj

# Compilar y publicar
RUN dotnet publish ./BackCoding-Challenge/src/WebApi/BackCoding.Challenge.WebApi.csproj -c Release -o /app/out

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "BackCoding.Challenge.WebApi.dll"]
