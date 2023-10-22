using System.Collections.Generic;
using System.Linq;
using Geometry.Indices;
using NUnit.Framework;

namespace Pika.Geometry.UnitTests.Runtime.Geometry.Indices
{
    public class TriangleTests
    {
        [Test]
        public void GetTriangles_EmptyList_ReturnsNoTriangles()
        {
            var input = new List<int>();

            var result = Triangle.GetTrianglesFromFlatList(input);
            
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetTriangles_WithGroupOfThree_ReturnsThreeSeparateTriangles()
        {
            var input = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

            var result = Triangle.GetTrianglesFromFlatList(input).ToList();
            
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result, Does.Contain((0, 1, 2)));
            Assert.That(result, Does.Contain((3, 4, 5)));
            Assert.That(result, Does.Contain((6, 7, 8)));
        }
        
    }
}