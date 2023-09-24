using System;
using Conditions;
using NUnit.Framework;
using Triangles;
using Unity.Mathematics;

namespace Pika.Continuum.Cloth.UnitTests.Conditions
{
    public class StretchConditionsTest
    {
        [Test]
        public void GetDeformationMapDerivatives_WithMatchingWorldAndRestWithDefaultB_ReturnsZero()
        {
            var (rest, world) = MakeMatchingTriangles();

            var result = StretchCondition.GetDeformationMapDerivatives(rest, world);
            
            Assert.That(math.length(result.wu), Is.EqualTo(1));
            Assert.That(math.length(result.wv), Is.EqualTo(1));
        }

        [Test]
        public void GetCondition_WithEdgesAtRestConfiguration_ReturnsZero()
        {
            var wu = Wu;
            var wv = Wv;
            var a = Area;
            var b = DefaultB;

            var result = StretchCondition.GetCondition(wu, wv, a, b);
            
            Assert.That(result.cu, Is.Zero);
            Assert.That(result.cv, Is.Zero);
        }
        
        #region Helpers

        private static (RestSpaceTriangle, WorldSpaceTriangle) MakeMatchingTriangles()
        {
            var p0 = math.double2(1, 0.5);
            var p1 = math.double2(2, 1);
            var p2 = math.double2(1.5, 2);

            var restSpace = new RestSpaceTriangle(p0, p1, p2);
            var worldSpace = new WorldSpaceTriangle(
                math.double3(p0, 0), 
                math.double3(p1, 0),
                math.double3(p2, 0));

            return (restSpace, worldSpace);
        }

        private static StretchConditionQuantities MakeStretchQuantities()
        {
            var (rest, world) = MakeMatchingTriangles();
            var velocities = Tuple.Create(double3.zero, double3.zero, double3.zero);

            return new StretchConditionQuantities(rest, world, DefaultB, velocities);
        }

        private static double2 DefaultB => math.double2(1, 1);
        private static double Area => 0.625;
        private static double3 Wu => math.double3(1, 0, 0);
        private static double3 Wv => math.double3(0, -1, 0);

        #endregion
    }
}