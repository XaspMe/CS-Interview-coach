using System;
using System.Collections.Generic;

namespace CS_Interview_coach
{
    /// <summary>
    /// Static class for initializing and action with loaded dictionary.
    /// </summary>
    public static class QADictionary
    {
        /// <summary>
        /// Variable to save the current position of the question, to add it to the stack of questions when generating the next.
        /// </summary>
        private static int currentPos;

        /// <summary>
        /// Dictionary for managing answer and questions
        /// </summary>
        public static Dictionary<int, QAInstance> questionAndAnswers = new Dictionary<int, QAInstance>();

        /// <summary>
        /// The stack to return to the previous question => BackToPreviousQuestion().
        /// </summary>
        public static Stack<int> Previous = new Stack<int>();


        /// <summary>
        /// Reloading dictionary after changing XML file with questions and answers. 1 step - clear curent value, 2 step - read XML
        /// </summary>
        public static void ReloadDictionary() 
        {
            QADictionary.questionAndAnswers.Clear();
            new XMLWorker(System.IO.Directory.GetCurrentDirectory() + Properties.Settings.Default.XMLname).Read();
        }

        /// <summary>
        /// Randomly generating next instance and saving previous q. number. If XML is empty return null value.
        /// </summary>
        /// <returns></returns>
        public static QAInstance GenerateNextQuestion() 
        {
            if(!(currentPos == 0))
                Previous.Push(currentPos);
            if (QADictionary.questionAndAnswers.Count > 0)
            {
                int random = new Random().Next(0, QADictionary.questionAndAnswers.Count);
                currentPos = random;
                return new QAInstance(questionAndAnswers[random].Question, questionAndAnswers[random].Answer);
            }
            else
                return null;
        }

        /// <summary>
        /// Function for returning to previuos question. Return null if stack count 0.
        /// </summary>
        /// <returns></returns>
        public static QAInstance BackToPreviousQuestion()  
        {
            if (!(Previous.Count == 0))
                return questionAndAnswers[Previous.Pop()];
            else 
                return null;
        }
    }
}
