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

namespace MPFitLib
{
    /// <summary>
    /// Definition of MPFIT configuration structure
    /// </summary>
    /// <remarks>
    /// NOTE: the user may set the values explicitly; OR, if a passed
    /// value is zero, then the "Default" value will be substituted by
    /// mpfit().
    /// </remarks>
    public class mp_config
    {
        /// <summary>
        /// Relative chi-square convergence criterion Default: 1e-10
        /// </summary>
        public double ftol = 1e-10;

        /// <summary>
        /// Relative parameter convergence criterion Default: 1e-10
        /// </summary>
        public double xtol = 1e-10;

        /// <summary>
        /// Orthogonality convergence criterion Default: 1e-10
        /// </summary>
        public double gtol = 1e-10;

        /// <summary>
        /// Finite derivative step size Default: MP_MACHEP0
        /// </summary>
        public double epsfcn = MPFit.MP_MACHEP0;

        /// <summary>
        /// Initial step bound Default: 100.0
        /// </summary>
        public double stepfactor = 100.0;

        /// <summary>
        /// Range tolerance for covariance calculation Default: 1e-14
        /// </summary>
        public double covtol = 1e-14;

        /// <summary>
        /// Maximum number of iterations.  If maxiter == MP_NO_ITER,
        /// then basic error checking is done, and parameter
        /// errors/covariances are estimated based on input
        /// parameter values, but no fitting iterations are done.
        /// Default: 200
        /// </summary>
        public int maxiter = 200;

        /// <summary>
        /// Maximum number of function evaluations, or 0 for no limit
        /// Default: 0 (no limit)
        /// </summary>
        public int maxfev = 0;

        /// <summary>
        /// Print level Default: 1
        /// </summary>
        public int nprint = 1;

        /// <summary>
        /// Scale variables by user values?
        /// 1 = yes, user scale values in diag;
        /// 0 = no, variables scaled internally(Default)
        /// </summary>
        public int douserscale = 0;

        /// <summary>
        /// Disable check for infinite quantities from user?
        /// 0 = do not perform check(Default)
        /// 1 = perform check
        /// </summary>
        public int nofinitecheck = 0;
    }
}
