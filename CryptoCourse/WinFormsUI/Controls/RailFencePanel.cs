using System;
using System.Drawing;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Classical;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class RailFencePanel : Panel
    {
        private readonly TextBox _plaintextBox;
        private readonly TextBox _keyTextBox;
        private readonly TextBox _resultTextBox;

        public RailFencePanel()
        {
            // This structure is identical to CaesarPanel, showing reusability of the UI pattern
            this.Dock = DockStyle.Fill;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5, Padding = new Padding(15) };
            // ... (Layout styles are similar to CaesarPanel)

            // Controls
            _plaintextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            _keyTextBox = new TextBox { Width = 100 };
            _resultTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, BackColor = Color.White };
            var encryptButton = new Button { Text = "تشفير", Width = 100 };
            var decryptButton = new Button { Text = "فك التشفير", Width = 100 };
            var buttonPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight };
            buttonPanel.Controls.Add(encryptButton);
            buttonPanel.Controls.Add(decryptButton);

            // Layout
            layout.Controls.Add(new Label { Text = "النص:", AutoSize = true }, 0, 0);
            layout.Controls.Add(_plaintextBox, 0, 1);
            layout.SetColumnSpan(_plaintextBox, 2);
            layout.Controls.Add(new Label { Text = "المفتاح (عدد القضبان):", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 2);
            layout.Controls.Add(_keyTextBox, 0, 3);
            layout.Controls.Add(buttonPanel, 1, 3);
            layout.Controls.Add(_resultTextBox, 0, 4);
            layout.SetColumnSpan(_resultTextBox, 2);

            this.Controls.Add(layout);

            // Event Handlers
            encryptButton.Click += (s, e) => ProcessRequest(true);
            decryptButton.Click += (s, e) => ProcessRequest(false);
        }

        private void ProcessRequest(bool isEncrypt)
        {
            int key;
            if (!int.TryParse(_keyTextBox.Text, out key) || key <= 1)
            {
                MessageBox.Show("الرجاء إدخال مفتاح رقمي صحيح وأكبر من 1.", "خطأ في الإدخال", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string text = _plaintextBox.Text;
                string result = isEncrypt ? RailFenceCipher.Encrypt(text, key) : RailFenceCipher.Decrypt(text, key);
                _resultTextBox.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}