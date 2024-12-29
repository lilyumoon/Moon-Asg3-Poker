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
                pictureBoxList[i].Image = imageListCards.Images[52];
            }
        }

        private void buttonBet_Click(object sender, EventArgs e)
        {

            // Get the 'bet' number from the UpDown control
            int bet = (int)numericUpDownBet.Value;
            // Tell game object to do the game logic side of 'Bet'
            game.startRound(bet);

            // Update credits and controls
            labelTotalCreditsCounter.Text = game.Credits.ToString();
            labelAmountWonCounter.Text = string.Empty;
            labelHandResult.Text = string.Empty;
            numericUpDownBet.Enabled = false;
            buttonBet.Enabled = false;
            buttonDraw.Enabled = true;
            validateBetValue();
            foreach (DataGridViewRow row in dataGridViewScoringInfo.Rows)
                row.DefaultCellStyle.BackColor = Color.White;

            // Assign drawn card images and enable ability to mark as 'held'
            for (int i = 0; i < 5; i++)
            {
                int cardImageIndex = game.cardImageIndices[i];
                pictureBoxList[i].Image = imageListCards.Images[cardImageIndex];
                pictureBoxList[i].Enabled = true;
            }

            // reset any 'held' labels
            foreach (Label label in labelHeldList)
                label.Visible = false;

        }

        /// <summary>
        /// Handler for the Draw button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDraw_Click(object sender, EventArgs e)
        {
            // Tell game object to do the game logic side of 'Draw'
            game.completeRound((int)numericUpDownBet.Value, out string handResult, out int winnings);
            
            // Update hand result and amount won labels
            labelHandResult.Text = handResult;
            labelAmountWonCounter.Text = winnings.ToString();

            // Update card images and disable ability to mark as 'held'
            for (int i = 0; i < 5; i++)
            {
                pictureBoxList[i].Image = imageListCards.Images[game.cardImageIndices[i]];
                pictureBoxList[i].Enabled = false;
            }

            // If the hand won, highlight how it won in the scoring info table
            if (handResult != "You lost your bet")
            {
                int tableIndex = 0;
                tableIndex = game.possibleHandResults.IndexOf(handResult);
                // Funnel all face card pair results into "jacks or better"
                if (tableIndex >= 8)
                    tableIndex = 8;
                dataGridViewScoringInfo.Rows[tableIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            }

            // Update credits and modify control interactability
            labelTotalCreditsCounter.Text = game.Credits.ToString();
            numericUpDownBet.Enabled = true;
            buttonDraw.Enabled = false;

            // Disable Bet button if there are no credits available to bet
            if (game.Credits > 0)
                buttonBet.Enabled = true;
        }

        // Handlers for the Card pictureboxes' 'Click' events
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
        /// Watches for bet exceeding available credits and modifies bet accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDownBet_ValueChanged(object sender, EventArgs e)
        {
            validateBetValue();
        }

        /// <summary>
        /// Handles edge cases of low credits and updates the bet UpDown control
        /// so bet cannot exceed the number of available credits.
        /// </summary>
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
