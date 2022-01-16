package calculator.runtime.java;

import calculator.runtime.java.actors.CalculatorActorImpl;
import io.dapr.actors.runtime.ActorRuntime;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class Application {

    public static void main(String[] args) {
        ActorRuntime.getInstance().registerActor(CalculatorActorImpl.class);
        SpringApplication.run(Application.class, args);
    }

}
