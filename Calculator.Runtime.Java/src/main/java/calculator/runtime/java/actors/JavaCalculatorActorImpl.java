package calculator.runtime.java.actors;

import calculator.runtime.java.models.Calculation;
import calculator.runtime.java.models.Parameter;
import io.dapr.actors.ActorId;
import io.dapr.actors.runtime.AbstractActor;
import io.dapr.actors.runtime.ActorRuntimeContext;
import lombok.SneakyThrows;
import lombok.extern.slf4j.Slf4j;
import reactor.core.publisher.Mono;
import org.mvel2.MVEL;

import java.time.OffsetDateTime;
import java.util.stream.Collectors;

import static java.lang.String.format;

@Slf4j
public class JavaCalculatorActorImpl extends AbstractActor implements JavaCalculatorActor {

    public JavaCalculatorActorImpl(ActorRuntimeContext runtimeContext, ActorId id) {
        super(runtimeContext, id);
    }

    @Override
    public Mono<Object> execute(Calculation calculation) {
        log.info("Received: {}", calculation);
        log.info("Received: {}", calculation.Parameters());

        var variables = calculation.Parameters()
                .stream()
                .collect(Collectors.toMap(Parameter::Alias, this::parseWithValueType));

        var result = MVEL.eval(calculation.Expression(), variables);

        return Mono.just(result);
    }

    private Object parseWithValueType(Parameter parameter) {
        return switch (parameter.Type()) {
            case Double -> Double.valueOf(parameter.Value());
            case Long -> Long.valueOf(parameter.Value());
            case DateTime -> OffsetDateTime.parse(parameter.Value());
            case Boolean -> parseBooleanExact(parameter.Value());
            case String -> parameter.Value();
        };
    }

    @SneakyThrows
    private Object parseBooleanExact(String value) {
        if (value.equalsIgnoreCase("true"))
            return true;
        if (value.equalsIgnoreCase("false"))
            return false;
        throw new Exception(format("Invalid boolean value: '%s'", value));
    }
}
