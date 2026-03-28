namespace stutvds.Common;

public static class ErrorCodes
{
    public static readonly ErrorDefinition Unauthorized = new(1, "User is not authorized");
    public static readonly ErrorDefinition MeUnauthorized = new(2, "Session expired. Please log in again");
    public static readonly ErrorDefinition NotFound = new(3, "Object not found");
    public static readonly ErrorDefinition AlreadyExist = new(4, "Object already exists");
}