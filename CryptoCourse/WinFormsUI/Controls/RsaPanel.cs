using System;
using System.Windows.Forms;
using CryptoCourse.Core.Algorithms.Modern.SecureWrappers;

namespace CryptoCourse.WinFormsUI.Controls
{
    public class RsaPanel : Panel
    {
        public RsaPanel()
        {
            this.Dock = DockStyle.Fill;
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 4, Padding = new Padding(15) };

            var generateKeysButton = new Button { Text = "توليد زوج مفاتيح جديد", AutoSize = true };
            var publicKeyBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
            var privateKeyBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
            var plaintextBox = new TextBox { Dock = DockStyle.Fill, Height = 60 };
            var resultTextBox = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, Height = 60, BackColor = System.Drawing.Color.White };
            var encryptButton = new Button { Text = "تشفير (بالمفتاح العام)", Width = 150 };
            var decryptButton = new Button { Text = "فك التشفير (بالمفتاح الخاص)", Width = 150 };
            var buttonPanel = new FlowLayoutPanel();
            buttonPanel.Controls.Add(encryptButton);
            buttonPanel.Controls.Add(decryptButton);

            layout.Controls.Add(generateKeysButton, 0, 0);
            layout.SetColumnSpan(generateKeysButton, 2);
            layout.Controls.Add(new Label { Text = "المفتاح العام:", AutoSize = true }, 0, 1);
            layout.Controls.Add(new Label { Text = "المفتاح الخاص:", AutoSize = true }, 1, 1);
            layout.Controls.Add(publicKeyBox, 0, 2);
            layout.Controls.Add(privateKeyBox, 1, 2);
            var bottomLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
            bottomLayout.Controls.Add(new Label { Text = "النص:", AutoSize = true }, 0, 0);
            bottomLayout.Controls.Add(new Label { Text = "النتيجة:", AutoSize = true }, 1, 0);
            bottomLayout.Controls.Add(plaintextBox, 0, 1);
            bottomLayout.Controls.Add(resultTextBox, 1, 1);
            layout.Controls.Add(bottomLayout, 0, 3);
            layout.SetColumnSpan(bottomLayout, 2);
            layout.Controls.Add(buttonPanel, 0, 4);

            this.Controls.Add(layout);

            generateKeysButton.Click += (s, e) => {
                string pubKey , privKey;
                RsaWrapper.GenerateKeys(out  pubKey, out  privKey);
                publicKeyBox.Text = pubKey;
                privateKeyBox.Text = privKey;
            };
            encryptButton.Click += (s, e) => {
                try { resultTextBox.Text = RsaWrapper.Encrypt(plaintextBox.Text, publicKeyBox.Text); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Encryption Error"); }
            };
            decryptButton.Click += (s, e) => {
                try { plaintextBox.Text = RsaWrapper.Decrypt(resultTextBox.Text, privateKeyBox.Text); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Decryption Error"); }
            };
        }
    }
}