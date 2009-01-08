using System;
using System.Collections;
using System.Linq;

using Moq;
using Xunit;

namespace Kigg.Web.Test
{
    using Infrastructure;

    public class BlockedIPCollectionFixture
    {
        private readonly Mock<IFile> _file;
        private readonly BlockedIPCollection _blockedIPCollection;

        public BlockedIPCollectionFixture()
        {
            _file = new Mock<IFile>();

            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns(string.Empty);

            _blockedIPCollection = new BlockedIPCollection("BlockedIPList.txt", _file.Object);
        }

        [Fact]
        public void When_Created_Collection_Should_Contain_The_IpAddress_Of_File()
        {
            _file.Expect(f => f.ReadAllText(It.IsAny<string>())).Returns("192.168.0.1{0}192.168.0.2".FormatWith(Environment.NewLine));

            var collection = new BlockedIPCollection("BlockedIPList.txt", _file.Object);

            Assert.Contains("192.168.0.1", collection);
            Assert.Contains("192.168.0.2", collection);
        }

        [Fact]
        public void Count_Should_Return_Zero_When_Collection_Is_Empty()
        {
            Assert.Equal(0, _blockedIPCollection.Count);
        }

        [Fact]
        public void IsReadOnly_Should_Always_False()
        {
            Assert.False(_blockedIPCollection.IsReadOnly);
        }

        [Fact]
        public void GetEnumerator_Should_Return_Enumerator()
        {
            var iterator = _blockedIPCollection.GetEnumerator();

            Assert.DoesNotThrow(() => iterator.MoveNext());
        }

        [Fact]
        public void GetEnumerator_Of_IEnumerable_Should_Return_Enumerator()
        {
            var iterator = ((IEnumerable)_blockedIPCollection).GetEnumerator();

            Assert.DoesNotThrow(() => iterator.MoveNext());
        }

        [Fact]
        public void Add_Should_Increase_Collection()
        {
            _blockedIPCollection.Add("192.168.0.1");

            Assert.Equal(1, _blockedIPCollection.Count);
        }

        [Fact]
        public void Add_Should_Not_Increase_Collection_When_Duplicate_Ip_Is_Specified()
        {
            _blockedIPCollection.Add("192.168.0.1");
            _blockedIPCollection.Add("192.168.0.1");

            Assert.Equal(1, _blockedIPCollection.Count);
        }

        [Fact]
        public void Clear_Should_Make_The_Collection_Empty()
        {
            _blockedIPCollection.Add("192.168.0.1");
            _blockedIPCollection.Clear();

            Assert.Empty(_blockedIPCollection);
        }

        [Fact]
        public void Contains_Should_Return_True_When_IpAaddress_Exists_In_Collection()
        {
            _blockedIPCollection.Add("192.168.0.1");

            Assert.True(_blockedIPCollection.Contains("192.168.0.1"));
        }

        [Fact]
        public void CopyTo_Should_Copy_The_IPAaddresses_In_The_Provided_Array()
        {
            _blockedIPCollection.Add("192.168.0.1");
            _blockedIPCollection.Add("192.168.0.2");

            var array = new string[_blockedIPCollection.Count];

            _blockedIPCollection.CopyTo(array, 0);

            _blockedIPCollection.ForEach(ip => Assert.True(array.Contains(ip)));
        }

        [Fact]
        public void Remove_Should_Decrease_Collection()
        {
            _blockedIPCollection.Add("192.168.0.1");
            _blockedIPCollection.Remove("192.168.0.1");

            Assert.Equal(0, _blockedIPCollection.Count);
        }

        [Fact]
        public void AddRange_Should_Increase_Collection()
        {
            _blockedIPCollection.AddRange(new[] { "192.168.0.1", "192.168.0.2" });

            Assert.Equal(2, _blockedIPCollection.Count);
        }

        [Fact]
        public void RemoveRange_Should_Decrease_Collection()
        {
            _blockedIPCollection.AddRange(new[] { "192.168.0.1", "192.168.0.2" });
            _blockedIPCollection.RemoveRange(new[] { "192.168.0.1", "192.168.0.2" });

            Assert.Equal(0, _blockedIPCollection.Count);
        }

        [Fact]
        public void Dispose_Should_Write_The_IpAddresses_In_File()
        {
            _file.Expect(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            _blockedIPCollection.Dispose();

            _file.Verify();
        }
    }
}