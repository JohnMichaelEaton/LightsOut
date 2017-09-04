using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lights_Out
{
    public partial class MainForm : Form
    {
        private const int GRIDOFFSET = 25;
        private const int GRIDLENGTH = 200;
        private const int NUMCELLS = 3;
        private const int CELLLENGTH = GRIDLENGTH / NUMCELLS;
        private const string VICTORYTEXT = "Congratulations! You've won!";
        private const string TITLETEXT = "Lights Out!";
        private bool[,] grid;
        private Random rand;

        public MainForm()
        {
            InitializeComponent();

            rand = new Random();
            grid = new bool[NUMCELLS, NUMCELLS];
            for(int row = 0; row < NUMCELLS; row++)
            {
                for (int column = 0; column < NUMCELLS; column++)
                {
                    grid[row, column] = true;
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            for(int row = 0; row < NUMCELLS; row++)
            {
                for (int column = 0; column < NUMCELLS; column++)
                {
                    grid[row, column] = rand.Next(2) == 1;
                }

                this.Invalidate();
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnExit.PerformClick();
        }
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < GRIDOFFSET || e.X > CELLLENGTH * NUMCELLS + GRIDOFFSET
                || e.Y < GRIDOFFSET || e.Y > CELLLENGTH * NUMCELLS + GRIDOFFSET)
            {
                return;
            }

            int row = (e.Y - GRIDOFFSET) / CELLLENGTH;
            int column = (e.X - GRIDOFFSET) / CELLLENGTH;

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = column - 1; j <= column + 1; j++)
                {
                    if (i >= 0 && i < NUMCELLS && j >= 0 && j < NUMCELLS)
                    {
                        grid[i, j] = !grid[i, j];
                    }
                }
            }

            this.Invalidate();

            if (PlayerWon())
            {
                MessageBox.Show(this, VICTORYTEXT, TITLETEXT, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            for (int row = 0; row < NUMCELLS; row++)
            {
                for (int column = 0; column < NUMCELLS; column++)
                {
                    Brush brush;
                    Pen pen;

                    if(grid[row, column])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    int x = column * CELLLENGTH + GRIDOFFSET;
                    int y = row * CELLLENGTH + GRIDOFFSET;

                    graphics.DrawRectangle(pen, x, y, CELLLENGTH, CELLLENGTH);
                    graphics.FillRectangle(brush, x + 1, y + 1, CELLLENGTH - 1, CELLLENGTH - 1);
                }
            }
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnNewGame.PerformClick();
        }
        private bool PlayerWon()
        {
            foreach (bool state in grid)
            {
                if (state)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
