/* 
 * MINPACK-1 Least Squares Fitting Library
 *
 * Original public domain version by B. Garbow, K. Hillstrom, J. More'
 *   (Argonne National Laboratory, MINPACK project, March 1980)
 * 
 * Translation to C Language by S. Moshier (moshier.net)
 * Translation to C# Language by D. Cuccia (http://davidcuccia.wordpress.com)
 * 
 * Enhancements and packaging by C. Markwardt
 *   (comparable to IDL fitting routine MPFIT
 *    see http://cow.physics.wisc.edu/~craigm/idl/idl.html)
 */

/* Test routines for MPFit library
   $Id: TestMPFit.cs,v 1.1 2010/05/04 dcuccia Exp $
   added changes from testmpfit.c,v 1.6 2010/11/13
   added changes from testmpfit.c,v 1.7 2012/01/23
   added changes from cmpfit version 1.4 (file version not provided)
*/

using System;
using System.Collections.Generic;

namespace MPFitLib.Test
{
    public static class ForwardModels
    {
        /* 
         * linear fit function
         *
         * m - number of data points
         * n - number of parameters (2)
         * p - array of fit parameters 
         * dy - array of residuals to be returned
         * CustomUserVariable - private data (struct vars_struct *)
         *
         * RETURNS: error code (0 = success)
         */
        public static int LinFunc(double[] p, double[] dy, IList<double>[]? dvec, object? vars)
        {
            int i;
            double[] x, y, ey;
            double f;

            if (vars is not CustomUserVariable v) return 1;

            x = v.X;
            y = v.Y;
            ey = v.Ey;

            for (i = 0; i < dy.Length; i++)
            {
                f = p[0] + p[1] * x[i];     /* Linear fit function; note f = a + b*x */
                dy[i] = (y[i] - f) / ey[i];
            }

            return 0;
        }

        /* 
        * quadratic fit function
        *
        * m - number of data points
        * n - number of parameters (2)
        * p - array of fit parameters 
        * dy - array of residuals to be returned
        * CustomUserVariable - private data (struct vars_struct *)
        *
        * RETURNS: error code (0 = success)
        */
        public static int QuadFunc(double[] p, double[] dy, IList<double>[]? dvec, object? vars)
        {
            int i;
            double[] x, y, ey;

            if (vars is not CustomUserVariable v) return 1;
            x = v.X;
            y = v.Y;
            ey = v.Ey;

            /* Console.Write ("QuadFunc %f %f %f\n", p[0], p[1], p[2]); */

            for (i = 0; i < dy.Length; i++)
            {
                dy[i] = (y[i] - p[0] - p[1] * x[i] - p[2] * x[i] * x[i]) / ey[i];
            }

            return 0;
        }


        /* 
         * gaussian fit function
         *
         * m - number of data points
         * n - number of parameters (4)
         * p - array of fit parameters 
         *     p[0] = constant offset
         *     p[1] = peak y value
         *     p[2] = x centroid position
         *     p[3] = gaussian sigma width
         * dy - array of residuals to be returned
         * CustomUserVariable - private data (struct vars_struct *)
         *
         * RETURNS: error code (0 = success)
         */
        public static int GaussFunc(double[] p, double[] dy, IList<double>[]? dvec, object? vars)
        {
            int i;
            if (vars is not CustomUserVariable v) return 1;

            double[] x, y, ey;
            double xc, sig2;

            x = v.X;
            y = v.Y;
            ey = v.Ey;

            sig2 = p[3] * p[3];

            for (i = 0; i < dy.Length; i++)
            {
                xc = x[i] - p[2];
                dy[i] = (y[i] - p[1] * Math.Exp(-0.5 * xc * xc / sig2) - p[0]) / ey[i];
            }

            return 0;
        }

        public static int LineFunc(double[] p, double[] dy, IList<double>[]? dvec, object? vars)
        {
            if (vars is not LineFitData lineFitData) return 1;

            var x = lineFitData.X;
            var y = lineFitData.Y;

            for (int i = 0; i < x.Length; i++)
            {
                dy[i] = y[i] - (p[0] + p[1] * x[i]);
                if (dvec != null)
                {
                    var dvec0 = dvec[0];
                    var dvec1 = dvec[1];
                    if (dvec0 != null)
                    {
                        dvec0[i] = -p[0];
                    }
                    if (dvec1 != null)
                    {
                        dvec1[i] = -x[i];
                    }
                }
            }
            return 0;
        }
        public static int GaussianFuncAndDerivs(double[] p, ref double[] dy, ref IList<double>[]? dvec, object? vars)
        {
            if (vars is not CustomUserVariable v) return 1;
            double[] x, y, ey;
            double sig2;

            x = v.X;
            y = v.Y;
            ey = v.Ey;

            sig2 = p[3] * p[3];
            var dvec1 = new double[dy.Length];
            var residuals = new double[dy.Length];

            for (int i = 0; i < dy.Length; i++)
            {
                double xc = x[i] - p[2];
                double exp = Math.Exp(-0.5 * xc * xc / sig2);
                residuals[i] = (y[i] - p[1] * exp - p[0]) / ey[i];

                // NOTE: it would make sense to store the first 2 derivatives in vars since they don't change.
                if (dvec != null)
                {
                    if (dvec[0] != null)
                    {
                        dvec[0][i] = -1.0 / ey[i];
                    }
                    if (dvec[1] != null)
                    {
                        dvec1[i] = -exp / ey[i];
                    }
                    if (dvec[2] != null)
                    {
                        dvec[2][i] = -p[1] * xc * exp / (ey[i] * sig2);
                    }
                    if (dvec[3] != null)
                    {
                        dvec[3][i] = -p[1] * xc * xc * exp / (ey[i] * p[3] * sig2);
                    }
                }
            }

            // Array assignment rather than element-wise.
            dy = residuals;

            if (dvec != null)
            {
              if (dvec[1] != null)
              {
                  dvec[1] = dvec1;
              }
            }
            return 0;
        }
    }
}
