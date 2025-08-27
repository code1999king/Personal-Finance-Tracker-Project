

namespace BusinessLogic
{
    /// <summary>
    /// Indicates to object's save mode. 
    /// If its value is AddNew, then the save operation adds new record to database.
    /// If the value is Update, then the save operation updates existing record in database.
    /// </summary>
    internal enum BllSaveMode
    {
        AddNew,
        Update
    }
}
