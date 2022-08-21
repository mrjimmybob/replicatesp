using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace replicatesp
{


    class Spinner
    {
        int origRow = Console.CursorTop;
        int origCol = Console.CursorLeft;
        int delay = 10;
        Symbols symbols;
        int posX;
        int posY;
        bool abort;
        ConsoleColor color;
        


        public Spinner(Symbols.SpinnerType st, ConsoleColor c, int x = 0, int y = 0, int d = 50)
        {
            posX = x;
            posY = y;
            delay = d;
            symbols = new Symbols(st);
            abort = false;
            color = c;
            Print();
        }

        public void ChildThreadPrint()
        {
            do
            {
                foreach (string s in symbols)
                {
                    WriteAt(s, posX, posY);
                    Thread.Sleep(delay);
                    // if (abort) break;
                }
            } while (!abort);
        }

        public void Print()
        {
            ThreadStart childref = new ThreadStart(ChildThreadPrint);
            Thread childThread = new Thread(childref);
            childThread.Start();
        }

        private void WriteAt(string s, int x, int y)
        {
            int previousRow = Console.CursorTop;
            int previousCol = Console.CursorLeft;

            try
            {
                Console.CursorVisible = false;
                Console.OutputEncoding = Encoding.UTF8;
                Console.SetCursorPosition(origCol + x, origRow + y);
                Console.ForegroundColor = color;
                Console.Write(s);
                Console.SetCursorPosition(previousRow, previousCol);
            }
            catch (ArgumentOutOfRangeException e)
            {
                // Ignore errors
            }
        }
    }
}
