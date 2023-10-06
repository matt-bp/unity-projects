using Conditions;
using NUnit.Framework;
using Triangles;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Triangles
{
    public class RestSpaceTriangleTests
    {
        [Test]
        public void Area_WithTestTriangle_ReturnsCorrectArea()
        {
            var triangle = MakeTriangle();

            var result = triangle.Area();
            
            Assert.That(result, Is.EqualTo(0.625));
        }

        [Test]
        public void D_WithTestTriangle_ReturnsCorrectResult()
        {
            var triangle = MakeTriangle();

            var result = triangle.D();
            
            Assert.That(result, Is.EqualTo(1.25));
        }

        [Test]
        public void Dwu_WithTestTriangle_Returns3x3Matrices()
        {
            var triangle = MakeTriangle();

            var result = triangle.Dwu();
            
            Assert.That(result.dx0, Is.EqualTo(-0.8));
            Assert.That(result.dx1, Is.EqualTo(1.2));
            Assert.That(result.dx2, Is.EqualTo(-0.4));
        }
        
        [Test]
        public void Dwv_WithTestTriangle_Returns3x3Matrices()
        {
            var triangle = MakeTriangle();

            var result = triangle.Dwv();

            Assert.That(result.dx0, Is.EqualTo(-0.4));
            Assert.That(result.dx1, Is.EqualTo(-0.4));
            Assert.That(result.dx2, Is.EqualTo(0.8));
        }
        
        #region Helpers

        private static RestSpaceTriangle MakeTriangle()
        {
            return new RestSpaceTriangle(
                math.double2(1, 0.5), 
                math.double2(2, 1), 
                math.double2(1.5, 2));
        }
        
        #endregion
    }
}