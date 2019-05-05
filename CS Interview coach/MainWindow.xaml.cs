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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CS_Interview_coach
{
    public partial class MainWindow : Window
    {
        private bool EditMode = false;
        private string AnswerValue;
        private string QuestionValue;

        public MainWindow()
        {
            InitializeComponent();
            QADictionary.ReloadDictionary();
            GenNext();
        }

        /// <summary>
        /// Create the next question in the user interface in case the XML file exists.
        /// </summary>
        public void GenNext()
        {
            QAInstance instance = QADictionary.GenerateNextQuestion();

            if (instance == null)
                MessageBox.Show($"No records found in the XML file.");
            else
            {
                AnswerTB.Text = instance.Answer;
                QuestionTB.Text = instance.Question;
            }
        }

        public void GenPrev()
        {
            QAInstance instance = QADictionary.BackToPreviousQuestion();
            if (instance != null)
            {
                AnswerTB.Text = instance.Answer;
                QuestionTB.Text = instance.Question;
            }
        }



        /// <summary>
        /// Go to next question
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GenNext();
        }

        /// <summary>
        /// Call form to adding new pair.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Add().Show();
            IsEnabled = false;
        }

        /// <summary>
        /// Back to prev question.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Previous_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GenPrev();
        }

        /// <summary>
        /// Invert .isenabled state for UiElements
        /// </summary>
        /// <param name="controls"></param>
        private void ControlEnableInvert(FrameworkElement[] controls)
        {
            foreach (var item in controls)
            {
                if (item is FrameworkElement && item != null)
                    ((FrameworkElement)item).IsEnabled = !((FrameworkElement)item).IsEnabled;
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (EditMode)  // Disabling UI edit mode
            {
                if (MessageBox.Show("Save changes?", "Apply?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    QAInstance instance = new QAInstance(QuestionTB.Text, AnswerTB.Text);
                    new XMLWorker(System.IO.Directory.GetCurrentDirectory() + Properties.Settings.Default.XMLname).UpdateExisting(
                        new QAInstance(QuestionValue, AnswerValue), new QAInstance(QuestionTB.Text, AnswerTB.Text));
                }
                else
                {
                    QuestionTB.Text = QuestionValue;
                    AnswerTB.Text = AnswerValue;
                }
                EditMode = false;

                AnswerTB.Background = Brushes.Transparent;

                QuestionTB.Background = Brushes.Transparent;
                EditButton.Content = "Edit";
                QuestionValue = null;
                AnswerValue = null;
                QADictionary.ReloadDictionary();

                ControlEnableInvert(new FrameworkElement[] { AddButton, Next, Previous });

                AnswerTB.IsReadOnly = true;
                QuestionTB.IsReadOnly = true;
            }
            else // Edit mode in UI
            {
                AnswerValue = AnswerTB.Text;
                QuestionValue = QuestionTB.Text;
                EditMode = true;
                EditButton.Content = "Save";
                AnswerTB.Background = Brushes.WhiteSmoke;
                QuestionTB.Background = Brushes.WhiteSmoke;

                ControlEnableInvert(new FrameworkElement[] { AddButton, Next, Previous });

                AnswerTB.IsReadOnly = false;
                QuestionTB.IsReadOnly = false;
            }
        }

        /// <summary>
        /// Hiding answer block on UI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ShowAnswer = (CheckBox)sender;
            if (ShowAnswer != null)
            {
                if (!ShowAnswer.IsChecked == true)
                    AnswerTB.Foreground = new SolidColorBrush(Colors.White);
                else
                    AnswerTB.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// Changing font size on UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender is Slider)
            {
                int Size = (int)(sender as Slider).Value;
                AnswerTB.FontSize = Size;
                QuestionTB.FontSize = Size;
            }
        }

        /// <summary>
        /// Hotkey binding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
                GenNext();
            if (e.Key == Key.Left)
                GenPrev();
        }
    }
}
