using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    delegate void dePrintDigit(int x, int y, int d);
    delegate void deSaveAnswer();

    class Sudoku
    {
        public const int max = 9; // map size
        public const int sqr = 3;

        bool found;
        int count=0;

        public int [,] map { get; private set; }

        private dePrintDigit PrintDigit;
        private deSaveAnswer SaveAnswer;

        public Sudoku(dePrintDigit printDigit, deSaveAnswer saveAnswer)
        {
            map = new int[max, max];
            PrintDigit = printDigit;
            SaveAnswer = saveAnswer;
            ClearMap();
        }

        public void ClearMap()
        {
            for (int x = 0; x < max; x++)
                for(int y = 0; y < max; y ++)
                {
                    ClearDigit(x,y);
                }
        }

        public bool PlaceDigit(int x, int y, int d)
        {
            if (x < 0 || x >= Sudoku.max) return false;
            if (y < 0 || y >= Sudoku.max) return false;
            if (d <= 0 || d > Sudoku.max) return false;

            if (map[x, y] == d) return true;
            if (map[x, y] != 0) return false;

            for (int cx = 0; cx < Sudoku.max; cx++)
                if (map[cx, y] == d)
                    return false;

            for (int cy = 0; cy < Sudoku.max; cy++)
                if (map[x, cy] == d)
                    return false;

            int sx = Sudoku.sqr * (x / Sudoku.sqr);
            int sy = Sudoku.sqr * (y / Sudoku.sqr);

            for (int cx = sx; cx < sx + Sudoku.sqr; cx++)
                for (int cy = sy; cy < sy + Sudoku.sqr; cy++)
                    if (map[cx, cy] == d)
                        return false;
            map[x, y] = d;
            PrintDigit(x, y, d);
            return true;
        }

        public bool Solve()
        {
            found = false;
            NextDigit(0);
            return found;
        }

        public void NextDigit(int step)
        {
            count++;
            if (found) return;
            if(step == max*max)
            {
                found = true;
                SaveAnswer();
                return;
            }

            int x = step % max;
            int y = step / max;

            if(map[x,y] > 0)
            {
                NextDigit(step + 1);
                return;
            }

            for(int d = 1; d <= max; d++)
            {
                if(PlaceDigit(x,y,d))
                {
                    NextDigit(step + 1);
                    ClearDigit(x,y);
                }
            }
        }

        private void ClearDigit(int x, int y)
        {
            if (x < 0 || x >= Sudoku.max) return;
            if (y < 0 || y >= Sudoku.max) return;

            if (map[x, y] == 0) return;

            map[x, y] = 0;
            PrintDigit(x, y, 0);
        }

        //private void SaveAnswer()
        //{
        //    Console.WriteLine();
        //    Console.WriteLine();
        //    Console.WriteLine("Количество итераций:" + " " +count);
        //    Console.ReadKey();
        //}

    }
}
