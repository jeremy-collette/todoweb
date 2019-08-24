namespace todoweb.Server.UnitTest
{
    using System.Collections.Generic;

    using FluentAssertions;
    using Xunit;

    using todoweb.Server.Core;

    public class PasswordHasherTests
    {
        [Fact]
        void CorrectPasswordTest()
        {
            var passwordHasher = new PasswordHasher();
            var password = "foo";
            var hash = passwordHasher.GetHash(password);
            passwordHasher.Verify(password, hash).Should().BeTrue();
        }

        [Fact]
        void IncorrectPasswordTest()
        {
            var passwordHasher = new PasswordHasher();
            var hash = passwordHasher.GetHash("foo");
            passwordHasher.Verify("bar", hash).Should().BeFalse();
        }

        [Fact]
        void DifferentHashesTest()
        {
            var passwordHasher = new PasswordHasher();
            var hashSet = new HashSet<byte[]>();
            for (var i=0; i < 100; ++i)
            {
                var hash = passwordHasher.GetHash("foo");
                hashSet.Should().NotContain(hash);
                hashSet.Add(hash);
            }
        }
    }
}
