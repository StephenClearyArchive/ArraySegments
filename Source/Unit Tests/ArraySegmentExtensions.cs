using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArraySegments;

namespace Unit_Tests
{
    [TestClass]
    public class ArraySegmentExtensions
    {
        [TestMethod]
        public void Take_TakesElements()
        {
            var segment = new[] { 1, 2, 3 }.AsArraySegment();
            ArraySegment<int> result = segment.Take(1);
            Assert.AreSame(segment.Array, result.Array);
            Assert.AreEqual(segment.Offset, result.Offset);
            Assert.AreEqual(result.Count, 1);
            Assert.IsTrue(result.SequenceEqual(new[] { 1 }));
        }
    }
}
