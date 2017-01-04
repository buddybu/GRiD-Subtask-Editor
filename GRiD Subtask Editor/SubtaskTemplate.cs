using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRiD_Subtask_Editor
{
    [Serializable]
    class SubtaskTemplate
    {
        private List<SubtaskItem> template;

        public List<SubtaskItem> Template
        {
            get
            {
                return template;
            }
        }

        public int Count
        {
            get
            {
                return template.Count;
            }
        }

        public SubtaskTemplate()
        {
            template = new List<SubtaskItem>();
        }

        public void AddSubtaskItem(SubtaskItem item)
        {
            template.Add(item);
        }

    }
}
