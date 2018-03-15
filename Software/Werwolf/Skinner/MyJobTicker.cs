using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Printing;

namespace Skinner
{
    public class MyJobTicker : IJobTicker
    {
        public int NumberOfJobs { get; set; }
        public int Solved { get; set; }

        public bool ErrorOccured { get; set; }

        public void Exited(int IDJob, int ExitCode)
        {
            Solved++;
            float p = Solved * 1f / NumberOfJobs;
            if (ExitCode == 0)
                Console.WriteLine("Seite " + (IDJob + 1) + " wurde erstellt. ("
                + p.ToString("P") + " fertig)");
            else
            {
                Console.WriteLine("Seite " + (IDJob + 1) + " konnte nicht erstellt werden. (Exitcode: "
                  + ExitCode + ")");
                ErrorOccured = true;
            }
        }

        public void Reset(int NumberOfJobs)
        {
            this.NumberOfJobs = NumberOfJobs;
            this.Solved = 0;
            this.ErrorOccured = false;
            Console.WriteLine(NumberOfJobs + " Seiten werden erstellt...");
        }
    }
}
