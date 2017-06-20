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

        public Iteration(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public void AddIterationEntry(IterationEntry entry)
        {
            entries.Add(entry);
        }

        public override string ToString()
        {
            string result = Name;

            foreach (var item in entries)
            {
                result += Environment.NewLine;
                result += item.ToString();
            }

            return result;
        }
    }
}
