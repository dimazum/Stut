public class ErrorDefinition
{
    public int Code { get; }
    public string Message { get; }

    public ErrorDefinition(int code, string message)
    {
        Code = code;
        Message = message;
    }
}