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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        IRepository<Account> repository = new Implementation<Account>();
        private void button2_Click(object sender, EventArgs e)
        {
            Account account = new Account()
            {

                AccountName = textBox1.Text,
                password = textBox2.Text,
                //AID= new Guid()

            };
            try
            {
                repository.Insert(account);
                MessageBox.Show("New Account Added");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var data = repository.Find(a => a.AccountName == textBox1.Text && a.password == textBox2.Text);
            var login = data.Any();
            try
            {
                if (login)
                {
                    this.Visible = false;
                    Form1 form1 = new Form1();

                    form1.Load += (ss, ee) =>
                    {
                        form1.LblUserName.Text = data.Select(a => a.AccountName).FirstOrDefault();
                        form1.lblGUID.Text = data.Select(b => b.AID).FirstOrDefault().ToString();

                    };
                    
                    form1.Show();

                    //addPreviousYear addPreviousYear = new addPreviousYear();
                    //addPreviousYear.Load += (sss, eee) =>
                    //  {
                    //      addPreviousYear.label2.Text= data.Select(b => b.AID).FirstOrDefault().ToString();
                    //  };
                    
                }
                else
                {
                    MessageBox.Show("Wrong User Id or Password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }
    }
}
