using System.Collections.Generic;
using Geometry;
using NUnit.Framework;

namespace PIka.Geometry.UnitTests.Runtime.Geometry
{
    public class TrianglePairTests
    {
        [Test]
        public void MakeFromSharedEdges_WithOneTriangle_ReturnsEmptyList()
        {
            var triangles = new List<(int, int, int)>()
            {
                (0, 1, 2),
            };

            var results = TrianglePair.MakeFromSharedEdges(triangles);

            Assert.That(results, Has.Count.EqualTo(0));
        }

        [Test]
        public void MakeFromSharedEdges_WithTwoTrianglesSharing_ReturnsThatTrianglePair()
        {
            var triangles = new List<(int, int, int)>()
            {
                (0, 1, 2),
                (2, 1, 3)
            };
            
            var results = TrianglePair.MakeFromSharedEdges(triangles);

            Assert.That(results, Has.Count.EqualTo(1));
            Assert.That(results[0], Is.EqualTo((3, 2, 1, 0)));
        }

        [Test]
        public void MakeFromSharedEdges_ToTriangleNotSharing_ReturnsEmptyList()
        {
            var triangles = new List<(int, int, int)>()
            {
                (0, 1, 2),
                (4, 1, 3)
            };
            
            var results = TrianglePair.MakeFromSharedEdges(triangles);

            Assert.That(results, Has.Count.EqualTo(0));
        }

        [Test]
        public void MakeFromSharedEdges_SixSharingTriangles_ReturnsSixTrianglePairs()
        {
            var triangles = new List<(int, int, int)>()
            {
                (0, 1, 2),
                (3, 2, 1),
                (4, 1, 0),
                (1, 4, 6),
                (6, 5, 1), // If winding order is inconsistent, (6, 1, 5), an exception will be thrown
                (5, 3, 1),
            };
            
            var results = TrianglePair.MakeFromSharedEdges(triangles);
            
            Assert.That(results, Contains.Item((3, 2, 1, 0)));
            Assert.That(results, Contains.Item((4, 1, 0, 2)));
            Assert.That(results, Contains.Item((6, 1, 4, 0)));
            Assert.That(results, Contains.Item((5, 1, 6, 4)));
            Assert.That(results, Contains.Item((3, 1, 5, 6)));
            Assert.That(results, Contains.Item((5, 3, 1, 2)));
        }
    }
}