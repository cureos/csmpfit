/* 
 * MINPACK-1 Least Squares Fitting Library
 *
 * Original public domain version by B. Garbow, K. Hillstrom, J. More'
 *   (Argonne National Laboratory, MINPACK project, March 1980)
 * See the file DISCLAIMER for copyright information.
 * 
 * Tranlation to C Language by S. Moshier (moshier.net)
 * Translation to C# Language by D. Cuccia (http://davidcuccia.wordpress.com)
 * 
 * Enhancements and packaging by C. Markwardt
 *   (comparable to IDL fitting routine MPFIT
 *    see http://cow.physics.wisc.edu/~craigm/idl/idl.html)
 */

/* Helper class for strongly-typed IList<T> subarray. Allows indexing into 
 * larger parent array for safety (no pointers or unsafe compiler flags) 
   $Id: DelimitedArrayOfDouble.cs,v 1.1 2013/06/27 dcuccia Exp $
 */

using System;
using System.Collections;

namespace MPFitLib
{
    /// <summary>
    /// Class that represents a "sub-array" within a larger array by implementing
    /// appropriate indexing using an offset and sub-count. This was implemented in
    /// the C# version in order to preserve the existing code semantics while also
    /// allowing the code to be compiled w/o use of /unsafe compilation flag. This
    /// permits execution of the code in low-trust environments, such as that required
    /// by the CoreCLR runtime of Silverlight (Mac/PC) and Moonlight (Linux)
    /// </summary>
    /// <remarks>Note - modifications to this structure will modify the parent (source) array!</remarks>
    public class DelimitedArrayOfDouble
    {
        private int _offset;
        private int _count;
        private double[] _array;

        public DelimitedArrayOfDouble(double[] array, int offset, int count)
        {
            _array = array;
            _offset = offset;
            _count = count;
        }

        public void SetOffset(int offset)
        {
            if (offset + _count > _array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            _offset = offset;
        }

        public void SetOffsetAndCount(int offset, int count)
        {
            if (offset + count > _array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            _offset = offset;
            _count = count;
        }

        public double this[int index]
        {
            get
            {
                return _array[_offset + index];
            }
            set
            {
                _array[_offset + index] = value;
            }
        }
        
        public int Count
        {
            get { return _count; }
        }
        
        public int IndexOf(double item)
        {
            int start = _offset;
            int stop = _offset + _count;
            for (int i = start; i < stop; i++)
            {
                if (_array[i] == item)
                    return i - _offset;
            }

            return -1;
        }
        
        public void CopyTo(double[] array, int arrayIndex)
        {
            Array.Copy(_array, _offset, array, arrayIndex, _count);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
    }
}