/* 
 * MINPACK-1 Least Squares Fitting Library
 *
 * Original public domain version by B. Garbow, K. Hillstrom, J. More'
 *   (Argonne National Laboratory, MINPACK project, March 1980)
 * See the file DISCLAIMER for copyright information.
 * 
 * Translation to C Language by S. Moshier (moshier.net)
 * Translation to C# Language by D. Cuccia (http://davidcuccia.wordpress.com)
 * 
 * Enhancements and packaging by C. Markwardt
 *   (comparable to IDL fitting routine MPFIT
 *    see http://cow.physics.wisc.edu/~craigm/idl/idl.html)
 */

/* Main MPFit library routines (double precision) 
   $Id: MPFit.cs,v 1.1 2010/05/04 dcuccia Exp $
   added changes from mpfit.h v1.14 2010/11/13
    and mpfit.c v1.20 2010/11/13
   added changes from mpfit.h v1.16 2016/06/02
    and mpfit.c v1.24 2013/04/23
   added changes from mpfit version 1.4 (no file versions provided)
 */

using System.Collections.Generic;

namespace MPFitLib
{
    /// <summary>
    /// User-function delegate structure required by MPFit.Solve
    /// </summary>
    /// <param name="a">I - Parameters</param>
    /// <param name="fvec">O - function values</param>
    /// <param name="dvec">O - function derivatives (optional)</param>
    /// <param name="prv">I/O - function private data (cast to object type in user function)</param>
    /// <returns>0 if computation successful, non-zero otherwise.</returns>
    public delegate int mp_func(double[] a, double[] fvec, IList<double>[]? dvec, object? prv);

    /// <summary>
    /// User-function delegate structure required by MPFit.Solve.
    /// Supports reference parameters for <paramref name="fvec"/> and <paramref name="dvec"/>.
    /// </summary>
    /// <param name="a">I - Parameters</param>
    /// <param name="fvec">O - function values</param>
    /// <param name="dvec">O - function derivatives (optional)</param>
    /// <param name="prv">I/O - function private data (cast to object type in user function)</param>
    /// <returns>0 if computation successful, non-zero otherwise.</returns>
    public delegate int mp_func_ref(double[] a, ref double[] fvec, ref IList<double>[]? dvec, object? prv);

}
