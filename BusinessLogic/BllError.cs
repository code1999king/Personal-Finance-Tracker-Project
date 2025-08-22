

namespace BusinessLogic
{
    /// <summary>
    /// Represents error codes returned by Business Logic Layer (BLL)
    /// </summary>
    public enum BllError
    {
        Error,

        // User errors:
        InvalidUsername,
        TooLongPassword,
        TooShortPassword,
        WrongUsernameOrPassword,
    }
}
