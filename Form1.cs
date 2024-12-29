using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moon_Asg3_Poker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            populateScoringTable();
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

        }
    }
}
