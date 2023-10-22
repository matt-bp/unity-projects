using System.Collections.Generic;
using Conditions;
using NUnit.Framework;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Conditions
{
    public class BendConditionQuantitiesTests
    {
        [Test]
        public void C_OnSimpleRestQuad_ReturnsZero()
        {
            var bq = MakeRestQuantities();

            var result = bq.C;

            Assert.That(result, Is.Zero);
        }

        [Test]
        public void C_OnBentQuad_ReturnsHalfPi()
        {
            var bq = MakeBentQuadQuantities();

            var result = bq.C;
            
            Assert.That(result, Is.EqualTo(math.PI_DBL / 2));
        }
        
        [Test, Ignore("Cuz")]
        public void Dc_OnRestQuad_ReturnsZero()
        {
            var bq = MakeRestQuantities();

            var result = bq.Dc;

            Assert.That(result.Dx0, Is.EqualTo(double3.zero));
            Assert.That(result.Dx1, Is.EqualTo(double3.zero));
            Assert.That(result.Dx2, Is.EqualTo(double3.zero));
            Assert.That(result.Dx3, Is.EqualTo(double3.zero));
        }
        
        [Test]
        public void Dc_OnBentTrianglePair_ReturnsUnknown()
        {
            var bq = MakeBentQuadQuantities();

            var result = bq.Dc;

            // Negative, because it's the second half of the equation 54.
            Assert.That(result.Dx0.x, Is.EqualTo(-0.25).Within(0.001));
        }
        
        #region Helpers

        private static IBendConditionQuantities MakeRestQuantities()
        {
            var x0 = math.double3(3, 1, 2);
            var x1 = math.double3(1, 1, 3);
            var x2 = math.double3(1, 1, 1);
            var x3 = math.double3(-1, 1, 2);

            var bq = new BendConditionQuantities(x0, x1, x2, x3, new List<double3>());

            return bq;
        }

        /// <summary>
        /// The triangle is bent by 90 degrees.
        /// </summary>
        /// <returns>Quantities associated with these triangle.</returns>
        private static IBendConditionQuantities MakeBentQuadQuantities()
        {            
            var x0 = math.double3(3, -1, 2);
            var x1 = math.double3(1, 1, 3);
            var x2 = math.double3(1, 1, 1);
            var x3 = math.double3(-1, -1, 2);

            var bq = new BendConditionQuantities(x0, x1, x2, x3, new List<double3>());

            return bq;
        }
        
        #endregion
    }
}