using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Interview_coach
{
    /// <summary>
    /// Class for Question and answer pair
    /// </summary>
    public class QAInstance
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        
        public QAInstance(string question, string answer)
        {
            this.Question = question;
            this.Answer = answer;
        }
    }
}
