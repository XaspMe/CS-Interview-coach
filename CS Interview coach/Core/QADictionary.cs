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
        /// Dictionary for managing answer and questions
        /// </summary>
        public static Dictionary<int, QAInstance> questionAndAnswers = new Dictionary<int, QAInstance>();
        /// <summary>
        /// The queue to return to the previous question => BackToPreviousQuestion().
        /// </summary>
        private static Queue<int> Previous = new Queue<int>(2);

        /// <summary>
        /// Reloading dictionary after changing XML file with questions and answers. 1 step - clear curent value, 2 step - read XML
        /// </summary>
        public static void ReloadDictionary() 
        {
            QADictionary.questionAndAnswers.Clear();
            new XMLWorker(System.IO.Directory.GetCurrentDirectory() + Properties.Settings.Default.XMLname).Read();
        }

        /// <summary>
        /// Randomly generating next instance and saving previous q. number. If XML is empty return null value
        /// </summary>
        /// <returns></returns>
        public static QAInstance GenerateNextQuestion() 
        {
            if (QADictionary.questionAndAnswers.Count > 0)
            {
                int random = new Random().Next(0, QADictionary.questionAndAnswers.Count);
                Previous.Enqueue(random);
                return new QAInstance(questionAndAnswers[random].Question, questionAndAnswers[random].Answer);
            }
            else
                return null;
        }

        /// <summary>
        /// Just function for returning to previuos question. Return null if Dictionary not stored selected key
        /// </summary>
        /// <returns></returns>
        public static QAInstance BackToPreviousQuestion()  
        {
            if (questionAndAnswers.ContainsKey(Previous.Peek()))
                return questionAndAnswers[Previous.Peek()];
            else
                return null;
        }
    }
}
