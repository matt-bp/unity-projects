﻿using System.Collections.Generic;
using Conditions;
using NSubstitute;
using NUnit.Framework;
using Triangles;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Conditions
{
    public class ShearConditionQuantitiesTests
    {
        /// <summary>
        /// This condition test was based on the following matching world and rest space triangle:
        /// (-0.5, 0.5), (0.5, 0.5), (0.5, -0.5).
        /// </summary>
        [Test]
        public void C_OnSimpleRest_ReturnsZero()
        {
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.A.Returns(0.5);
            stubCombined.Wu.Returns(math.double3(1, 0, 0));
            stubCombined.Wv.Returns(math.double3(0, 1, 0));
            var stretchQuantities = new ShearConditionQuantities(stubCombined, new List<double3>());

            var result = stretchQuantities.C;
            
            Assert.That(result, Is.EqualTo(0));
        }
        
        [Test]
        public void C_OnOffsetRest_ReturnsDotMultipliedByArea()
        {
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.A.Returns(0.625);
            stubCombined.Wu.Returns(math.double3(1, 0, 1));
            stubCombined.Wv.Returns(math.double3(1, 0, 1));
            var stretchQuantities = new ShearConditionQuantities(stubCombined, new List<double3>());
        
            var result = stretchQuantities.C;
            
            Assert.That(result, Is.EqualTo(1.25));
        }

        [Test]
        public void D2C_OnSimpleRest_ReturnsIdentityMultipliedByAScalar()
        {
            var stubCombined = Substitute.For<ICombinedTriangle>();
            stubCombined.Dwu.Returns(new WithRespectTo<double3x3>()
            {
               dx0 = math.double3x3(2),
               dx1 = math.double3x3(2),
               dx2 = math.double3x3(2)
            });
            stubCombined.Dwv.Returns(new WithRespectTo<double3x3>()
            {
                dx0 = math.double3x3(2),
                dx1 = math.double3x3(2),
                dx2 = math.double3x3(2)
            });
            var stretchQuantities = new ShearConditionQuantities(stubCombined, new List<double3>());

            var result = stretchQuantities.D2C;

            AssertDouble3X3HasSameShapeAsIdentity(result.dx0.dx0);
            AssertDouble3X3HasSameShapeAsIdentity(result.dx0.dx1);
            AssertDouble3X3HasSameShapeAsIdentity(result.dx0.dx2);
            
            AssertDouble3X3HasSameShapeAsIdentity(result.dx1.dx0);
            AssertDouble3X3HasSameShapeAsIdentity(result.dx1.dx1);
            AssertDouble3X3HasSameShapeAsIdentity(result.dx1.dx2);
            
            AssertDouble3X3HasSameShapeAsIdentity(result.dx2.dx0);
            AssertDouble3X3HasSameShapeAsIdentity(result.dx2.dx1);
            AssertDouble3X3HasSameShapeAsIdentity(result.dx2.dx2);
        }
        
        #region Helpers

        private static void AssertDouble3X3HasSameShapeAsIdentity(double3x3 mat)
        {
            Assert.That(mat[0][0], Is.Not.Zero);
            Assert.That(mat[0][1], Is.Zero);
            Assert.That(mat[0][2], Is.Zero);
            
            Assert.That(mat[1][0], Is.Zero);
            Assert.That(mat[1][1], Is.Not.Zero);
            Assert.That(mat[1][2], Is.Zero);
            
            Assert.That(mat[2][0], Is.Zero);
            Assert.That(mat[2][1], Is.Zero);
            Assert.That(mat[2][2], Is.Not.Zero);
        }
        
        #endregion
    }
}