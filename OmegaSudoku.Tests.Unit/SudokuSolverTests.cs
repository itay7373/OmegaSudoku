using System;
using System.Collections.Generic;
using System.Text;
using OmegaSudoku.Exceptions;
using OmegaSudoku.Solver;


namespace OmegaSudoku.Tests.Unit
{
    public class SudokuSolverTests
    {

        [Fact]
        public void Solve_WhenBoardSizeIsInvalid_ShouldReturnInvalidBoardSizeException()
        {
            //Arrange
            string invalidBoard = "90500078060207100000083056000610034037008001505004360001030680000890003670000820";

            //Act & Assert
            Assert.Throws<InvalidBoardSizeException>(() => new SudokuSolver(invalidBoard));
        }


        [Fact]
        public void Solve_WhenBoardIsEmpty_ShouldReturnEmptyBoardException()
        {
            //Arrange
            string EmptyBoard = "";

            //Act & Assert
            Assert.Throws<EmptyBoardException>(() => new SudokuSolver(EmptyBoard));
        }

        [Fact]
        public void Solve_WhenBoardContainsInvalidValues_ShouldReturnInvalidValusException()
        {
            //Arrange
            string inputWithInvalidChar = "905000780602071000000830560006*00340370080015050043600010306800008900036700008209";

            //Act & Assert
            Assert.Throws<InvalidValuesException>(() => new SudokuSolver(inputWithInvalidChar));
        }

        [Fact]
        public void Solve_WhenBoardIsUnsolvable_ShouldReturnUnsolvableBoardException()
        {
            //Arrange
            SudokuSolver sut = new SudokuSolver("905000780602071000000830560006100340370080015050043600010306800008900036700008209");

            // Act
            Action action = () => sut.Solve();

            // Assert
            Assert.Throws<UnsolvableBoardException>(action);
        }

        [Fact]
        public void Solve_WhenBoardHasIdenticalNumbersInSameRow_ShouldReturnIdenticalNumbersRowException()
        {
            //Arrange
            string invalidBoard = "110000000000000000000000000000000000000000000000000000000000000000000000000000000";

            //Act & Assert
            Assert.Throws<IdenticalNumbersRowException>(() => new SudokuSolver(invalidBoard));
        }

        [Fact]
        public void Solve_WhenBoardHasIdenticalNumbersInSameColumn_ShouldReturnIdenticalNumbersColumnException()
        {
            //Arrange
            string invalidBoard = "100000000100000000000000000000000000000000000000000000000000000000000000000000000";

            //Act & Assert
            Assert.Throws<IdenticalNumbersColumnException>(() => new SudokuSolver(invalidBoard));
        }

        [Fact]
        public void Solve_WhenBoardHasIdenticalNumbersInSameBox_ShouldReturnIdenticalNumbersBoxException()
        {
            //Arrange
            string invalidBoard = "100000000010000000000000000000000000000000000000000000000000000000000000000000000";

            //Act & Assert
            Assert.Throws<IdenticalNumbersBoxException>(() => new SudokuSolver(invalidBoard));
        }

        [Theory]
        [InlineData("000060080020000000001000000070000102500030000000000400004201000300700600000000050", "947165283823974516651328947478596132516432879239817465764251398385749621192683754")]
        [InlineData("070000043040009610800634900094052000358460020000800530080070091902100005007040802", "679518243543729618821634957794352186358461729216897534485276391962183475137945862")]
        [InlineData("301086504046521070500000001400800002080347900009050038004090200008734090007208103", "371986524846521379592473861463819752285347916719652438634195287128734695957268143")]
        [InlineData("008317000004205109000040070327160904901450000045700800030001060872604000416070080", "298317645764285139153946278327168954981453726645792813539821467872634591416579382")]
        [InlineData("040890630000136820800740519000467052450020700267010000520003400010280970004050063", "142895637975136824836742519398467152451328796267519348529673481613284975784951263")]
        public void Solve_WhenBoardIsValid_ShouldReturnSolvedBoard(string value, string expectedValue)
        {
            // Arrange
            SudokuSolver sut = new SudokuSolver(value);

            // Act
            sut.Solve();
            string actualValue = sut.GetBoard();

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

    }
}
