using System;
using System.Collections.Generic;
using Conditions;
using LinearAlgebra;
using Triangles;
using Unity.Mathematics;
using NSubstitute;
using NUnit.Framework;
using StretchConditionQuantities = Conditions.StretchConditionQuantities;
using static Pika.Continuum.Cloth.UnitTests.PikaAssert;

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
            var stretchQuantities = new StretchConditionQuantities(stubCombined, b, new List<double3>());

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
            var stretchQuantities = new StretchConditionQuantities(stubCombined, b, new List<double3>());

            var result = stretchQuantities.Cv;
            
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void Dcu_AtRestConfig_ReturnsExpectedResult()
        {
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.A.Returns(0.625);
            stubCombined.Wu.Returns(math.double3(1.6, 0.2, 0));
            stubCombined.Dwu.Returns(new WithRespectTo<double>
            {
                dx0 = -0.8,
                dx1 = 1.2,
                dx2 = -0.4
            });
            var b = MakeBAtRest();
            var stretchQuantities = new StretchConditionQuantities(stubCombined, b, new List<double3>());

            var result = stretchQuantities.Dcu;

            // I'm getting these expected values by multiplying each Jacobian by Wu norm and A.
            AssertDouble3WithinTolerance(result.dx0, math.double3(-0.496138938, -0.062017367, 0), 0.0001);
            AssertDouble3WithinTolerance(result.dx1, math.double3(0.744208408, 0.093026051, 0), 0.0001);
;           AssertDouble3WithinTolerance(result.dx2, math.double3(-0.248069469, -0.031008684, 0), 0.0001);
        }

        [Test]
        public void Dcv_AtOneStepInSimulation_ReturnsExpectedResult()
        {
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.A.Returns(0.625);
            stubCombined.Wv.Returns(math.double3(-0.2, 0.6, 0));
            stubCombined.Dwv.Returns(new WithRespectTo<double>
            {
                dx0 = -0.4,
                dx1 = -0.4,
                dx2 = 0.8
            });
            var b = MakeBAtRest();
            var stretchQuantities = new StretchConditionQuantities(stubCombined, b, new List<double3>());

            var result = stretchQuantities.Dcv;

            // I'm getting these expected values by multiplying each Jacobian by Wv norm and A.
            AssertDouble3WithinTolerance(result.dx0, math.double3(0.079056942, -0.237170825, 0), 0.0001);
            AssertDouble3WithinTolerance(result.dx1, math.double3(0.079056942, -0.237170825, 0), 0.0001);
            AssertDouble3WithinTolerance(result.dx2, math.double3(-0.158113883, 0.474341649, 0), 0.0001);
        }
        
        #region Helpers

        private static double2 MakeBAtRest() => math.double2(1, 1);
        
        #endregion
    }
}