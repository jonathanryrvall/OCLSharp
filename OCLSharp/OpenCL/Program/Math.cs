using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.OpenCL.Program
{
    /// <summary>
    /// OpenCL program
    /// </summary>
    public partial class OpenCLProgram
    {
        protected double acos(double x)
        {
            return Math.Acos(x);
        }

        protected float acos(float x)
        {
            return (float)Math.Acos(x);
        }

        protected T acosh<T>(T x) where T : struct
        {
            return x;
       //return Math.Acosh
        }

        protected T acospi<T>(T x) where T : struct
        {
            return x;
        }

        protected T asin<T>(T x) where T : struct
        {
            return x;
        }

        protected T asinh<T>(T x) where T : struct
        {
            return x;
        }

        protected T asinpi<T>(T x) where T : struct
        {
            return x;
        }

        protected T atan<T>(T x) where T : struct
        {
            return x;
        }

        protected T atan2<T>(T y, T x) where T : struct
        {
            return x;
        }

        protected T atanh<T>(T x) where T : struct
        {
            return x;
        }

        protected T atanpi<T>(T x) where T : struct
        {
            return x;
        }

        protected T atan2pi<T>(T y, T x) where T : struct
        {
            return x;
        }

        protected T cbrt<T>(T x) where T : struct
        {
            return x;
        }

        protected T ceil<T>(T x) where T : struct
        {
            return x;
        }

        protected T copysign<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T cos<T>(T x) where T : struct
        {
            return x;
        }

        protected T cosh<T>(T x) where T : struct
        {
            return x;
        }

        protected T cospi<T>(T x) where T : struct
        {
            return x;
        }

        protected T erfc<T>(T x) where T : struct
        {
            return x;
        }

        protected T exp<T>(T x) where T : struct
        {
            return x;
        }

        protected T exp10<T>(T x) where T : struct
        {
            return x;
        }

        protected T exp2<T>(T x) where T : struct
        {
            return x;
        }

        protected T expm1<T>(T x) where T : struct
        {
            return x;
        }

        protected T fabs<T>(T x) where T : struct
        {
            return x;
        }

        protected T fdim<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T floor<T>(T x) where T : struct
        {
            return x;
        }

        protected T fma<T>(T a, T b, T c) where T : struct
        {
            return a;
        }

        protected T fmax<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T fmax<T>(T x, float y) where T : struct
        {
            return x;
        }

        protected T fmax<T>(T x, double y) where T : struct
        {
            return x;
        }

        protected T fmin<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T fmin<T>(T x, float y) where T : struct
        {
            return x;
        }

        protected T fmin<T>(T x, double y) where T : struct
        {
            return x;
        }

        protected T fmod<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T hypot<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected int ilogb<T>(T x) where T : struct
        {
            return 0;
        }

        protected T ldexp<T>(T x, int k) where T : struct
        {
            return x;
        }

        protected T lgamma<T>(T x) where T : struct
        {
            return x;
        }

        protected T log<T>(T x) where T : struct
        {
            return x;
        }

        protected T log2<T>(T x) where T : struct
        {
            return x;
        }

        protected T log10<T>(T x) where T : struct
        {
            return x;
        }

        protected T log1p<T>(T x) where T : struct
        {
            return x;
        }

        protected T logb<T>(T x) where T : struct
        {
            return x;
        }

        protected T mad<T>(T a, T b, T c) where T : struct
        {
            return a;
        }

        protected T maxmag<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T minmag<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T modf<T>(T x, T[] y) where T : struct
        {
            return x;
        }

        protected T nextafter<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T pow<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T pown<T>(T x, int y) where T : struct
        {
            return x;
        }

        protected T powr<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T reminder<T>(T x, T y) where T : struct
        {
            return x;
        }

        protected T remquo<T>(T x, T y, int[] quo)
        {
            return x;
        }

        protected T remquo<T>(T x, T y, int quo)
        {
            return x;
        }

        protected T rint<T>(T x) where T : struct
        {
            return x;
        }

        protected T rootn<T>(T x, int y) where T : struct
        {
            return x;
        }

        protected T round<T>(T x) where T : struct
        {
            return x;
        }

        protected T sqrt<T>(T x) where T : struct
        {
            return x;
        }

        protected T rsqrt<T>(T x) where T : struct
        {
            return x;
        }

        protected T sin<T>(T x) where T : struct
        {
            return x;
        }

        protected T sinh<T>(T x) where T : struct
        {
            return x;
        }

        protected T sincos<T>(T x) where T : struct
        {
            return x;
        }
        protected T sinpi<T>(T x) where T : struct
        {
            return x;
        }

        protected T tan<T>(T x) where T : struct
        {
            return x;
        }

        protected T tanh<T>(T x) where T : struct
        {
            return x;
        }

        protected T tanpi<T>(T x) where T : struct
        {
            return x;
        }

        protected T trunc<T>(T x) where T : struct
        {
            return x;
        }

        protected T tgamma<T>(T x) where T : struct
        {
            return x;
        }

        protected T radians<T>(T x) where T : struct
        {
            return x;
        }

        protected T degrees<T>(T x) where T : struct
        {
            return x;
        }

        protected T clamp<T>(T x, T minval, T maxval) where T : struct
        {
            return x;
        }

        protected T mix<T>(T x, T y, T a) where T : struct
        {
            return x;
        }

        protected T smoothstep<T>(T edge0, T edge1, T x) where T : struct
        {
            return x;
        }
    }
}
