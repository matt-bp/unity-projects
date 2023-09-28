using NUnit.Framework;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests
{
    public static class PikaAssert
    {
        public static void AssertDouble3WithinTolerance(double3 result, double3 expected, double tolerance)
        {
            Assert.That(result.x, Is.EqualTo(expected.x).Within(tolerance));
        }
    }
}