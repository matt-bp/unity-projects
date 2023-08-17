using System.Collections.Generic;
using DataStructures;
using NUnit.Framework;

namespace UnitTests.Runtime.DataStructures
{
    public class GridTests
    {
        [Test]
        public void MakeMatrix_WithSmallMatrix_CreatesIt()
        {
            var expected = new List<List<double>> { new() { 0.5, 0.5 }, new() { 0.5, 0.5} };

            var result = Grid<double>.MakeMatrix(2, 0.5);
            
            AssertListOfListsAreEqual(result, expected);
        }

        [Test]
        public void MakeVector_WithSmallVector_CreatesIt()
        {
            var expected = new List<double> { 0.2, 0.2, 0.2 };

            var result = Grid<double>.MakeVector(3, 0.2);
            
            AssertListsAreEqual(result, expected);
        }

        #region Helpers
        
        private static void AssertListOfListsAreEqual(List<List<double>> result, List<List<double>> expected)
        {
            Assert.That(result, Has.Count.EqualTo(expected.Count));

            for (var i = 0; i < expected.Count; i++)
            {
                for (var j = 0; j < expected[i].Count; j++)
                {
                    Assert.That(result[i][j], Is.EqualTo(expected[i][j]));
                }
            }
        }
        
        private static void AssertListsAreEqual(List<double> result, List<double> expected)
        {
            Assert.That(result, Has.Count.EqualTo(expected.Count));

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.That(result[i], Is.EqualTo(expected[i]));
            }
        }
        
        #endregion
    }
}