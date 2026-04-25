namespace stutvds;

public static class ErrorCodes
{
    public static readonly ErrorDefinition Unauthorized = new(1, "User is not authorized");
    public static readonly ErrorDefinition MeUnauthorized = new(2, "Session expired. Please log in again");
    public static readonly ErrorDefinition NotFound = new(3, "Object not found");
    public static readonly ErrorDefinition AlreadyExist = new(4, "Object already exists");
    public static readonly ErrorDefinition ValidationError = new(5, "Validation error");
    public static readonly ErrorDefinition AlreadyLearner = new(6, "You already learner");


}