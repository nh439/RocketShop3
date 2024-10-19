using FluentAssertions;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.IntegrationTest.Framework.Extension
{
    [TestClass]
    public class EnumerableBooleanTest
    {
        [TestMethod]
        public void AllTrueTest_ReturnTrue()
        {
            var initial = new[] { true, true, true, true, true, true, true, true };
            var result = initial.AllTrue();
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(new[] { true,true,false,true})]
        [DataRow(new[] { false,false,false,false})]
        public void AllTrueTest_ReturnFalse(bool[] initial)
        {
            var result = initial.AllTrue();
            result.Should().BeFalse();
        }
    }
}
