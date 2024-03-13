namespace LispInterpreter.AST;

public class SyntaxException(string message) 
    : Exception(message: message);