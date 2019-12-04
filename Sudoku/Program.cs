using System;
using System.IO;
using System.Threading;

namespace Sudoku
{
    class Program
    {
        Sudoku sudoku;
        int difficulty = 30;
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Start();
            Console.ReadKey();
            //Console.WriteLine("Hello World!");
        }

        public void Start()
        {
            PrintFrame();
            sudoku = new Sudoku(PrintDigit, SaveAnswer);
            do
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                GenerateRandom(difficulty);
                //LoadSudoku("sudoku.txt");
                Console.ForegroundColor = ConsoleColor.Green;
            }
            while (!sudoku.Solve());
            //PrintDigit(1, 2, 3);
        }

        private void GenerateRandom(int count)
        {
            Random random = new Random();
            if (count == 0) return;
            if (count > Sudoku.max * Sudoku.max) return;
            sudoku.ClearMap();

            for (int c = 0; c < count; c++)
            {
                int x, y, d;
                int loop = 777;
                do
                {
                    x = random.Next(0, Sudoku.max);
                    y = random.Next(0, Sudoku.max);
                    d = random.Next(1, Sudoku.max + 1);
                }
                while
                (--loop > 0 && !sudoku.PlaceDigit(x, y, d));
            }
            Console.SetCursorPosition(20, 5);
            Console.WriteLine("Puzzle ready");
        }

        public void PrintFrame()
        {
            string symbol = " ";
            for (int px = 0; px <= (Sudoku.sqr + 1) * Sudoku.sqr; px++)
                for (int py = 0; py <= (Sudoku.sqr + 1) * Sudoku.sqr; py++)
                {
                    if (px % (Sudoku.sqr + 1) == 0 &&
                        py % (Sudoku.sqr + 1) == 0)
                        symbol = "+";
                    else
                        if (px % (Sudoku.sqr + 1) == 0)
                        symbol = "|";
                    else
                        if (py % (Sudoku.sqr + 1) == 0)
                        symbol = "-";
                    else
                        symbol = " ";
                    Console.SetCursorPosition(px, py);
                    Console.Write(symbol);
                }

        }

        public void PrintDigit(int x, int y, int d)
        {
            int px = 1 + x + x / Sudoku.sqr;
            int py = 1 + y + y / Sudoku.sqr;
            Console.SetCursorPosition(px, py);
            Console.Write(d == 0 ? " " : d.ToString());
            //Thread.Sleep(10);
        }

        public void LoadSudoku(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            int j = 0;
            while (j < lines.Length)
            {
                if (lines[j].Contains("Поиск слова"))
                    break;
                j++;
            }
            j = j + 3;


            for (int y = 0; y < Sudoku.max; y++)
                for (int x = 0; x < Sudoku.max; x++)
                {
                    if (lines[j] != " ")
                        sudoku.PlaceDigit(x, y, Convert.ToInt32(lines[j]));
                    j++;
                }
        }

        public void SaveAnswer()
        {
            using (StreamWriter file = new StreamWriter("solver.txt"))
            {
                string symbol = " ";
                for (int py = 0; py <= (Sudoku.sqr + 1) * Sudoku.sqr; py++)
                {
                    for (int px = 0; px <= (Sudoku.sqr + 1) * Sudoku.sqr; px++)

                    {
                        if (px % (Sudoku.sqr + 1) == 0 &&
                            py % (Sudoku.sqr + 1) == 0)
                            symbol = "+";
                        else
                            if (px % (Sudoku.sqr + 1) == 0)
                            symbol = "|";
                        else
                            if (py % (Sudoku.sqr + 1) == 0)
                            symbol = "-";
                        else
                        {
                            int x = px - 1 - px / (Sudoku.sqr + 1);
                            int y = py - 1 - py / (Sudoku.sqr + 1);
                            symbol = sudoku.map[x, y].ToString();
                        }
                        file.Write(symbol);
                    }
                    file.WriteLine();
                }
                Console.SetCursorPosition(20, 7);
                Console.WriteLine("Solved");
                //Console.ReadKey();
            }
        }
    }
}
