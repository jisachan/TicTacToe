using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Collections.Generic;

namespace TicTacToe_JosefindelaMotte
{
    class Program
    {
        string[,] gameBoard = default;
        bool gameHasEnded = false;
        string playerMarker = "X";
        string aiMarker = "O";
        int counter = 0;
        const int max = 1000;
        const int min = -1000;

        static void Main(string[] args)
        { 
            Program game = new Program();
            game.SetGameHasEnded(false);
            game.gameBoard = game.MakeGameBoard();
            game.ShowGameBoard(game.gameBoard);
            game.PlayGame();
        }

        string[,] MakeGameBoard()
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            return new string[,]
            {
                {" ♥","1","2","3"},
                {" 1"," "," "," "},
                {" 2"," "," "," "},
                {" 3"," "," "," "}
            };
        }

        void ShowGameBoard(string[,] gameBoard)
        {
            for (int row = 0; row < gameBoard.GetLength(1); row++)
            {
                for (int col = 0; col < gameBoard.GetLength(1); col++)
                {
                    Console.Write(gameBoard[row, col]);
                    Console.Write(" | ");
                }
                Console.WriteLine("");
                Console.WriteLine(" ---------------");
            }
        }

        void PlayGame()
        {
            gameHasEnded = false;
            while (gameHasEnded == false)
            {
                PlayerTurn();

                if (CheckWinner(playerMarker))
                {
                    PrintGameEndingStrings("YOU WON! (This shouldn't show on the screen, please tell me if it does! ToT)");

                }
                else if (CheckTie())
                {
                    PrintGameEndingStrings("The game ended in a tie. Try again!");
                }

                AITurn();

                if (CheckWinner(aiMarker))
                {
                    PrintGameEndingStrings("The AI player won. Better luck next time!");
                }
                if (CheckTie())
                {
                    PrintGameEndingStrings("The game ended in a tie. Try again!");
                }
            }
        }

        void ResetGame()
        {
            gameBoard = MakeGameBoard();
            ShowGameBoard(gameBoard);
            PlayGame();
        }

        void SetGameHasEnded(bool result)
        {
            gameHasEnded = result;
        }

        void PlayerTurn()
        {
            //bool based while loop containing try catch, to avoid user input errors.
            bool playerTurn = true;
            while (playerTurn)
            {
                bool userErrorCheck = true;
                while (userErrorCheck)
                {
                    try
                    {
                        Console.WriteLine("\nChoose the row and column of the square you want to place your mark on.");
                        Console.Write("Row: ");
                        string rowXnr = Console.ReadLine();
                        Console.Write("Column: ");
                        string colYnr = Console.ReadLine();
                        int rowX = int.Parse(rowXnr);
                        int colY = int.Parse(colYnr);

                        //checking that the user entered a valid number for the game.							
                        if (rowX > gameBoard.GetLength(1) || rowX < 1 || colY > gameBoard.GetLength(1) || colY < 1)
                        {
                            Console.WriteLine("\nYou tried playing on a square that doesn't exist.");
                            Console.WriteLine("Please try again.");
                            userErrorCheck = false;
                        }
                        //checking that the user doesn't play on an already occupied square
                        else if (gameBoard[rowX, colY] != " ")
                        {
                            Console.WriteLine("\nThere is already a marker on this square.");
                            Console.WriteLine("Please enter coordinates to place your mark on an empty square.");
                            userErrorCheck = false;
                        }
                        else
                        {
                            Console.Clear();
                            gameBoard[rowX, colY] = playerMarker;
                            ShowGameBoard(gameBoard);
                            return;
                        }
                        userErrorCheck = false;
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong. Please make sure you enter valid numbers.");
                    }
                }
            }
        }

        void AITurn()
        {
            int bestScore = min;
            int bestI = 0;
            int bestJ = 0;

            for (int i = 0; i < gameBoard.GetLength(1); i++)
            {
                for (int j = 0; j < gameBoard.GetLength(1); j++)
                {
                    // Is the spot available?
                    if (gameBoard[i, j] == " ")
                    {
                        gameBoard[i, j] = aiMarker;
                        int score = minimax(gameBoard, max, min, false);
                        gameBoard[i, j] = " ";

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestI = i;
                            bestJ = j;
                        }
                    }
                }
            }
            gameBoard[bestI, bestJ] = aiMarker;
            Console.Clear();
            ShowGameBoard(gameBoard);
        }

        bool CheckWinner(string marker)
        {
            for (int row = 0; row < gameBoard.GetLength(1); row++)
            {
                counter = 0;
                for (int col = 0; col < gameBoard.GetLength(1); col++)
                {
                    if (gameBoard[row, col] == marker)
                    {
                        counter++;
                    }
                    if (counter == gameBoard.GetLength(1) - 1)
                    {
                        return true;
                    }
                }
            }

            for (int row = 0; row < gameBoard.GetLength(1); row++)
            {
                counter = 0;
                for (int col = 0; col < gameBoard.GetLength(1); col++)
                {
                    if (gameBoard[col, row] == marker)
                    {
                        counter++;
                    }
                    if (counter == gameBoard.GetLength(1) - 1)
                    {
                        return true;
                    }
                }
            }

            counter = 0;
            for (int diagonal = 0; diagonal < gameBoard.GetLength(1); diagonal++)
            {
                if (gameBoard[diagonal, diagonal] == marker)
                {
                    counter++;
                }
                if (counter == gameBoard.GetLength(1) - 1)
                {
                    return true;
                }
            }

            counter = 0;
            for (int bDiagonal = 1; bDiagonal < gameBoard.GetLength(1); bDiagonal++)
            {
                if (gameBoard[bDiagonal, gameBoard.GetLength(1) - bDiagonal] == marker)
                {
                    counter++;
                }
                if (counter == gameBoard.GetLength(1) - 1)
                {
                    return true;
                }
            }
            return false;
        }

        bool CheckTie()
        {
            foreach (var slot in gameBoard)
            {
                if (slot == " ")
                {
                    return false;
                }
            }
            return true;
        }

        void PrintGameEndingStrings(string resultString)
        {
            Console.WriteLine(resultString);
            Console.WriteLine("Press any key to try again.");
            Console.ReadKey();
            SetGameHasEnded(true);
            ResetGame();
        }
        int minimax(string[,] gameBoard, int alpha, int beta, bool isMaximizing)
        {
            if (CheckWinner(aiMarker))
            {
                return 1;
            }
            else if (CheckWinner(playerMarker))
            {
                return -1;
            }
            else if (CheckTie())
            {
                return 0;
            }

            if (isMaximizing)
            {
                int bestScore = min;
                for (int i = 0; i < gameBoard.GetLength(1); i++)
                {
                    for (int j = 0; j < gameBoard.GetLength(1); j++)
                    {
                        // Is the slot available?
                        if (gameBoard[i, j] == " ")
                        {
                            gameBoard[i, j] = aiMarker;
                            int score = minimax(gameBoard, alpha, beta, false);
                            gameBoard[i, j] = " ";
                            bestScore = Math.Max(score, bestScore);
                            alpha = Math.Max(score, alpha);
                            if(beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                int bestScore = max;
                for (int i = 0; i < gameBoard.GetLength(1); i++)
                {
                    for (int j = 0; j < gameBoard.GetLength(1); j++)
                    {
                        // Is the slot available?
                        if (gameBoard[i, j] == " ")
                        {
                            gameBoard[i, j] = playerMarker;
                            int score = minimax(gameBoard, alpha, beta, true);
                            gameBoard[i, j] = " ";
                            bestScore = Math.Min(score, bestScore);
                            beta = Math.Min(score, beta);
                            if(beta <= alpha)
                            {
                                break;
                            }
                        }
                    }
                }
                return bestScore;
            }
        }
    }
}
