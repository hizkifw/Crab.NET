namespace Crab.Errors;

public class UnwrapException(string message) : Exception(message);

public class TryLockException(string message) : Exception(message);

public class WouldBlockException() : TryLockException("The operation would block.");