using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zakat.App_Code;

namespace Zakat
{
    public partial class Accounts : Form
    {
        IRepository<Account> acc = new Implementation<Account>();
        private readonly Form1 _form1;
        public Accounts(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;
        }

        private void Account_Load(object sender, EventArgs e)
        {
            label1.Text = Form1.guid.ToString();
            var accountname = acc.GetAll().FirstOrDefault((a => a.AID == new Guid(label1.Text)));
            textBox1.Text =accountname.AccountName;
            textBox2.Text = accountname.password;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var accountname = acc.GetAll().FirstOrDefault((a => a.AID == new Guid(label1.Text)));
            accountname.AccountName = textBox1.Text;
            accountname.password = textBox2.Text;
            acc.Update(accountname);
            MessageBox.Show("Account Updated");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Are you sure you want to delete accout? It will delete all the associated data !!", 
                "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if(result==DialogResult.Yes)
            {
                using (var context = new ZakatCS())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            var account = acc.GetAll().Where(a => a.AID == new Guid(label1.Text)).FirstOrDefault();
                            var years = context.ZakatYears.Where(b => b.AID == account.AID).ToList();
                            foreach (var item in years)
                            {
                                context.ZakatItems.RemoveRange(context.ZakatItems.Where(c => c.ZakatYear.ZYear == item.ZYear));
                            }
                            foreach (var item in years)
                            {
                                context.ZakatYears.RemoveRange(context.ZakatYears.Where(d => d.AID == account.AID));
                            }
                           // context.SaveChanges();
                            acc.Delete(account);
                            transaction.Commit();
                            if (_form1 != null)
                            {
                                Form1 temp = _form1;
                                temp.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                label1.Text = "Bach gya saley";
            }
        }
    }
}
