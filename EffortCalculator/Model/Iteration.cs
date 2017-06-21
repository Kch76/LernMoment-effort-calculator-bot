using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EffortCalculator.Model
{
    class Iteration
    {
        private List<IterationEntry> entries = new List<IterationEntry>();

        public Iteration(string name, string projectName)
        {
            Name = name;
            ProjectName = projectName;
        }

        /// <summary>
        /// Der Name dieser Iteration
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Der Name des Projektes für die diese Iteration ausgeführt wird
        /// </summary>
        public string ProjectName { get; private set; }

        public void AddIterationEntry(IterationEntry entry)
        {
            entries.Add(entry);
        }

        public IReadOnlyList<IterationEntry> GetAllEntries()
        {
            return entries.AsReadOnly();
        }

        public override string ToString()
        {
            string result = Name + " (" + ProjectName + ")";

            foreach (var item in entries)
            {
                result += Environment.NewLine;
                result += item.ToString();
            }

            return result;
        }
    }
}
