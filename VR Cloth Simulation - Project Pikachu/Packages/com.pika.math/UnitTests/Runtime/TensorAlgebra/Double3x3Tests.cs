using NUnit.Framework;
using TensorAlgebra;
using Unity.Mathematics;

namespace UnitTests.Runtime.TensorAlgebra
{
    public class Double3x3Tests
    {
        [Test]
        public void Dot_WithOrthogonalVector_ReturnsDotProductOfEachVector()
        {
            var m = math.double3x3(
                math.double3(5, 5, 5),
                math.double3(3, 3, 3),
                math.double3(2, 2, 2));

            var v = math.double3(1, 0.5, 2);

            var result = m.Dot(v);
            
            Assert.That(result, Is.EqualTo(math.double3(17.5, 10.5, 7)));
        }
    }
}