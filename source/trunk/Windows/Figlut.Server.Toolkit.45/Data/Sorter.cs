using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figlut.Server.Toolkit.Data
{
    public static class Sorter
    {
        public static List<int> IntersectSortedIntegers(this List<int> source, List<int> target)
        {
            // Set initial capacity to a "full-intersection" size
            // This prevents multiple re-allocations
            var ints = new List<int>(Math.Min(source.Count, target.Count));

            var i = 0;
            var j = 0;

            while (i < source.Count && j < target.Count)
            {
                // Compare only once and let compiler optimize the switch-case
                switch (source[i].CompareTo(target[j]))
                {
                    case -1:
                        i++;

                        // Saves us a JMP instruction
                        continue;
                    case 1:
                        j++;

                        // Saves us a JMP instruction
                        continue;
                    default:
                        ints.Add(source[i++]);
                        j++;

                        // Saves us a JMP instruction
                        continue;
                }
            }

            // Free unused memory (sets capacity to actual count)
            ints.TrimExcess();

            return ints;
        }
    }
}
