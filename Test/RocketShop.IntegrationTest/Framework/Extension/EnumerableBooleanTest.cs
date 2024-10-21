using FluentAssertions;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Data;
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

        [TestMethod]
        public void AllFalseTest_ReturnTrue()
        {
            var initial = new[] { false, false, false, false, false, false, false, false };
            var result = initial.AllFalse();
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(new[] { true,true,true,true})]
        [DataRow(new[] { false,false,true,false})]
        public void AllFalseTest_ReturnFalse(bool[] initial)
        {
            var result = initial.AllFalse();
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(new[] { true, true, true, true })]
        [DataRow(new[] { false, false, true, false })]
        public void SomeTrueTest_ReturnTrue(bool[] initial)
        {
            var result = initial.SomeTrue();
            result.Should().BeTrue();
        }

        [TestMethod]
        public void SomeTrueTest_ReturnFalse()
        {
            var initial = new[] { false, false, false, false, false, false, false, false };
            var result = initial.SomeTrue();
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(new[] { false, false, false, false })]
        [DataRow(new[] { true, false, true, false })]
        public void SomeFalseTest_ReturnTrue(bool[] initial)
        {
            var result = initial.SomeFalse();
            result.Should().BeTrue();
        }

        [TestMethod]
        public void SomeFalseTest_ReturnFalse()
        {
            var initial = new[] { true, true, true, true };
            var result = initial.SomeFalse();
            result.Should().BeFalse();
        }
        [TestMethod]
        public void AndTest_ReturnTrue()
        {
            var initial1 = true;
            var initial2 = true;
            var initial3 = true;
            var result = initial1.And(initial2,initial3);
            result.Should().BeTrue();
        }
        [TestMethod]
        public void AndTest_ReturnFalse()
        {
            var initial1 = true;
            var initial2 = true;
            var initial3 = true;
            var result = initial1.And(initial2,initial3);
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(false, true, true)]
        [DataRow(false, false, true)]
        public void AndTest_ReturnFalse(bool initial1, bool initial2, bool initial3)
        {
            var result = initial1.Or(initial2,initial3);
            result.Should().BeTrue();
        }

        [TestMethod]
        [DataRow(true,true,true)]
        [DataRow(false,true,true)]
        [DataRow(false,false,true)]
        public void OrTest_ReturnTrue(bool initial1,bool initial2,bool initial3)
        {
            var result = initial1.Or(initial2,initial3);
            result.Should().BeTrue();
        }

        [TestMethod]
        public void OrTest_ReturnFalse()
        {
            var initial1 = false;
            var initial2 = false;
            var initial3 = false;
            var result = initial1.Or(initial2,initial3);
            result.Should().BeFalse();
        }
        [TestMethod]
        [DataRow(true,true,true)]
        [DataRow(false,true,true)]
        [DataRow(false,false,true)]
        public void NorTest_ReturnTrue(bool initial1,bool initial2,bool initial3)
        {
            var result = initial1.Nor(initial2,initial3);
            result.Should().BeFalse();
        }

        [TestMethod]
        public void NorTest_ReturnFalse()
        {
            var initial1 = false;
            var initial2 = false;
            var initial3 = false;
            var result = initial1.Nor(initial2,initial3);
            result.Should().BeTrue();
        }

        [TestMethod]
        public void NandTest_ReturnFalse()
        {
            var initial1 = true;
            var initial2 = true;
            var initial3 = true;
            var result = initial1.Nand(initial2, initial3);
            result.Should().BeFalse();
        }

        [TestMethod]
        [DataRow(false, true, true)]
        [DataRow(false, false, true)]
        public void NandTest_ReturnTrue(bool initial1, bool initial2, bool initial3)
        {
            var result = initial1.Nand(initial2, initial3);
            result.Should().BeTrue();
        }
    }
}
