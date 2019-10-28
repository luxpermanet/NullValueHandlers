using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NullValueHandlers.Test {
    [TestClass]
    public class OptionTest {
        [TestMethod]
        public void HasValueShouldReturnFalseWhenOptionHasNoValueTest() {
            // Arrange
            var option = Option<TestClass>.None;

            // Assert
            Assert.IsFalse(option.HasValue);
        }

        [TestMethod]
        public void OptionShouldNotEnumerateWhenItHasNoValueTest() {
            // Arrange
            var option = Option<TestClass>.None;

            // Act
            bool canEnumerate = false;
            foreach (TestClass unused in option) {
                canEnumerate = true;
            }

            // Assert
            Assert.IsFalse(canEnumerate);
        }

        [TestMethod]
        public void ValueShouldThrowInvalidOperationExceptionWhenOptionHasNoValueTest() {
            // Arrange
            var option = Option<TestClass>.None;

            // Act
            bool throws = ThrowsExact<InvalidOperationException>(() => {
                TestClass unused = option.Value;
            });

            // Assert
            Assert.IsTrue(throws);
        }

        [TestMethod]
        public void SingleShouldShouldThrowNullExceptionWhenParameterIsNullTest() {
            // Arrange
            bool throws = ThrowsExact<ArgumentNullException>(() => {
                var unused = Option<TestClass>.Single(null);
            });

            // Assert
            Assert.IsTrue(throws);
        }

        [TestMethod]
        public void HasValueShouldReturnTrueWhenOptionHasValueTest() {
            // Arrange
            var option = Option<TestClass>.Single(new TestClass());

            // Assert
            Assert.IsTrue(option.HasValue);
        }

        [TestMethod]
        public void ValueShouldReturnIntrinsicValueWhenOptionHasValueTest() {
            // Arrange
            TestClass expectedValue = new TestClass();
            var option = Option<TestClass>.Single(expectedValue);

            // Act
            TestClass value = option.Value;

            // Assert
            Assert.AreSame(expectedValue, value);
        }

        [TestMethod]
        public void OptionShouldEnumerateWhenItHasValueTest() {
            // Arrange
            var option = Option<TestClass>.Single(new TestClass());

            // Act
            bool canEnumerate = false;
            foreach (TestClass unused in option) {
                canEnumerate = true;
            }

            // Assert
            Assert.IsTrue(canEnumerate);
        }

        private class TestClass { }

        /// <summary>
        /// Determines if the provided action throws the expected exact exception type
        /// </summary>
        /// <typeparam name="TException">Expected exception type</typeparam>
        /// <param name="action">Action to test for expected exception</param>
        /// <exception cref="ArgumentNullException">Throws when the action parameter is null</exception>
        public static bool ThrowsExact<TException>(Action action) where TException : Exception {
            if (action == null) {
                throw new ArgumentNullException(nameof(action));
            }

            try {
                action();
            } catch (Exception ex) {
                if (ex.GetType() == typeof(TException)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Async version of <see cref="ThrowsExact{TException}"/>
        /// </summary>
        private static async Task<bool> ThrowsExactAsync<TException>(Func<Task> action) where TException : Exception {
            if (action == null) {
                throw new ArgumentNullException(nameof(action));
            }

            try {
                await action();
            } catch (Exception ex) {
                if (ex.GetType() == typeof(TException)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if the provided action throws the expected exception type
        /// <para>NOTE that this will work for derived exception types as well. If you do not want this behaviour use <see cref="ThrowsExact{TException}"/> instead</para>
        /// </summary>
        /// <typeparam name="TException">Expected exception type</typeparam>
        /// <param name="action">Action to test for expected exception</param>
        /// <exception cref="ArgumentNullException">Throws when the action parameter is null</exception>
        private static bool Throws<TException>(Action action) where TException : Exception {
            if (action == null) {
                throw new ArgumentNullException(nameof(action));
            }

            try {
                action();
            } catch (TException) {
                return true;
            } catch (Exception) {
                // ignored
            }

            return false;
        }
    }
}
