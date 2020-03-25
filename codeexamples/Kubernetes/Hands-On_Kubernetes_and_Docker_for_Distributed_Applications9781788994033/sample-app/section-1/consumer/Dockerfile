FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY app.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o output

FROM microsoft/dotnet:2.1-runtime
COPY --from=builder /app/output .
ENTRYPOINT ["dotnet", "app.dll"]