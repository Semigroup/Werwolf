using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SpellRanger.Structures;

namespace SpellRanger.Latex
{
    public class LatexWriter : StreamWriter
    {
        public LatexWriter(string path) : base(File.OpenWrite(path))
        {
        }

        public void BeginDocument()
        {

        }

        public void EndDocument()
        {

        }

        public void WriteLineInBrackets(string text)
        {
            this.Write("{");
            this.Write(text);
            this.Write("}");
            this.WriteLine();
        }

        public void WriteCorrectLine(string corrputedText)
        {
            corrputedText = corrputedText.Replace("×", "$\\times$");
            corrputedText = corrputedText.Replace("−", "$-$");
            corrputedText = corrputedText.Replace("_wish_", "\\emph{wish}");
            corrputedText = corrputedText.Replace("_Wish_", "\\emph{wish}");
            corrputedText = corrputedText.Replace("_greater restoration_", "\\emph{greater restoration}");
            corrputedText = corrputedText.Replace("_dispel magic_", "\\emph{dispel magic}");
            corrputedText = corrputedText.Replace("_rod of cancellation_", "\\emph{rod of cancellation}");
            corrputedText = corrputedText.Replace("_prismatic wall_", "\\emph{prismatic wall}");
            corrputedText = corrputedText.Replace("_antimagic_", "\\emph{antimagic}");
            corrputedText = corrputedText.Replace("_daylight_", "\\emph{daylight}");
            corrputedText = corrputedText.Replace("_heal_", "\\emph{heal}");
            corrputedText = corrputedText.Replace("_contingency_", "\\emph{contingency}");
            corrputedText = corrputedText.Replace("_water breathing_", "\\emph{water breathing}");
            corrputedText = corrputedText.Replace("_wall of force_", "\\emph{wall of force}");
            corrputedText = corrputedText.Replace("_true resurrection_", "\\emph{true resurrection}");

            this.WriteLine(corrputedText);
        }

        public void WriteSpell(Spell spell)
        {
            this.WriteLine(@"\begin{spell}");
            this.WriteLineInBrackets(spell.name);
            this.WriteLineInBrackets(spell.SpellType.ToString());
            this.WriteLineInBrackets(spell.Time.ToString().Replace("Konzentration", "\\textbf{Konzentration}"));
            this.WriteLineInBrackets(spell.Range.ToString());
            this.WriteLineInBrackets(spell.components.raw + ".");
            this.WriteLineInBrackets(spell.Duration.ToString());
            this.WriteCorrectLine(spell.description);
            if (spell.higher_levels != null && spell.higher_levels.Length > 0)
            {
                this.Write("\\paragraph{Auf Höheren Stufen}");
                this.WriteLine(spell.higher_levels);
            }
            this.WriteLine(@"\end{spell}");
        }
    }
}
