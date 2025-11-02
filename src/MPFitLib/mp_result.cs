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

using System;

namespace MPFitLib
{
    /// <summary>
    /// Definition of results structure, for when fit completes
    /// </summary>
    public class mp_result
    {
        /// <summary>
        /// Final chi^2
        /// </summary>
        public double bestnorm;

        /// <summary>
        /// Starting value of chi^2
        /// </summary>
        public double orignorm;

        /// <summary>
        /// Number of iterations
        /// </summary>
        public int niter;

        /// <summary>
        /// Number of function evaluations
        /// </summary>
        public int nfev;

        /// <summary>
        /// Fitting status code
        /// </summary>
        public int status;

        /// <summary>
        /// Total number of parameters
        /// </summary>
        public int npar;

        /// <summary>
        /// Number of free parameters
        /// </summary>
        public int nfree;

        /// <summary>
        /// Number of pegged parameters
        /// </summary>
        public int npegged;

        /// <summary>
        /// Number of residuals (= num. of data points)
        /// </summary>
        public int nfunc;

        /// <summary>
        /// Final residuals
        /// nfunc-vector, or null if not desired
        /// </summary>
        public double[]? resid;

        /// <summary>
        /// Final parameter uncertainties (1-sigma)
        /// npar-vector, or null if not desired
        /// </summary>
        public double[]? xerror;

        /// <summary>
        /// Final parameter covariance matrix
        /// npar x npar array, or null if not desired
        /// </summary>
        public double[]? covar;

        /// <summary>
        /// MPFIT version string
        /// </summary>
        public string? version;

        /// <summary>
        /// Initializes a new instance of the mp_result class with the specified number of parameters.
        /// </summary>
        /// <param name="numParameters">The number of parameters for which to allocate error values.
        /// Must be greater than zero.</param>
        /// <remarks>On exit, <see cref="xerror"/> will be redefined to <c>npar</c> provided in the
        /// <c>Solve</c> method.</remarks>
        public mp_result(int numParameters)
        {
            xerror = new double[numParameters];
        }

        /// <summary>
        /// Initializes a new instance of the mp_result class.
        /// </summary>
        /// <param name="showResid">Should <see cref="resid"/> be provided in <see cref="mp_result"/>
        /// on <c>Solve</c> exit? Default value <c>false</c>.</param>
        /// <param name="showXerror">Should <see cref="xerror"/> be provided in <see cref="mp_result"/>
        /// on <c>Solve</c> exit? Default value <c>false</c>.</param>
        /// <param name="showCovar">Should <see cref="covar"/> be provided in <see cref="mp_result"/>
        /// on <c>Solve</c> exit? Default value <c>false</c>.</param>
        public mp_result(bool showResid = false, bool showXerror = false, bool showCovar = false)
        {
            // Initialized arrays will be allocated by MPFit.Solve() as needed
            if (showResid) resid = Array.Empty<double>();
            if (showXerror) xerror = Array.Empty<double>();
            if (showCovar) covar = Array.Empty<double>();
        }
    }
}
