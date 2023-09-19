using NUnit.Framework;
using Triangles;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Triangles
{
    public class RestSpaceTriangleTests
    {
        [Test]
        public void Area_WithHalfAUnitTriangle_ReturnsOne()
        {
            var triangle = MakeTriangle();

            var result = triangle.Area();
            
            Assert.That(result, Is.EqualTo(0.625));
        }

        [Test]
        public void D_WithHalfTriangle_ReturnsOne()
        {
            var triangle = MakeTriangle();

            var result = triangle.D();
            
            Assert.That(result, Is.EqualTo(1.25));
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