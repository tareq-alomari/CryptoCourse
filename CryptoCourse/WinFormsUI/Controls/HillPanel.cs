using System;
using System.Drawing;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Classical;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class HillPanel : Panel
    {
        private readonly TextBox _plaintextBox;
        private readonly TextBox _keyTextBox;
        private readonly TextBox _resultTextBox;

        public HillPanel()
        {
            this.Dock = DockStyle.Fill;
            // Using a structure similar to previous panels
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
            layout.Controls.Add(new Label { Text = "المفتاح (نص بطول مربع كامل مثل 4, 9, 16):", AutoSize = true }, 0, 2);
            var keyAndButtonLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            keyAndButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            keyAndButtonLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            keyAndButtonLayout.Controls.Add(_keyTextBox, 0, 0);
            keyAndButtonLayout.Controls.Add(buttonPanel, 1, 0);
            layout.Controls.Add(keyAndButtonLayout, 0, 3);

            layout.Controls.Add(_resultTextBox, 0, 4);

            this.Controls.Add(layout);

            // Event Handlers
            encryptButton.Click += (s, e) => ProcessRequest(true);
            decryptButton.Click += (s, e) => ProcessRequest(false);
        }

        private void ProcessRequest(bool isEncrypt)
        {
            string text = _plaintextBox.Text;
            string key = _keyTextBox.Text;

            try
            {
                string result = isEncrypt ? HillCipher.Encrypt(text, key) : HillCipher.Decrypt(text, key);
                _resultTextBox.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ في المعالجة", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}