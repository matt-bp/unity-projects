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
            stubCombined.Wu.Returns(math.double3(1, 0, 0));
            stubCombined.Dwu.Returns(new WithRespectTo<double3x3>
            {
                dx0 = double3x3.identity * -0.8,
                dx1 = double3x3.identity * 1.2,
                dx2 = double3x3.identity * -0.4
            });
            var b = MakeBAtRest();
            var stretchQuantities = new StretchConditionQuantities(stubCombined, b);

            var result = stretchQuantities.Dcu;

            Assert.That(result.dx0, Is.EqualTo(math.double3(0, 0, 0)));
            Assert.That(result.dx1, Is.EqualTo(math.double3(0, 0, 0)));
            Assert.That(result.dx2, Is.EqualTo(math.double3(0, 0, 0)));
        }
        
        #region Helpers

        private static double2 MakeBAtRest() => math.double2(1, 1);

        #endregion
    }
}