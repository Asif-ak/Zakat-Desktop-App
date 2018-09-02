using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Zakat.App_Code;


namespace Zakat
{
    public partial class Form1 : Form
    {
        IRepository<ZakatYear> zakatyear = new Implementation<ZakatYear>();
        IRepository<ZakatItem> ZakatItemView = new Implementation<ZakatItem>();
        List<ZakatYear> allyears;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //allyears = zakatyear.GetAll().Where(year => year.AID == new Guid(lblGUID.Text)).ToList();
        }


        private void addNewYearToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void AddPreviousYearToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (allyears.Any(a => a.ZYear == DateTime.Now.Year.ToString()))
                MessageBox.Show("Year Already Exist");
            else
            {
                zakatyear.Insert(new ZakatYear { ZYear = DateTime.Now.Year.ToString(), Date = DateTime.Now.Date, AID = new Guid(lblGUID.Text) });
                MessageBox.Show("New Year Added");
                updatecombo();
            }
        }

        private void addPreviousYearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            addPreviousYear addPreviousYear = new addPreviousYear(this);
            addPreviousYear.Show();
        }

        private void addNewZakatItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            updatecombo();
            //addPreviousYear a = new addPreviousYear();

        }
        public void UpdateComboOnInvoke()
        {
            //updatecombo()
            comboBox1.Invoke(new Action(() => updatecombo()));
        }
        void updatecombo()
        {
            comboBox1.Items.Clear();
            allyears = zakatyear.GetAll();
            foreach (var item in allyears.Where(a => a.AID == new Guid(lblGUID.Text)))
            {
                comboBox1.Items.Add(item.ZYear);

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel3.Visible = true;
            label6.Text = string.Empty;
            label7.Text = string.Empty;
            //double totalzakat = 0;
            label3.Text = comboBox1.Text;

            var count = Zakatgrid(new Guid(lblGUID.Text), label3.Text);
            if (dataGridView1.Rows.Count == 0)//dataGridView1.CurrentRow.Cells[0].Value == null
            {
                label6.Text = 0.ToString();
                label7.Text = 0.ToString();
            }
            else
            {
                label6.Text = calculatezakat(dataGridView1, count);
                label7.Text = count.ToString();
            }


        }
        string calculatezakat(DataGridView row, int count)
        {
            var zakat = 0.00;
            var totalzakat = 0.00;
            foreach (DataGridViewRow item in row.Rows)
            {
                zakat = (double)item.Cells[4].Value;
                totalzakat = totalzakat + zakat;
            }

            return totalzakat.ToString();

        }
        int Zakatgrid(Guid guid, string year)
        {
            ZakatCS zakatCS = new ZakatCS();
            //The entity or complex type cannot be constructed in a LINQ to Entities query.
            // the above error was removed with the help of 
            //https://social.msdn.microsoft.com/Forums/en-US/cdd1e6b3-11e3-4341-ae90-9f6093010c0c/the-entity-or-complex-type-categories-cannot-be-constructed-in-a-linq-to-entities-query?forum=adodotnetentityframework

            var newlist = (from a in zakatCS.Accounts
                           join b in zakatCS.ZakatYears on a.AID equals b.AID
                           join c in zakatCS.ZakatItems on b.ZID equals c.ZID
                           where a.AID == guid && b.ZYear == year
                           select (new
                           {
                               ID = c.ID,
                               ItemName = c.ItemName,
                               Description = c.Description,
                               MarketPrice = c.MarketPrice,
                               ItemZakat = c.ItemZakat,
                               year = c.ZakatYear
                           })).ToList().Select(item => new ZakatItem
                           {
                               ID = item.ID,
                               Description = item.Description,
                               ItemName = item.ItemName,
                               MarketPrice = item.MarketPrice,
                               ItemZakat = item.ItemZakat,

                           }).ToList();




            //var list = ZakatItemView.Find(a => a.ZakatYear.ZYear == label3.Text).ToList();
            var bindinglist = new BindingList<ZakatItem>(newlist);
            dataGridView1.DataSource = newlist;
            return newlist.Count;
        }



        double markettprice = 0;
        double itemzakat = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Cannot Leave Name Empty");
            }
            if (textBox3.Text == string.Empty) { MessageBox.Show("Cannot Leave Market Price Empty"); }
            if (!double.TryParse(textBox3.Text, out double a))
            {
                MessageBox.Show("Please Insert Numerical Value");
                textBox3.Focus();
            }

            using (ZakatItem newitem = new ZakatItem())
            {
                newitem.ItemName = textBox1.Text;
                newitem.Description = textBox2.Text;
                newitem.MarketPrice = markettprice;
                newitem.ItemZakat = itemzakat;
                // in below vaiable, guid is get to insert value in the year of correct user
                int zakatYear = allyears.FirstOrDefault(year => year.ZYear == label3.Text && year.AID == new Guid(lblGUID.Text)).ZID;
                newitem.ZID = zakatYear;


                try
                {
                    ZakatItemView.Insert(newitem);
                    MessageBox.Show("New Item Added");
                    var count = Zakatgrid(new Guid(lblGUID.Text), label3.Text);
                    label6.Text = calculatezakat(dataGridView1, count);
                    label7.Text = count.ToString();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" +
                        ex.StackTrace + "\n" + ex.Source + "\n" + ex.ToString());
                }
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            try
            {
                markettprice = Convert.ToDouble(textBox3.Text);
                itemzakat = markettprice * (2.5 / 100);
                label12.Text = itemzakat.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

        }
        internal static Guid guid;
        private void lblGUID_TextChanged(object sender, EventArgs e)
        {
            Guid aid = Guid.Parse(lblGUID.Text);
            allyears = zakatyear.Find(a => a.AID == aid).ToList();
            guid = aid;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            lblitemid.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var zakatitemupdate = ZakatItemView.GetAll().FirstOrDefault(a => a.ID == Convert.ToInt32(lblitemid.Text));
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Cannot Leave Name Empty");
            }
            if (textBox3.Text == string.Empty) { MessageBox.Show("Cannot Leave Market Price Empty"); }
            if (!double.TryParse(textBox3.Text, out double sa))
            {
                MessageBox.Show("Please Insert Numerical Value");
                textBox3.Focus();
            }
            zakatitemupdate.ItemName = textBox1.Text;
            zakatitemupdate.Description = textBox2.Text;
            zakatitemupdate.MarketPrice = markettprice;
            zakatitemupdate.ItemZakat = itemzakat;
            ZakatItemView.Update(zakatitemupdate);
            MessageBox.Show("Item Updated");
            var count = Zakatgrid(new Guid(lblGUID.Text), label3.Text);
            label6.Text = calculatezakat(dataGridView1, count);
            label7.Text = count.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            Login login = new Login();
            login.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var zakatitemdelete = ZakatItemView.GetAll().FirstOrDefault(a => a.ID == Convert.ToInt32(lblitemid.Text));
            ZakatItemView.Delete(zakatitemdelete);
            MessageBox.Show("Item Deleted");
            var count = Zakatgrid(new Guid(lblGUID.Text), label3.Text);
            label6.Text = calculatezakat(dataGridView1, count);
            label7.Text = count.ToString();
        }

        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Accounts accounts = new Accounts(this);
            accounts.Show();
        }

        private void copyZakatItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyZakatItems copyZakatItems = new CopyZakatItems();
            copyZakatItems.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void printReportOnOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!printReportOnOffToolStripMenuItem.Checked)
                printReportOnOffToolStripMenuItem.Checked = true;
            else
                printReportOnOffToolStripMenuItem.Checked = false;

        }

        private void printReportOnOffToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (printReportOnOffToolStripMenuItem.Checked)
            {
                this.Width = 1084;
                this.Height = 478;

                zakatreport crystalReport1 = new zakatreport();

                try
                {
                    ParameterFieldDefinitions parameterFieldDefinitions1 = crystalReport1.DataDefinition.ParameterFields;
                    ParameterFieldDefinition parameterFieldDefinition1 = parameterFieldDefinitions1["AID"];
                    ParameterDiscreteValue parameterDiscreteValue1 = new ParameterDiscreteValue();
                    parameterDiscreteValue1.Value = lblGUID.Text;
                    ParameterValues parameterValues1 = new ParameterValues(parameterFieldDefinition1.CurrentValues);
                    parameterValues1.Add(parameterDiscreteValue1);
                    parameterFieldDefinition1.ApplyCurrentValues(parameterValues1);

                    ParameterFieldDefinitions parameterFieldDefinitions2 = crystalReport1.DataDefinition.ParameterFields;
                    ParameterFieldDefinition parameterFieldDefinition2 = parameterFieldDefinitions2["ZYear"];
                    ParameterDiscreteValue parameterDiscreteValue2 = new ParameterDiscreteValue();
                    parameterDiscreteValue2.Value = comboBox1.Text;
                    ParameterValues parameterValues2 = new ParameterValues(parameterFieldDefinition2.CurrentValues);
                    parameterValues2.Add(parameterDiscreteValue2);
                    parameterFieldDefinition2.ApplyCurrentValues(parameterValues2);

                    //crystalReportViewer1.ShowRefreshButton = false;
                    crystalReportViewer1.ShowParameterPanelButton = false;
                    crystalReportViewer1.ShowCopyButton = false;
                    crystalReportViewer1.ShowExportButton = false;


                    crystalReportViewer1.ReportSource = crystalReport1;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                this.Width = 548;
                this.Height = 478;


            }
        }

        private void printReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == null || comboBox1.Text == string.Empty)
            {
                printReportOnOffToolStripMenuItem.Enabled = false;
                MessageBox.Show("Please select the year first");

            }
            else
            {
                printReportOnOffToolStripMenuItem.Enabled = true;
            }
        }
    }
}
