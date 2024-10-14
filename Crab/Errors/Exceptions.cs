namespace Crab.Errors;

/// <summary>
/// The exception that is thrown when an attempt is made to unwrap a value that
/// is not present, or when an attempt is made to unwrap a value that is an
/// error.
/// </summary>
/// <param name="message">The message to include in the exception.</param>
public class UnwrapException(string message) : Exception(message);

/// <summary>
/// The exception that is thrown when an attempt to lock a mutex fails.
/// </summary>
public class TryLockException(string message) : Exception(message);

/// <summary>
/// The exception that is thrown when locking the mutex would block.
/// </summary>
public class WouldBlockException() : TryLockException("The operation would block.");