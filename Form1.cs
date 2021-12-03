using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Security.Credentials;

namespace ResetNotForThisSite
{
    public partial class Form1 : Form
    {
        private readonly PasswordVault passwordVault;

        public Form1()
        {
            passwordVault = new Windows.Security.Credentials.PasswordVault();

            InitializeComponent();
        }

        class Item
        {
            internal readonly PasswordCredential cred;

            public Item(PasswordCredential cred)
            {
                this.cred = cred;
            }

            public override string ToString() => $"サイト {cred.Resource} アカウント {cred.UserName}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            accounts.Items.Clear();
            accounts.Items.AddRange(
                passwordVault.RetrieveAll()
                    .Where(entry => entry.Properties.Any(kv => kv.Key.Equals("hidden") && kv.Value is bool && (bool)kv.Value))
                    .Select(it => new Item(it))
                    .ToArray()
            );

            if (accounts.Items.Count >= 1)
            {
                accounts.SelectedIndex = 0;
            }
        }

        private void remove_Click(object sender, EventArgs e)
        {
            var item = accounts.SelectedItem as Item;
            if (item != null)
            {
                passwordVault.Remove(item.cred);
                MessageBox.Show("削除しました。");
            }
            Reload();
        }
    }
}
