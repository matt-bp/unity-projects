using System.Collections.Generic;
using MattMath._1D;
using NUnit.Framework;

namespace Tests.Runtime._1D
{
    public class ConjugateGradient1DTests
    {
        [Test]
        public void CgAdd_WithValues_AddsThemTogether()
        {
            var expected = new List<double> { 6.0, -6.0 };
            var first = new List<double> { 4.0, -3.0 };
            var second = new List<double> { 2.0, -3.0 };

            var result = ConjugateGradient.CgAdd(first, second);

            AssertGridVectorsEqual(result, expected);
        }
        
        [Test]
        public void CgSub_WithValues_ReturnsSubtractedResult()
        {
            var expected = new List<double> { 2.0, 0.0 };
            var first = new List<double> { 4.0, -3.0 };
            var second = new List<double> { 2.0, -3.0 };

            var result = ConjugateGradient.CgSub(first, second);

            AssertGridVectorsEqual(result, expected);
        }
        
        [Test]
        public void CgDot_WithValues_ReturnsDotProduct()
        {
            const double expected = 5;
            var first = new List<double> { 1.0, 3.0 };
            var second = new List<double> { 2.0, 1.0 };

            var result = ConjugateGradient.CgDot(first, second);

            Assert.That(result, Is.EqualTo(expected).Within(0.01));
        }
        
        [Test]
        public void CgMult_WithValues_MultipliesCorrectly()
        {
            var expected = new List<double> { 2.0, 1.5 };
            var first = new List<double> { 4.0, 3.0 };

            var result = ConjugateGradient.CgMult(first, 0.5);

            AssertGridVectorsEqual(result, expected);
        }
        
        [Test]
        public void CgMult_WithMatrixAndVector_ReturnsVector()
        {
            var expected = new List<double> { 4.5, 9.5 };
            var vector = new List<double> { 2.0, 0.5 };
            var matrix = new List<List<double>>
            {
                // In column major order
                new() { 2.0, 4.0 },
                new() { 1.0, 3.0 }
            };


            var result = ConjugateGradient.CgMult(matrix, vector);

            AssertGridVectorsEqual(result, expected);
        }
        
        
        [Test]
        public void Solve_WithExampleFromPaper1D_ReturnsExpectedResult()
        {
            var expected = new List<double> { 2.0, -2.0 };
            var vector = new List<double> { 2.0, -8.0 };
            var matrix = new List<List<double>>
            {
                // In column major order
                new() { 3.0, 2.0 },
                new() { 2.0, 6.0 }
            };

            var result = ConjugateGradient.Solve(matrix, vector, 100, 0.0001);

            AssertGridVectorsEqual(result, expected);
        }

        [TestCase(100.0)]
        [TestCase(-100.0)]
        [TestCase(0.0)]
        public void ConstrainedSolve_OneConstrainedIndexWithVaryingValues_DoesntUpdateThatIndex(double secondValue)
        {
            var expected = new List<double> { 1.0, 0.0 };
            var vector = new List<double> { 3.0, secondValue };
            var matrix = new List<List<double>>
            {
                // In column major order
                new() { 3.0, 2.0 },
                new() { 2.0, 6.0 }
            };
            var constrainedIndices = new List<int> { 1 };

            var result = ConjugateGradient.ConstrainedSolve(matrix, vector, 100, 0.0001, constrainedIndices);
            
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