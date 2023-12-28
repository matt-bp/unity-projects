using NUnit.Framework;
using Wright.Library.File;

namespace UnitTests.Wright.Library.File
{
    public class FilenameSanitizerTests
    {
        [Test]
        public void Sanitize_WithDashed_ReplacedWithUnderscore()
        {
            const string input = @"Hello\e.dat";
            const string expected = "Hello_e.dat";

            var result = FilenameSanitizer.Sanitize(input);
            
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void Sanitize_WithColon_ReplacedWithUnderscore()
        {
            const string input = @"Hello:e.dat";
            const string expected = "Hello_e.dat";

            var result = FilenameSanitizer.Sanitize(input);
            
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}