using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellRanger.Structures
{
   public class SpellType
    {
        public int Level { get; set; }
        public string School { get; set; }
        public bool IsRitual { get; set; }

        public SpellType(Spell spell)
        {
            this.IsRitual = spell.ritual;

            if (spell.level == "cantrip")
                this.Level = 0;
            else
                this.Level = spell.level[0] - '0';

            if (!IsRitual)
            switch (spell.school)
            {
                case "conjuration":
                    this.School = "Beschwörung";
                    break;
                case "abjuration":
                    this.School = "Bannzauber";
                    break;
                case "enchantment":
                    this.School = "Verzauberung";
                    break;
                case "evocation":
                    this.School = "Herbeirufung";
                    break;
                case "necromancy":
                    this.School = "Nekromantie";
                    break;
                case "illusion":
                    this.School = "Illusion";
                    break;
                case "divination":
                    this.School = "Erkenntniszauber";
                    break;
                case "transmutation":
                    this.School = "Verwandlung";
                    break;
                default:
                    throw new NotImplementedException();
            }
            else
                switch (spell.school)
                {
                    case "conjuration":
                        this.School = "Beschwörungsritual";
                        break;
                    case "abjuration":
                        this.School = "Bannritual";
                        break;
                    case "enchantment":
                        this.School = "Verzauberungsritual";
                        break;
                    case "evocation":
                        this.School = "Herbeirufungsritual";
                        break;
                    case "necromancy":
                        this.School = "Nekromantieritual";
                        break;
                    case "illusion":
                        this.School = "Illusionsritual";
                        break;
                    case "divination":
                        this.School = "Erkenntnisritual";
                        break;
                    case "transmutation":
                        this.School = "Wandlungsritual";
                        break;
                    default:
                        throw new NotImplementedException();
                }
        }

        public override string ToString()
        {
            if (Level > 0)
                return School + " der Stufe " + Level + ".";
            else
                return "Zaubertrick (" + School + ").";
        }
    }
}
