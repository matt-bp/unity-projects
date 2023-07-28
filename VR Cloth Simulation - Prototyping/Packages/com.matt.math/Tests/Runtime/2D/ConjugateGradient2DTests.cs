using System;
using System.Collections.Generic;
using MattMath;
using MattMath._2D;
using NUnit.Framework;
using UnityEngine;
using Unity.Mathematics;

namespace Tests.Runtime._2D
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

            var result = ConjugateGradient.Add(first, second);

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

            var result = ConjugateGradient.Sub(first, second);

            AssertGridVectorsEqual(result, expected);
        }

        [Test]
        public void Dot_With2DValues_ReturnsTheFullDotProduct()
        {
            const double expected = 19.25;
            var first = MakeFirst();
            var second = MakeSecond();

            var result = ConjugateGradient.Dot(first, second);
            
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
            
            var result = ConjugateGradient.Mult(first, constant);

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