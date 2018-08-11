using FluentAssertions;
using NUnit.Framework;
using Vostok.Sys.Metrics.Windows.Helpers;

namespace Vostok.Sys.Metrics.Windows.Tests.Unit
{
    public class ArrayBuffer_Tests
    {
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(10, 1)]
        [TestCase(1, 17)]
        [TestCase(1000, 1)]
        [TestCase(1000, 10)]
        [TestCase(1000, 100)]
        public void Should_return_array_with_enough_size(int size, int step)
        {
            var buffer = new ResizeableBuffer();
            for (var i = 0; i <= size; i++)
                buffer.Get(i).Length.Should().BeGreaterOrEqualTo(i);
        }
        
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void Should_not_waste_memory_with_large_arrays(int size)
        {
            var buffer = new ResizeableBuffer();
            buffer.Get(size).Length.Should().BeLessThan(2 * size);
        }
        
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 16)]
        [TestCase(100, 130)]
        [TestCase(100, 1)]
        public void Should_not_recreate_buffer(int size1, int size2)
        {
            var buffer = new ResizeableBuffer();
            var arr1 = buffer.Get(size1);
            var arr2 = buffer.Get(size2);
            arr2.Should().BeSameAs(arr1);
        }
        
        [TestCase(10*1024*1024, 1)]
        public void Should_recreate_large_buffers(int size1, int size2)
        {
            var buffer = new ResizeableBuffer();
            var arr1 = buffer.Get(size1);
            var arr2 = buffer.Get(size2);
            arr2.Should().NotBeSameAs(arr1);
        }
        
        [TestCase(10*1024*1024, 1)]
        public void Should_not_recreate_large_buffers_with_opt_in(int size1, int size2)
        {
            var buffer = new ResizeableBuffer();
            var arr1 = buffer.Get(size1);
            var arr2 = buffer.Get(size2, false);
            arr2.Should().BeSameAs(arr1);
        }
    }
}