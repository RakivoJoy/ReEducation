using ExceptionsDemo;

namespace ExceptionsDemoTests
{
    public class ProcessingServiceTests
    {
        private readonly ProcessingService _service = new();
        private readonly string _testDirectory = Path.Combine(Path.GetTempPath(), "ExceptionsDemoTests");

        public ProcessingServiceTests()
        {
            Directory.CreateDirectory(_testDirectory);
        }

        /// <summary>
        /// Tests that ArgumentException is thrown when fileName is null
        /// </summary>
        [Fact]
        public void ProcessFile_WithNullFileName_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _service.ProcessFile(null!));
            Assert.Contains("Filnamn får inte vara tomt eller null", ex.Message);
        }

        /// <summary>
        /// Tests that ArgumentException is thrown when fileName is empty string
        /// </summary>
        [Fact]
        public void ProcessFile_WithEmptyFileName_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _service.ProcessFile(""));
            Assert.Contains("Filnamn får inte vara tomt eller null", ex.Message);
        }

        /// <summary>
        /// Tests that ArgumentException is thrown when fileName is only whitespace
        /// </summary>
        [Fact]
        public void ProcessFile_WithWhitespaceFileName_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _service.ProcessFile("   "));
            Assert.Contains("Filnamn får inte vara tomt eller null", ex.Message);
        }

        /// <summary>
        /// Tests that InvalidOperationException is thrown when file doesn't exist
        /// (wrapped by the service's exception handling)
        /// </summary>
        [Fact]
        public void ProcessFile_WithNonExistentFile_ThrowsInvalidOperationException()
        {
            // Arrange
            string nonExistentFile = Path.Combine(_testDirectory, "nonexistent_file_12345.txt");

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _service.ProcessFile(nonExistentFile));
            Assert.Contains("Det gick inte att processa filen", ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<FileNotFoundException>(ex.InnerException);
        }

        /// <summary>
        /// Tests that InvalidOperationException is thrown when file is empty
        /// </summary>
        [Fact]
        public void ProcessFile_WithEmptyFile_ThrowsInvalidOperationException()
        {
            // Arrange
            string emptyFile = Path.Combine(_testDirectory, "empty_file.txt");
            File.WriteAllText(emptyFile, "");

            try
            {
                // Act & Assert
                var ex = Assert.Throws<InvalidOperationException>(() => _service.ProcessFile(emptyFile));
                // The empty file exception gets wrapped, so we check for the wrapper message
                Assert.Contains("Det gick inte att processa filen", ex.Message);
                Assert.NotNull(ex.InnerException);
                Assert.IsType<InvalidOperationException>(ex.InnerException);
            }
            finally
            {
                File.Delete(emptyFile);
            }
        }

        /// <summary>
        /// Tests that FormatException is thrown when file contains non-numeric data
        /// </summary>
        [Fact]
        public void ProcessFile_WithNonNumericContent_ThrowsFormatException()
        {
            // Arrange
            string invalidFormatFile = Path.Combine(_testDirectory, "invalid_format.txt");
            File.WriteAllText(invalidFormatFile, "not a number");

            try
            {
                // Act & Assert
                var ex = Assert.Throws<FormatException>(() => _service.ProcessFile(invalidFormatFile));
                Assert.NotNull(ex.Message);
            }
            finally
            {
                File.Delete(invalidFormatFile);
            }
        }

        /// <summary>
        /// Tests that FormatException is thrown when file contains decimal number
        /// (int.Parse doesn't accept decimals)
        /// </summary>
        [Fact]
        public void ProcessFile_WithDecimalNumber_ThrowsFormatException()
        {
            // Arrange
            string decimalFile = Path.Combine(_testDirectory, "decimal_number.txt");
            File.WriteAllText(decimalFile, "3.14");

            try
            {
                // Act & Assert
                var ex = Assert.Throws<FormatException>(() => _service.ProcessFile(decimalFile));
                Assert.NotNull(ex.Message);
            }
            finally
            {
                File.Delete(decimalFile);
            }
        }

        /// <summary>
        /// Note: In .NET, dividing a double by zero doesn't throw an exception - it returns Infinity
        /// This test verifies we get an exception explicitly for zero input, as per the modified service code that throws DivideByZeroException for zero.
        /// </summary>
        [Fact]
        public void ProcessFile_WithZero_ReturnsDivideByZeroException()
        {
            // Arrange
            string zeroFile = Path.Combine(_testDirectory, "zero.txt");
            File.WriteAllText(zeroFile, "0");

            try
            {
                // Act & Assert
                var ex = Assert.Throws<DivideByZeroException>(() => _service.ProcessFile(zeroFile));
                Assert.NotNull(ex.Message);
            }
            finally
            {
                File.Delete(zeroFile);
            }
        }

        /// <summary>
        /// Tests that ProcessFile correctly calculates 100.0 / 5 = 20.0
        /// </summary>
        [Fact]
        public void ProcessFile_WithValidPositiveNumber_ReturnsCorrectResult()
        {
            // Arrange
            string validFile = Path.Combine(_testDirectory, "valid_positive.txt");
            File.WriteAllText(validFile, "5");

            try
            {
                // Act
                double result = _service.ProcessFile(validFile);

                // Assert
                Assert.Equal(20.0, result);
            }
            finally
            {
                File.Delete(validFile);
            }
        }

        /// <summary>
        /// Tests that ProcessFile correctly calculates 100.0 / -4 = -25.0
        /// </summary>
        [Fact]
        public void ProcessFile_WithValidNegativeNumber_ReturnsCorrectResult()
        {
            // Arrange
            string negativeFile = Path.Combine(_testDirectory, "valid_negative.txt");
            File.WriteAllText(negativeFile, "-4");

            try
            {
                // Act
                double result = _service.ProcessFile(negativeFile);

                // Assert
                Assert.Equal(-25.0, result);
            }
            finally
            {
                File.Delete(negativeFile);
            }
        }

        /// <summary>
        /// Tests that ProcessFile correctly calculates 100.0 / 1 = 100.0
        /// </summary>
        [Fact]
        public void ProcessFile_WithOne_ReturnsHundred()
        {
            // Arrange
            string oneFile = Path.Combine(_testDirectory, "one.txt");
            File.WriteAllText(oneFile, "1");

            try
            {
                // Act
                double result = _service.ProcessFile(oneFile);

                // Assert
                Assert.Equal(100.0, result);
            }
            finally
            {
                File.Delete(oneFile);
            }
        }

        /// <summary>
        /// Tests that ProcessFile correctly calculates 100.0 / 2 = 50.0
        /// </summary>
        [Fact]
        public void ProcessFile_WithTwo_ReturnsFifty()
        {
            // Arrange
            string twoFile = Path.Combine(_testDirectory, "two.txt");
            File.WriteAllText(twoFile, "2");

            try
            {
                // Act
                double result = _service.ProcessFile(twoFile);

                // Assert
                Assert.Equal(50.0, result);
            }
            finally
            {
                File.Delete(twoFile);
            }
        }

        /// <summary>
        /// Tests that ProcessFile handles large integer values correctly
        /// 100.0 / 1000000 = 0.0001
        /// </summary>
        [Fact]
        public void ProcessFile_WithLargeNumber_ReturnsSmallDecimal()
        {
            // Arrange
            string largeFile = Path.Combine(_testDirectory, "large_number.txt");
            File.WriteAllText(largeFile, "1000000");

            try
            {
                // Act
                double result = _service.ProcessFile(largeFile);

                // Assert
                Assert.Equal(0.0001, result, 6);
            }
            finally
            {
                File.Delete(largeFile);
            }
        }

        /// <summary>
        /// Tests that int.Parse handles leading/trailing whitespace correctly
        /// </summary>
        [Fact]
        public void ProcessFile_WithWhitespaceAroundNumber_ReturnsCorrectResult()
        {
            // Arrange
            string whitespaceFile = Path.Combine(_testDirectory, "whitespace_number.txt");
            File.WriteAllText(whitespaceFile, "  10  ");

            try
            {
                // Act
                double result = _service.ProcessFile(whitespaceFile);

                // Assert
                Assert.Equal(10.0, result);
            }
            finally
            {
                // Cleanup
                File.Delete(whitespaceFile);
            }
        }

        /// <summary> 
        /// Tests that the finally block always executes by checking resource cleanup
        /// </summary>
        [Fact]
        public void ProcessFile_EnsuresFinallyBlockExecutes_WithException()
        {
            // Arrange
            string invalidFile = Path.Combine(_testDirectory, "invalid_finally_test.txt");
            File.WriteAllText(invalidFile, "not a number");

            // Note: The finally block closes the reader, so we can verify
            // by attempting to delete immediately (would fail if not closed)
            try
            {
                try
                {
                    // Act
                    _service.ProcessFile(invalidFile);
                }
                catch (FormatException)
                {
                    // Expected exception
                }

                // Assert - If we get here, the file was properly closed by finally block
                // We can now delete it without issues
                File.Delete(invalidFile);
                Assert.True(!File.Exists(invalidFile));
            }
            finally
            {
                if (File.Exists(invalidFile))
                    File.Delete(invalidFile);
            }
        }
    }
}

