#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0
WORKDIR /src
COPY ["TweetBook4/TweetBook4.csproj", "TweetBook4/"]
RUN dotnet restore "TweetBook4/TweetBook4.csproj"
COPY . .
WORKDIR "/src/TweetBook4"
RUN dotnet build "TweetBook4.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TweetBook4.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TweetBook4.dll"]