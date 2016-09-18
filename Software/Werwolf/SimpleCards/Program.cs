using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using Assistment.Texts;
using Assistment.Texts.Paper;

namespace SimpleCards
{
    static class Program
    {

        public static SizeF Size = new SizeF(300, 424.3f);
        public static Pen Rand = new Pen(Color.Black, 2);
        public static Pen Linie = new Pen(Color.Black, 0.1f);

        public static xFont[] Fonts = { new FontGraphicsMeasurer("Exocet", 14), new FontGraphicsMeasurer("Calibri", 8) };
        public static xFont[] FontsKlein = { new FontGraphicsMeasurer("Exocet", 11), new FontGraphicsMeasurer("Calibri", 8) };

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CardSheet cs = new CardSheet(3, 3, Size);
            cs.add(Card("Engel des Imperators",
                        @"Der Besitzer dieser Karte ist ein \dEngel des Imperators\d, d. h., er gehört zu den frommsten und aufrichtigsten Mitgliedern des Imperiums der Menschheit.",
                        "Sollte der Besitzer dieser Karte sterben, so erleidet das Imperium einen ganz besonders schweren Rückschlag, weswegen der Gott-Imperator in diesem Fall in der darauffolgenden Nacht eine Aktion weniger zur Verfügung hat.",
                        "Diese Gesinnungskarte gibt es nur einmal im Spiel. Sollte ihr Besitzer sterben, so wird sie aus dem Spiel entfernt."));

            cs.addRange(Card(2, "Märtyrer des Imperiums",
                        @"Der Besitzer dieser Karte ist ein \dMärtyrer des Imperiums\d, d. h., er gehört zu den eifrigsten und radikalsten Mitgliedern des Imperiums der Menschheit.",
                        "Sollte der Besitzer dieser Karte sterben, so verwandelt er seinen Tod in eine spektakuläre Heldentat, wodurch der Glaube der Imperialen Bevölkerung aufs Neue entfacht wird. Deswegen erhält der Gott-Imperator in diesem Fall in der nächsten Nacht eine Extra-Aktion.",
                        "Diese Gesinnungskarte gibt es nur zweimal im Spiel. Sollte ihr Besitzer sterben, so wird sie aus dem Spiel entfernt."));

            cs.addRange(Card(6, "Imperiumstreuer",
                   @"Der Besitzer dieser Karte ist ein \dImperiumstreuer\d, d. h., er ist ein Mitglied des Imperiums der Menschheit. Er gehört also zu den Guten."));

            cs.add(Card("Adeptus Mechanicus",
                          @"Der Besitzer dieser Karte ist ein Anhänger des Adeptus Mechanicus. D. h., er ist neutral und kann mit beiden Parteien gewinnen, solange er diese Karte besitzt.",
                          @"In jeder Nacht kann der Primordiale Vernichter bzw. der Gott-Imperator einen Aktionspunkt benutzen, um den Besitzer dieser Karte zum Chaos bzw. zum Imperium zu konvertieren. Geschieht dies, so tauscht der Besitzer diese Karte gegen eine \iAnhänger der Primordialen Wahrheit\i- bzw. \iImperiumstreuer\i-Gesinnungskarte.",
                          "Diese Gesinnungskarte gibt es nur einmal im Spiel. Sollte ihr Besitzer sterben oder seine Gesinnung tauschen, so wird sie aus dem Spiel entfernt."));

            cs.addRange(Card(3, "Anhänger der Primordialen Wahrheit",
                         @"Der Besitzer dieser Karte ist ein Anhänger der Primordialen Wahrheit. D. h., er ist böse und gewinnt mit dem Chaos."));

            cs.addRange(Card(2, "Märtyrer des Chaos",
                            @"Der Besitzer dieser Karte ist ein \dMärtyrer des Chaos\d, d. h., er gehört zu den eifrigsten und radikalsten Anhängern der Primordialen Wahrheit.",
                            "Sollte der Besitzer dieser Karte sterben, so verwandelt er seinen Tod in ein spektakuläres Blutbad, wodurch die Allgemeinbevölkerung aufs Tiefste verunsichert wird. Deswegen erhält der Primordiale Vernichter in diesem Fall in der nächsten Nacht eine Extra-Aktion.",
                            "Diese Gesinnungskarte gibt es nur zweimal im Spiel. Sollte ihr Besitzer sterben, so wird sie aus dem Spiel entfernt."));

            cs.add(Card("Champion des Chaos",
                        @"Der Besitzer dieser Karte ist ein \dChampion des Chaos\d, d. h., er gehört zu den niederträchtigsten und bessensten Anhängern der Primordialen Wahrheit.",
                        "Sollte der Besitzer dieser Karte sterben, so erlebt das Imperium einen ganz besonders hohen Erfolg, weswegen der Primordiale Vernichter in diesem Fall in der darauffolgenden Nacht eine Aktion weniger zur Verfügung hat.",
                        "Diese Gesinnungskarte gibt es nur einmal im Spiel. Sollte ihr Besitzer sterben, so wird sie aus dem Spiel entfernt."));

            cs.addRange(Card(5, "Verführt",
                        @"Der Besitzer dieser Karte wurde von einem \dVerführer der Primordialen Wahrheit\d verführt. Er ist nun ein Anhänger der Primordialen Wahrheit, d. h., er gehört zu den Bösen.",
                        "Sollte der Verführer sterben, so stirbt auch der Besitzer dieser Karte."));

            
            cs.addRange(Card(9, "Imperialer Bürger",
                       @"Pendant zum Dorfbewohner.",
                       @"Ein Imperialer Bürger kann zum \iGrenadier\i oder zum \iFrater\i befördert werden."));

            cs.addRange(Card(5, "Grenadier",
                       @"Der Besitzer dieser Rolle wurde vom Gott-Imperator befördert und ist nun Mitglied der Planetaren Verteidigungsstreitkräfte seines Planeten. Deswegen hat seine \dTodesstimme nun doppeltes Gewicht\d, da er ihr notfalls mit Waffengewalt Nachdruck verleihen kann.",
                       @"Ein Grenadier kann zum \iKommissar\i oder zum \iGardisten\i befördert werden."));

            cs.addRange(Card(1, "Gardist",
                       @"Der Besitzer dieser Rolle wurde vom Gott-Imperator nochmal befördert und ist nun Mitglied der Imperialen Armee.",
                       @"Die \dTodesstimme\d des Gardisten hat weiterhin \ddoppeltes Gewicht\d.",
                       @"In jeder Nacht darf der Gardist einen Mitspieler auswählen, der sterben soll.",
                       @"Diese Rolle gibt es nur einmal im Spiel. Stirbt der Gardist, so wird seine Rollenkarte ganz aus dem Spiel entfernt."));
          
            cs.addRange(Card(1, "Kommissar",
                       @"Der Besitzer dieser Rolle wurde vom Gott-Imperator nochmal befördert und ist nun Mitglied des Officio Commissariat.",
                       @"Stimmt der Kommissar in einer Abstimmung tagsüber mit \dTod\d bzw. \dLeben\d, so ist seine Stimme allein entscheidend und das Ergebnis dieser Abstimmung \dTod\d bzw. \dLeben\d.",
                       @"Stimmt der Kommissar in einer Abstimmung tagsüber mit \dNeutral\d, so wird seine Stimme ignoriert und das Ergebnis dieser Abstimmung durch die Mehrheit der anderen Stimmen entschieden.",
                       @"Diese Rolle gibt es nur einmal im Spiel. Stirbt der Kommissar, so wird seine Rollenkarte ganz aus dem Spiel entfernt."));

            cs.addRange(Card(5, "Frater",
                      @"Der Besitzer dieser Rolle wurde vom Gott-Imperator befördert und ist nun Mitglied der Frateris Militia seines Planeten. Deswegen hat seinen \dLebensstimme nun doppeltes Gewicht\d.",
                      @"Ein Frater kann vom Verführer nicht verführt werden.",
                      @"Ein Frater kann zum \iPriester\i oder zum \iInquisitor\i befördert werden."));

            cs.addRange(Card(1, "Priester",
                    @"Der Besitzer dieser Rolle wurde vom Gott-Imperator nochmal befördert und ist nun Mitglied der Ekklesiarchie.",
                    @"Die \dLebensstimme\d des Priesters hat weiterhin \ddoppeltes Gewicht\d.",
                    @"Der Priester kann vom Verführer nicht verführt werden.",
                    @"In jeder Nacht darf der Priester eine Person wählen, die normalerweise sterben würde und ihren Tod aufheben.",
                    @"Diese Rolle gibt es nur einmal im Spiel. Stirbt der Priester, so wird seine Rollenkarte ganz aus dem Spiel entfernt."));
           
            cs.addRange(Card(1, "Inquisitor",
                     @"Der Besitzer dieser Rolle wurde vom Gott-Imperator nochmal befördert und ist nun Mitglied des Ordo Haereticus.",
                     @"Die \dLebensstimme\d des Inquisitors hat weiterhin \ddoppeltes Gewicht\d.",
                     @"Der Inquisitor kann vom Verführer nicht verführt werden.",
                     @"In jeder Nacht darf der Inquisitor eine Person wählen, deren Gesinnungskarte er sehen will.",
                     @"Diese Rolle gibt es nur einmal im Spiel. Stirbt der Inquisitor, so wird seine Rollenkarte ganz aus dem Spiel entfernt."));


            cs.addRange(Card(1, "Techpriester",
                    @"Der Besitzer dieser Rolle ist ein Techpriester, d. h., ein Mitglied des Adeptus Mechanicum.",
                    @"Der Tech-Priester kann in jeder Nacht vom Gott-Imperator bzw. vom Primordialen Vernichter zum Imperium bzw. zum Chaos konvertiert werden.",
                    @"Der Techpriester kann vom Verführer nicht verführt werden.",
                    @"Am Ende jeden Tages lädt sich das Plasmagewehr des Techpriesters um Eins auf. Zu Beginn besitzt sein Plasmagewehr eine Ladung von Null.",
                    @"In jeder Nacht darf der Tech-Priester als Erstes seine aktuelle Gesinnungskarte anschauen.
Danach darf er einen Mitspieler auswählen, dessen Gesinnungskarte er sehen will.
Danach darf er entscheiden X Ladungen seines Plasmagewehrs zu benutzen, um X Mitspieler zu töten.",
                    @"Diese Rolle gibt es nur einmal im Spiel. Stirbt der Techpriester, so wird seine Rollenkarte ganz aus dem Spiel entfernt."));


            cs.addRange(Card(5, "Häretiker",
                      @"Pendant zum Werwolf, d. h., in jeder Nacht wachen alle Häretiker, Psioniker, Chaos-Kultisten, Hexer, Dämonenbeschwörer, Khorne-Berserker und Verführer auf und wählen eine Person aus, die sterben soll.",
                      @"Ein Häretiker kann zum \iChaos-Kultisten\i oder zum \iPsioniker\i befördert werden."));

            cs.addRange(Card(5, "Chaos-Kultist",
                       @"Der Besitzer dieser Rolle wurde vom Primordialen Vernichter befördert und ist nun Mitglied eines Chaos-Kultes. Er wacht weiterhin mit den anderen Anhängern der Primordialen Wahrheit auf, um nachts eine Person zu töten.",
                       @"Der Chaos-Kultist wurde von Nurgle mit übermenschlicher Gesundheit beschenkt. Sollte er sterben, so wird offen bekanntgegeben, dass er ein Anhänger der Primordialen Wahrheit ist, und sein Tod auf das Ende der nächsten Nacht verschoben.",
                       @"Ein Chaos-Kultist kann zum \iVerführer\i oder zum \iKhorne-Berserker\i befördert werden."));

            cs.addRange(Card(1, "Verführer",
                       @"Der Besitzer dieser Rolle wurde vom Primordialen Vernichter nochmal befördert und ist nun ein Glaubensmagier von Slaanesh, dem Gott der Ekstase, Dekadenz und Ausschweifungen. Er wacht weiterhin mit den anderen Anhängern der Primordialen Wahrheit auf, um nachts eine Person zu töten.",
                       @"Der Verführer wurde von Nurgle mit übermenschlicher Gesundheit beschenkt. Sollte er sterben, so wird offen bekanntgegeben, dass er ein Anhänger der Primordialen Wahrheit ist, und sein Tod auf das Ende der nächsten Nacht verschoben.",
                       @"In jeder Nacht darf der Verführer einen Mitspieler auswählen, der verführt werden soll. Verführte erhalten die \iVerführt\i-Gesinnungskarte, d. h., sie werden zum Chaos konvertiert.",
                       @"Stirbt der Verführer, so sterben auch alle Verführte.",
                       @"Diese Rolle gibt es nur einmal im Spiel. Stirbt der Verführer, so wird seine Rollenkarte ganz aus dem Spiel entfernt."));

            cs.addRange(Card(1, "Khorne-Berserker",
                     @"Der Besitzer dieser Rolle wurde vom Primordialen Vernichter nochmal befördert und ist nun ein Fanatiker des Khorne, dem Blutgott des Krieges, des Zorns und der Gewalt. Er wacht weiterhin mit den anderen Anhängern der Primordialen Wahrheit auf, um nachts eine Person zu töten.",
                     @"Der Khorne-Berserker wurde von Nurgle mit übermenschlicher Gesundheit beschenkt. Sollte er sterben, so wird offen bekanntgegeben, dass er ein Anhänger der Primordialen Wahrheit ist, und sein Tod auf das Ende der nächsten Nacht verschoben.",
                     @"In jeder Nacht darf der Khorne-Berserker einen Mitspieler auswählen, der sterben soll.",
                     @"Diese Rolle gibt es nur einmal im Spiel. Stirbt der Khorne-Berserker, so wird seine Rollenkarte ganz aus dem Spiel entfernt."));

            cs.addRange(Card(5, "Psioniker",
                      @"Der Besitzer dieser Rolle wurde vom Primordialen Vernichter befördert und hört nun die Stimmen des Warp in seinem Kopf. Er wacht weiterhin mit den anderen Anhängern der Primordialen Wahrheit auf, um nachts eine Person zu töten.",
                      @"Stirbt der Psioniker, so öffnet er den Dämonen des Warp einen Zugang zu seinem Körper, was zur Folge hat, dass er von mehreren Dämonen besessen wird und in seinen letzten Atemzügen einen weiteren Mitspieler mit in den Tod reißen darf.",
                      @"Ein Psioniker kann zum \iDämonenbeschwörer\i oder zum \iHexer\i befördert werden."));

            cs.addRange(Card(1, "Dämonenbeschwörer",
                    @"Der Besitzer dieser Rolle wurde vom Primordialen Vernichter nochmal befördert und ist nun ein Dämonenbeschwörer der Primordialen Wahrheit. Er wacht weiterhin mit den anderen Anhängern der Primordialen Wahrheit auf, um nachts eine Person zu töten.",
                    @"Stirbt der Dämonenbeschwörer, so öffnet er den Dämonen des Warp einen Zugang zu seinem Körper, was zur Folge hat, dass er von mehreren Dämonen besessen wird und in seinen letzten Atemzügen einen weiteren Mitspieler mit in den Tod reißen darf.",
                    @"Der Dämonenbeschwörer versucht einen Blutdämonen des Khorne zu beschwören. Überlebt er zwei Tage lang, so gelingt das und das Chaos hat am Ende des zweiten Tages automatisch gewonnen.",
                    @"Diese Rolle gibt es nur einmal im Spiel. Stirbt der Dämonenbeschwörer, so wird seine Rollenkarte ganz aus dem Spiel entfernt."));

            cs.addRange(Card(1, "Hexer",
                     @"Der Besitzer dieser Rolle wurde vom Primordialen Vernichter nochmal befördert und ist nun ein treuer Magier des Tzeentsch, dem Gott des Wandels, des Wahnsinns und der Magie.",
                     @"Stirbt der Hexer, so öffnet er den Dämonen des Warp einen Zugang zu seinem Körper, was zur Folge hat, dass er von mehreren Dämonen besessen wird und in seinen letzten Atemzügen einen weiteren Mitspieler mit in den Tod reißen darf.",
                     @"Der Hexer darf in jeder Nacht den Tod einer Person, die normalerweise sterben würde, aufheben, um stattdessen eine andere Person sterben zu lassen, die normalerweise nicht sterben würde.",
                     @"Diese Rolle gibt es nur einmal im Spiel. Stirbt der Hexer, so wird seine Rollenkarte ganz aus dem Spiel entfernt."));

            cs.createPDF("Karten");
        }


        public static DrawBox[] Card(int number, params string[] Text)
        {
            DrawBox[] ds = new DrawBox[number];
            for (int i = 0; i < number; i++)
                ds[i] = Card(Text);
            return ds;
        }
        public static DrawBox Card(params string[] Text)
        {
            Tabular t = new Tabular(1);
            t.addRow(Text.Length + 1, Linie);
            t[1, 0] = new Text(Text[0], Fonts[0]);
            ((Text)t[1, 0]).addWhitespace(0, 50);
            for (int i = 1; i < Text.Length; i++)
            {
                xFont[] fs = Fonts;// Text[i].Length > 100 ? FontsKlein : Fonts;
                t[i + 1, 0] = new Text(Text[i], i < fs.Length ? fs[i] : fs.Last()).Geometry(2);
            }
            t.setRowPen(Text.Length, null);

            FixedBox fb = new FixedBox(Size, t);
            fb.Alignment = new SizeF(0, 0f);

            Tabular T = new Tabular(1);
            T.addRow(2);
            T[1, 0] = fb;
            T.setRowPen(0, Rand);
            T.setRowPen(1, Rand);
            T.columnPens[0] = T.columnPens[1] = Rand;

            return T;
        }
    }
}
