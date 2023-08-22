using LinearAlgebra;
using NUnit.Framework;
using Unity.Mathematics;

namespace UnitTests.Runtime.LinearAlgebra
{
    public class Double2Tests
    {
        [Test]
        public void OuterProduct_WithNonZeroVectors_ReturnsOuterProduct()
        {
            var column = math.double2(2, 4);
            var row = math.double2(3, 5);
            var expected = math.double2x2(6, 10, 12, 20);

            var result = Double2.OuterProduct(column, row);
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}