# --- Giai đoạn 1: Build ---
# Sử dụng image .NET SDK để biên dịch ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Sao chép file .csproj và restore các package NuGet
# Sao chép riêng lẻ để tận dụng cache của Docker
COPY *.sln .
COPY SportGo.API/*.csproj ./SportGo.API/
COPY SportGo.Service/*.csproj ./SportGo.Service/
COPY SportGo.Repository/*.csproj ./SportGo.Repository/
# (Thêm các dòng COPY *.csproj khác nếu bạn có thêm project)

RUN dotnet restore "SportGo.API/SportGo.API.csproj"

# Sao chép toàn bộ mã nguồn còn lại
COPY . .

# Publish ứng dụng với cấu hình Release
WORKDIR "/source/SportGo.API"
RUN dotnet publish "SportGo.API.csproj" -c Release -o /app/publish --no-restore

# --- Giai đoạn 2: Final ---
# Sử dụng image ASP.NET Runtime nhỏ gọn hơn để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Sao chép kết quả đã publish từ giai đoạn build
COPY --from=build /app/publish .

# Mở cổng 8080 cho container
EXPOSE 8080

# Lệnh để khởi chạy ứng dụng khi container bắt đầu
ENTRYPOINT ["dotnet", "SportGo.API.dll"]
