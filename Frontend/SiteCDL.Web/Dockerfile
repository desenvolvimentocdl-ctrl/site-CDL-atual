#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG TARGETARCH
#WORKDIR /src
#COPY ["Backend/SiteCDL.Application/SiteCDL.Application.csproj", "Backend/SiteCDL.Application/"]
#COPY ["Backend/SiteCDL.Domain/SiteCDL.Domain.csproj", "Backend/SiteCDL.Domain/"]
#COPY ["Backend/SiteCDL.Infrastructure/SiteCDL.Infrastructure.csproj", "Backend/SiteCDL.Infrastructure/"]
#COPY ["Frontend/SiteCDL.Web/SiteCDL.Web.csproj", "Frontend/SiteCDL.Web/"]
#COPY ["Tests/SiteCDL.Tests/SiteCDL.Tests.csproj", "Tests/SiteCDL.Tests/"]
#COPY ["Backend/SiteCDL.API/SiteCDL.API.csproj", "Backend/SiteCDL.API/"]
#RUN dotnet restore "Frontend/SiteCDL.Web/SiteCDL.Web.csproj" -a $TARGETARCH
#COPY . .
#WORKDIR "/src/Frontend/SiteCDL.Web"
#RUN dotnet build "SiteCDL.Web.csproj" -c Release -o /app/build -a $TARGETARCH
#
#FROM build AS publish
#RUN apt-get update -yq \
    #&& apt-get install -yq ca-certificates curl gnupg \
    #&& mkdir -p /etc/apt/keyrings \
    #&& curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key | gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg \
    #&& echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_18.x nodistro main" | tee /etc/apt/sources.list.d/nodesource.list \
    #&& apt-get update -yq \
    #&& apt-get install nodejs -yq
#RUN dotnet publish "SiteCDL.Web.csproj" -c Release -o /app/publish -a $TARGETARCH
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "SiteCDL.Web.dll"]
#