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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace IHM
{
    public partial class Form1 : Form
    {
        double moyenne = 0, mediane = 0, currentScore = 0;
        bool run = false;
        List<Image> images;
        List<Image> imagesTraitees;
        int position = 0;

        public enum State
        {
            INIT,
            READY,
            RUN,
            RUN_STOP
        }

        private State currentState = State.INIT;

        private Thread t1;

        public Form1()
        {
            InitializeComponent();
            processState(State.INIT);
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
            //Thread t1 = new Thread(traitement);
            //t1.Start();
            processState(State.RUN);

            traitement();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            images = LoadBmpImages();
            loadFirst();
            processState(State.READY);
        }

        private void loadFirst() { 
            if (images.Count == 0)
            {
                return;
            }
            position = 0;
            pictureBoxPRE.Image = images[position];
            labelNumero.Text = (position + 1) + "/" + images.Count;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // bouton qui permet d'arrêter le lancement
            processState(State.RUN_STOP);
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

            while(currentState == State.RUN)
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
                processState(State.READY);
                loadFirst();
                return;
            }
            labelNumero.Text = (position + 1) + "/" + images.Count;

            pictureBoxPRE.Image = images[position];
        }


        private void processState(State newState)
        {
            currentState = newState;
            switch (newState)
            {
                case State.INIT:
                    statePanel.BackColor = Color.LightGray;
                    stateLabel.Text = "Initialisé";
                    button1.Enabled = true;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    break;
                case State.READY:
                    statePanel.BackColor = Color.LightGreen;
                    stateLabel.Text = "Prêt";
                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    break;
                case State.RUN:
                    statePanel.BackColor = Color.Green;
                    stateLabel.Text = "Lancé";
                    button1.Enabled = false;
                    button2.Enabled = false;
                    button3.Enabled = true;
                    button4.Enabled = false;
                    break;
                case State.RUN_STOP:
                    statePanel.BackColor = Color.Orange;
                    stateLabel.Text = "Pause";
                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    button4.Enabled = true;
                    break;
            }
        }

    }
}
