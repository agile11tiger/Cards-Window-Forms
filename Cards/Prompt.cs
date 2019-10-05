using System.Windows.Forms;

namespace DurakGame
{
    /// <summary>
    /// Borrowed from http://stackoverflow.com/questions/5427020/prompt-dialog-in-windows-forms
    /// </summary>
    public static class Prompt
    {
        /// <summary>
        /// Shows a dialogue box with an input and returns the text entered
        /// </summary>
        /// <param name="text">The message to show</param>
        /// <param name="caption">The dialog window's title</param>
        /// <returns>The text entered, or an empty string if they cancelled</returns>
        public static string ShowDialog(string text, string caption)
        {
            var prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            var textLabel = new Label() { Left = 50, Top = 20, Text = text };
            var textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            var confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
