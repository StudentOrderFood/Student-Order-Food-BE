FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 80
EXPOSE 443
EXPOSE 8080 

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderFood_BE.WebAPI/OrderFood_BE.WebAPI.csproj", "OrderFood_BE.WebAPI/"]
COPY ["OrderFood_BE.Application/OrderFood_BE.Application.csproj", "OrderFood_BE.Application/"]
COPY ["OrderFood_BE.Domain/OrderFood_BE.Domain.csproj", "OrderFood_BE.Domain/"]
COPY ["OrderFood_BE.Infrastructure/OrderFood_BE.Infrastructure.csproj", "OrderFood_BE.Infrastructure/"]
COPY ["OrderFood_BE.Shared/OrderFood_BE.Shared.csproj", "OrderFood_BE.Shared/"]
COPY ["OrderFood.BackgroundServices/OrderFood.BackgroundServices.csproj", "OrderFood.BackgroundServices/"]

RUN dotnet restore "OrderFood_BE.WebAPI/OrderFood_BE.WebAPI.csproj"

COPY . .

RUN dotnet build "OrderFood_BE.WebAPI/OrderFood_BE.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderFood_BE.WebAPI/OrderFood_BE.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderFood_BE.WebAPI.dll"]
