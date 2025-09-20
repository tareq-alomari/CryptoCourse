using System;
using System.Drawing;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Classical;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class AffinePanel : Panel
    {
        private readonly TextBox _plaintextBox;
        private readonly TextBox _keyATextBox;
        private readonly TextBox _keyBTextBox;
        private readonly TextBox _resultTextBox;

        public AffinePanel()
        {
            this.Dock = DockStyle.Fill;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5, Padding = new Padding(15) };
            // ... (Layout styles are similar to CaesarPanel, adjusted for new controls)

            // Controls
            _plaintextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            _keyATextBox = new TextBox { Width = 80 };
            _keyBTextBox = new TextBox { Width = 80 };
            _resultTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, BackColor = Color.White };
            var encryptButton = new Button { Text = "تشفير", Width = 100 , Height = 30 };
            var decryptButton = new Button { Text = "فك التشفير", Width = 100 , Height = 30 };

            // Layout arrangement
            var keyPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill, WrapContents = false };
            keyPanel.Controls.Add(new Label { Text = "المفتاح a:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) });
            keyPanel.Controls.Add(_keyATextBox);
            keyPanel.Controls.Add(new Label { Text = "المفتاح b:", AutoSize = true, Padding = new Padding(20, 6, 0, 0) });
            keyPanel.Controls.Add(_keyBTextBox);

            var buttonPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Fill };
            buttonPanel.Controls.Add(encryptButton);
            buttonPanel.Controls.Add(decryptButton);

            layout.Controls.Add(new Label { Text = "النص:", AutoSize = true }, 0, 0);
            layout.Controls.Add(_plaintextBox, 0, 1);
            layout.SetColumnSpan(_plaintextBox, 2);
            layout.Controls.Add(keyPanel, 0, 2);
            layout.SetColumnSpan(keyPanel, 2);
            layout.Controls.Add(buttonPanel, 0, 3);
            layout.SetColumnSpan(buttonPanel, 2);
            layout.Controls.Add(_resultTextBox, 0, 4);
            layout.SetColumnSpan(_resultTextBox, 2);

            this.Controls.Add(layout);

            // Event Handlers
            encryptButton.Click += (s, e) => ProcessRequest(true);
            decryptButton.Click += (s, e) => ProcessRequest(false);
        }

        private void ProcessRequest(bool isEncrypt)
        {
            int keyA, keyB;
            if (!int.TryParse(_keyATextBox.Text, out  keyA) || !int.TryParse(_keyBTextBox.Text, out  keyB))
            {
                MessageBox.Show("الرجاء إدخال مفاتيح رقمية صحيحة.", "خطأ في الإدخال", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (isEncrypt)
                {
                    _resultTextBox.Text = AffineCipher.Encrypt(_plaintextBox.Text, keyA, keyB);
                }
                else
                {
                    _resultTextBox.Text = AffineCipher.Decrypt(_plaintextBox.Text, keyA, keyB);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}