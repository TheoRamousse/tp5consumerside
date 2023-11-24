# Étape de construction pour .NET
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-dotnet
WORKDIR /app-dotnet

ARG src="./MqConsumer/."
# Copie des fichiers du projet .NET dans le conteneur
COPY ${src} .

# Construction de l'application .NET
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Utilisation de l'image d'exécution .NET Core pour l'exécution de l'application .NET et de Nginx pour servir l'application Angular
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

# Copie des fichiers publiés de .NET dans l'image d'exécution
COPY --from=build-dotnet /app-dotnet/out .

# Commande pour démarrer l'application .NET et Nginx
CMD dotnet "MqConsumer.dll"