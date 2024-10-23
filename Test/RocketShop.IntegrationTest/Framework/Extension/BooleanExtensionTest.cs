using FluentAssertions;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RocketShop.IntegrationTest.Framework.Extension
{
    [TestClass]
    public class BooleanExtensionTest
    {
        #region IsTrue
        [TestMethod]
        public void IsTrueTest_ReturnTrue()
        {
            bool? Initial = true;
            var result = Initial.IsTrue();
            result.Should().BeTrue();
        }
        [TestMethod]
        [DataRow(false)]
        [DataRow(null)]
        public void IsTrueTest_ReturnFalse(bool? Initial) {
            var result = Initial.IsTrue();
            result.Should().BeFalse();
        }
        #endregion
        #region IsFalse
        [TestMethod]
        public void IsFalseTest_ReturnTrue()
        {
            bool? Initial = false;
            var result = Initial.IsFalse();
            result.Should().BeTrue();
        }
        [TestMethod]
        [DataRow(true)]
        [DataRow(null)]
        public void IsFalseTest_ReturnFalse(bool? Initial)
        {
            var result = Initial.IsFalse();
            result.Should().BeFalse();
        }
        #endregion
        #region IsFalse
        [TestMethod]
        public void IsNullTest_ReturnTrue()
        {
            bool? Initial = null;
            var result = Initial.IsNull();
            result.Should().BeTrue();
        }
        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void IsNullTest_ReturnFalse(bool? Initial)
        {
            var result = Initial.IsNull();
            result.Should().BeFalse();
        }
        #endregion

        [TestMethod]
        public void TobooleanTest_ReturnTrue()
        {
            var Initial = "true";
            var result = Initial.ToBoolean();
            result.Should().BeTrue();
        }
        [TestMethod]
        [DataRow("false")]
        [DataRow("NH Framework")]
        public void TobooleanTest_ReturnFalse(string initial)
        {
            var result = initial.ToBoolean();
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow (true,false)]
        [DataRow (false,true)]
        public void XORTest_ReturnTrue(bool initial1,bool initial2)
        {
            var result = initial1.XOR(initial2);
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow (true,true)]
        [DataRow (false,false)]
        public void XORTest_ReturnFalse(bool initial1,bool initial2)
        {
            var result = initial1.XOR(initial2);
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(true, true)]
        [DataRow(false, false)]
        public void XNORTest_ReturnTrue(bool initial1,bool initial2)
        {
            var result = initial1.XNOR(initial2);
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(true, false)]
        [DataRow(false, true)]
        public void XNORTest_ReturnFalse(bool initial1,bool initial2)
        {
            var result = initial1.XNOR(initial2);
            result.Should().BeFalse();
        }

    }
}
