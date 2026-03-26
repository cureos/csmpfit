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
    /// Definition of a parameter constraint structure
    /// </summary>
    public class mp_par
    {
        /// <summary>
        /// 1 = fixed; 0 = free
        /// </summary>
        public int isFixed;

        /// <summary>
        /// 1 = low/upper limit; 0 = no limit
        /// </summary>
        public int[] limited = new int[2];

        /// <summary>
        /// Lower/upper limit boundary value
        /// </summary>
        public double[] limits = new double[2];

        /// <summary>
        /// Name of parameter, or <see langword="null"/> for none
        /// </summary>
        public string? parname;

        /// <summary>
        /// Step size for finite difference
        /// </summary>
        public double step;

        /// <summary>
        /// Relative step size for finite difference
        /// </summary>
        public double relstep;

        /// <summary>
        /// Sidedness of finite difference derivative:
        /// <list type="bullet">
        /// <item><description>0 - one-sided derivative computed automatically</description></item>
        /// <item><description>1 - one-sided derivative (f(x+h) - f(x)  )/h</description></item>
        /// <item><description>-1 - one-sided derivative (f(x)   - f(x-h))/h</description></item>
        /// <item><description>2 - two-sided derivative (f(x+h) - f(x-h))/(2*h)</description></item>
        /// <item><description>3 - user-computed analytical derivatives</description></item>
        /// </list>
        /// </summary>
        public int side;

        /// <summary>
        /// Derivative debug mode: 1 = Yes; 0 = No.
        /// </summary>
        /// <remarks>
        /// If yes, compute both analytical and numerical derivatives and print them to the console for
        /// comparison.
        /// <para>
        /// NOTE: when debugging, do <em>not</em> set <see cref="side"/> = 3, but rather to the kind of
        /// numerical derivative you want to compare the user-analytical one to (0, 1, -1, or 2).
        /// </para>
        /// </remarks>
        public int deriv_debug;

        /// <summary>
        /// Relative tolerance for derivative debug printout
        /// </summary>
        public double deriv_reltol;

        /// <summary>
        /// Absolute tolerance for derivative debug printout
        /// </summary>
        public double deriv_abstol;
    }
}
