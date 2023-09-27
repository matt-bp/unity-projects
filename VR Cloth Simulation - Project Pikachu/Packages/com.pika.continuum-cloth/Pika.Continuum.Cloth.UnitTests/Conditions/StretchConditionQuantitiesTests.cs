using System;
using Conditions;
using LinearAlgebra;
using Triangles;
using Unity.Mathematics;
using NSubstitute;
using NUnit.Framework;
using StretchConditionQuantities = Conditions.New.StretchConditionQuantities;

namespace Pika.Continuum.Cloth.UnitTests.Conditions
{
    public class StretchConditionQuantitiesTests
    {
        [Test]
        public void Cu_OnRest_ReturnsZero()
        {
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.A.Returns(0.625);
            stubCombined.Wu.Returns(math.double3(1, 0, 0));

            var b = MakeBAtRest();
            var stretchQuantities = new StretchConditionQuantities(stubCombined, b);

            var result = stretchQuantities.Cu;
            
            Assert.That(result, Is.Zero);
        }
        
        [Test]
        public void Cv_OnRest_ReturnsZero()
        {
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.A.Returns(0.625);
            stubCombined.Wv.Returns(math.double3(0, -1, 0));

            var b = MakeBAtRest();
            var stretchQuantities = new StretchConditionQuantities(stubCombined, b);

            var result = stretchQuantities.Cv;
            
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void Dcu_AtRestConfig_ReturnsExpectedResult()
        {
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.A.Returns(0.625);
            stubCombined.Wu.Returns(math.double3(1.6, 0.2, 0));
            stubCombined.Dwu.Returns(new WithRespectTo<double3x3>
            {
                dx0 = double3x3.identity * -0.8,
                dx1 = double3x3.identity * 1.2,
                dx2 = double3x3.identity * -0.4
            });
            var b = MakeBAtRest();
            var stretchQuantities = new StretchConditionQuantities(stubCombined, b);

            var result = stretchQuantities.Dcu;

            // I'm getting these expected values by multiplying each Jacobian by Wu norm and A.
            AssertDouble3WithinTolerance(result.dx0, math.double3(-0.496138938, -0.062017367, 0), 0.0001);
            AssertDouble3WithinTolerance(result.dx1, math.double3(0.744208408, 0.093026051, 0), 0.0001);
;           AssertDouble3WithinTolerance(result.dx2, math.double3(-0.248069469, -0.031008684, 0), 0.0001);
        }
        
        #region Helpers

        private static double2 MakeBAtRest() => math.double2(1, 1);
        
        private static void AssertDouble3WithinTolerance(double3 result, double3 expected, double tolerance)
        {
            Assert.That(result.x, Is.EqualTo(expected.x).Within(tolerance));
        }
        
        #endregion
    }
}