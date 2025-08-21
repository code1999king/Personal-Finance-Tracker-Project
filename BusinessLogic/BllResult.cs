

namespace BusinessLogic
{
    /// <summary>
    /// Represents the result of Business Logic Layer (BLL) operation.
    /// </summary>
    /// <typeparam name="T">Type of returned data</typeparam>
    public class BllResult<T>
    {
        /// <summary>
        /// Indicates whether the operation was successful or not.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Contains the returned data if the operation was successful. This property has a value only if IsSuccess = true
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Represents the error code if the operation was failed due to system error or validation error. This property has a value only if IsSuccess = false
        /// </summary>
        public BllError? Error { get; }

        /// <summary>
        /// BllResult constructor. It is private to ensure that the result object can only be created through the Success or Failure methods.
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="value"></param>
        /// <param name="error"></param>
        private BllResult(bool isSuccess, T value, BllError? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        /// <summary>
        /// Creates a successful result object with the specified value.
        /// </summary>
        /// <param name="value">The value to include in the successful result.</param>
        /// <returns>A <see cref="BllResult{T}"/> object represents a successful operation, and contains the specified value.</returns>
        public static BllResult<T> Success(T value)
        {
            return new BllResult<T>(true, value, null);
        }

        /// <summary>
        /// Creates a failed result object with the specified error.
        /// </summary>
        /// <param name="error">The error code to include in the failed result.</param>
        /// <returns>A <see cref="BllResult{T}"/> object represents a failure operation, and contains the specified error. If the error was not specified, the default error is 'BllError.Error'</returns>
        public static BllResult<T> Failure(BllError error = BllError.Error)
        {
            return new BllResult<T>(false, default(T), error);
        }
    }
}
