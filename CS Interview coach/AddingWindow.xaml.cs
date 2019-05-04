using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS_Interview_coach
{
    public partial class Add : Window
    {
        public Add()
        {
            InitializeComponent();
            this.Closed += new EventHandler(AddWindowsClosed);
        }

        /// <summary>
        /// Adding new record to XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)  
        {
            if (AddQuestion.Text.Length < 5 || AddAnswer.Text.Length < 5)
            {
                MessageBox.Show("Too short question or answer, minimum 5 characters.");
            }
            else
            {
                new XMLWorker(System.IO.Directory.GetCurrentDirectory() + Properties.Settings.Default.XMLname)
                    .Increment(new QAInstance(AddQuestion.Text.ToString(), AddAnswer.Text.ToString()));

                AddQuestion.Text = "";
                AddAnswer.Text = "";
                MessageBox.Show("Success");
            }
        }

        /// <summary>
        /// On closing this windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddWindowsClosed(object sender, EventArgs e) 
        {
            QADictionary.ReloadDictionary();
        }
    }
}
