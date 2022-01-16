FROM adoptopenjdk/openjdk16:alpine AS build
WORKDIR ./gradlew
COPY ./ .
RUN ./gradlew -x test build

FROM adoptopenjdk/openjdk16:alpine-jre
ENV USER=CalculatorRuntimeJava
WORKDIR /$USER

RUN addgroup -g 1001 -S $USER && \
    adduser -S -D -H -u 1001 -h /$USER -s /sbin/nologin -G $USER -g $USER $USER && \
    chown -R 1001:0 /$USER && \
    chmod -R g+w /$USER

USER $USER

COPY --from=build ./gradlew/build/libs/${USER}.jar ./

ENTRYPOINT ["sh", "-c", "\
java \
--add-opens java.base/jdk.internal.misc=ALL-UNNAMED -Dio.netty.tryReflectionSetAccessible=true \
-jar ${USER}.jar"]
