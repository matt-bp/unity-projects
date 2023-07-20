using NUnit.Framework;
using System.Collections.Generic;
using MattMath;

namespace Tests.Runtime
{
    public class ConjugateGradient1DTests
    {
        [Test]
        public void CgAdd_WithValues_AddsThemTogether()
        {
            var expected = new List<double> { 6.0, -6.0 };
            var first = new List<double> { 4.0, -3.0 };
            var second = new List<double> { 2.0, -3.0 };

            var result = ConjugateGradient1D.CgAdd(first, second);

            AssertGridVectorsEqual(result, expected);
        }
        
        #region Helpers
        
        private void AssertGridVectorsEqual(IReadOnlyList<double> result, IReadOnlyList<double> expected)
        {
            Assert.That(result.Count, Is.EqualTo(expected.Count));
            for (var row = 0; row < expected.Count; row++)
            {
                Assert.That(row, Is.LessThan(result.Count));
                Assert.That(result[row], Is.EqualTo(expected[row]));
            }
        }
        
        #endregion
    }
}