using FluentAssertions;
using RocketShop.Framework.NumberCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.IntegrationTest.Framework.Extension
{
    [TestClass]
    public class HexademcialTest
    {
        [TestMethod]
        [DataRow("FF")]
        [DataRow("0xFF")]
        public void Hexadecimal_ToIntTest(string hex)
        {
            var result = new Hexadecimal(hex).ToInt();
            result.Should().Be(255);
        }

        [TestMethod]
        [DataRow("8465413516546123144")]
        [DataRow("2645626265265462632626589456312312")]
        public void Hexadecimal_ToInt128Test(string rawExpected)
        {
            var expected = Int128.Parse(rawExpected);
            Hexadecimal hexadecimal = expected;
            var result = hexadecimal.ToInt128();
            result.Should().Be(expected);
        }


    }
}
