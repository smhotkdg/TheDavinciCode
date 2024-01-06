using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenCvSharp;

namespace TheDavinciCodeVol2
{
    public partial class Form1 : Form
    {
        IplImage m_InputImage = null;
        IplImage m_OutputImage = null;
        List<string> convertStringList = new List<string>();
        List<string> RealConvertData = new List<string>();
        List<string> RestorationStringList = new List<string>();
        byte[] temp = new byte[20000];
        string Binarycode = string.Empty;
        static int count = 0;

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 20000; i++)
            {
                temp[i] = 0;
            }groupBox1.Visible = false;
            groupBox2.Visible = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string imgPath;

                OpenFileDialog dlg = new OpenFileDialog();
                                
                dlg.Filter = "Image Files(*.bmp,*jpg) | *.bmp; *.jpg";
                dlg.ShowDialog();
                imgPath = dlg.FileName;

                m_InputImage = new IplImage(imgPath, LoadMode.AnyColor);
                Bitmap bmp = new Bitmap(m_InputImage.ToBitmap());

                pictureBoxIpl1.ImageIpl = m_InputImage;
                groupBox1.Visible = true;
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string txtPath;

                OpenFileDialog dlg = new OpenFileDialog();

                dlg.DefaultExt = "txt.*";
                dlg.Filter = "Text Files(.txt)|*.txt";
                dlg.ShowDialog();
                txtPath = dlg.FileName;

                temp = System.IO.File.ReadAllBytes(dlg.FileName);

                for (int i = 0; i < temp.Count(); i++)
                {
                    string convertstring = Convert.ToString(temp[i], 2);
                    if (int.Parse(convertstring) < 10000000)
                    {
                        string t = "0" + convertstring;
                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }
                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }

                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }

                        if (t.Count() < 8)
                        {
                            t = "0" + t;
                        }

                        convertstring = t;
                    }
                    convertStringList.Add(convertstring);
                    for (int j = 0; j < convertstring.Length; j++)
                    {
                        RealConvertData.Add(convertstring[j].ToString());
                    }
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_InputImage == null)
            {
                MessageBox.Show("Please select the ImageFile want to encrypt");
                return;
            }
            if(temp[0] == 0)
            {
                MessageBox.Show("Please select the TextFile want to encrypt");
                return;
            }
            try
            {
                string r = string.Empty, g = string.Empty, b = string.Empty, t = string.Empty;
                m_OutputImage = m_InputImage.Clone();
                int mR = 0;
                int mB = 0;
                int mG = 0;
                int index = -1;
                if (m_InputImage != null)
                {
                    if (temp[0] != 0)
                    {
                        int n = temp.Count();
                        count = n;

                        for (int y = 0; y < m_InputImage.Height; y++)
                        {
                            for (int x = 0; x < m_InputImage.Width; x++)
                            {
                                if (RealConvertData.Count == index)
                                {
                                    break;
                                }
                                if (index < RealConvertData.Count - 1)
                                {
                                    ConvertImage(y, x, ref index);
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Encryption is complete! save the image file.");

                pictureBoxIpl2.ImageIpl = m_OutputImage;
                groupBox2.Visible = true;

                SaveFileDialog save = new SaveFileDialog();
                //save.DefaultExt = "bmp.*";
                save.Filter = "Image Files(*.bmp,*jpg) | *.bmp; *.jpg";
                save.ShowDialog();
                //Bitmap bmp = new Bitmap(m_InputImage.ToBitmap());
                //BitmapConverter.ToIplImage(bmp, m_InputImage);


                Cv.SaveImage(save.FileName, m_InputImage);

                //bmp.Save(save.FileName);
            }
            catch { }

        }

        private void ConvertImage(int y, int x,ref int _index)
        {
            try
            {
                int mR = 0;
                int mB = 0;
                int mG = 0;
                CvColor c = m_InputImage[y, x];
                mR = c.R % 2;
                mG = c.G % 2;
                mB = c.B % 2;
                for (int i = 0; i < 3; i++)
                {
                    _index++;
                    if (i == 0)
                    {
                        if (mR == 0)
                        {
                            if (RealConvertData[_index] == "1")
                            {
                                m_InputImage[y, x] = new CvColor
                                {
                                    B = c.B,
                                    G = c.G,
                                    R = (byte)(c.R + 1)
                                };
                                CvColor tempc = m_InputImage[y, x];
                                c.R = tempc.R;
                            }
                        }
                        if (mR == 1)
                        {
                            if (RealConvertData[_index] == "0")
                            {
                                m_InputImage[y, x] = new CvColor
                                {
                                    B = c.B,
                                    G = c.G,
                                    R = (byte)(c.R + 1)
                                };
                                CvColor tempc = m_InputImage[y, x];
                                c.R = tempc.R;
                            }
                        }
                    }
                    if (i == 1)
                    {
                        if (mG == 0)
                        {
                            if (RealConvertData[_index] == "1")
                            {
                                m_InputImage[y, x] = new CvColor
                                {
                                    B = c.B,
                                    G = (byte)(c.G + 1),
                                    R = c.R
                                };
                                CvColor tempc = m_InputImage[y, x];
                                c.G = tempc.G;
                            }
                        }
                        if (mG == 1)
                        {
                            if (RealConvertData[_index] == "0")
                            {
                                m_InputImage[y, x] = new CvColor
                                {
                                    B = c.B,
                                    G = (byte)(c.G + 1),
                                    R = c.R
                                };
                                CvColor tempc = m_InputImage[y, x];
                                c.G = tempc.G;
                            }
                        }
                    }
                    if (i == 2)
                    {
                        if (mB == 0)
                        {
                            if (RealConvertData[_index] == "1")
                            {
                                m_InputImage[y, x] = new CvColor
                                {
                                    B = (byte)(c.B + 1),
                                    G = c.G,
                                    R = c.R
                                };
                                CvColor tempc = m_InputImage[y, x];
                                c.B = tempc.B;
                            }
                        }
                        if (mB == 1)
                        {
                            if (RealConvertData[_index] == "0")
                            {
                                m_InputImage[y, x] = new CvColor
                                {
                                    B = (byte)(c.B + 1),
                                    G = c.G,
                                    R = c.R
                                };
                                CvColor tempc = m_InputImage[y, x];
                                c.B = tempc.B;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        private void restorationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please select the Image want to Restore");
            try
            {
                string imgPath;

                OpenFileDialog dlg = new OpenFileDialog();

                //dlg.DefaultExt = "bmp.*";
                dlg.Filter = "Image Files(*.bmp,*jpg) | *.bmp; *.jpg";
                dlg.ShowDialog();
                imgPath = dlg.FileName;

                m_OutputImage = new IplImage(imgPath, LoadMode.AnyColor);
                //Bitmap bmp = new Bitmap(m_OutputImage.ToBitmap());            

                pictureBoxIpl2.ImageIpl = m_OutputImage;
                groupBox2.Visible = true;
                Restoration();
            }
            catch {                
            }
        }

        private void Restoration()
        {
            try
            {
                int mR = 0;
                int mB = 0;
                int mG = 0;

                if (m_OutputImage != null)
                {
                    for (int y = 0; y < m_OutputImage.Height; y++)
                    {
                        for (int x = 0; x < m_OutputImage.Width; x++)
                        {
                            CvColor cOutput = m_OutputImage[y, x];
                            mR = cOutput.R % 2;
                            mG = cOutput.G % 2;
                            mB = cOutput.B % 2;
                            for (int i = 0; i < 3; i++)
                            {
                                if (i == 0)
                                {
                                    if (mR == 0)
                                    {
                                        RestorationStringList.Add("0");
                                    }
                                    if (mR == 1)
                                    {
                                        RestorationStringList.Add("1");
                                    }
                                }
                                if (i == 1)
                                {
                                    if (mG == 0)
                                    {
                                        RestorationStringList.Add("0");
                                    }
                                    if (mG == 1)
                                    {
                                        RestorationStringList.Add("1");
                                    }
                                }
                                if (i == 2)
                                {
                                    if (mB == 0)
                                    {
                                        RestorationStringList.Add("0");
                                    }
                                    if (mB == 1)
                                    {
                                        RestorationStringList.Add("1");
                                    }
                                }
                            }
                        }
                    }
                }
                byte[] resultbyte = new byte[1000000];
                string temp = string.Empty;
                for (int i = 0; i < RestorationStringList.Count(); i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (i * 8 + 8 < RestorationStringList.Count())
                        {
                            temp += RestorationStringList[i * 8 + j];
                        }
                    }
                    if (temp != "" && i < resultbyte.Count() - 1)
                    {
                        resultbyte[i] = byte.Parse(Convert.ToInt16(temp, 2).ToString());
                        temp = string.Empty;
                    }
                    if (i > resultbyte.Count())
                    {
                        break;
                    }
                }

                MessageBox.Show("Translate success!! Save the file.");

                string txtPath;
                SaveFileDialog slg = new SaveFileDialog();
                slg.DefaultExt = "txt.*";
                slg.Filter = "Text Files(.txt)|*.txt";
                slg.ShowDialog();
                txtPath = slg.FileName;

                System.IO.File.WriteAllBytes(txtPath, resultbyte);
            }
            catch { }
        }

        private void madeByToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("blog.naver.com/smhotkdg");
        }       
    }
}
