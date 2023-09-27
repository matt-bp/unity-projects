using LinearAlgebra;
using NUnit.Framework;
using Triangles;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Triangles
{
    public class WorldSpaceTriangleTests
    {
        [Test]
        public void Dx1_SimpleVectors_ReturnsOneForBothDifferences()
        {
            var worldSpaceTriangle = MakeTriangle();

            var result = worldSpaceTriangle.Dx1;

            Assert.That(result, Is.EqualTo(math.double3(1, 0.3, 0)));
        }
        
        [Test]
        public void Dx2_SimpleVectors_ReturnsOneForBothDifferences()
        {
            var worldSpaceTriangle = MakeTriangle();

            var result = worldSpaceTriangle.Dx2;

            Assert.That(result, Is.EqualTo(math.double3(0.25, 1, 0)));
        }
        
        #region Helpers

        private static WorldSpaceTriangle MakeTriangle()
        {
            return new WorldSpaceTriangle(
                new Double3(double3.zero),
                new Double3(math.double3(1, 0.3, 0)),
                new Double3(math.double3(0.25, 1, 0)));
        }
        
        #endregion
    }
}