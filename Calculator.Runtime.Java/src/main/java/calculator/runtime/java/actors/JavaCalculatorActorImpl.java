package calculator.runtime.java.actors;

import calculator.runtime.java.models.Calculation;
import io.dapr.actors.ActorId;
import io.dapr.actors.runtime.AbstractActor;
import io.dapr.actors.runtime.ActorRuntimeContext;
import reactor.core.publisher.Mono;

public class JavaCalculatorActorImpl extends AbstractActor implements JavaCalculatorActor {

    public JavaCalculatorActorImpl(ActorRuntimeContext runtimeContext, ActorId id) {
        super(runtimeContext, id);
    }

    @Override
    public Mono<Object> execute(Calculation calculation) {
        return Mono.just(1);
    }
}
