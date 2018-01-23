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
using System.Xml.Serialization;

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
            textBox1.Text = "http://github.com/mlawicki/CommitsBranchesReader";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            string tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
           
           try
            {

                var branches = Repository.ListRemoteReferences(textBox1.Text)
                         .Where(elem => elem.IsLocalBranch)
                         .Select(elem => elem.CanonicalName
                                             .Replace("refs/heads/", ""));


                foreach (string branch in branches)
                {
                    string localTmp = tmp + branch;

                    string path2 = Repository.Clone(
                        textBox1.Text,
                        localTmp, 
                        new CloneOptions { BranchName = branch });

                        using (var Git = new Repository(path2))
                        {

                            foreach (var Commit in Git.Commits)

                            {

                                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                                row.Cells[0].Value = Commit.Author.Name;
                                row.Cells[1].Value = Commit.Committer.When;
                                dataGridView1.Rows.Add(row);

                            }
                        }
                }


            }
            catch (Exception ex){ MessageBox.Show("Błąd: "+ex.Message); }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        DataTable dt = new DataTable();
        private DataTable DataTableZdgv(DataGridView dataGridView1)
        {
            dt.TableName = "commity";
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
               dt.Columns.Add(column.Name);
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
            return dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "XML-File | *.xml";
                if (sfd.ShowDialog() == DialogResult.OK)
                {

                    try
                        {
                        DataTableZdgv(dataGridView1);
                        dt.WriteXml(sfd.FileName, XmlWriteMode.WriteSchema);
                    }
                        catch (Exception ex)
                        { MessageBox.Show("Błąd zapisu pliku XML: " + ex.Message); }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd tworzenia pliku XML: " + ex.Message);
            }
        }
    }
    }
