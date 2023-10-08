using Conditions;
using Forces;
using NSubstitute;
using NUnit.Framework;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Forces
{
    public class ShearConditionForceCalculatorTests
    {
        [Test]
        public void GetForce_WithContrivedConditionQuantities_ReturnsExpectedForce()
        {
            var stubQuantities = Substitute.For<IShearConditionQuantities>();
            stubQuantities.C.Returns(2);
            stubQuantities.Dc.Returns(new WithRespectTo<double3>()
            {
                dx0 = math.double3(1, 1, 1),
                dx1 = double3.zero,
                dx2 = double3.zero,
            });
            var forceCalculator = new ShearConditionForceCalculator(10, default, stubQuantities);

            var result = forceCalculator.GetForce(0);
            
            Assert.That(result, Is.EqualTo(math.double3(-20, -20, -20)));
        }
    }
}