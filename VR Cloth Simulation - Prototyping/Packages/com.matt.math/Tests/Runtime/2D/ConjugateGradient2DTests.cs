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
        public void CgAdd_WithValues_AddsThemTogether()
        {
            var expected = MakeExpected();
            var first = MakeFirst();
            var second = MakeSecond();

            var result = ConjugateGradient.CgAdd(first, second);

            AssertGridVectorsEqual(result, expected);
        }
        
        #region Helpers

        private static List<double2> MakeExpected() => new()
        {
            math.double2(7.0, 5),
            math.double2(1.0, 3.0)
        };
        
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