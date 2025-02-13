##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
##Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
##For more information, please see https://aka.ms/containercompat
#
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["TodoApi.csproj", "."]
#RUN dotnet restore "./TodoApi.csproj"
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "TodoApi.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "TodoApi.dll"]
# Base image
# Base image
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
## Build image
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["server/TodoApi.csproj", "server/"]  # מעתיקים את קובץ ה-csproj מתוך תיקיית server
#RUN dotnet restore "server/TodoApi.csproj"
#COPY ./server /src/server
  ## מעתיקים את כל התוכן מתוך תיקיית server
#
## Build the app
#WORKDIR "/src/."
#RUN dotnet build "server/TodoApi.csproj" -c Release -o /app/build
#
## Publish the app
#FROM build AS publish
#RUN dotnet publish "server/TodoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
## Final image
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "TodoApi.dll"]
#
#
#FROM mcr.microsoft.com/dotnet/aspnet:7.0
#WORKDIR /app
#COPY . .
#ENTRYPOINT ["dotnet", "TodoApi.dll"]

#FROM mcr.microsoft.com/dotnet/aspnet:7.0
#WORKDIR /app
#COPY ./server/bin/Debug/net7.0/TodoApi.dll /app/TodoApi.dll
#ENTRYPOINT ["dotnet", "TodoApi.dll"]

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TodoApi.csproj", "./"]
RUN dotnet restore "TodoApi.csproj"
COPY . . 
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]


