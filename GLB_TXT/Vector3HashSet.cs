using System.Numerics;

namespace GLB_TXT
{
    /// <summary>
    /// Represents a HashSet of Vector3 objects.
    /// </summary>
    public class Vector3HashSet : HashSet<Vector3>
    {
        /// <summary>
        /// Gets the index of the specified Vector3 in the HashSet.
        /// </summary>
        /// <param name="vector">The Vector3 to search for.</param>
        /// <returns>The zero-based index of the vector if found; otherwise, -1.</returns>
        public int IndexOf(Vector3 vector)
        {
            int index = 0;

            // Iterate through each Vector3 in the HashSet
            foreach (var item in this)
            {
                // Check if the current Vector3 equals the specified vector
                if (item.Equals(vector))
                {
                    // Return the index if found
                    return index;
                }
                index++;
            }

            // Return -1 if the vector is not found
            return -1;
        }
    }
}