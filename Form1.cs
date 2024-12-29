using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moon_Asg3_Poker
{
    public partial class Form1 : Form
    {
        private Game game = new Game();

        List<PictureBox> pictureBoxList;
        List<Label> labelHeldList;

        public Form1()
        {
            InitializeComponent();
            setup();
        }

        private void setup()
        {
            populateScoringTable();
            pictureBoxList = new List<PictureBox>
            {
                pictureBoxCard1,
                pictureBoxCard2,
                pictureBoxCard3,
                pictureBoxCard4,
                pictureBoxCard5
            };
            labelHeldList = new List<Label>
            {
                labelCard1,
                labelCard2,
                labelCard3,
                labelCard4,
                labelCard5
            };
        }

        private void populateScoringTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("name", typeof(string));
            table.Columns.Add("bet1", typeof(string));
            table.Columns.Add("bet2", typeof(string));
            table.Columns.Add("bet3", typeof(string));
            table.Columns.Add("bet4", typeof(string));
            table.Columns.Add("bet5", typeof(string));

            table.Rows.Add("ROYAL FLUSH...............................................................", 250, 500, 750, 1000, 4000);
            table.Rows.Add("STRAIGHT FLUSH.........................................................", 50, 100, 150, 200, 250);
            table.Rows.Add("FOUR OF A KIND...........................................................", 25, 50, 75, 100, 125);
            table.Rows.Add("FULL HOUSE.................................................................", 9, 18, 27, 36, 45);
            table.Rows.Add("FLUSH...........................................................................", 6, 12, 18, 24, 30);
            table.Rows.Add("STRAIGHT.....................................................................", 4, 8, 12, 16, 20);
            table.Rows.Add("THREE OF A KIND........................................................", 3, 6, 9, 12, 15);
            table.Rows.Add("TWO PAIR.....................................................................", 2, 4, 6, 8, 10);
            table.Rows.Add("JACKS OR BETTER.......................................................", 1, 2, 3, 4, 5);

            dataGridViewScoringInfo.DataSource = table;

            foreach (DataGridViewColumn column in dataGridViewScoringInfo.Columns)
                column.MinimumWidth = 69;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            labelTotalCreditsCounter.Text = game.Credits.ToString();
            buttonDraw.Enabled = false;
            for (int i = 0; i < 5; i++)
            {
                labelHeldList[i].Visible = false;
                pictureBoxList[i].Enabled = false;
            }
        }

        private void buttonBet_Click(object sender, EventArgs e)
        {
            int bet = (int)numericUpDownBet.Value;
            game.startRound(bet);

            numericUpDownBet.Enabled = false;
            buttonBet.Enabled = false;
            buttonDraw.Enabled = true;
            labelTotalCreditsCounter.Text = game.Credits.ToString();
            validateBetValue();

            // Enable each picture box interactability and assign drawn card images
            for (int i = 0; i < 5; i++)
            {
                pictureBoxList[i].Enabled = true;
                pictureBoxList[i].Image = imageListCards.Images[game.cardImageIndices[i]];
            }

            // reset any 'held' labels
            foreach (Label label in labelHeldList)
                label.Visible = false;

        }

        private void buttonDraw_Click(object sender, EventArgs e)
        {
            game.completeRound((int)numericUpDownBet.Value, out string handResult, out int winnings);
            labelDescription.Text = handResult;
            labelAmountWonCounter.Text = winnings.ToString();

            // Update card images and disable picture box interactability
            for (int i = 0; i < 5; i++)
            {
                pictureBoxList[i].Image = imageListCards.Images[game.cardImageIndices[i]];
                pictureBoxList[i].Enabled = false;
            }

            numericUpDownBet.Enabled = true;
            buttonDraw.Enabled = false;
            labelTotalCreditsCounter.Text = game.Credits.ToString();
            if (game.Credits > 0)
                buttonBet.Enabled = true;
        }

        private void pictureBoxCard1_Click(object sender, EventArgs e)
        {
            toggleHeld(0);
        }

        private void pictureBoxCard2_Click(object sender, EventArgs e)
        {
            toggleHeld(1);
        }

        private void pictureBoxCard3_Click(object sender, EventArgs e)
        {
            toggleHeld(2);
        }

        private void pictureBoxCard4_Click(object sender, EventArgs e)
        {
            toggleHeld(3);
        }

        private void pictureBoxCard5_Click(object sender, EventArgs e)
        {
            toggleHeld(4);
        }

        private void toggleHeld(int index)
        {
            game.toggleCardHeldState(index);
            // invert the 'held' label visibility state
            labelHeldList[index].Visible ^= true;
        }

        /// <summary>
        /// Event handler for the UpDown bet control's ValueChanged event. 
        /// Watches for bet exceeding credits and modifies bet accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDownBet_ValueChanged(object sender, EventArgs e)
        {
            validateBetValue();
        }

        private void validateBetValue()
        {
            if (numericUpDownBet.Value > game.Credits)
            {
                int defaultBet = 1;
                if (game.Credits >= defaultBet)
                    defaultBet = game.Credits;
                numericUpDownBet.Value = defaultBet;
            }
        }
    }
}
