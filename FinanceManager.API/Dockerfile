FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY FinanceManager.API.csproj /build/
RUN dotnet restore ./build/FinanceManager.API.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./FinanceManager.API.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "FinanceManager.API.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet FinanceManager.API.dll