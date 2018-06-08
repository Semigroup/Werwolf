using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellRanger.Structures
{
    public class Range
    {
        public string Text { get; set; }

        public Range(string english)
        {
            if (english.Contains("Self"))
            {
                if (english.Length > 4)
                    this.Text = GetBracketText(english);
                else
                    this.Text = "Selbst";
            }
            else if (english.Contains("Unlimited"))
                this.Text = "Unbegrenzt";
            else if (english.Contains("Touch"))
                this.Text = "Berührung";
            else if (english.Contains("Sight"))
                this.Text = "Sichtbereich";
            else if (english.Contains("Special"))
                this.Text = "Besonders";
            else
                this.Text = DistanceToDistanz(english);
        }

        public override string ToString()
        {
            return Text + ".";
        }

        public string GetBracketText(string english)
        {
            int a = 0, b = 0;
            for (int i = 0; i < english.Length; i++)
            {
                if (english[i] == '(')
                    a = i;
                else if (english[i] == ')')
                    b = i;
            }
            string brack = english.Substring(a + 1, b - a-1);

            string[] comps = brack.Split(' ');

            string d = DistanceToDistanz(comps[0]);
            switch (comps[1].ToLower())
            {
                case "sphere":
                case "radius":
                    return d + " Radius";
                case "line":
                    return d + " Strahl";
                case "cone":
                    return "Kegel der Länge " + d;
                case "cube":
                    return "Würfel mit " + d + " Seiten";
                default:
                    throw new NotImplementedException();
            }
        }

        public string GetStartNumber(string text)
        {
            string number = "";
            for (int i = 0; i < text.Length; i++)
            {
                if ('0' <= text[i] && text[i] <= '9')
                    number += text[i];
                else
                    return number;
            }
                    return number;
        }

        public string DistanceToDistanz(string distance)
        {
            string number = GetStartNumber(distance);
            distance = distance.ToLower();
            //meter bzw squares
            int d = int.Parse(number);
            if (distance.Contains("foot") || distance.Contains("feet"))
                d /= 5;
            else if (distance.Contains("mile"))
                d *= 1609;
            else
                throw new NotImplementedException();

            if (d < 1000)
                return d + "m";
            else
                return (d / 1000) + "km";
        }
    }
}
