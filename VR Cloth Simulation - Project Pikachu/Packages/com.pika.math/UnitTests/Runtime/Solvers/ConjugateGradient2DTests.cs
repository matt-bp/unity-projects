using System.Collections.Generic;
using NUnit.Framework;
using Solvers;
using Unity.Mathematics;

namespace UnitTests.Runtime.Solvers
{
public class ConjugateGradient2DTests
    {
        [Test]
        public void Add_With2DValues_AddsThemTogether()
        {
            var expected = new List<double2>
            {
                math.double2(7.0, 5),
                math.double2(1.0, 3.0)
            };
            var first = MakeFirst();
            var second = MakeSecond();

            var result = ConjugateGradient2D.Add(first, second);

            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void Sub_With2DValues_SubtractsThem()
        {
            var expected = new List<double2>
            {
                math.double2(1.0, 2),
                math.double2(1.0, 1.0)
            };
            var first = MakeFirst();
            var second = MakeSecond();

            var result = ConjugateGradient2D.Sub(first, second);

            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void Dot_With2DValues_ReturnsTheFullDotProduct()
        {
            const double expected = 19.25;
            var first = MakeFirst();
            var second = MakeSecond();

            var result = ConjugateGradient2D.Dot(first, second);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Mult_With2DValuesAndConstant_ReturnsMultiplication()
        {
            var expected = new List<double2>()
            {
                math.double2(2.0, 1.75),
                math.double2(0.5, 1.0)
            };
            var first = MakeFirst();
            const double constant = 0.5;

            var result = ConjugateGradient2D.Mult(first, constant);

            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void Mult_With2DValuesAndIdentity_ReturnsTheVector()
        {
            var expected = new List<double2> { math.double2(4.0, 3.5) };
            var matrix = new List<List<double2x2>> { new() { math.double2x2(1.0, 0.0, 0.0, 1.0) } };

            var result = ConjugateGradient2D.Mult(matrix, expected);
            
            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void Solve_WithExampleFromPaper_ReturnsCorrectVector()
        {
            var expected = new List<double2> { math.double2(2.0, -2.0) };
            var vector = new List<double2> { math.double2(2.0, -8.0) };
            var matrix = new List<List<double2x2>> { new() { math.double2x2(3.0, 2.0, 2.0, 6.0) } };

            var result = ConjugateGradient2D.Solve(matrix, vector, 100, 0.0001);
            
            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void ConstrainedSolve_OneConstrainedIndex_DoesntChangeItsValue()
        {
            var expected = new List<double2> { double2.zero };
            var vector = new List<double2> { math.double2(3.0, 100.0) };
            var matrix = new List<List<double2x2>> { new() { math.double2x2(3.0, 2.0, 2.0, 6.0) } };
            var constrainedIndices = new List<int> { 0 };
            
            var result = ConjugateGradient2D.ConstrainedSolve(matrix, vector, 100, 0.0001, constrainedIndices);
            
            AssertGridVectorsEqual(result, expected);
            
        }

        #region Helpers

        private static List<double2> MakeFirst() => new()
        {
            math.double2(4.0, 3.5),
            math.double2(1.0, 2.0)
        };

        private static List<double2> MakeSecond() => new()
        {
            math.double2(3.0, 1.5),
            math.double2(0.0, 1.0)
        };

        private static void AssertGridVectorsEqual(List<double2> result, List<double2> expected)
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