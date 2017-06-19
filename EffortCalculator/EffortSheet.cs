using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortCalculator
{
    class EffortSheet
    {
        private List<EffortIssue> effortEntries = new List<EffortIssue>();

        public EffortSheet(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Diese Übersicht enthält Aufwände ab (inklusive) diesem Datum.
        /// </summary>
        public DateTime From { get; private set; }

        /// <summary>
        /// Diese Übersicht enthält Aufwände bis (inklusive) diesem Datum.
        /// </summary>
        public DateTime To { get; private set; }

        /// <summary>
        /// Der Name für diese Übersicht.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Fügt der Übersicht einen weitern Aufwandseintrag hinzu
        /// </summary>
        /// <param name="entry"></param>
        public void AddEffortEntry(EffortIssue entry)
        {
            effortEntries.Add(entry);
        }

        /// <summary>
        /// Berechnet den kompletten Aufwand für alle Einträge in dieser Übersicht.
        /// </summary>
        /// <returns></returns>
        public float GetOverallEffort()
        {
            float effort = 0f;
            foreach (var entry in effortEntries)
            {
                effort += entry.EffortInHours;
            }

            return effort;
        }

        public override string ToString()
        {
            string result = Name;
            result += Environment.NewLine; 

            foreach (var item in effortEntries)
            {
                result += Environment.NewLine;
                result += item.ToString();
            }

            result += Environment.NewLine;
            result += $"Der noch abzurechnende Aufwand beträgt: {GetOverallEffort()}h";

            return result;
        }
    }
}
