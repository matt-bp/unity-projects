using System;
using Conditions.New;
using LinearAlgebra;
using Triangles;
using Unity.Mathematics;
using NSubstitute;
using NUnit.Framework;

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

            var b = math.double2(1, 1);
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

            var b = math.double2(1, 1);
            var stretchQuantities = new StretchConditionQuantities(stubCombined, b);

            var result = stretchQuantities.Cv;
            
            Assert.That(result, Is.Zero);
        }
        
        #region Helpers
        
        #endregion
    }
}