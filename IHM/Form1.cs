using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace IHM
{
    public partial class Form1 : Form
    {
        double moyenne = 0, mediane = 0, currentScore = 0;
        bool run = false;
        List<Image> images;
        List<Image> imagesTraitees;
        int position = 0;

        private Thread t1;

        public Form1()
        {
            InitializeComponent();
        }

        public List<Image> LoadBmpImages()
        {
            List<Image> images = new List<Image>();
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.bmp");
                if (files.Length == 0)
                {
                    MessageBox.Show("Aucune image BMP trouvée dans le dossier sélectionné.");
                    return images;
                }

                foreach (string file in files)
                {
                    Image image = Image.FromFile(file);
                    images.Add(image);
                }
            }
            return images;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            run = true;
            button3.Enabled = true;
            //Thread t1 = new Thread(traitement);
            //t1.Start();
            traitement();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            images = LoadBmpImages();
            if(images.Count == 0)
            {
                return;
            }
            button2.Enabled = true;
            pictureBoxPRE.Image = images[0];
            labelNumero.Text =  "1/" + images.Count;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // bouton qui permet d'arrêter le lancement
            run = false;
        }

        // ou alors bouton 4 sert plutôt à sauvegarder les scores ?
        private void button4_Click(object sender, EventArgs e)
        {
            // bouton pour sauvegarder l'image (laquelle ?)
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
            }

        }

        private void traitement()
        {
            PictureBox[] pictureBoxes = new PictureBox[]
            {
                pictureBoxPRE1, pictureBoxPRE2, pictureBoxPRE3, pictureBoxPRE4, pictureBoxPRE5, pictureBoxPRE6, pictureBoxPRE7, pictureBoxPRE8, pictureBoxPRE9, pictureBoxPRE10

            };

            while(run)
            {
                goToNext(pictureBoxes);
                Application.DoEvents();
                Thread.Sleep(100);
               // MessageBox.Show("OK");
                
            }
        }
        private void goToNext(PictureBox[] pictureBoxes)
        {
            for (int i = pictureBoxes.Length - 1; i > 0; i--)
            {
                if (pictureBoxes[i - 1].Image != null)
                {
                    pictureBoxes[i].Image = pictureBoxes[i - 1].Image;
                }

            }
            pictureBoxes[0].Image = pictureBoxPRE.Image;

            position++;
            if (position == images.Count)
            {
                run = false;
                return;
            }
            labelNumero.Text = (position + 1) + "/" + images.Count;

            pictureBoxPRE.Image = images[position];
        }

    }
}
