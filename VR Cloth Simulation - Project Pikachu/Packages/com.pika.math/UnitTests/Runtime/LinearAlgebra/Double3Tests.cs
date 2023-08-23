using LinearAlgebra;
using NUnit.Framework;
using Unity.Mathematics;

namespace UnitTests.Runtime.LinearAlgebra
{
    public class Double3Tests
    {
        [Test]
        public void OuterProduct_WithNonZeroVectors_ReturnsOuterProduct()
        {
            var column = math.double3(2, 4, 6);
            var row = math.double3(3, 5, 7);
            var expected = math.double3x3(6, 10, 14,  12, 20, 28,  18, 30, 42);

            var result = Double3.OuterProduct(column, row);
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}