using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUtils.Math
{
    public class Counting
    {
		// Lazy way to not do a for
        public static IEnumerable<Int32> FromAToB ( Int32 Start, Int32 End )
        {
            if ( Start < End )
            {
                for ( Int32 Iterator = Start ; Iterator < End ; Iterator++ )
                    yield return Iterator;
            }
            else if ( Start > End )
            {
                for ( Int32 Iterator = Start ; Iterator > End ; Iterator-- )
                    yield return Iterator;
            }
            else
                yield return Start;
        }
    }
}
