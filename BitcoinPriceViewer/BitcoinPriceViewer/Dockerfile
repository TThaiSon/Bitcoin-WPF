# Stage 1: Build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:6.0-windowsservercore-ltsc2022
WORKDIR /app

# Sao chép và khôi phục các package
COPY *.csproj ./
RUN dotnet restore

# Sao chép toàn bộ và build ứng dụng
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/runtime:6.0.33-windowsservercore-ltsc2022
WORKDIR /app
COPY --from=build-env /app/out ./

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "BitcoinPriceViewer.dll"]
