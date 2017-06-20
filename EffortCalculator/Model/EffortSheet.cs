using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortCalculator.Model
{
    class EffortSheet
    {
        private List<EffortEntry> effortEntries = new List<EffortEntry>();
        private List<Iteration> iterations = new List<Iteration>();

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
        public void AddEffortEntry(EffortEntry entry)
        {
            effortEntries.Add(entry);
        }

        /// <summary>
        /// Fügt der Übersicht einen weitern Aufwandseintrag hinzu
        /// </summary>
        /// <param name="entry"></param>
        public void AddIteration(Iteration it)
        {
            iterations.Add(it);
        }

        public Iteration GetIteration(string name)
        {
            return iterations.FirstOrDefault(x => { return x.Name == name; });
        }

        /// <summary>
        /// Berechnet den kompletten Aufwand für alle Einträge auf stündlicher Basis in dieser Übersicht.
        /// </summary>
        /// <returns></returns>
        private float SumHourlyEffort()
        {
            float effort = 0f;
            foreach (var entry in effortEntries)
            {
                effort += entry.EffortInHours;
            }

            return effort;
        }

        /// <summary>
        /// Summiert alle Iterationen in dieser Übersicht.
        /// </summary>
        /// <returns></returns>
        private int SumIterations()
        {
            return iterations.Count;
        }

        public override string ToString()
        {
            string result = Name;
            result += Environment.NewLine;

            result += Environment.NewLine;
            result += "Stündliche Aufwände:";
            foreach (var item in effortEntries)
            {
                result += Environment.NewLine;
                result += item.ToString();
            }

            result += Environment.NewLine;
            result += $"Der noch abzurechnende stündliche Aufwand beträgt: {SumHourlyEffort()}h";

            result += Environment.NewLine;
            result += "Iterationen:";
            foreach (var item in iterations)
            {
                result += Environment.NewLine;
                result += item.ToString();
            }

            result += Environment.NewLine;
            result += $"Die noch abzurechnende Anzahl an Iterationen beträgt: {SumIterations()}";
            return result;
        }
    }
}
