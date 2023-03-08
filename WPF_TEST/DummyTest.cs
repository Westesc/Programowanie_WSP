using System;

namespace WPF_TEST {
    public class DummyTest {

        Project_wps.App app = new App();

        [Fact/*(Skip = "Skipped because reasons...")*/]
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
        public void AddTest()
        {
            Assert.Equal(7, app.Add(3, 4));
        }

        [Fact]
        public void ThrowsException() {
            Assert.ThrowsAsync<InvalidOperationException>(
                () => throw new InvalidOperationException()
            ) ;
        }

    }
}