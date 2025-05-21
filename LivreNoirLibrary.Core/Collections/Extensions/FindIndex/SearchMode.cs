
namespace LivreNoirLibrary.Collections
{
    /// <summary>
    /// Specifies the type of search for the FindIndex methods.
    /// </summary>
    public enum SearchMode
    {
        /// <summary>
        /// Matches the exact same value to the given.
        /// </summary>
        Equal,
        /// <summary>
        /// Matches the largest value less than the given (except the equal value). 
        /// </summary>
        Previous,
        /// <summary>
        /// Matches the largest value less or equal than the given.
        /// </summary>
        PreviousOrEqual,
        /// <summary>
        /// Matches the smallest value greater than the given (except the equal value). 
        /// </summary>
        Next,
        /// <summary>
        /// Matches the smallest value greater or equal than the given. 
        /// </summary>
        NextOrEqual,
    }
}
