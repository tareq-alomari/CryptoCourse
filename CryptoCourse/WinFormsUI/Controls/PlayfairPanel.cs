using CryptoCourse.Core.Algorithms.Classical;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class PlayfairPanel : Panel
    {
        private readonly TextBox _plaintextBox;
        private readonly TextBox _keyTextBox;
        private readonly TextBox _resultTextBox;
        private readonly TableLayoutPanel _matrixLayout;

        public PlayfairPanel()
        {
            // ... (Layout initialization is similar to before) ...
            this.Dock = DockStyle.Fill;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(10) };

            // Text boxes and buttons setup (similar to previous version)
            _plaintextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            _resultTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, BackColor = Color.White };
            layout.Controls.Add(new Label { Text = "النص:", AutoSize = true }, 0, 0);
            layout.Controls.Add(_plaintextBox, 0, 1);
            layout.Controls.Add(new Label { Text = "النتيجة:", AutoSize = true }, 0, 2);
            layout.Controls.Add(_resultTextBox, 0, 3);

            // Right panel for key and matrix
            var rightPanel = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3 };
            rightPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _keyTextBox = new TextBox { Dock = DockStyle.Fill };
            var encryptButton = new Button { Text = "تشفير", Width = 100 };
            var decryptButton = new Button { Text = "فك التشفير", Width = 100 };
            var buttonPanel = new FlowLayoutPanel();
            buttonPanel.Controls.Add(encryptButton);
            buttonPanel.Controls.Add(decryptButton);

            var matrixBox = new GroupBox { Text = "Playfair Matrix", Dock = DockStyle.Fill, Font = new Font(this.Font, FontStyle.Bold) };
            _matrixLayout = new TableLayoutPanel { Dock = DockStyle.Fill }; // Dynamic layout panel
            matrixBox.Controls.Add(_matrixLayout);

            rightPanel.Controls.Add(new Label { Text = "المفتاح:", AutoSize = true }, 0, 0);
            rightPanel.Controls.Add(_keyTextBox, 0, 1);
            rightPanel.Controls.Add(matrixBox, 0, 2);
            rightPanel.Controls.Add(buttonPanel, 0, 3);

            layout.Controls.Add(rightPanel, 1, 0);
            layout.SetRowSpan(rightPanel, 4);

            this.Controls.Add(layout);

            // Event Handlers
            _keyTextBox.TextChanged += (s, e) => UpdateMatrixDisplay();
            encryptButton.Click += (s, e) => ProcessRequest(true);
            decryptButton.Click += (s, e) => ProcessRequest(false);

            UpdateMatrixDisplay(); // Initial call
        }

        private void UpdateMatrixDisplay()
        {
            _matrixLayout.Controls.Clear();
            _matrixLayout.ColumnStyles.Clear();
            _matrixLayout.RowStyles.Clear();

            string key = _keyTextBox.Text.ToLower();
            bool isArabic = !string.IsNullOrEmpty(key) && "ابتثجحخدذرزسشصضطظعغفقكلمنهوي".Contains(key[0]);

            int rows = isArabic ? 4 : 5;
            int cols = isArabic ? 7 : 5;
            _matrixLayout.RowCount = rows;
            _matrixLayout.ColumnCount = cols;

            string alphabet = isArabic ? "ابتثجحخدذرزسشصضطظعغفقكلمنهوي" : "abcdefghiklmnopqrstuvwxyz";
            var keyString = new StringBuilder();
            var used = new HashSet<char>();
            foreach (char c in key) if (alphabet.Contains(c) && used.Add(c)) keyString.Append(c);
            foreach (char c in alphabet) if (used.Add(c)) keyString.Append(c);

            var cellFont = new Font("Consolas", 12F, FontStyle.Bold);
            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var label = new Label { Text = keyString.Length > index ? keyString[index++].ToString().ToUpper() : "", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BorderStyle = BorderStyle.FixedSingle, Font = cellFont };
                    _matrixLayout.Controls.Add(label, j, i);
                }
            }
        }

        private void ProcessRequest(bool isEncrypt)
        {
            try
            {
                string result = isEncrypt ? PlayfairCipher.Encrypt(_plaintextBox.Text, _keyTextBox.Text) : PlayfairCipher.Decrypt(_plaintextBox.Text, _keyTextBox.Text);
                _resultTextBox.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}