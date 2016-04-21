using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Text.RegularExpressions;

namespace ImXORy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        //static byte[] BytesStringToBytesArray(string bytesString)
        //{
        //    var bytesCount = bytesString.Length / 2;

        //    var bytes = new byte[bytesCount];


        //}
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }





        private void Crypt_Button_Click(object sender, RoutedEventArgs e)
        {
            if(PlainText_TextBox.Text.Length == 0)
            {
                EncryptedText_TextBox.Text = "";
                return;
            }
                

            byte[] plainTextBytes = GetBytes(PlainText_TextBox.Text);
            byte[] passwordBytes = GetBytes(Password_TextBox.Text);

            var passLength = passwordBytes.Count();

            for (int index = 0; index < plainTextBytes.Count(); index++)
            {
                plainTextBytes[index] = (byte)(plainTextBytes[index] ^ passwordBytes[index % passLength]);
            }

            var xoredBytesToBase64 = Convert.ToBase64String(plainTextBytes);
            var Base64WithoutEnding = xoredBytesToBase64.Substring(0, xoredBytesToBase64.Length - 2);

            EncryptedText_TextBox.Text = Base64WithoutEnding;
            //var passLength = Password_TextBox.Text.Length;

            //var encrypted = "";

            //var currentPassChar = ' ';
            //var encryptedChar = "";
            //var pos = 0;
            //foreach(char ch in PlainText_TextBox.Text)
            //{
            //    currentPassChar = Password_TextBox.Text[pos % passLength];
                
            //    //var xoredToDigit = (int)currentPassChar ^ (int)ch;
            //    var xoredToDigit = currentPassChar ^ ch;
            //    var bytes = (byte)xoredToDigit;

            //    encryptedChar = String.Format("{0:X4}", xoredToDigit);
            //    //encryptedChar = ((int)currentPassChar ^ (int)ch).ToString();
            //    //encryptedChar = ((char)((int)currentPassChar ^ (int)ch)).ToString();

            //    encrypted += encryptedChar;

            //    pos++;
            //}


            //EncryptedText_TextBox.Text = encrypted;
        }

        private void DeCrypt_Button_Click(object sender, RoutedEventArgs e)
        {
            //if(EncryptedText_TextBox.Text.Length % 4 != 0)
            //{
            //    PlainText_TextBox.Text = "Невозможная шифр-комбинация! Проверьте правильность ввода.";
            //    return;
            //}

            //var regex = @"^[0-9A-F]+$";

            //if(!Regex.IsMatch(EncryptedText_TextBox.Text.ToUpper(), regex))
            //{
            //    PlainText_TextBox.Text = "Невозможная шифр-комбинация! Проверьте правильность ввода.";
            //    return;
            //}


            //var decrypted = "";
            //var passIndex = 0;
            //for(int pos = 0; pos < EncryptedText_TextBox.Text.Length; pos += 4)
            //{
            //    var currentBytes = EncryptedText_TextBox.Text.Substring(pos, 4);
                

            //    passIndex++;
            //}

            try
            {
                var base64 = EncryptedText_TextBox.Text + "==";

                var cryptedBytes = Convert.FromBase64String(base64);

                byte[] passwordBytes = GetBytes(Password_TextBox.Text);
                var passLength = passwordBytes.Count();

                for (int index = 0; index < cryptedBytes.Count(); index++)
                {
                    cryptedBytes[index] = (byte)(cryptedBytes[index] ^ passwordBytes[index % passLength]);
                }

                var decrypted = GetString(cryptedBytes);

                PlainText_TextBox.Text = decrypted;
            }
            catch
            {
                PlainText_TextBox.Text = "Невозможная шифр-комбинация! Проверьте правильность ввода.";
                return;
            }
        }
    }
}
