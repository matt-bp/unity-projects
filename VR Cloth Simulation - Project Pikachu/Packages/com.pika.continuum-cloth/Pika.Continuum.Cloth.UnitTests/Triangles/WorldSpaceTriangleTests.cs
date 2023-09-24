using LinearAlgebra;
using NUnit.Framework;
using Triangles;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Triangles
{
    public class WorldSpaceTriangleTests
    {
        [Test]
        public void GetDifferences_SimpleVectors_ReturnsOneForBothDifferences()
        {
            var worldSpaceTriangle = new WorldSpaceTriangle(
                new Double3(double3.zero),
                new Double3(math.double3(1, 0, 0)),
                new Double3(math.double3(0, 1, 0)));

            var result = worldSpaceTriangle.GetDifferences();

            Assert.That(result.dx1, Is.EqualTo(math.double3(1, 0, 0)));
            Assert.That(result.dx2, Is.EqualTo(math.double3(0, 1, 0)));
        }
    }
}