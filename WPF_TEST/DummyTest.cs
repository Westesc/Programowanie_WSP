using System;

namespace WPF_TEST {
    public class DummyTest {

        [Fact(Skip = "Skipped because reasons...")]
        public void FactFalse() {
            Assert.False(false);
        }

        [Fact]
        public void FactTrue() {
            Assert.True(true);
        }

        [Fact]
        public void IntegerIs32() {
            const int expectedValue = 32;
            int testedValue = 8 * 4;

            Assert.Equal(expectedValue, testedValue);
        }

        [Fact]
        public void ThrowsException() {
            Assert.ThrowsAsync<InvalidOperationException>(
                () => throw new InvalidOperationException()
            ) ;
        }

    }
}