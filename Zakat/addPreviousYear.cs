using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zakat.App_Code;

namespace Zakat
{
    //public class addPreviousYearEventArgs:EventArgs
    //{
    //    public bool IsAdded = false;
    //}
    public partial class addPreviousYear : Form
    {
        IRepository<ZakatYear> previousyears = new Implementation<ZakatYear>();
        List<ZakatYear> Allyears;
        ZakatYear zakatYear;
        private readonly Form _form1;
        public addPreviousYear(Form1 form1)
        {
            InitializeComponent();
            //Allyears = previousyears.GetAll().Where(year => year.AID == new Guid(guidlabel.Text)).ToList();
            _form1 = form1;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Allyears = previousyears.GetAll().Where(year => year.AID == new Guid(guidlabel.Text)).ToList();
            string pattern = @"^(19|20)\d{2}$";
            if (Regex.IsMatch(textBox1.Text, pattern, RegexOptions.Compiled))
            {
                try
                {
                    Task.Factory.StartNew(() =>
                    {
                        zakatYear = new ZakatYear
                        {
                            ZYear = textBox1.Text,
                            Date = DateTime.Now.Date,
                            AID = Form1.guid
                        };

                        if (Allyears.Any(a => a.ZYear == zakatYear.ZYear))
                            MessageBox.Show(zakatYear.ZYear + " Already Exist");
                        else
                        {
                            previousyears.Insert(zakatYear);
                            MessageBox.Show("New Year Added");

                        }
                        if (this.textBox1.InvokeRequired)
                        {
                            textBox1.Invoke(new Action(() => textBox1.Text = string.Empty));
                        }
                        //textBox1.Text = string.Empty;

                        return true;
                    }).ContinueWith((antecedent) =>
                    {

                        if (antecedent.Result && (Form1)_form1 != null)
                        {
                            Form1 temp = (Form1)_form1;
                            temp.UpdateComboOnInvoke();
                        }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please enter valid year");
            }
            



        }

        private void addPreviousYear_Load(object sender, EventArgs e)
        {
            guidlabel.Text = Form1.guid.ToString();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var context = new ZakatCS())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var year = previousyears.GetAll().Where(b => b.AID == Form1.guid)
                            .AsEnumerable().FirstOrDefault(c => c.ZYear == textBox1.Text);
                        context.ZakatItems.RemoveRange(context.ZakatItems.Where(a => a.ZakatYear.ZYear == year.ZYear));
                        previousyears.Delete(year);
                        transaction.Commit();
                        MessageBox.Show("Year and Associated Data Deleted");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

    }
}
