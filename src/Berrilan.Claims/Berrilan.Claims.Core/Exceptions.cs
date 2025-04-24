namespace Berrilan.Claims.Core.Exceptions;

public class UserNotValidException(string? Email) : Exception($"User {Email} not authorized");

public class ItemNotFoundException(string Type, string ObjectId) : Exception($"Item[{ObjectId}] of type[{Type}] not found");

public class CredentialNotValidException(string message) : Exception(message);
