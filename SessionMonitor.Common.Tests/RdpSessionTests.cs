using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SessionMonitor.Common.Tests
{
    [TestFixture]
    public class RdpSessionTests
    {
        [Test]
        public void TestEqualsRdpSession()
        {
            var rdp1 = new RdpSession();
            var rdp2 = new RdpSession();

            var areEqual = rdp1.Equals(rdp2);

            Assert.AreEqual(rdp1, rdp2);

            Assert.IsTrue(areEqual);
        }

        [Test]
        public void FooTest()
        {
            Assert.IsTrue(true);
        }
    }
}
