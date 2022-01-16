package calculator.runtime.java.serialization;

import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.core.JsonGenerator;
import com.fasterxml.jackson.core.json.JsonReadFeature;
import com.fasterxml.jackson.core.json.JsonWriteFeature;
import com.fasterxml.jackson.databind.*;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import io.dapr.client.ObjectSerializer;
import io.dapr.client.domain.CloudEvent;
import io.dapr.serializer.DaprObjectSerializer;
import io.dapr.utils.TypeRef;

import java.io.IOException;
import java.time.OffsetDateTime;
import java.time.ZoneId;
import java.time.format.DateTimeFormatter;

public class ISO8601DaprObjectSerializer extends ObjectSerializer implements DaprObjectSerializer {

    private static final DateTimeFormatter ISO8601 = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss.SSSX")
            .withZone(ZoneId.of("UTC"));

    private static final JavaTimeModule TIME_MODULE = new JavaTimeModule();
    private static final ObjectMapper OBJECT_MAPPER = new ObjectMapper()
            .disable(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES)
            .enable(DeserializationFeature.READ_ENUMS_USING_TO_STRING)
            .enable(SerializationFeature.WRITE_ENUMS_USING_TO_STRING)
            .enable(MapperFeature.ACCEPT_CASE_INSENSITIVE_PROPERTIES)
            .enable(MapperFeature.ACCEPT_CASE_INSENSITIVE_ENUMS)
            .setSerializationInclusion(JsonInclude.Include.NON_NULL)
            .enable(JsonReadFeature.ALLOW_NON_NUMERIC_NUMBERS.mappedFeature())
            .disable(JsonWriteFeature.WRITE_NAN_AS_STRINGS.mappedFeature())
            .disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS)
            .registerModule(TIME_MODULE);

    static  {
        TIME_MODULE.addSerializer(OffsetDateTime.class, new JsonSerializer<>() {
            @Override
            public void serialize(OffsetDateTime offsetDateTime, JsonGenerator jsonGenerator, SerializerProvider serializerProvider) throws IOException {
                jsonGenerator.writeString(offsetDateTime.format(ISO8601));
            }

        });
    }

    public ISO8601DaprObjectSerializer() {
        super();
    }

    @Override
    public byte[] serialize(Object state) throws IOException {
        if (state == null) {
            return null;
        }

        if (state.getClass().equals(Void.class)) {
            return null;
        }

        // Have this check here to be consistent with deserialization (see deserialize() method below).
        if (state instanceof byte[]) {
            return (byte[]) state;
        }

        // Not string, not primitive, so it is a complex type: we use JSON for that.
        return OBJECT_MAPPER.writeValueAsBytes(state);
    }

    @Override
    public <T> T deserialize(byte[] content, TypeRef<T> type) throws IOException {
        var javaType = OBJECT_MAPPER.constructType(type.getType());

        if ((javaType == null) || javaType.isTypeOrSubTypeOf(Void.class)) {
            return null;
        }

        if (javaType.isPrimitive()) {
            return deserializePrimitives(content, javaType);
        }

        if (content == null) {
            return (T) null;
        }

        // Deserialization of GRPC response fails without this check since it does not come as base64 encoded byte[].
        if (javaType.hasRawClass(byte[].class)) {
            return (T) content;
        }

        if (content.length == 0) {
            return (T) null;
        }

        if (javaType.hasRawClass(CloudEvent.class)) {
            return (T) CloudEvent.deserialize(content);
        }

        return OBJECT_MAPPER.readValue(content, javaType);
    }

    @Override
    public String getContentType() {
        return "application/json";
    }

    /**
     * Parses a given String to the corresponding object defined by class.
     *
     * @param content  Value to be parsed.
     * @param javaType Type of the expected result type.
     * @param <T>      Result type.
     * @return Result as corresponding type.
     * @throws IOException if cannot deserialize primitive time.
     */
    private static <T> T deserializePrimitives(byte[] content, JavaType javaType) throws IOException {
        if ((content == null) || (content.length == 0)) {
            if (javaType.hasRawClass(boolean.class)) {
                return (T) Boolean.FALSE;
            }

            if (javaType.hasRawClass(byte.class)) {
                return (T) Byte.valueOf((byte) 0);
            }

            if (javaType.hasRawClass(short.class)) {
                return (T) Short.valueOf((short) 0);
            }

            if (javaType.hasRawClass(int.class)) {
                return (T) Integer.valueOf(0);
            }

            if (javaType.hasRawClass(long.class)) {
                return (T) Long.valueOf(0L);
            }

            if (javaType.hasRawClass(float.class)) {
                return (T) Float.valueOf(0);
            }

            if (javaType.hasRawClass(double.class)) {
                return (T) Double.valueOf(0);
            }

            if (javaType.hasRawClass(char.class)) {
                return (T) Character.valueOf(Character.MIN_VALUE);
            }

            return null;
        }

        return OBJECT_MAPPER.readValue(content, javaType);
    }

    public static ObjectMapper getMapper() {
        return OBJECT_MAPPER;
    }
}
