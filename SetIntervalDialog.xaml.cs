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

namespace AutoClicker
{
    /// <summary>
    /// Interaction logic for SetIntervalDialog.xaml
    /// </summary>
    public partial class SetIntervalDialog : Window
    {
        public int hours => int.Parse(hoursField.Text);
        public int minutes => int.Parse(minutesField.Text);
        public int seconds => int.Parse(secondsField.Text);
        public int milliseconds => int.Parse(millisecondsField.Text);

        public SetIntervalDialog()
        {
            InitializeComponent();
            hoursField.TextInput += TextInput;
        }

        private new void TextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Int32 selectionStart = textBox.SelectionStart;
            Int32 selectionLength = textBox.SelectionLength;
            String newText = String.Empty;
            int count = 0;
            foreach (Char c in textBox.Text.ToCharArray())
            {
                if (Char.IsDigit(c) || Char.IsControl(c) || (c == '.' && count == 0))
                {
                    newText += c;
                    if (c == '.')
                        count += 1;
                }
            }
            textBox.Text = newText;
            textBox.SelectionStart = selectionStart <= textBox.Text.Length ? selectionStart : textBox.Text.Length;
        }

        public bool ContainsLetter(string str)
        {
            foreach (char c in str.ToCharArray())
            {
                if (c != '0' || c != '1' || c != '2' || c != '3' || c != '4' || c != '5' || c != '6' || c != '7' || c != '8' || c != '9')
                {
                    return true;
                }
            }
            return false;
        }
    }
}
