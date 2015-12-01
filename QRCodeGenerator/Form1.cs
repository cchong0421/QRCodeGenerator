/* =========================================================================================
 * === 程式說明
 * === 專案名稱：QRCodeGenerator (QRCode 名片產生器)
 * =========================================================================================
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace QRCodeGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                MessageBox.Show("姓氏不可為空白");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("BEGIN:VCARD\r\n");
            sb.Append("VERSION:2.1\r\n");
            sb.AppendFormat("N;LANGUAGE=zh-tw;CHARSET=utf-8:{0};{1}\r\n", txtLastName.Text.Trim(), txtFirstName.Text.Trim());

            if (string.IsNullOrEmpty(txtFN.Text))
                txtFN.Text = string.Format("{0}{1}", txtLastName.Text, txtFirstName.Text);
            sb.AppendFormat("FN;CHARSET=utf-8:{0}\r\n", txtFN.Text.Trim());

            if (!string.IsNullOrEmpty(txtORG.Text))
                sb.AppendFormat("ORG;CHARSET=utf-8:{0};{1}\r\n", txtORG.Text.Trim(), txtDepart.Text.Trim());

            if (!string.IsNullOrEmpty(txtMobile.Text))
                sb.AppendFormat("TEL;CELL;VOICE:{0}\r\n", txtMobile.Text.Trim());

            if (!string.IsNullOrEmpty(txtWork.Text))
                sb.AppendFormat("TEL;WORK;VOICE:{0}\r\n", txtWork.Text.Trim());

            if (!string.IsNullOrEmpty(txtHome.Text))
                sb.AppendFormat("TEL;HOME;VOICE:{0}\r\n", txtHome.Text.Trim());

            if (!string.IsNullOrEmpty(txtEmail.Text))
                sb.AppendFormat("EMAIL;PREF;INTERNET:{0}\r\n", txtEmail.Text.Trim());

            sb.AppendFormat("REV:{0}\r\n", DateTime.Now.ToString("yyyyMMddThhmmssZ"));
            sb.Append("END:VCARD");

            txtVCardResult.Text = sb.ToString();

            string sEncode = "utf-8";
            string EncoderContent = GetEncodingString(txtVCardResult.Text, sEncode);

            BarcodeWriter bw = new BarcodeWriter();
            bw.Format = BarcodeFormat.QR_CODE;
            bw.Options.Width = 260;
            bw.Options.Height = 237;
            bw.Options.PureBarcode = true;

            Bitmap bitmap = bw.Write(EncoderContent);
            pictureBox1.Image = (Image)bitmap;
        }

        private void txtLastName_Leave(object sender, EventArgs e)
        {
            txtFN.Text = string.Format("{0}{1}", txtLastName.Text, txtFirstName.Text);
        }
        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            txtFN.Text = string.Format("{0}{1}", txtLastName.Text, txtFirstName.Text);
        }
        private string GetEncodingString(string srcString, string sEncode)
        {
            Encoding e8859Encode = Encoding.GetEncoding("iso-8859-1");
            Encoding srcEncode = Encoding.Unicode;
            Encoding dstEncode = Encoding.GetEncoding(sEncode);
            byte[] srcBytes = srcEncode.GetBytes(srcString);
            byte[] dstBytes = Encoding.Convert(srcEncode, dstEncode, srcBytes);
            char[] dstChars = new char[e8859Encode.GetCharCount(dstBytes, 0, dstBytes.Length)];
            e8859Encode.GetChars(dstBytes, 0, dstBytes.Length, dstChars, 0);
            return new string(dstChars);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Png Image|*.png";
            dialog.Title = "Save an Image File";
            dialog.InitialDirectory = "C:\\";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (dialog.FileName != "")
                {
                    pictureBox1.Image.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show("Save done!!");
                }
            }
            
        }
    }
}
