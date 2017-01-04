using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRiD_Subtask_Editor
{
    [Serializable()]
    public class SubtaskItem
    {

        bool? addSubtask;
        string summary;
        string description;
        string assignee;
        double estimate;

        public bool? AddSubtask
        {
            get { return addSubtask; }
            set { addSubtask = value; }
        }
        public string Summary
        {
            get { return summary; }
            set { summary = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string Assignee
        {
            get { return assignee; }
            set { assignee = value; }
        }
        public double Estimate
        {
            get { return estimate; }
            set { estimate = value; }
        }
    }
}
