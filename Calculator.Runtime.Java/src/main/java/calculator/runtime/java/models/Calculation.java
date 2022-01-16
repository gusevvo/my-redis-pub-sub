package calculator.runtime.java.models;

import java.util.List;

public record Calculation(String Expression, List<Parameter> Parameters) { }