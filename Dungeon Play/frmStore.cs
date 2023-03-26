using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Management;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Dungeon_Play
{
    public partial class frmStore : Form
    {
        public frmMain mainForm = new frmMain();
        public List<Inventory> inventories = new List<Inventory>();
        public List<CharacterItem> characterItems = new List<CharacterItem>();
        public InventoryDBDataContext idb = new InventoryDBDataContext();
        public CharacterItemDBDataContext cdb = new CharacterItemDBDataContext();

        public frmStore()
        {
            InitializeComponent();
        }

        private void frmStore_Load(object sender, EventArgs e)
        {
            //Gets all information from the database from the Inventories table.
            var results = from Inventory in idb.Inventories select Inventory;

            //Fills the Inventory list.
            foreach (var elements in results)
            {
                lstInventory.Items.Add(elements.itemName + "  " + elements.quantity);
                inventories.Add(elements);
            }

            //To get the information from frmMain class for CharacterList.
            characterItems = mainForm.getCharacterList();

            //This adds only the items from the CharacterItem class that have a
            //position number.  This indicates that it is an inventory item.
            foreach (var element in characterItems)
            {
                if (element.slot > 0)
                {
                    lstCharacter.Items.Add(element.itemName);
                }
            }
        }

        //Information from the database shown when selected in SQL fields.
        private void lstInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int select = lstInventory.SelectedIndex;
            priceTextBox.Text = inventories[select].price.ToString("n0");
            quantityTextBox.Text = inventories[select].quantity.ToString();
            armorClassTextBox.Text = inventories[select].armorClass.ToString();
            attackValueTextBox.Text = inventories[select].attackValue.ToString();
            minDamageTextBox.Text = inventories[select].minDamage.ToString();
            maxDamageTextBox.Text = inventories[select].maxDamage.ToString();
        }

        //Purchase item selected and adjust SQL.
        private void btnPurchase_Click(object sender, EventArgs e)
        {
            if (lstInventory.SelectedItem != null)
            {
                string select = lstInventory.SelectedItem.ToString();
                string name = select.Substring(0, select.LastIndexOf(' '));

                Inventory look = idb.Inventories.FirstOrDefault(item => item.itemName == name);

                look.quantity -= 1;
                idb.SubmitChanges();

//**            Experimental                                          **//
                CharacterItem newItem = new CharacterItem();
                newItem.itemName = look.itemName;
                newItem.bonusType = look.bonusType;
                newItem.bonusValue = look.bonusValue;
                newItem.minDamage = look.minDamage;
                newItem.maxDamage = look.maxDamage;
                newItem.attackValue = look.attackValue;
                newItem.armorClass = look.armorClass;
                newItem.weight = look.weight;
                newItem.volume = look.volume;
                newItem.price = look.price;
                newItem.quantity = 1;
                newItem.slot = 0;
                newItem.imageName = look.imageName;

                cdb.CharacterItems.InsertOnSubmit(newItem);
                cdb.SubmitChanges();

//**                                                                **//

                mainForm.addToCharacterList(name, look.price);
                lstCharacter.Items.Clear();
                lstInventory.Items.Clear();
                frmStore_Load(sender, e);
            }
        }

        //Sell the Item selected and adjust SQL.
        private void btnSell_Click(object sender, EventArgs e)
        {
            string name = lstCharacter.SelectedItem.ToString();

            Inventory look = idb.Inventories.FirstOrDefault(item => item.itemName == name);

            //Adjusts the quantity for the Inventory Class
            if (look.quantity < 5 && look.itemName != "")
            {
                look.quantity += 1;
                idb.SubmitChanges();
            }

            mainForm.removeFromCharacterList(name, look.price, sender, e);
            lstCharacter.Items.Clear();
            lstInventory.Items.Clear();
            frmStore_Load(sender, e);
        }

        //Close this window.
        private void btnReturnMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
