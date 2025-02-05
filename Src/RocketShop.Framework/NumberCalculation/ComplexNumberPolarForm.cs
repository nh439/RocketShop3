using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.NumberCalculation
{
    public struct ComplexNumberPolarForm
    {
        public double Magnitude { get; set; }
        public double Degrees { get; set; }
        public ComplexNumberPolarForm(double magnitude, double degrees)
        {
            Magnitude = magnitude;
            if(degrees > 360)
            {
                Degrees = degrees % 360;
            }
            else
            {
                Degrees = degrees;
            }
        }
        public ComplexNumberPolarForm()
        {
            Magnitude = 0;
            Degrees = 0;
        }
        public ComplexNumberPolarForm(Complex complex)
        {
            Magnitude = complex.Magnitude;
            Degrees = complex.Phase * (180 / Math.PI);
        }

        public ComplexNumberPolarForm(string value)
        {
            var parts = value.Split('∠');
            Magnitude = double.Parse(parts[0]);
            Degrees = double.Parse(parts[1].Replace("°", ""));
        }

        public static implicit operator Complex(ComplexNumberPolarForm polar) =>
            new Complex(polar.Magnitude * Math.Cos(polar.Degrees), polar.Magnitude * Math.Sin(polar.Degrees));

        public static implicit operator ComplexNumberPolarForm(Complex complex) =>
            new ComplexNumberPolarForm(complex);

        public override string ToString() => $"{Magnitude} ∠ {Degrees}°";



    }
}
