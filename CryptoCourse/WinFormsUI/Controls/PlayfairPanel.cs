using System;
using System.Drawing;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Classical;
using System.Text;
using System.Collections.Generic;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class PlayfairPanel : Panel
    {
        private readonly TextBox _plaintextBox;
        private readonly TextBox _keyTextBox;
        private readonly TextBox _resultTextBox;
        private readonly Label[,] _matrixLabels = new Label[5, 5]; // 2D array for the matrix display

        public PlayfairPanel()
        {
            this.Dock = DockStyle.Fill;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5, Padding = new Padding(10) };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Input and Result TextBoxes (left column)
            var textLayout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4 };
            textLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            textLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            textLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            textLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            _plaintextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical };
            _resultTextBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical, ReadOnly = true, BackColor = Color.White };

            textLayout.Controls.Add(new Label { Text = "النص:", AutoSize = true }, 0, 0);
            textLayout.Controls.Add(_plaintextBox, 0, 1);
            textLayout.Controls.Add(new Label { Text = "النتيجة:", AutoSize = true }, 0, 2);
            textLayout.Controls.Add(_resultTextBox, 0, 3);
            layout.Controls.Add(textLayout, 0, 0);
            layout.SetRowSpan(textLayout, 5);

            // Key Input (right column)
            layout.Controls.Add(new Label { Text = "المفتاح (Key):", AutoSize = true }, 1, 0);
            _keyTextBox = new TextBox { Dock = DockStyle.Top };
            layout.Controls.Add(_keyTextBox, 1, 1);

            // Matrix Display (right column)
            var matrixBox = new GroupBox { Text = "Playfair Matrix", Dock = DockStyle.Fill, Font = new Font(this.Font, FontStyle.Bold) };
            var matrixLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 5, RowCount = 5 };
            var cellFont = new Font("Consolas", 14F, FontStyle.Bold);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    _matrixLabels[i, j] = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, BorderStyle = BorderStyle.FixedSingle, Font = cellFont };
                    matrixLayout.Controls.Add(_matrixLabels[i, j], j, i);
                }
            }
            matrixBox.Controls.Add(matrixLayout);
            layout.Controls.Add(matrixBox, 1, 2);
            layout.SetRowSpan(matrixBox, 2);

            // Buttons (right column)
            var buttonPanel = new FlowLayoutPanel { FlowDirection = FlowDirection.LeftToRight, Dock = DockStyle.Top, Height = 40 };
            var encryptButton = new Button { Text = "تشفير", Width = 100 };
            var decryptButton = new Button { Text = "فك التشفير", Width = 100 };
            buttonPanel.Controls.Add(encryptButton);
            buttonPanel.Controls.Add(decryptButton);
            layout.Controls.Add(buttonPanel, 1, 4);

            this.Controls.Add(layout);

            // Event Handlers
            _keyTextBox.TextChanged += (s, e) => UpdateMatrixDisplay();
            encryptButton.Click += (s, e) => ProcessRequest(true);
            decryptButton.Click += (s, e) => ProcessRequest(false);

            // Initial matrix display
            UpdateMatrixDisplay();
        }

        private void UpdateMatrixDisplay()
        {
            string key = _keyTextBox.Text.ToUpper().Replace("J", "I");
            var keyString = new StringBuilder();
            var usedChars = new HashSet<char>();
            foreach (char c in key) { if (char.IsLetter(c) && usedChars.Add(c)) keyString.Append(c); }
            for (char c = 'A'; c <= 'Z'; c++) { if (c != 'J' && usedChars.Add(c)) keyString.Append(c); }

            int index = 0;
            for (int i = 0; i < 5; i++) { for (int j = 0; j < 5; j++) { _matrixLabels[i, j].Text = keyString.Length > index ? keyString[index++].ToString() : ""; } }
        }

        private void ProcessRequest(bool isEncrypt)
        {
            try
            {
                string key = _keyTextBox.Text;
                if (string.IsNullOrWhiteSpace(key))
                {
                    MessageBox.Show("الرجاء إدخال مفتاح.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (isEncrypt)
                {
                    _resultTextBox.Text = PlayfairCipher.Encrypt(_plaintextBox.Text, key);
                }
                else
                {
                    _resultTextBox.Text = PlayfairCipher.Decrypt(_plaintextBox.Text, key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}