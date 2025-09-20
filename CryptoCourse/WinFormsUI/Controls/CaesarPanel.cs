using System;
using System.Drawing;
using System.Windows.Forms;
// Note: We will add the 'using' for the Core project later
 using CryptoCourse.Core.Algorithms.Classical; 

namespace CryptoCourse.WinFormsUI.Controls
{
    public class CaesarPanel : Panel
    {
        // Define UI controls as private fields
        private readonly TextBox _plaintextBox;
        private readonly TextBox _keyTextBox;
        private readonly TextBox _resultTextBox;
        private readonly Button _encryptButton;
        private readonly Button _decryptButton;

        public CaesarPanel()
        {
            this.Dock = DockStyle.Fill; // Make this panel fill its container (_contentPanel)

            // Use a TableLayoutPanel for organized layout within this panel
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5, // More rows for spacing
                Padding = new Padding(15)
            };

            // Define row and column styles
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Label
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F)); // TextBox
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Label
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Key + Buttons
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F)); // Result TextBox

            // --- Create and add controls ---

            // Plaintext Input
            layout.Controls.Add(new Label { Text = "النص (Plaintext):", AutoSize = true }, 0, 0);
            _plaintextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            layout.SetColumnSpan(_plaintextBox, 2);
            layout.Controls.Add(_plaintextBox, 0, 1);

            // Key Input
            layout.Controls.Add(new Label { Text = "المفتاح (رقم صحيح):", Anchor = AnchorStyles.Left, AutoSize = true }, 0, 2);

            _keyTextBox = new TextBox { Width = 100 };
            layout.Controls.Add(_keyTextBox, 0, 3);

            // Buttons
            var buttonPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
            _encryptButton = new Button { Text = "تشفير", Width = 100  , Height = 30};
            _decryptButton = new Button { Text = "فك التشفير", Width = 100, Height = 30 };
            buttonPanel.Controls.Add(_encryptButton);
            buttonPanel.Controls.Add(_decryptButton);
            layout.Controls.Add(buttonPanel, 1, 3);

            // Result Output
            _resultTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical, ReadOnly = true, BackColor = Color.White };
            layout.SetColumnSpan(_resultTextBox, 2);
            layout.Controls.Add(_resultTextBox, 0, 4);

            this.Controls.Add(layout);

            // Attach event handlers to buttons
            // We will add the logic in Step 3
            _encryptButton.Click += EncryptButton_Click;
            _decryptButton.Click += DecryptButton_Click;
        }
        private void ProcessRequest(bool isEncrypt)
        {
            try
            {
                string inputText = _plaintextBox.Text;
                int key;
                if (!int.TryParse(_keyTextBox.Text,  out key))
                {
                    MessageBox.Show("الرجاء إدخال مفتاح رقمي صحيح.", "خطأ في الإدخال", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // For decryption, we simply use a negative key
                int processKey = isEncrypt ? key : -key;

                string resultText = CaesarCipher.Process(inputText, processKey);
                _resultTextBox.Text = resultText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ غير متوقع: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            ProcessRequest(isEncrypt: true);
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            // For decryption, we take the text from the result box if the plain text is empty
            if (string.IsNullOrEmpty(_plaintextBox.Text))
            {
                _plaintextBox.Text = _resultTextBox.Text;
            }
            ProcessRequest(isEncrypt: false);
        }
    }
}