package calculator.runtime.java.actors;

import calculator.runtime.java.models.Calculation;
import io.dapr.actors.ActorMethod;
import io.dapr.actors.ActorType;
import reactor.core.publisher.Mono;

@ActorType(name = "CalculatorActor")
public interface CalculatorActor {
    @ActorMethod(name = "Execute", returns = Object.class)
    Mono<Object> execute(Calculation calculation);
}
