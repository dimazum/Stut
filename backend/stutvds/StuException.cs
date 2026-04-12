using System;

public class StuException : Exception
{
    public int Code { get; }

    public StuException(string message, int code = 0) 
        : base(message)
    {
        Code = code;
    }
    
    public StuException(ErrorDefinition errorDefinition) 
        : base(errorDefinition.Message)
    {
        Code = errorDefinition.Code;
    }
}