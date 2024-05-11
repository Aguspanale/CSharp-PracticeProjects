﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathGame;
namespace MathGameTests
{
    
    internal class MenuTest
    {
        private StringWriter _consoleOutput;
        private StringReader _consoleInput;
        private TextWriter _originalConsoleOutput;
        private TextReader _originalConsoleInput;
        private Menu menu = new Menu();
        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Starting Test");
            _consoleOutput = new StringWriter();
            _originalConsoleOutput = Console.Out;
            _originalConsoleInput = Console.In;
            Console.SetOut(_consoleOutput);
            menu = new Menu();
        }
        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("Finishing Test");
            Console.SetOut(_originalConsoleOutput);
            Console.SetIn(_originalConsoleInput);
            _consoleOutput.Dispose();
        }
        [Test]
        public void test01MenuAsksName()
        {
            string expectedOutput = startAndDefineStartingOutputWith("Agus");
            string actualOutput = _consoleOutput.ToString();
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [Test]
        public void test02MenuAsksGame()
        {
            ExpectedOutputFromTestingSelectingGame("Sum");
        }
        [Test]
        public void test03MenuAsksDifferentGame()
        {
            ExpectedOutputFromTestingSelectingGame("Substract");
        }

        [Test]
        public void test04PlaySumGameAndWin() 
        {
            string expectedOutput = ExpectedOutputFromTestingSelectingGame("Sum") +
                "How much is 3 + 4?" +
                "\r\n" +
                "That's correct!" +
                "\r\n\r\n";
            _consoleInput = new StringReader("7");
            Console.SetIn(_consoleInput);

            menu.PlaySelectedGame(3,4);

            string actualOutput = _consoleOutput.ToString();
            Assert.AreEqual(expectedOutput, actualOutput);

        }
        [Test]
        public void test05PlaySumGameAndLose()
        {
            string expectedOutput = ExpectedOutputFromTestingSelectingGame("Sum") +
                "How much is 4 + 6?" +
                "\r\n" +
                "Incorrect answer, correct answer was 10" +
                "\r\n\r\n";
            _consoleInput = new StringReader("120");
            Console.SetIn(_consoleInput);

            menu.PlaySelectedGame(4, 6);

            string actualOutput = _consoleOutput.ToString();
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [Test]
        public void test06PlayMultiplyGameAndWin()
        {
            string expectedOutput = ExpectedOutputFromTestingSelectingGame("Multiply") +
                "How much is 4 x 6?" +
                "\r\n" +
                "That's correct!" +
                "\r\n\r\n";
            _consoleInput = new StringReader("24");
            Console.SetIn(_consoleInput);

            menu.PlaySelectedGame(4, 6);

            string actualOutput = _consoleOutput.ToString();
            Assert.AreEqual(expectedOutput, actualOutput);
        }
        [Test]
        public void test07AttemptingToDivideNonDivisibleNumbersShouldThrowException()
        {
            ExpectedOutputFromTestingSelectingGame("Divide");
            Exception exception = Assert.Throws<InvalidOperationException>(() => menu.PlaySelectedGame(4, 3));
            Assert.That(exception.Message, Is.EqualTo("Invalid denominator 3 for numerator 4"));
        }
        [Test]
        public void test08AttemptingToDivideByZeroShouldThrowException()
        {
            ExpectedOutputFromTestingSelectingGame("Divide");
            Exception exception = Assert.Throws<InvalidOperationException>(() => menu.PlaySelectedGame(4, 0));
            Assert.That(exception.Message, Is.EqualTo("Division by zero"));
        }
        [Test]
        public void test09GeneratedDivisionShouldNeverThrowException()
        {
            //Statistical test, the random numbers generated by PlaySelectedGame should always be correct
            for (int i = 0; i < 1000; i++) 
            {
                _consoleInput = new StringReader("Divide");
                Console.SetIn(_consoleInput);
                menu.AskDesiredGame();
                _consoleInput = new StringReader("10");
                Console.SetIn(_consoleInput);
                menu.PlaySelectedGame();
            }
        }

        private string ExpectedOutputFromTestingSelectingGame(string game)
        {
            string expectedOutput =
                            "¿What game do you want to play? (Case sensitive)" +
                            "\r\n\r\n" +
                            "Sum Multiply Divide Substract" +
                            "\r\n" +
                            "Game set to "+ game + "!" +
                            "\r\n\r\n";
            _consoleInput = new StringReader(game);
            Console.SetIn(_consoleInput);
            menu.AskDesiredGame();
            string actualOutput = _consoleOutput.ToString();
            Assert.AreEqual(expectedOutput, actualOutput);
            return expectedOutput;
        }

        private string startAndDefineStartingOutputWith(string name)
        {
            string expectedOutput =
                            "Welcome to the math game!\r\nplease enter your name: " +
                            "\r\n" +
                            "Hello " + name + "!" +
                            "\r\n\r\n";
            _consoleInput = new StringReader(name);
            Console.SetIn(_consoleInput);
            menu.Start();
            menu.readName();
            return expectedOutput;
        }
    }
}
