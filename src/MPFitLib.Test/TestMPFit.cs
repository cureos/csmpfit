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
using System.IO;
using NUnit.Framework;

namespace MPFitLib.Test
{
    public class TestMPFit
    {
        /// <summary>
        /// Test harness routine, which contains test data, invokes mpfit()
        /// </summary>
        [Test]
        public void TestLinFit()
        {
            double[] x =
            [
                -1.7237128E+00, 1.8712276E+00, -9.6608055E-01,
                -2.8394297E-01, 1.3416969E+00, 1.3757038E+00,
                -1.3703436E+00, 4.2581975E-02, -1.4970151E-01,
                8.2065094E-01
            ];
            double[] y =
            [
                1.9000429E-01, 6.5807428E+00, 1.4582725E+00,
                2.7270851E+00, 5.5969253E+00, 5.6249280E+00,
                0.787615, 3.2599759E+00, 2.9771762E+00,
                4.5936475E+00
            ];

            var ey = new double[10];
            /*        y = a + b * x */
            /*               a    b */
            double[] p = [1.0, 1.0]; /* Parameter initial conditions */
            double[] pactual = [3.20, 1.78]; /* Actual values used to make data */

            var result = new mp_result(showXerror: true);

            for (var i = 0; i < 10; i++)
            {
                ey[i] = 0.07; /* Data errors */
            }

            var v = new CustomUserVariable
            {
                X = x,
                Y = y,
                Ey = ey
            };

            /* Call fitting function for 10 data points and 2 parameters */
            var status = MPFit.Solve(ForwardModels.LinFunc, 10, 2, p, null, null, v, result);

            PrintResult(p, pactual, result);

            Assert.That(status, Is.EqualTo(1));
            Assert.That(p, Is.EqualTo(pactual).Within(0.01));
        }

        /// <summary>
        /// Test harness routine, which contains test quadratic data, invokes Solve()
        /// </summary>
        [Test]
        public void TestQuadFit()
        {
            double[] x =
            [
                -1.7237128E+00, 1.8712276E+00, -9.6608055E-01,
                -2.8394297E-01, 1.3416969E+00, 1.3757038E+00,
                -1.3703436E+00, 4.2581975E-02, -1.4970151E-01,
                8.2065094E-01
            ];
            double[] y =
            [
                2.3095947E+01, 2.6449392E+01, 1.0204468E+01,
                5.40507, 1.5787588E+01, 1.6520903E+01,
                1.5971818E+01, 4.7668524E+00, 4.9337711E+00,
                8.7348375E+00
            ];
            var ey = new double[10];
            double[] p = [1.0, 1.0, 1.0]; /* Initial conditions */
            double[] pactual = [4.7, 0.0, 6.2]; /* Actual values used to make data */

            var result = new mp_result(showXerror: true);

            for (var i = 0; i < 10; i++)
            {
                ey[i] = 0.2; /* Data errors */
            }

            var v = new CustomUserVariable { X = x, Y = y, Ey = ey };

            /* Call fitting function for 10 data points and 3 parameters */
            var status = MPFit.Solve(ForwardModels.QuadFunc, 10, 3, p, null, null, v, result);

            PrintResult(p, pactual, result);

            Assert.That(status, Is.EqualTo(1));
            Assert.That(p, Is.EqualTo(pactual).Within(0.1));
        }

        /// <summary>
        /// Test harness routine, which contains test quadratic data;
        /// Example of how to fix a parameter
        /// </summary>
        [Test]
        public void TestQuadFix()
        {
            double[] x =
            [
                -1.7237128E+00, 1.8712276E+00, -9.6608055E-01,
                -2.8394297E-01, 1.3416969E+00, 1.3757038E+00,
                -1.3703436E+00, 4.2581975E-02, -1.4970151E-01,
                8.2065094E-01
            ];
            double[] y =
            [
                2.3095947E+01, 2.6449392E+01, 1.0204468E+01,
                5.40507, 1.5787588E+01, 1.6520903E+01,
                1.5971818E+01, 4.7668524E+00, 4.9337711E+00,
                8.7348375E+00
            ];

            var ey = new double[10];
            double[] p = [1.0, 0.0, 1.0]; /* Initial conditions */
            double[] pactual = [4.7, 0.0, 6.2]; /* Actual values used to make data */

            var result = new mp_result(showXerror: true);

            var pars = new[] /* Parameter constraints */
            {
                new mp_par(),
                new mp_par { isFixed = 1 }, /* Fix parameter 1 */
                new mp_par()
            };

            for (var i = 0; i < 10; i++)
            {
                ey[i] = 0.2;
            }

            var v = new CustomUserVariable { X = x, Y = y, Ey = ey };

            /* Call fitting function for 10 data points and 3 parameters (1
               parameter fixed) */
            var status = MPFit.Solve(ForwardModels.QuadFunc, 10, 3, p, pars, null, v, result);

            PrintResult(p, pactual, result);

            Assert.That(status, Is.EqualTo(1));
            Assert.That(p, Is.EqualTo(pactual).Within(0.03));
        }


        /// <summary>
        /// Test harness routine, which contains test gaussian-peak data
        /// </summary>
        [Test]
        public void TestGaussFit()
        {
            double[] x =
            [
                -1.7237128E+00, 1.8712276E+00, -9.6608055E-01,
                -2.8394297E-01, 1.3416969E+00, 1.3757038E+00,
                -1.3703436E+00, 4.2581975E-02, -1.4970151E-01,
                8.2065094E-01
            ];
            double[] y =
            [
                -4.4494256E-02, 8.7324673E-01, 7.4443483E-01,
                4.7631559E+00, 1.7187297E-01, 1.1639182E-01,
                1.5646480E+00, 5.2322268E+00, 4.2543168E+00,
                6.2792623E-01
            ];
            var ey = new double[10];
            double[] p = [0.0, 1.0, 1.0, 1.0]; /* Initial conditions */
            double[] pactual = [0.0, 4.70, 0.0, 0.5]; /* Actual values used to make data*/
            var pars = new[] /* Parameter constraints */
            {
                new mp_par(),
                new mp_par(),
                new mp_par(),
                new mp_par()
            };

            var result = new mp_result(showXerror: true);

            /* No constraints */
            for (var i = 0; i < 10; i++) ey[i] = 0.5;

            var v = new CustomUserVariable { X = x, Y = y, Ey = ey };

            /* Call fitting function for 10 data points and 4 parameters (no
               parameters fixed) */
            var status = MPFit.Solve(ForwardModels.GaussFunc, 10, 4, p, pars, null, v, result);

            PrintResult(p, pactual, result);

            Assert.That(status, Is.EqualTo(1));
            Assert.That(p, Is.EqualTo(pactual).Within(0.5));
        }


        /// <summary>
        /// Test harness routine, which contains test gaussian-peak data
        /// Example of fixing two parameter
        /// Commented example of how to put boundary constraints
        /// </summary>
        [Test]
        public void TestGaussFix()
        {
            double[] x =
            [
                -1.7237128E+00, 1.8712276E+00, -9.6608055E-01,
                -2.8394297E-01, 1.3416969E+00, 1.3757038E+00,
                -1.3703436E+00, 4.2581975E-02, -1.4970151E-01,
                8.2065094E-01
            ];
            double[] y =
            [
                -4.4494256E-02, 8.7324673E-01, 7.4443483E-01,
                4.7631559E+00, 1.7187297E-01, 1.1639182E-01,
                1.5646480E+00, 5.2322268E+00, 4.2543168E+00,
                6.2792623E-01
            ];
            var ey = new double[10];
            double[] p = [0.0, 1.0, 0.0, 0.1]; /* Initial conditions */
            double[] pactual = [0.0, 4.70, 0.0, 0.5]; /* Actual values used to make data*/

            var result = new mp_result(showXerror: true);

            var pars = new[] /* Parameter constraints */
            {
                new mp_par { isFixed = 1 }, /* Fix parameters 0 and 2 */
                new mp_par(),
                new mp_par { isFixed = 1 },
                new mp_par()
            };

            /* How to put limits on a parameter.  In this case, parameter 3 is
               limited to be between -0.3 and +0.2.
            pars[3].limited[0] = 0;
            pars[3].limited[1] = 1;
            pars[3].limits[0] = -0.3;
            pars[3].limits[1] = +0.2;
            */
            for (var i = 0; i < 10; i++)
            {
                ey[i] = 0.5;
            }

            var v = new CustomUserVariable { X = x, Y = y, Ey = ey };

            /* Call fitting function for 10 data points and 4 parameters (2
               parameters fixed) */
            var status = MPFit.Solve(ForwardModels.GaussFunc, 10, 4, p, pars, null, v, result);

            PrintResult(p, pactual, result);

            Assert.That(status, Is.EqualTo(1));
            Assert.That(p, Is.EqualTo(pactual).Within(0.5));
        }

        [Test]
        public void TestAnalyticDerivatives()
        {
            const double intercept = 7.0;
            const double slope = -3.0;
            var pactual = new[] { intercept, slope };
            var p = new[] { 5.9, -1.1 };
            var x = new double[] { -5, -3, -1, 0, 1, 3, 5 };
            var y = new double[x.Length];


            for (uint i = 0; i < x.Length; i++)
            {
                y[i] = intercept + slope * x[i];
            }

            var mpPar = new[] { new mp_par(), new mp_par() };
            mpPar[0].side = 3;
            mpPar[1].side = 3;

            var result = new mp_result();

            var status = MPFit.Solve(ForwardModels.LineFunc, x.Length, 2, p, mpPar, null, new LineFitData(x, y),
                result);

            PrintResult(p, pactual, result);

            Assert.That(status, Is.EqualTo(2));
            Assert.That(p, Is.EqualTo(pactual).Within(0.01));
        }

        [Test]
        public void TestGaussianWithDerivs()
        {
            double[] x =
            [
                -1.7237128E+00, 1.8712276E+00, -9.6608055E-01,
                -2.8394297E-01, 1.3416969E+00, 1.3757038E+00,
                -1.3703436E+00, 4.2581975E-02, -1.4970151E-01,
                8.2065094E-01
            ];
            double[] y =
            [
                -4.4494256E-02, 8.7324673E-01, 7.4443483E-01,
                4.7631559E+00, 1.7187297E-01, 1.1639182E-01,
                1.5646480E+00, 5.2322268E+00, 4.2543168E+00,
                6.2792623E-01
            ];
            var ey = new double[10];
            double[] p = [0.0, 1.0, 1.0, 1.0]; // Initial conditions
            double[] pactual = [0.0, 4.70, 0.0, 0.5]; // Actual values used to make data
            mp_par[] pars =
            [
                new() { side = 3, deriv_debug = 0 },
                new() { side = 1, deriv_debug = 0 },
                new() { side = 3, deriv_debug = 0 },
                new() { side = 1, deriv_debug = 0 }
            ];

            var result = new mp_result();

            /* No constraints */

            for (uint i = 0; i < 10; i++) ey[i] = 0.5;

            var v = new CustomUserVariable { X = x, Y = y, Ey = ey };

            /* Call fitting function for 10 data points and 4 parameters (no
               parameters fixed) */

            var logger = new StringWriter();
            var status = MPFit.Solve(ForwardModels.GaussianFuncAndDerivs, 10, 4, p, pars, null, v, result, logger);

            PrintResult(p, pactual, result);
            Console.WriteLine(logger.ToString());

            Assert.That(status, Is.EqualTo(1));
            Assert.That(p, Is.EqualTo(pactual).Within(0.5));
        }

        /* Simple routine to print the fit results */
        private static void PrintResult(double[] x, double[]? xact, mp_result? result)
        {
            if (result != null)
            {
                Console.Write("  CHI-SQUARE = {0}    ({1} DOF)\n",
                    result.bestnorm, result.nfunc - result.nfree);
                Console.Write("        NPAR = {0}\n", result.npar);
                Console.Write("       NFREE = {0}\n", result.nfree);
                Console.Write("     NPEGGED = {0}\n", result.npegged);
                Console.Write("       NITER = {0}\n", result.niter);
                Console.Write("        NFEV = {0}\n", result.nfev);
                Console.Write("\n");
            }

            if (xact != null)
            {
                for (var i = 0; i < x.Length; i++)
                {
                    Console.Write("  P[{0}] = {1} +/- {2}     (ACTUAL {3})\n",
                        i, x[i], result?.xerror?[i] ?? double.NaN, xact[i]);
                }
            }
            else
            {
                for (var i = 0; i < x.Length; i++)
                {
                    Console.Write("  P[{0}] = {1} +/- {2}\n",
                        i, x[i], result?.xerror?[i] ?? double.NaN);
                }
            }
        }
    }

}
