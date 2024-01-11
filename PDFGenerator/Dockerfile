FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PDFGenerator/PDFGenerator.csproj", "PDFGenerator/"]
COPY ["Model/Model.csproj", "Model/"]

RUN dotnet restore "PDFGenerator/PDFGenerator.csproj"
COPY . .
WORKDIR "/src/PDFGenerator"
RUN dotnet build "PDFGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PDFGenerator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PDFGenerator.dll"]
