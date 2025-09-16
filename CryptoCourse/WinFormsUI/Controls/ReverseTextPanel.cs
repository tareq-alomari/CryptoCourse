using System;
using System.Drawing;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Classical;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class ReverseTextPanel : Panel
    {
        public ReverseTextPanel()
        {
            this.Dock = DockStyle.Fill;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(15) };

            var plaintextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            var resultTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, BackColor = Color.White };
            var processButton = new Button { Text = "نفذ العكس", Width = 150 };

            layout.Controls.Add(new Label { Text = "النص:", AutoSize = true }, 0, 0);
            layout.Controls.Add(plaintextBox, 0, 1);
            layout.Controls.Add(processButton, 0, 2);
            layout.Controls.Add(resultTextBox, 0, 3);

            this.Controls.Add(layout);

            processButton.Click += (s, e) => {
                resultTextBox.Text = ReverseTextCipher.Process(plaintextBox.Text);
            };
        }
    }
}