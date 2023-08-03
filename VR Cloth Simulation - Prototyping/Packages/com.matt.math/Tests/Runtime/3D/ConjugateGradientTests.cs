using System.Collections.Generic;
using MattMath._3D;
using NUnit.Framework;
using Unity.Mathematics;

namespace Tests.Runtime._3D
{
    public class ConjugateGradientTests
    {
        [Test]
        public void Add_With3DValues_AddsThemTogether()
        {
            var expected = new List<double3>
            {
                math.double3(7.0, 5.0, 6.0),
                math.double3(1.0, 3.0, 1.5)
            };
            var first = MakeFirst();
            var second = MakeSecond();

            var result = ConjugateGradient.Add(first, second);
            
            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void Sub_With3DValues_SubtractsThem()
        {
            var expected = new List<double3>
            {
                math.double3(1.0, 2.0, 4.0),
                math.double3(1.0, 1.0, 0.5)
            };
            var first = MakeFirst();
            var second = MakeSecond();
            
            var result = ConjugateGradient.Sub(first, second);
            
            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void Mult_With3DValuesAndConstant_ReturnsScaledVector()
        {
            var expected = new List<double3>
            {
                math.double3(2.0, 1.75, 2.5),
                math.double3(0.5, 1.0, 0.5),
            };
            var first = MakeFirst();
            const double constant = 0.5;

            var result = ConjugateGradient.Mult(first, constant);
            
            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void Mult_With3DValuesAndIdentity_ReturnsTheValues()
        {
            var expected = new List<double3> { math.double3(4.0, 3.5, 0.1) };
            var matrix = new List<List<double3x3>> { new() { double3x3.identity } };

            var result = ConjugateGradient.Mult(matrix, expected);
            
            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void Dot_With3DValues_ReturnsTheFullDotProduct()
        {
            const double expected = 24.75;
            var first = MakeFirst();
            var second = MakeSecond();

            var result = ConjugateGradient.Dot(first, second);

            Assert.That(result, Is.EqualTo(expected));
        }

        #region Helpers

        private static List<double3> MakeFirst() => new()
        {
            math.double3(4.0, 3.5, 5.0),
            math.double3(1.0, 2.0, 1.0)
        };

        private static List<double3> MakeSecond() => new()
        {
            math.double3(3.0, 1.5, 1.0),
            math.double3(0.0, 1.0, 0.5)
        };
        
        private static void AssertGridVectorsEqual(List<double3> result, List<double3> expected)
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