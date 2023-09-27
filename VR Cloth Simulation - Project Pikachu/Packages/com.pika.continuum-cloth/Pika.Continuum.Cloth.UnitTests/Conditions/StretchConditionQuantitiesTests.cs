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
        public void StretchConditionQuantities_CuOnRest_ReturnsZero()
        {
            var stretchQuantities = MakeStretchQuantities();

            var cu = stretchQuantities.Cu;
            
            Assert.That(cu, Is.Zero);
        }
        
        #region Helpers

        private static StretchConditionQuantities MakeStretchQuantities()
        {
            const double area = 0.625;
            var b = math.double2(1, 1);
            
            
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.A.Returns(area);
            stubCombined.Wu.Returns(math.double3(1, 0, 0));

            var velocities = Tuple.Create(double3.zero, double3.zero, double3.zero);

            return new StretchConditionQuantities(stubCombined, b, velocities);
        }

        #endregion
    }
}