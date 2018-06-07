using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellRanger.Structures
{
    public class Time
    {
        public enum Kind
        {
            BonusAction,
            Action,
            Minutes,
            Hours,
            Reaction
        }

        public int Seconds { get; set; }
        public string OriginalText { get; set; }
        public Kind MyKind { get; set; }

        public Time(string text)
        {
            this.OriginalText = text;
            if (text.Contains("reaction"))
            {
                MyKind = Kind.Reaction;
                Seconds = 0;
            }
            else if(text.Contains("bonus action"))
            {
                MyKind = Kind.BonusAction;
                Seconds = 1;
            }
            else if (text.Contains(" action"))
            {
                MyKind = Kind.Action;
                Seconds = 6;
            }
            else if (text.Contains(" minute"))
            {
                MyKind = Kind.Minutes;
                Seconds = 60* GetNumberAt(text, 0);
            }
            else if (text.Contains(" hour"))
            {
                MyKind = Kind.Minutes;
                Seconds = 60* 60* GetNumberAt(text, 0);
            }
            else
                throw new NotImplementedException();
        }

        public int GetNumberAt(string text, int startPosition)
        {
            string numberText = "";
            int i = startPosition;
            while (i < text.Length)
            {
                if ('0' <= text[i] && text[i] <= '9')
                {
                    numberText += text[i];
                    i++;
                }
                else
                    i = text.Length;
            }
            if (numberText.Length == 0)
                return 0;
            else
                return int.Parse(numberText);
        }
    }
}
