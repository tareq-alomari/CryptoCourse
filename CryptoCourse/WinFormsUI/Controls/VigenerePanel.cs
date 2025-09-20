using System;
using System.Drawing;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Classical;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class VigenerePanel : Panel
    {
        private readonly TextBox _plaintextBox;
        private readonly TextBox _keyTextBox;
        private readonly TextBox _resultTextBox;

        public VigenerePanel()
        {
            this.Dock = DockStyle.Fill;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(15) };

            _plaintextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            _keyTextBox = new TextBox { Dock = DockStyle.Fill };
            _resultTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, BackColor = Color.White };
            var encryptButton = new Button { Text = "تشفير", Width = 100 };
            var decryptButton = new Button { Text = "فك التشفير", Width = 100 };
            var buttonPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill, Height = 40 };
            buttonPanel.Controls.Add(encryptButton);
            buttonPanel.Controls.Add(decryptButton);

            layout.Controls.Add(new Label { Text = "النص:", AutoSize = true }, 0, 0);
            layout.Controls.Add(_plaintextBox, 0, 1);
            layout.Controls.Add(new Label { Text = "المفتاح (كلمة):", AutoSize = true }, 0, 2);
            var keyAndButtonLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            keyAndButtonLayout.Controls.Add(_keyTextBox, 0, 0);
            keyAndButtonLayout.Controls.Add(buttonPanel, 1, 0);
            layout.Controls.Add(keyAndButtonLayout, 0, 3);
            layout.Controls.Add(_resultTextBox, 0, 4);

            this.Controls.Add(layout);

            encryptButton.Click += (s, e) => ProcessRequest(true);
            decryptButton.Click += (s, e) => ProcessRequest(false);
        }

        private void ProcessRequest(bool isEncrypt)
        {
            if (string.IsNullOrWhiteSpace(_keyTextBox.Text))
            {
                MessageBox.Show("الرجاء إدخال مفتاح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _resultTextBox.Text = VigenereCipher.Process(_plaintextBox.Text, _keyTextBox.Text, isEncrypt);
        }
    }
}