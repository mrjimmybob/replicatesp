using System;
using System.Collections.Generic;
using System.Text;

namespace replicatesp
{
    public class Symbols
    {
        protected string[] symbols;
        public enum SpinnerType
        {
            SimpleBars = 0,
            Square,
            SquareInverse,
            CircleComplete,
            Bars,
            CircleSimple,
            SquareComplete,
            Block,
            SquareCompleteInverse,
            BlockComplete,
            BlockRaising,
            Braily,
            Dot,
            SemiSquare,
            SemiCircle,
            Arrow,
            BlockThinning,
            SlidingBash,
            SlidingO,
            SlidingDot,
            Stick
        }

        public static string symbolOk = string.Empty;
        public static string symbolNotOk = string.Empty;

        public Symbols(SpinnerType patternType)
        {
            symbolOk = "☑";
            symbolNotOk = "☒";
            switch (patternType)
            {
                case SpinnerType.SimpleBars:
                    symbols = new string[] { "-", "\\", "|", "/" }; 
                    break;
                case SpinnerType.Square:
                    symbols = new string[] { "▖", "▘", "▝", "▗" };
                    break;
                case SpinnerType.SquareInverse:
                    symbols = new string[] { "▜", "▟", "▙", "▛" };
                    break;
                case SpinnerType.CircleComplete:
                    symbols = new string[] { "◔", "◑", "◕", "◒", "◐", "◓" };
                    break;
                case SpinnerType.Bars:
                    symbols = new string[] { "─", "╲", "│", "╱" };
                    break;
                case SpinnerType.CircleSimple:
                    symbols = new string[] { "◑", "◒", "◐", "◓" };
                    break;
                case SpinnerType.SquareComplete:
                    symbols = new string[] { "▖", "▞", "▛", "█", "▜", "▚", "▘", "▚", "▙", "█", "▟", "▞" };
                    break;
                case SpinnerType.Block:
                    symbols = new string[] { "▙", "▛", "▜", "▟", "▄", "▖", "▘", "▝", "▗", "▄", "▛", "▜", "▟" };
                    break;
                case SpinnerType.SquareCompleteInverse:
                    symbols = new string[] { "░", "▒", "▓", "▓", "▒", "░" };
                    break;
                case SpinnerType.BlockComplete:
                    symbols = new string[] { " ", "░", "▒", "▓", "█", "▓", "▒", "░" };
                    break;
                case SpinnerType.BlockRaising:
                    symbols = new string[] { "▁", "▂", "▃", "▄", "▅", "▆", "▇", "█", "▇", "▆", "▅", "▄", "▃" };
                    break;
                case SpinnerType.Braily:
                    symbols = new string[] { "⣾", "⣽", "⣻", "⢿", "⡿", "⣟", "⣯", "⣷" };
                    break;
                case SpinnerType.Dot:
                    symbols = new string[] { "⠁", "⠂", "⠄", "⡀", "⢀", "⠠", "⠐", "⠈" };
                    break;
                case SpinnerType.SemiSquare:
                    symbols = new string[] { "◰", "◳", "◲", "◱" };
                    break;
                case SpinnerType.SemiCircle: 
                    symbols = new string[] { "◴", "◷", "◶", "◵" };
                    break;
                case SpinnerType.Arrow:
                    symbols = new string[] { "←", "↖", "↑", "↗", "→", "↘", "↓", "↙" };
                    break;
                case SpinnerType.BlockThinning:
                    symbols = new string[] { "▉", "▊", "▋", "▌", "▍", "▎", "▏", "▎", "▍", "▌", "▋", "▊", "▉" };
                    break;
                case SpinnerType.SlidingBash:
                    symbols = new string[] { "(#====)", "(=#===)", "(==#==)", "(===#=)", "(====#)", "(===#=)", "(==#==)", "(=#===)" };
                    symbolOk = "(....☑)";
                    symbolNotOk = "(....☒)";
                    break;
                case SpinnerType.SlidingO:
                    symbols = new string[] { "(O----)", "(-O---)", "(--O--)", "(---O-)", "(----O)", "(---O-)", "(--O--)", "(-O---)" };
                    symbolOk = "(....☑)";
                    symbolNotOk = "(....☒)";
                    break;
                case SpinnerType.SlidingDot:
                    symbols = new string[] { "(·....)", "(.·...)", "(..·..)", "(...·.)", "(....·)", "(...·.)", "(..·..)", "(.·...)" };
                    symbolOk = "(....☑)";
                    symbolNotOk = "(....☒)";
                    break;
                case SpinnerType.Stick:
                default:
                    symbols = new string[] { "┤", "┘", "┴", "└", "├", "┌", "┬", "┐" };
                    break;
            }
        }
        public MyEnumerator GetEnumerator()
        {
            return new MyEnumerator(this);
        }

        public class MyEnumerator
        {
            int nIndex;
            Symbols collection;
            public MyEnumerator(Symbols coll)
            {
                collection = coll;
                nIndex = -1;
            }

            public bool MoveNext()
            {
                nIndex++;
                return (nIndex < collection.symbols.Length);
            }

            public string Current => collection.symbols[nIndex];
        }
    }
}

