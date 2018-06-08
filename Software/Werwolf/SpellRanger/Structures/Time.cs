using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellRanger.Structures
{
    public class Time : IComparable<Time>
    {
        public enum Kind
        {
            Instantaneous,
            Reaction,
            BonusAction,
            Action,
            Round,
            Minutes,
            Hours,
            Days,
            UntilDispelled,
            UntilDispelledOrTriggered,
            Special
        }

        public int Seconds { get; set; }
        public string OriginalText { get; set; }
        public Kind MyKind { get; set; }
        public bool UpTo { get; set; }
        public bool Concentration { get; set; }

        public Time(string text)
        {
            this.OriginalText = text;
            text = text.ToLower();

            UpTo = text.Contains("up to");
            Concentration = text.Contains("concentration");

            if (text.Contains("instantaneous"))
            {
                MyKind = Kind.Instantaneous;
                Seconds = 0;
            }
            else if (text.Contains("special"))
            {
                MyKind = Kind.Special;
                Seconds = int.MaxValue;
            }
            else if (text.Contains("until dispelled or triggered"))
            {
                MyKind = Kind.UntilDispelledOrTriggered;
                Seconds = int.MaxValue;
            }
            else if (text.Contains("until dispelled"))
            {
                MyKind = Kind.UntilDispelled;
                Seconds = int.MaxValue;
            }
            else if (text.Contains("reaction"))
            {
                MyKind = Kind.Reaction;
                Seconds = 1;
            }
            else if (text.Contains("bonus action"))
            {
                MyKind = Kind.BonusAction;
                Seconds = 3;
            }
            else if (text.Contains(" action"))
            {
                MyKind = Kind.Action;
                Seconds = 6;
            }
            else if (text.Contains(" round"))
            {
                MyKind = Kind.Round;
                Seconds = 6 * GetFirstNumber(text);
            }
            else if (text.Contains(" minute"))
            {
                MyKind = Kind.Minutes;
                Seconds = 60* GetFirstNumber(text);
            }
            else if (text.Contains(" hour"))
            {
                MyKind = Kind.Hours;
                Seconds = 60* 60 * GetFirstNumber(text);
            }
            else if (text.Contains(" day"))
            {
                MyKind = Kind.Days;
                Seconds = 24 * 60 * 60 * GetFirstNumber(text);
            }
            else
                throw new NotImplementedException();
        }

        public int GetFirstNumber(string text)
        {
            int startPosition = 0;
            while (startPosition < text.Length)
            {
                if ('0' < text[startPosition] && text[startPosition] < '9')
                    return GetNumberAt(text, startPosition);
                startPosition++;
            }
            return 0;
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

        public int CompareTo(Time other)
        {
            return this.Seconds - other.Seconds;
        }

        public override string ToString()
        {
            string ta = GetTimeAmount();
            if (UpTo)
                ta = "Bis zu " + ta;
            else
                ta = ta.Substring(0, 1).ToUpper() + ta.Substring(1);
            if (Concentration)
                ta += " (Konzentration).";
            else
                ta += ".";
            return ta;
        }

        public string GetTimeAmount()
        {
            switch (MyKind)
            {
                case Kind.Instantaneous:
                    return "Sofort";
                case Kind.Reaction:
                    return OriginalText;
                case Kind.BonusAction:
                    return "eine Extraaktion";
                case Kind.Action:
                    return "eine Aktion";
                case Kind.Round:
                    int rounds = (Seconds / 6);
                    if (rounds == 1)
                        return "eine Runde";
                    else
                        return rounds + " Runden";
                case Kind.Minutes:
                    int min = (Seconds / 60);
                    if (min == 1)
                        return "eine Minute";
                    else
                        return min + " Minuten";
                case Kind.Hours:
                    int hours = (Seconds / 3600);
                    if (hours == 1)
                        return "eine Stunde";
                    else
                        return hours + " Stunden";
                case Kind.Days:
                    int days = (Seconds / (60* 60* 24));
                    if (days == 1)
                        return "ein Tag";
                    else
                        return days + " Tage";
                case Kind.UntilDispelled:
                    return "bis der Zauber gebannt wird";
                case Kind.UntilDispelledOrTriggered:
                    return "bis der Zauber gebannt wird";
                case Kind.Special:
                    return "besonders";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
