using Microsoft.VisualBasic.FileIO;
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
        int delay = 20;
        Symbols symbols;
        static string symbolEnd = string.Empty;
        string symbolOk;
        string symbolNotOk;
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
            symbolOk = Symbols.symbolOk;
            symbolNotOk = Symbols.symbolNotOk;
    }

        public void ChildThreadPrint()
        {
            do
            {
                foreach (string s in symbols)
                {
                    WriteAt(s, posX, posY);
                    Thread.Sleep(delay);
                    if (abort) break;
                }
            } while (!abort);
            WriteAt(symbolEnd, posX, posY);
        }

        public void Delete()
        { 
            abort = true;
        }
    
        public void Delete(bool error)
        {
            abort = true;
            symbolEnd = error ? Symbols.symbolNotOk : Symbols.symbolOk;
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
            catch 
            {
                // Ignore errors
            }
        }
    }
}
