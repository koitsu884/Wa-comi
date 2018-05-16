FROM microsoft/aspnetcore-build:latest
RUN apt-get update
RUN apt-get install -y libgdiplus
RUN apt-get install -y libc6-dev

COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
EXPOSE 80/tcp
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh