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

        // Task colorTransition;

        public Form1()
        {
            InitializeComponent();
            setup();
        }

        /// <summary>
        /// Performs initial data population operations.
        /// </summary>
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

        /// <summary>
        /// Populates the payout reference data contained in the scoring table.
        /// </summary>
        private void populateScoringTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("name", typeof(string));
            table.Columns.Add("bet1", typeof(string));
            table.Columns.Add("bet2", typeof(string));
            table.Columns.Add("bet3", typeof(string));
            table.Columns.Add("bet4", typeof(string));
            table.Columns.Add("bet5", typeof(string));

            table.Rows.Add("ROYAL FLUSH...............................................................", 1000, 2000, 3000, 4000, 5000);
            table.Rows.Add("STRAIGHT FLUSH.........................................................", 500, 1000, 1500, 2000, 2500);
            table.Rows.Add("FOUR OF A KIND...........................................................", 50, 100, 150, 200, 250);
            table.Rows.Add("FULL HOUSE.................................................................", 15, 30, 45, 60, 75);
            table.Rows.Add("FLUSH...........................................................................", 6, 12, 18, 24, 30);
            table.Rows.Add("STRAIGHT.....................................................................", 5, 10, 15, 20, 25);
            table.Rows.Add("THREE OF A KIND........................................................", 3, 6, 9, 12, 15);
            table.Rows.Add("TWO PAIR.....................................................................", 2, 4, 6, 8, 10);
            table.Rows.Add("JACKS OR BETTER.......................................................", 1, 2, 3, 4, 5);

            dgvScoringInfo.DataSource = table;

            // Added this formatting logic here because it was difficult to find in the Design editor...
            foreach (DataGridViewColumn column in dgvScoringInfo.Columns)
                column.MinimumWidth = 69;
        }

        /*
         * Did not end up using the next 2 methods.
         */

        /// <summary>
        /// Transitions a specified row's color over a specified duration, then changes it back to its initial color.
        /// </summary>
        /// <param name="rowIndex">The index of the data grid view's row to transition the color of.</param>
        /// <param name="duration">The time in ms over which the transition will occur.</param>
        /// <returns>A task representing the async operation of transitioning a specified row's color.
        /// The task completes when the color transition finishes.</returns>
        private async Task transitionRowColor(int rowIndex, int duration)
        {
            Color startColor = dgvScoringInfo.Rows[rowIndex].DefaultCellStyle.BackColor;
            Color endColor = Color.Goldenrod;

            int steps = 10; // number of steps between startColor and endColor
            int delay = duration / steps; // time to wait between steps

            for (int i = 0; i <= steps; i++)
            {
                float interpolationFactor = (float)i / steps; // this will be between 0 and 1 depending on step progress
                Color interpolatedColor = getInterpolatedColor(startColor, endColor, interpolationFactor);
                dgvScoringInfo.Rows[rowIndex].DefaultCellStyle.BackColor = interpolatedColor;

                // Wait for 'delay' number of ms before finishing loop iteration
                await Task.Delay(delay);
                dgvScoringInfo.Rows[rowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        /// <summary>
        /// Calculates a new color based on a linear interpolation between a start and end color.
        /// </summary>
        /// <param name="startColor">The color to start from.</param>
        /// <param name="endColor">The color to end at.</param>
        /// <param name="interpolationFactor">A value between 0 and 1 determining the interpolation degree.</param>
        /// <returns></returns>
        private Color getInterpolatedColor(Color startColor, Color endColor, float interpolationFactor)
        {
            Color interpolatedColor;
            
            // Calculate each interpolated color component based on start/end colors and interpolation factor
            int r = (int)(startColor.R + (endColor.R - startColor.R) * interpolationFactor);
            int g = (int)(startColor.G + (endColor.G - startColor.G) * interpolationFactor);
            int b = (int)(startColor.B + (endColor.B - startColor.B) * interpolationFactor);

            // Combine each interpolated color component to create the interpolated color
            interpolatedColor = Color.FromArgb(r, g, b);

            return interpolatedColor;
        }

        /// <summary>
        /// Event handler for the Form's load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Do various UI initializations
            labelTotalCreditsCounter.Text = game.Credits.ToString();
            buttonDraw.Enabled = false;
            for (int i = 0; i < 5; i++)
            {
                labelHeldList[i].Visible = false;
                pictureBoxList[i].Enabled = false;
                pictureBoxList[i].Image = imageListCards.Images[52];
            }
        }

        /// <summary>
        /// Event handler for Bet button's click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBet_Click(object sender, EventArgs e)
        {
            // Get the 'bet' number from the UpDown control and tell 'game' to do game logic 
            int bet = (int)numericUpDownBet.Value;
            game.startRound(bet);

            // Update UI to reflect the state of a hand currently being in progress
            labelTotalCreditsCounter.Text = game.Credits.ToString();
            labelAmountWonCounter.Text = string.Empty;
            labelHandResult.Text = string.Empty;
            numericUpDownBet.Enabled = false;
            buttonBet.Enabled = false;
            buttonDraw.Enabled = true;
            validateBetValue();
            foreach (DataGridViewRow row in dgvScoringInfo.Rows)
                row.DefaultCellStyle.BackColor = Color.White;

            // Update card images
            updateCardImages();

            // Enable ability to mark as 'held' and reset 'held' labels
            for (int i = 0; i < 5; i++)
            {
                pictureBoxList[i].Enabled = true;
                labelHeldList[i].Visible = false;
            }
        }

        private void updateCardImages()
        {
            for (int i = 0; i < 5; i++)
            {
                int cardImageIndex = game.CardImageIndices[i];
                pictureBoxList[i].Image = imageListCards.Images[cardImageIndex];
            }
        }

        /// <summary>
        /// Event for the Draw button's click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDraw_Click(object sender, EventArgs e)
        {
            // Tell 'game' object to do game logic
            game.completeRound((int)numericUpDownBet.Value, out string handResult, out int winnings);

            // Update card images
            updateCardImages();
            
            // Disable ability to mark cards as 'held'
            for (int i = 0; i < 5; i++)
            {
                pictureBoxList[i].Enabled = false;
            }

            // Update credits and modify control interactability
            labelTotalCreditsCounter.Text = game.Credits.ToString();
            labelHandResult.Text = handResult;
            labelAmountWonCounter.Text = winnings.ToString();
            numericUpDownBet.Enabled = true;
            buttonDraw.Enabled = false;

            // If the hand did not lose, highlight how it won in the scoring info table
            if (handResult != "You lost your bet")
            {
                int tableIndex = 0;
                tableIndex = game.PossibleHandResults.IndexOf(handResult);
                // Simplify all "face card pair" results into "jacks or better"
                if (tableIndex >= 8)
                    tableIndex = 8;
                //transitionRowColor(tableIndex, 200);
                dgvScoringInfo.Rows[tableIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            }

            // Disable Bet button if there are no credits available to bet
            if (game.Credits > 0)
                buttonBet.Enabled = true;
        }

        /* 
         * Handlers for the Card pictureboxes' 'Click' events.
         */
        private void pictureBoxCard1_Click(object sender, EventArgs e) { toggleHeldState(0); }

        private void pictureBoxCard2_Click(object sender, EventArgs e) { toggleHeldState(1); }

        private void pictureBoxCard3_Click(object sender, EventArgs e) { toggleHeldState(2); }

        private void pictureBoxCard4_Click(object sender, EventArgs e) { toggleHeldState(3); }

        private void pictureBoxCard5_Click(object sender, EventArgs e) { toggleHeldState(4); }

        /// <summary>
        /// Inverts the 'held' state of a particular card and updates the UI to reflect it.
        /// </summary>
        /// <param name="index">The index of the card to toggle the held state of.</param>
        private void toggleHeldState(int index)
        {
            game.toggleCardHeldState(index);
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
