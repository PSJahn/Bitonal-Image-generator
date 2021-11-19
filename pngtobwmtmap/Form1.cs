using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pngtobwmtmap
{
    public partial class Form1 : Form
    {
        String imagePath = "empty";
        OpenFileDialog fd = new OpenFileDialog();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Resource1.preview_empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fd.Multiselect = false;
            if(fd.ShowDialog().Equals(DialogResult.OK))
            {
                imagePath = fd.FileName;
                updatePath();
            }
        }

        List<Color> pixels = new List<Color>();

        private void updatePath()
        {
            if (imagePath.Equals("empty"))
            {
                return;
            }
            Bitmap img = new Bitmap(imagePath);
            pixels.Clear();

            for (int j = 0; j < img.Height; j++)
            {
                for (int i = 0; i < img.Width; i++)
                {
                    Color pixel = img.GetPixel(i, j);

                    if(checkBox1.Checked)
                    {
                        if (pixel.R > trackBar1.Value)
                        {
                            pixel = Color.White;
                        }
                        else
                        {
                            pixel = Color.Black;
                        }
                    } else
                    {
                        if (pixel.R > trackBar1.Value)
                        {
                            pixel = Color.Black;
                        }
                        else
                        {
                            pixel = Color.White;
                        }
                    }
                    pixels.Add(pixel);
                }
            }
        
            int x = 0;
            int y = 0;
            //img.LockBits();
            foreach(Color pixel in pixels)
            {
                y = x / img.Width;
                y = (int)y;
                img.SetPixel(x-(img.Width*y), y, pixel);
                x++;
            }
            //img.UnlockBits();   
            pictureBox1.Image = img;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            updatePath();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            bmp.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\img.png", ImageFormat.Png);
            MessageBox.Show("Saved to desktop!");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            updatePath();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = "P1\n" + pictureBox1.Image.Width + " " + pictureBox1.Image.Height + "\n";
            int x = 0;
            int y = 0;
            int yprev = 0;
            int progress = 0;
            progressBar1.Maximum = pixels.Count;
            foreach (Color pixel in pixels)
            {
                
                y = x / pictureBox1.Image.Width;
                y = (int)y;
                if(y!=yprev)
                {
                    text = text.Substring(0,text.Length-1);
                    text += "\n";
                    yprev = y;
                }
                int asd = Color.White == pixel ? asd = 0 : asd = 1;
                text += asd + " ";
                //img.SetPixel(x - (img.Width * y), y, pixel);
                x++;
                progress++;
                progressBar1.Value = progress;
                Application.DoEvents();
            }
            richTextBox1.Text = text;
        }
    }
}
