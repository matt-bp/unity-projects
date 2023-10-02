using System.Collections.Generic;
using Conditions;
using NSubstitute;
using NUnit.Framework;
using Triangles;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Conditions
{
    public class ShearConditionQuantitiesTests
    {
        [Test]
        public void C_OnRest_ReturnsZero()
        {
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.A.Returns(0.625);
            stubCombined.Wu.Returns(math.double3(1, 0, 1));
            stubCombined.Wv.Returns(math.double3(1, 0, 1));
            var stretchQuantities = new ShearConditionQuantities(stubCombined);

            var result = stretchQuantities.C;
            
            Assert.That(result, Is.EqualTo(1.25));
        }
    }
}