using LinearAlgebra;
using NSubstitute;
using NUnit.Framework;
using Triangles;
using Unity.Mathematics;
using static Pika.Continuum.Cloth.UnitTests.PikaAssert;

namespace Pika.Continuum.Cloth.UnitTests.Triangles
{
    public class CombinedTriangleTests
    {
        [Test]
        public void A_OnTestTriangle_ReturnsOne()
        {
            var stubRest = Substitute.For<IRestSpaceTriangle>();
            stubRest.Area().Returns(0.625);
            var stubWorld = Substitute.For<IWorldSpaceTriangle>();
            var combined = new CombinedTriangle(stubRest, stubWorld);

            var result = combined.A;
            
            Assert.That(result, Is.EqualTo(0.625));
        }

        /// <summary>
        /// For this test, I had the 1th world position actually be (2.5, 1). So we get some change there.
        /// </summary>
        [Test]
        public void Wu_OnTestTriangle_ReturnsMapDerivative()
        {
            var stubRest = Substitute.For<IRestSpaceTriangle>();
            stubRest.Dv1.Returns(0.5);
            stubRest.Dv2.Returns(1.5);
            stubRest.D().Returns(1.25);
            var stubWorld = Substitute.For<IWorldSpaceTriangle>();
            stubWorld.Dx1.Returns(math.double3(1.5, 0.5, 0));
            stubWorld.Dx2.Returns(math.double3(0.5, 1, 0));
            var combined = new CombinedTriangle(stubRest, stubWorld);

            var result = combined.Wu;
            
            Assert.That(result, Is.EqualTo(math.double3(1.6, 0.2, 0)));
        }

        [Test]
        public void Wv_OnTestTriangle_ReturnsVMapDerivative()
        {
            var stubRest = Substitute.For<IRestSpaceTriangle>();
            stubRest.Du1.Returns(1);
            stubRest.Du2.Returns(0.5);
            stubRest.D().Returns(1.25);
            var stubWorld = Substitute.For<IWorldSpaceTriangle>();
            stubWorld.Dx1.Returns(math.double3(1.5, 0.5, 0));
            stubWorld.Dx2.Returns(math.double3(0.5, 1, 0));
            var combined = new CombinedTriangle(stubRest, stubWorld);

            var result = combined.Wv;
            
            AssertDouble3WithinTolerance(result, math.double3(-0.2, 0.6, 0), 0.0001);
        }
    }
}