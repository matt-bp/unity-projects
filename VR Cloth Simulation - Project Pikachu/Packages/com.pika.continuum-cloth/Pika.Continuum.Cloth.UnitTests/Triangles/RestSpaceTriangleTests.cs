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
        public void D_WithTestTriangle_ReturnsOne()
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

            AssertOnlyDiagonalEntriesAreSet(result.dx0, -0.8);
            AssertOnlyDiagonalEntriesAreSet(result.dx1, 1.2);
            AssertOnlyDiagonalEntriesAreSet(result.dx2, -0.4);
        }
        
        #region Helpers

        private static RestSpaceTriangle MakeTriangle()
        {
            return new RestSpaceTriangle(
                math.double2(1, 0.5), 
                math.double2(2, 1), 
                math.double2(1.5, 2));
        }

        private static void AssertOnlyDiagonalEntriesAreSet(double3x3 mat, double value)
        {
            Assert.That(mat[0][0], Is.EqualTo(value));
            Assert.That(mat[0][1], Is.EqualTo(0));
            Assert.That(mat[0][2], Is.EqualTo(0));
            
            Assert.That(mat[1][0], Is.EqualTo(0));
            Assert.That(mat[1][1], Is.EqualTo(value));
            Assert.That(mat[1][2], Is.EqualTo(0));
            
            Assert.That(mat[2][0], Is.EqualTo(0));
            Assert.That(mat[2][1], Is.EqualTo(0));
            Assert.That(mat[2][2], Is.EqualTo(value));
        }
        
        #endregion
    }
}