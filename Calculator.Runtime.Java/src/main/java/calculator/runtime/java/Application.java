package calculator.runtime.java;

import calculator.runtime.java.actors.JavaCalculatorActorImpl;
import io.dapr.actors.runtime.ActorRuntime;
import lombok.extern.slf4j.Slf4j;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@Slf4j
@SpringBootApplication
public class Application {

    public static void main(String[] args) {
        ActorRuntime.getInstance().registerActor(JavaCalculatorActorImpl.class);

        log.info("We are here");
        try {
            SpringApplication.run(Application.class, args);
        } catch (Exception ex) {
            log.error("Something gone wrong:", ex);
        }
    }

}
