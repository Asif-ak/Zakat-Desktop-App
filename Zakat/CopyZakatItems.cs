using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Zakat.App_Code;

namespace Zakat
{
    public partial class CopyZakatItems : Form
    {
        IRepository<ZakatYear> zakatyear = new Implementation<ZakatYear>();

        List<ZakatYear> previousyears = new List<ZakatYear>();
        public CopyZakatItems()
        {
            InitializeComponent();
        }

        private void CopyZakatItems_Load(object sender, EventArgs e)
        {
            guidlabel.Text = Form1.guid.ToString();
            TocomboBox.Items.Clear();
            FromcomboBox.Items.Clear();
            // to get all years from this account

            //var allyears = zakatyear.GetAll().Where(account=>account.AID== new Guid(guidlabel.Text));
            // to get current year from all years

            //foreach (var currentyear in allyears.Where(year => year.ZYear == Convert.ToString(DateTime.Now.Year)))
            //{
            //    TocomboBox.Items.Add(currentyear.ZYear);
            //}

            // a much better approach :

            var thisyear = zakatyear.GetAll().Where(account => account.AID == new Guid(guidlabel.Text))
                .FirstOrDefault(year => year.ZYear == Convert.ToString(DateTime.Now.Year)).ZYear;

            TocomboBox.Items.Add(thisyear);

            // fetching previous years =>(!= current years)
            previousyears = zakatyear.GetAll().Where(a => a.AID == new Guid(guidlabel.Text)).ToList();
            foreach (var item in previousyears.Where(b => b.ZYear != Convert.ToString(DateTime.Now.Year)))
            {
                FromcomboBox.Items.Add(item.ZYear);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var context = new ZakatCS())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    IList<ZakatItem> items = new List<ZakatItem>();
                    IRepository<ZakatItem> zakatitems = new Implementation<ZakatItem>();
                    
                    try
                    {
                        var fromzid = previousyears.FirstOrDefault(a => a.ZYear == FromcomboBox.Text && a.AID == new Guid(guidlabel.Text)).ZID;
                        var currentzid= previousyears.FirstOrDefault(a => a.ZYear == TocomboBox.Text && a.AID == new Guid(guidlabel.Text)).ZID;

                        IEnumerable<ZakatItem> fromitems = zakatitems.GetAll().Where(a => a.ZID == fromzid);
                        foreach (var item in fromitems)
                        {
                            ZakatItem toitem = new ZakatItem
                            {
                                ItemName = item.ItemName,
                                ItemZakat=item.ItemZakat,
                                Description=item.Description,
                                MarketPrice=item.MarketPrice,
                                ZID=currentzid
                                
                            };
                            items.Add(toitem);
                            //context.ZakatItems.Add(toitem);
                            //zakatitems.Insert(toitem);
                        }
                        // some unknown reasons, addrange() is not working..
                        // its working now, we were not saving changes.
                        
                        context.ZakatItems.AddRange(items); context.SaveChanges();
                          //.Where(a => a.ZakatYear.ZYear == TocomboBox.Text));
                        transaction.Commit();
                        MessageBox.Show($"Data Copied from {FromcomboBox.Text} to {TocomboBox.Text}");
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
