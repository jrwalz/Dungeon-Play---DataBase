
namespace Dungeon_Play
{
    partial class frmStore
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label armorClassLabel;
            System.Windows.Forms.Label attackValueLabel;
            System.Windows.Forms.Label maxDamageLabel;
            System.Windows.Forms.Label minDamageLabel;
            System.Windows.Forms.Label priceLabel;
            System.Windows.Forms.Label quantityLabel;
            this.lstInventory = new System.Windows.Forms.ListBox();
            this.inventoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnReturnMain = new System.Windows.Forms.Button();
            this.btnPurchase = new System.Windows.Forms.Button();
            this.inventoryBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.armorClassTextBox = new System.Windows.Forms.TextBox();
            this.attackValueTextBox = new System.Windows.Forms.TextBox();
            this.maxDamageTextBox = new System.Windows.Forms.TextBox();
            this.minDamageTextBox = new System.Windows.Forms.TextBox();
            this.priceTextBox = new System.Windows.Forms.TextBox();
            this.quantityTextBox = new System.Windows.Forms.TextBox();
            this.lstCharacter = new System.Windows.Forms.ListBox();
            this.btnSell = new System.Windows.Forms.Button();
            this.LblStoreHeader = new System.Windows.Forms.Label();
            this.lblCharacterHeader = new System.Windows.Forms.Label();
            armorClassLabel = new System.Windows.Forms.Label();
            attackValueLabel = new System.Windows.Forms.Label();
            maxDamageLabel = new System.Windows.Forms.Label();
            minDamageLabel = new System.Windows.Forms.Label();
            priceLabel = new System.Windows.Forms.Label();
            quantityLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // armorClassLabel
            // 
            armorClassLabel.AutoSize = true;
            armorClassLabel.Location = new System.Drawing.Point(287, 111);
            armorClassLabel.Name = "armorClassLabel";
            armorClassLabel.Size = new System.Drawing.Size(97, 20);
            armorClassLabel.TabIndex = 22;
            armorClassLabel.Text = "armor Class:";
            // 
            // attackValueLabel
            // 
            attackValueLabel.AutoSize = true;
            attackValueLabel.Location = new System.Drawing.Point(287, 159);
            attackValueLabel.Name = "attackValueLabel";
            attackValueLabel.Size = new System.Drawing.Size(102, 20);
            attackValueLabel.TabIndex = 24;
            attackValueLabel.Text = "attack Value:";
            // 
            // maxDamageLabel
            // 
            maxDamageLabel.AutoSize = true;
            maxDamageLabel.Location = new System.Drawing.Point(287, 207);
            maxDamageLabel.Name = "maxDamageLabel";
            maxDamageLabel.Size = new System.Drawing.Size(107, 20);
            maxDamageLabel.TabIndex = 32;
            maxDamageLabel.Text = "max Damage:";
            // 
            // minDamageLabel
            // 
            minDamageLabel.AutoSize = true;
            minDamageLabel.Location = new System.Drawing.Point(287, 255);
            minDamageLabel.Name = "minDamageLabel";
            minDamageLabel.Size = new System.Drawing.Size(103, 20);
            minDamageLabel.TabIndex = 34;
            minDamageLabel.Text = "min Damage:";
            // 
            // priceLabel
            // 
            priceLabel.AutoSize = true;
            priceLabel.Location = new System.Drawing.Point(287, 63);
            priceLabel.Name = "priceLabel";
            priceLabel.Size = new System.Drawing.Size(47, 20);
            priceLabel.TabIndex = 36;
            priceLabel.Text = "price:";
            // 
            // quantityLabel
            // 
            quantityLabel.AutoSize = true;
            quantityLabel.Location = new System.Drawing.Point(287, 303);
            quantityLabel.Name = "quantityLabel";
            quantityLabel.Size = new System.Drawing.Size(69, 20);
            quantityLabel.TabIndex = 38;
            quantityLabel.Text = "quantity:";
            // 
            // lstInventory
            // 
            this.lstInventory.FormattingEnabled = true;
            this.lstInventory.ItemHeight = 20;
            this.lstInventory.Location = new System.Drawing.Point(33, 48);
            this.lstInventory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstInventory.Name = "lstInventory";
            this.lstInventory.Size = new System.Drawing.Size(229, 384);
            this.lstInventory.TabIndex = 0;
            this.lstInventory.SelectedIndexChanged += new System.EventHandler(this.lstInventory_SelectedIndexChanged);
            // 
            // btnReturnMain
            // 
            this.btnReturnMain.AutoSize = true;
            this.btnReturnMain.Location = new System.Drawing.Point(70, 461);
            this.btnReturnMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReturnMain.Name = "btnReturnMain";
            this.btnReturnMain.Size = new System.Drawing.Size(176, 35);
            this.btnReturnMain.TabIndex = 20;
            this.btnReturnMain.Text = "Return to Main Menu";
            this.btnReturnMain.UseVisualStyleBackColor = true;
            this.btnReturnMain.Click += new System.EventHandler(this.btnReturnMain_Click);
            // 
            // btnPurchase
            // 
            this.btnPurchase.Location = new System.Drawing.Point(343, 461);
            this.btnPurchase.Name = "btnPurchase";
            this.btnPurchase.Size = new System.Drawing.Size(114, 35);
            this.btnPurchase.TabIndex = 22;
            this.btnPurchase.Text = "Purchase";
            this.btnPurchase.UseVisualStyleBackColor = true;
            this.btnPurchase.Click += new System.EventHandler(this.btnPurchase_Click);
            // 
            // inventoryBindingSource1
            // 
            this.inventoryBindingSource1.DataSource = typeof(Dungeon_Play.Inventory);
            // 
            // armorClassTextBox
            // 
            this.armorClassTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.inventoryBindingSource1, "armorClass", true));
            this.armorClassTextBox.Location = new System.Drawing.Point(400, 108);
            this.armorClassTextBox.Name = "armorClassTextBox";
            this.armorClassTextBox.Size = new System.Drawing.Size(100, 26);
            this.armorClassTextBox.TabIndex = 23;
            // 
            // attackValueTextBox
            // 
            this.attackValueTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.inventoryBindingSource1, "attackValue", true));
            this.attackValueTextBox.Location = new System.Drawing.Point(400, 156);
            this.attackValueTextBox.Name = "attackValueTextBox";
            this.attackValueTextBox.Size = new System.Drawing.Size(100, 26);
            this.attackValueTextBox.TabIndex = 25;
            // 
            // maxDamageTextBox
            // 
            this.maxDamageTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.inventoryBindingSource1, "maxDamage", true));
            this.maxDamageTextBox.Location = new System.Drawing.Point(400, 204);
            this.maxDamageTextBox.Name = "maxDamageTextBox";
            this.maxDamageTextBox.Size = new System.Drawing.Size(100, 26);
            this.maxDamageTextBox.TabIndex = 33;
            // 
            // minDamageTextBox
            // 
            this.minDamageTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.inventoryBindingSource1, "minDamage", true));
            this.minDamageTextBox.Location = new System.Drawing.Point(400, 252);
            this.minDamageTextBox.Name = "minDamageTextBox";
            this.minDamageTextBox.Size = new System.Drawing.Size(100, 26);
            this.minDamageTextBox.TabIndex = 35;
            // 
            // priceTextBox
            // 
            this.priceTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.inventoryBindingSource1, "price", true));
            this.priceTextBox.Location = new System.Drawing.Point(400, 60);
            this.priceTextBox.Name = "priceTextBox";
            this.priceTextBox.Size = new System.Drawing.Size(100, 26);
            this.priceTextBox.TabIndex = 37;
            // 
            // quantityTextBox
            // 
            this.quantityTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.inventoryBindingSource1, "quantity", true));
            this.quantityTextBox.Location = new System.Drawing.Point(400, 300);
            this.quantityTextBox.Name = "quantityTextBox";
            this.quantityTextBox.Size = new System.Drawing.Size(100, 26);
            this.quantityTextBox.TabIndex = 39;
            // 
            // lstCharacter
            // 
            this.lstCharacter.FormattingEnabled = true;
            this.lstCharacter.ItemHeight = 20;
            this.lstCharacter.Location = new System.Drawing.Point(533, 48);
            this.lstCharacter.Name = "lstCharacter";
            this.lstCharacter.Size = new System.Drawing.Size(214, 384);
            this.lstCharacter.TabIndex = 40;
            // 
            // btnSell
            // 
            this.btnSell.Location = new System.Drawing.Point(587, 461);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(105, 35);
            this.btnSell.TabIndex = 41;
            this.btnSell.Text = "Sell";
            this.btnSell.UseVisualStyleBackColor = true;
            this.btnSell.Click += new System.EventHandler(this.btnSell_Click);
            // 
            // LblStoreHeader
            // 
            this.LblStoreHeader.AutoSize = true;
            this.LblStoreHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblStoreHeader.Location = new System.Drawing.Point(86, 17);
            this.LblStoreHeader.Name = "LblStoreHeader";
            this.LblStoreHeader.Size = new System.Drawing.Size(132, 20);
            this.LblStoreHeader.TabIndex = 42;
            this.LblStoreHeader.Text = "Store Inventory";
            // 
            // lblCharacterHeader
            // 
            this.lblCharacterHeader.AutoSize = true;
            this.lblCharacterHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCharacterHeader.Location = new System.Drawing.Point(558, 17);
            this.lblCharacterHeader.Name = "lblCharacterHeader";
            this.lblCharacterHeader.Size = new System.Drawing.Size(167, 20);
            this.lblCharacterHeader.TabIndex = 43;
            this.lblCharacterHeader.Text = "Character Inventory";
            // 
            // frmStore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1737, 597);
            this.Controls.Add(this.lblCharacterHeader);
            this.Controls.Add(this.LblStoreHeader);
            this.Controls.Add(this.btnSell);
            this.Controls.Add(this.lstCharacter);
            this.Controls.Add(armorClassLabel);
            this.Controls.Add(this.armorClassTextBox);
            this.Controls.Add(attackValueLabel);
            this.Controls.Add(this.attackValueTextBox);
            this.Controls.Add(maxDamageLabel);
            this.Controls.Add(this.maxDamageTextBox);
            this.Controls.Add(minDamageLabel);
            this.Controls.Add(this.minDamageTextBox);
            this.Controls.Add(priceLabel);
            this.Controls.Add(this.priceTextBox);
            this.Controls.Add(quantityLabel);
            this.Controls.Add(this.quantityTextBox);
            this.Controls.Add(this.btnPurchase);
            this.Controls.Add(this.btnReturnMain);
            this.Controls.Add(this.lstInventory);
            this.Name = "frmStore";
            this.Text = "Blacksmith Store";
            this.Load += new System.EventHandler(this.frmStore_Load);
            ((System.ComponentModel.ISupportInitialize)(this.inventoryBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inventoryBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstInventory;
        private System.Windows.Forms.BindingSource inventoryBindingSource;
        private System.Windows.Forms.Button btnReturnMain;
        private System.Windows.Forms.Button btnPurchase;
        private System.Windows.Forms.BindingSource inventoryBindingSource1;
        private System.Windows.Forms.TextBox armorClassTextBox;
        private System.Windows.Forms.TextBox attackValueTextBox;
        private System.Windows.Forms.TextBox maxDamageTextBox;
        private System.Windows.Forms.TextBox minDamageTextBox;
        private System.Windows.Forms.TextBox priceTextBox;
        private System.Windows.Forms.TextBox quantityTextBox;
        private System.Windows.Forms.ListBox lstCharacter;
        private System.Windows.Forms.Button btnSell;
        private System.Windows.Forms.Label LblStoreHeader;
        private System.Windows.Forms.Label lblCharacterHeader;
    }
}