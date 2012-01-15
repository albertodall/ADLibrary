using System;
using System.Collections.Generic;

namespace AD.Math.Combinatoric
{
    /// <summary>
    /// Interface for Permutations, Combinations and any other classes that present
    /// a collection of collections based on an input collection.  The enumerators that 
    /// this class inherits defines the mechanism for enumerating through the collections.  
    /// </summary>
    /// <typeparam name="T">The of the elements in the collection, not the type of the collection.</typeparam>
    public interface ICombinatoricResult<T> : IEnumerable<IList<T>>
    {
        /// <summary>
        /// The count of items in the collection.  This is not inherited from
        /// ICollection since this meta-collection cannot be extended by users.
        /// </summary>
        long Count { get; }

        /// <summary>
        /// The type of the result, determining how the collections are 
        /// determined from the inputs.
        /// </summary>
        GenerateOption Type { get; }

        /// <summary>
        /// The size of the input collection.
        /// </summary>
        int NElements { get; }

        /// <summary>
        /// The size of each output collection.
        /// </summary>
        int ClassK { get; }
    }
}
