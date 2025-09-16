using System;
using System.Drawing;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Classical;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class ReverseBlocksPanel : Panel
    {
        private readonly TextBox _plaintextBox;
        private readonly TextBox _keyTextBox; // Block Size
        private readonly TextBox _resultTextBox;

        public ReverseBlocksPanel()
        {
            this.Dock = DockStyle.Fill;
            // Using a familiar layout structure
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5, Padding = new Padding(15) };

            _plaintextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            _keyTextBox = new TextBox { Width = 100 };
            _resultTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, BackColor = Color.White };
            var processButton = new Button { Text = "نفذ عكس البلوكات", Width = 150 };

            layout.Controls.Add(new Label { Text = "النص:", AutoSize = true }, 0, 0);
            layout.Controls.Add(_plaintextBox, 0, 1);
            layout.SetColumnSpan(_plaintextBox, 2);
            layout.Controls.Add(new Label { Text = "حجم البلوك (المفتاح):", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 2);
            layout.Controls.Add(_keyTextBox, 0, 3);
            layout.Controls.Add(processButton, 1, 3);
            layout.Controls.Add(_resultTextBox, 0, 4);
            layout.SetColumnSpan(_resultTextBox, 2);

            this.Controls.Add(layout);

            // Event Handler for the button
            processButton.Click += ProcessButton_Click;
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            int blockSize;
            if (!int.TryParse(_keyTextBox.Text, out  blockSize) || blockSize <= 0)
            {
                MessageBox.Show("الرجاء إدخال حجم بلوك (رقم صحيح موجب).", "خطأ في الإدخال", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                _resultTextBox.Text = ReverseBlocksCipher.Process(_plaintextBox.Text, blockSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}