using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        Bitmap image;
        Pixel[,] mImage;
        double[,] ff;

        public Form1()
        {
            InitializeComponent();
            image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            mImage = new Pixel[image.Width, image.Height];
            comboBox1.Items.Add("Линейная фильтрация");
            comboBox1.Items.Add("Медианнная фильтрация");
            comboBox1.Items.Add("Размытие Гаусса");
            comboBox1.SelectedIndex = 0;
            textBox1.Visible = false;
            textBox2.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            richTextBox1.Text = "0,11 0,11 0,11\r0,11 0,11 0,11\r0,11 0,11 0,11";

            comboBox1.SelectedIndexChanged += ChangeInterface;

        }

        public Bitmap LinearFiltering(Pixel[,] pImage, double[,] f)
        {
            Bitmap nImage = new Bitmap(image);
            Pixel[,] pImage2 = new Pixel[image.Width, image.Height];
            for (int i = 0; i < nImage.Width; i++)
            {
                for (int j = 0; j < nImage.Height; j++)
                {
                    pImage2[i, j] = new Pixel();
                }
            }

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    int ni, nj;
                    int ki, kj;
                    ni = i - (int)Math.Truncate(f.GetLength(0) / 2.0);
                    nj = j - (int)Math.Truncate(f.GetLength(1) / 2.0);
                    ki = i + (int)Math.Truncate(f.GetLength(0) / 2.0);
                    kj = j + (int)Math.Truncate(f.GetLength(1) / 2.0);

                    double sumR = 0, sumG = 0, sumB = 0;

                    for (int i2 = ni, fi = 0; i2 < ki + 1; i2++, fi++)
                    {
                        for (int j2 = nj, fj = 0; j2 < kj + 1; j2++, fj++)
                        {
                            if (i2 >= 0 && i2 < nImage.Width)
                            {
                                if (j2 >= 0 && j2 < nImage.Height)
                                {
                                    //Находимся в границах
                                    sumR += f[fi, fj] * pImage[i2, j2].R;
                                    sumG += f[fi, fj] * pImage[i2, j2].G;
                                    sumB += f[fi, fj] * pImage[i2, j2].B;
                                }
                                else
                                {
                                    if (j2 < 0)
                                    {
                                        //Ушли за границу по j влево
                                        sumR += f[fi, fj] * pImage[i2, (-1) * j2].R;
                                        sumG += f[fi, fj] * pImage[i2, (-1) * j2].G;
                                        sumB += f[fi, fj] * pImage[i2, (-1) * j2].B;
                                    }
                                    if (j2 > nImage.Height - 1)
                                    {
                                        //Ушли за границу по j вправо
                                        sumR += f[fi, fj] * pImage[i2, (j - (j2 - j))].R;
                                        sumG += f[fi, fj] * pImage[i2, (j - (j2 - j))].G;
                                        sumB += f[fi, fj] * pImage[i2, (j - (j2 - j))].B;

                                    }
                                }
                            }
                            else
                            {
                                if (i2 < 0)
                                {
                                    //Ушли за границу по i вверх
                                    if (j2 < 0)
                                    {
                                        //Ушли за границу по j влево
                                        sumR += f[fi, fj] * pImage[(-1) * i2, (-1) * j2].R;
                                        sumG += f[fi, fj] * pImage[(-1) * i2, (-1) * j2].G;
                                        sumB += f[fi, fj] * pImage[(-1) * i2, (-1) * j2].B;
                                    }
                                    else
                                    {
                                        if (j2 > nImage.Height - 1)
                                        {
                                            sumR += f[fi, fj] * pImage[(-1) * i2, (j - (j2 - j))].R;
                                            sumG += f[fi, fj] * pImage[(-1) * i2, (j - (j2 - j))].G;
                                            sumB += f[fi, fj] * pImage[(-1) * i2, (j - (j2 - j))].B;
                                        }
                                        else
                                        {
                                            sumR += f[fi, fj] * pImage[(-1) * i2, j2].R;
                                            sumG += f[fi, fj] * pImage[(-1) * i2, j2].G;
                                            sumB += f[fi, fj] * pImage[(-1) * i2, j2].B;
                                        }
                                    }
                                }
                                if (i2 > nImage.Width - 1)
                                {
                                    //Ушли за границу по i вниз
                                    if (j2 > nImage.Height - 1)
                                    {
                                        //Ушли за границу по j вправо
                                        sumR += f[fi, fj] * pImage[(i - (i2 - i)), (j - (j2 - j))].R;
                                        sumG += f[fi, fj] * pImage[(i - (i2 - i)), (j - (j2 - j))].G;
                                        sumB += f[fi, fj] * pImage[(i - (i2 - i)), (j - (j2 - j))].B;

                                    }
                                    else
                                    {
                                        if (j2 < 0)
                                        {
                                            sumR += f[fi, fj] * pImage[(i - (i2 - i)), (-1) * j2].R;
                                            sumG += f[fi, fj] * pImage[(i - (i2 - i)), (-1) * j2].G;
                                            sumB += f[fi, fj] * pImage[(i - (i2 - i)), (-1) * j2].B;
                                        }
                                        else
                                        {
                                            sumR += f[fi, fj] * pImage[(i - (i2 - i)), j2].R;
                                            sumG += f[fi, fj] * pImage[(i - (i2 - i)), j2].G;
                                            sumB += f[fi, fj] * pImage[(i - (i2 - i)), j2].B;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (sumR > 255)
                    {
                        sumR = 255;
                    }
                    if (sumG > 255)
                    {
                        sumG = 255;
                    }
                    if (sumB > 255)
                    {
                        sumB = 255;
                    }

                    if (sumR < 0)
                    {
                        sumR = 0;
                    }
                    if (sumG < 0)
                    {
                        sumG = 0;
                    }
                    if (sumB < 0)
                    {
                        sumB = 0;
                    }


                    pImage2[i, j].R = (int)sumR;
                    pImage2[i, j].G = (int)sumG;
                    pImage2[i, j].B = (int)sumB;
                }
            }

            for (int i = 0; i < nImage.Width; i++)
            {
                for (int j = 0; j < nImage.Height; j++)
                {
                    Color c = Color.FromArgb(pImage2[i, j].R, pImage2[i, j].G, pImage2[i, j].B);
                    nImage.SetPixel(i, j, c);
                }
            }

            return nImage;
        }

        public Bitmap MedianFiltering(Pixel[,] pImage, double[,] f)
        {
            Bitmap nImage = new Bitmap(image);
            Pixel[,] pImage2 = new Pixel[image.Width, image.Height];
            for (int i = 0; i < nImage.Width; i++)
            {
                for (int j = 0; j < nImage.Height; j++)
                {
                    pImage2[i, j] = new Pixel();
                }
            }

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    int ni, nj;
                    int ki, kj;
                    ni = i - (int)Math.Truncate(f.GetLength(0) / 2.0);
                    nj = j - (int)Math.Truncate(f.GetLength(1) / 2.0);
                    ki = i + (int)Math.Truncate(f.GetLength(0) / 2.0);
                    kj = j + (int)Math.Truncate(f.GetLength(1) / 2.0);

                    List<int> nR = new List<int>();
                    List<int> nG = new List<int>();
                    List<int> nB = new List<int>();


                    for (int i2 = ni, fi = 0; i2 < ki + 1; i2++, fi++)
                    {
                        for (int j2 = nj, fj = 0; j2 < kj + 1; j2++, fj++)
                        {
                            if (i2 >= 0 && i2 < nImage.Width)
                            {
                                if (j2 >= 0 && j2 < nImage.Height)
                                {
                                    //Находимся в границах
                                    nR.Add(pImage[i2, j2].R);
                                    nG.Add(pImage[i2, j2].G);
                                    nB.Add(pImage[i2, j2].B);
                                }
                                else
                                {
                                    if (j2 < 0)
                                    {
                                        //Ушли за границу по j влево
                                        nR.Add(pImage[i2, (-1) * j2].R);
                                        nG.Add(pImage[i2, (-1) * j2].G);
                                        nB.Add(pImage[i2, (-1) * j2].B);
                                    }
                                    if (j2 > nImage.Height - 1)
                                    {
                                        nR.Add(pImage[i2, (j - (j2 - j))].R);
                                        nG.Add(pImage[i2, (j - (j2 - j))].G);
                                        nB.Add(pImage[i2, (j - (j2 - j))].B);

                                    }
                                }
                            }
                            else
                            {
                                if (i2 < 0)
                                {
                                    //Ушли за границу по i вверх
                                    if (j2 < 0)
                                    {
                                        //Ушли за границу по j влево
                                        nR.Add(pImage[(-1) * i2, (-1) * j2].R);
                                        nG.Add(pImage[(-1) * i2, (-1) * j2].G);
                                        nB.Add(pImage[(-1) * i2, (-1) * j2].B);
                                    }
                                    else
                                    {
                                        if (j2 > nImage.Height - 1)
                                        {
                                            nR.Add(pImage[(-1) * i2, (j - (j2 - j))].R);
                                            nG.Add(pImage[(-1) * i2, (j - (j2 - j))].G);
                                            nB.Add(pImage[(-1) * i2, (j - (j2 - j))].B);
                                        }
                                        else
                                        {
                                            nR.Add(pImage[(-1) * i2, j2].R);
                                            nG.Add(pImage[(-1) * i2, j2].G);
                                            nB.Add(pImage[(-1) * i2, j2].B);
                                        }
                                    }
                                }
                                if (i2 > nImage.Width - 1)
                                {
                                    //Ушли за границу по i вниз
                                    if (j2 > nImage.Height - 1)
                                    {
                                        //Ушли за границу по j вправо
                                        nR.Add(pImage[(i - (i2 - i)), (j - (j2 - j))].R);
                                        nG.Add(pImage[(i - (i2 - i)), (j - (j2 - j))].G);
                                        nB.Add(pImage[(i - (i2 - i)), (j - (j2 - j))].B);

                                    }
                                    else
                                    {
                                        if (j2 < 0)
                                        {
                                            nR.Add(pImage[(i - (i2 - i)), (-1) * j2].R);
                                            nG.Add(pImage[(i - (i2 - i)), (-1) * j2].G);
                                            nB.Add(pImage[(i - (i2 - i)), (-1) * j2].B);
                                        }
                                        else
                                        {
                                            nR.Add(pImage[(i - (i2 - i)), j2].R);
                                            nG.Add(pImage[(i - (i2 - i)), j2].G);
                                            nB.Add(pImage[(i - (i2 - i)), j2].B);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    pImage2[i, j].R = Search(nR, nR.Count / 2);
                    pImage2[i, j].G = Search(nG, nR.Count / 2);
                    pImage2[i, j].B = Search(nB, nR.Count / 2);

                }
            }

            for (int i = 0; i < nImage.Width; i++)
            {
                for (int j = 0; j < nImage.Height; j++)
                {
                    Color c = Color.FromArgb(pImage2[i, j].R, pImage2[i, j].G, pImage2[i, j].B);
                    nImage.SetPixel(i, j, c);
                }
            }

            return nImage;
        }

        public void Gauss(int r, double sig)
        {

            double s = 0;
            double g;

            double[,] sss = new double[2 * r + 1, 2 * r + 1];

            double sig_sqr = 2.0 * sig * sig;
            double pi_siq_sqr = sig_sqr * Math.PI;

            for (int i = -r; i <= r; ++i)
            {
                for (int j = -r; j <= r; ++j)
                {
                    g = 1.0 / pi_siq_sqr * Math.Exp(-1.0 * (i * i + j * j) / (sig_sqr));
                    s += g;
                    sss[i + r, j + r] = g;

                }

            }

            ff = sss;

        }

        public static int Search(List<int> Arr, int FIndingIndex)
        {
            List<int> More = new List<int>();
            List<int> Less = new List<int>();
            List<int> Equal = new List<int>();
            Random rnd = new Random();
            int index = rnd.Next(0, Arr.Count - 1);

            for (int i = 0; i < Arr.Count; i++)
                if (Arr[i] < Arr[index])
                    Less.Add(Arr[i]);
                else
                if (Arr[i] == Arr[index])
                    Equal.Add(Arr[i]);
                else
                    More.Add(Arr[i]);

            if (FIndingIndex < Less.Count)
                return Search(Less, FIndingIndex);
            else
                if (FIndingIndex < Less.Count + Equal.Count)
                return Equal[0];
            else
                return Search(More, FIndingIndex - Less.Count - Equal.Count);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                Bitmap imageT = new Bitmap(openFileDialog.FileName);
                image = new Bitmap(imageT, pictureBox1.Width, pictureBox1.Height);
                imageT.Dispose();
                pictureBox1.Image = image;
            }

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    mImage[i, j] = new Pixel();
                }
            }

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color c = image.GetPixel(i, j);
                    mImage[i, j].R = c.R;
                    mImage[i, j].G = c.G;
                    mImage[i, j].B = c.B;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    RichBoxReading();
                    pictureBox2.Image = LinearFiltering(mImage, ff);
                    break;
                case 1:
                    double[,] fm = new double[Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text)];
                    pictureBox2.Image = MedianFiltering(mImage, fm);
                    break;
                case 2:
                    Gauss(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
                    pictureBox2.Image = LinearFiltering(mImage, ff);
                    break;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDileFialog = new SaveFileDialog();
            saveDileFialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveDileFialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            saveDileFialog.RestoreDirectory = true;
            image = (Bitmap)pictureBox2.Image;

            if (saveDileFialog.ShowDialog() == DialogResult.OK)
            {
                if (image != null)
                {
                    image.Save(saveDileFialog.FileName);
                }
            }
        }

        public void ChangeInterface(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    richTextBox1.Visible = true;
                    textBox1.Visible = false;
                    textBox2.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;

                    richTextBox1.Text = "0,11 0,11 0,11\r0,11 0,11 0,11\r0,11 0,11 0,11";
                    break;
                case 1:
                    richTextBox1.Visible = false;
                    textBox1.Visible = true;
                    textBox2.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;

                    textBox1.Text = "3";
                    textBox2.Text = "3";
                    label1.Text = "Высота матрицы";
                    label2.Text = "Ширина матрицы";
                    break;
                case 2:
                    richTextBox1.Visible = false;
                    textBox1.Visible = true;
                    textBox2.Visible = true;
                    label1.Visible = true;
                    label2.Visible = true;

                    textBox1.Text = "6";
                    textBox2.Text = "3";
                    label1.Text = "Радиус воздействия";
                    label2.Text = "Коэффициент сигма";
                    break;
            }
        }

        public void RichBoxReading()
        {
            double[,] md;
            List<double> ld = new List<double>();
            List<string> ls = new List<string>();
            string temp = "";
            int str = 0, stl = 0;
            for ( int i = 0; i < richTextBox1.Text.Length; i++) 
            {
                char tempS = richTextBox1.Text[i];
                if ((tempS > 47 && tempS < 58) || (tempS > 43 && tempS < 46))  
                {
                    temp += richTextBox1.Text[i];
                }
                else
                {
                    ls.Add(temp);
                    temp = "";
                    stl++;
                    if (tempS == 10)
                    {
                        str++;
                        stl = 0;
                    }
                }
            }

            str++;
            stl++;

            ls.Add(temp);

            md = new double[str, stl];

            for(int i = 0; i < str; i++)
            {
                for(int j = 0; j < stl; j++)
                {
                    md[i, j] = Convert.ToDouble(ls[i * stl + j]);
                }
            }

            ff = md;

        }
    }

    public class Pixel
    {
        public int R;
        public int G;
        public int B;

        public double SR
        {
            get
            {
                return ((R + G + B) / 3);
            }
        }
    }
}
