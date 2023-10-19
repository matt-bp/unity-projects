using NUnit.Framework;
using TensorAlgebra;
using Unity.Mathematics;

namespace UnitTests.Runtime.TensorAlgebra
{
    public class Double3Tests
    {
        [Test]
        public void Dot_WithMatrix_ReturnsDotOfEachRow()
        {
            var m = math.double3x3(
                math.double3(5, 3, 2),
                math.double3(5, 3, 2),
                math.double3(5, 3, 2));

            var v = math.double3(1, 0.5, 2);

            var result = v.Dot(m);
            
            Assert.That(result, Is.EqualTo(math.double3(17.5, 10.5, 7)));
        }
    }
}