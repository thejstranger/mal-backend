
FROM microsoft/dotnet:sdk as core_build

WORKDIR '/app'

ENV ASPNETCORE_URLS=http://*:4321
EXPOSE 4321

COPY ./ ./

RUN dotnet publish MAL.WebAPI/MAL.WebAPI.csproj -o /app/publishedBuild/Debug -c Debug -r linux-x64

FROM microsoft/dotnet:aspnetcore-runtime

WORKDIR '/app'

COPY --from=core_build /app/publishedBuild/Debug /app

ENTRYPOINT ["sh", "-c"]
CMD ["/app/MAL.WebAPI"]