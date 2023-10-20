using LinearAlgebra;
using NUnit.Framework;
using Unity.Mathematics;

namespace UnitTests.Runtime.LinearAlgebra
{
    public class SkewMatrixTests
    {
        [Test]
        public void MakeFromVector_WithUniqueVectorElements_ReturnsCorrectShape()
        {
            var v = math.double3(1, 2, 3);

            var result = SkewMatrix.MakeFromVector(v);
            
            // Based on section 2 paragraph 2 of Pritchard, each index into this matrix (really a way to pass around 3 vectors),
            //  should be column vectors representing rows of the underlying matrix.
            Assert.That(result[0], Is.EqualTo(math.double3(0, -3, 2)));
            Assert.That(result[1], Is.EqualTo(math.double3(3, 0, -1)));
            Assert.That(result[2], Is.EqualTo(math.double3(-2, 1, 0)));
        }
    }
}