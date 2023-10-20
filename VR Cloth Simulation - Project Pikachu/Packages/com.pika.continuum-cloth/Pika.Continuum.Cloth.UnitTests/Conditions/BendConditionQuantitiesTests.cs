using Conditions;
using LinearAlgebra;
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

        [Test]
        public void Matrix()
        {
            var a = math.double3x3(1, 2, 3, 4, 5, 6, 7, 8, 9);

            var one = a[0];
            Assert.That(one, Is.EqualTo(math.double3(1, 4, 7)));

        }

        [Test]
        public void Dc_OnRestQuad_ReturnsZero()
        {
            var bq = MakeRestQuantities();

            var result = bq.Dc;

            Assert.That(result.Dx0, Is.EqualTo(double3.zero));
        }
        
        #region Helpers

        private static IBendConditionQuantities MakeRestQuantities()
        {
            var x0 = math.double3(3, 1, 2);
            var x1 = math.double3(1, 1, 3);
            var x2 = math.double3(1, 1, 1);
            var x3 = math.double3(-1, 1, 2);

            var bq = new BendConditionQuantities(x0, x1, x2, x3);

            return bq;
        }

        private static IBendConditionQuantities MakeBentQuadQuantities()
        {            
            var x0 = math.double3(3, -1, 2);
            var x1 = math.double3(1, 1, 3);
            var x2 = math.double3(1, 1, 1);
            var x3 = math.double3(-1, -1, 2);

            var bq = new BendConditionQuantities(x0, x1, x2, x3);

            return bq;
        }
        
        #endregion
    }
}