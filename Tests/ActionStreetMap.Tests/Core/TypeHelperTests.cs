﻿using ActionStreetMap.Core.Utilities;
using NUnit.Framework;

namespace ActionStreetMap.Tests.Core
{
    [TestFixture]
    public class TypeHelperTests
    {
        [Test]
        public void CanSanitizeFloat()
        {
            Assert.AreEqual("150", SanitizeHelper.SanitizeFloat("150m"));
            Assert.AreEqual("150.2", SanitizeHelper.SanitizeFloat("150.2m"));
        }
    }
}