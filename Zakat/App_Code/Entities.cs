using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration;

namespace Zakat.App_Code
{
    [Table("A")]
    class Account
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AID { get; set; }
        [Required]
        public string AccountName { get; set; }
        public string password { get; set; }
        public ICollection<ZakatYear> ZakatYears { get; set; }
    }
    [Table("B")]
    class ZakatYear
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ZID { get; set; }
        public string ZYear { get; set; }
        [Required]
        public DateTime Date { get; set; }
        //[Required]
        public Guid AID { get; set; }
        public virtual Account Accounts { get; set; }
        public ICollection<ZakatItem> ZakatItems { get; set; }

    }
    [Table("C")]
    class ZakatItem:IDisposable
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string ItemName { get; set; }
        public string Description { get; set; }
        [Required]
        public double MarketPrice { get; set; }
        
        [Required]
        public double ItemZakat { get; set; }
        [Required]
        public int ZID { get; set; }
        
        public virtual ZakatYear ZakatYear { get; set; }
        
        public void Dispose()
        {
            
        }
        
    }
    class ZakatCS : DbContext
    {
        public ZakatCS() : base("ZakatCS") // change context name if needed
        {

        }
        public DbSet<ZakatYear> ZakatYears { get; set; }
        public DbSet<ZakatItem> ZakatItems { get; set; }
        public DbSet<Account> Accounts { get; set; }


        
    }
}
