using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace CommitsBranchesReader
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


 

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                string path = Repository.Clone(
                    textBox1.Text,
                    tmp);

                using (var Git = new Repository(path))
                {
                    foreach (var Commit in Git.Commits)

                    {
                        int n = 0;

                        dataGridView1.Rows[n].Cells[1].Value = Commit.Author.Name;
                        dataGridView1.Rows[n].Cells[2].Value = Commit.Committer.When;
                        dataGridView1.Rows[n].Cells[3].Value = Branch.Name;
                        dataGridView1.Rows.Add();

                        n++;
                    }
                }
            }
            catch (Exception ex){ MessageBox.Show(ex.Message); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
