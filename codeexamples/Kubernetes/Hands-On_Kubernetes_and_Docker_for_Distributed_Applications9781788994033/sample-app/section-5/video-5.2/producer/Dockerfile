FROM gradle:4.10.2-jdk11-slim AS builder
WORKDIR /home/gradle/project
ADD ./*.gradle ./
COPY ./producer.properties ./
COPY ./src ./src
USER root
RUN gradle build 
RUN mkdir -p /app && \
    cp build/libs/app.jar /app/ && \
    cp producer.properties /app/

FROM openjdk:11-jre-slim
COPY --from=builder /home/gradle/project/build/libs/app.jar /app/app.jar
COPY --from=builder /home/gradle/project/producer.properties /app/
CMD java -jar /app/app.jar
