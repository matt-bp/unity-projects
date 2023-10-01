using Conditions;
using Forces;
using NSubstitute;
using NUnit.Framework;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Forces
{
    public class ConditionForcesTests
    {
        [Test]
        public void GetForce_WithChosenVariables_ReturnsCorrectResult()
        {
            var stubQuantities = Substitute.For<IConditionQuantities>();
            stubQuantities.Dcu.Returns(new WithRespectTo<double3>()
            {
                dx0 = math.double3(1, 0, 1),
                dx1 = double3.zero,
                dx2 = double3.zero
            });
            stubQuantities.Dcv.Returns(new WithRespectTo<double3>()
            {
                dx0 = math.double3(0, 1, 0),
                dx1 = double3.zero,
                dx2 = double3.zero
            });
            stubQuantities.Cu.Returns(0.5);
            stubQuantities.Cv.Returns(2);
            const int k = 3;
            var cf = new ConditionForces(k, default, stubQuantities);

            var result = cf.GetForce(0);
            
            Assert.That(result, Is.EqualTo(math.double3(-1.5, -6, -1.5)));
        }

        [Test]
        public void GetDampingForce_OnFirstParticle_ReturnsResultsFromEquation()
        {
            var stubQuantities = Substitute.For<IConditionQuantities>();
            stubQuantities.Dcu.Returns(new WithRespectTo<double3>()
            {
                dx0 = math.double3(1, 0, 1),
                dx1 = double3.zero,
                dx2 = double3.zero
            });
            stubQuantities.Dcv.Returns(new WithRespectTo<double3>()
            {
                dx0 = math.double3(0, 1, 0),
                dx1 = double3.zero,
                dx2 = double3.zero
            });
            stubQuantities.CuDot.Returns(0.5);
            stubQuantities.CvDot.Returns(2);
            const int kd = 3;
            var cf = new ConditionForces(default, kd, stubQuantities);

            var result = cf.GetDampingForce(0);
            
            Assert.That(result, Is.EqualTo(math.double3(-1.5, -6, -1.5)));
        }
    }
}