namespace SolutionSetup
{
    /// <summary>
    /// Outcome of a garage operation: whether it succeeded and a message that can be shown to the user.
    /// </summary>
    /// <param name="Success">True if the operation succeeded.</param>
    /// <param name="Message">Confirmation on success, or the reason on failure.</param>
    public sealed record OperationResult(bool Success, string Message)
    {
        /// <summary>Creates a successful result with the given message.</summary>
        public static OperationResult Ok(string message) => new(true, message);

        /// <summary>Creates a failed result with the given reason.</summary>
        public static OperationResult Fail(string message) => new(false, message);
    }
}
