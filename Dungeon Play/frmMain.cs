using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Management;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Threading;

namespace Dungeon_Play
{
    public partial class frmMain : Form
    {
        static dynamic results = "";
        InitializeOnce controller;

        //These are all used to help track where the picture came from and where it is going
        //so that the pictureboxes know what goes where.
        public static int previousPicture;
        public static int currentPicture;
        public static Image previousImage;
        public static Image currentImage;

        //These are not necessary lists, but they hold important information in regard to potential
        //expansion.
        public List<string> fileName = new List<string>();
        public List<Image> itemImages = new List<Image>();

        //Types of image holders, the emptyImage is in a pictureBox that holds the "empty" picture
        //for a non-picture.
        public static Image emptyImage;
        public Image itemImage;
        public static Image convertedImage;

        //This is once to fill the ListArray characterItemsList with null values to
        //get a clean set of index's.
        public CharacterItem characterItem = new CharacterItem();

        //Used to Create a psueudo player for template purposes.
        public Character player = new Character();
        public List<Character> players = new List<Character>();

        //The main list array and therefore set to static to maintain data value bewtween the
        //two forms.
        public static List<CharacterItem> characterItemsList = new List<CharacterItem>();

        //Data Source connections to the databases and tables.
        CharacterItemDBDataContext db = new CharacterItemDBDataContext();
        InventoryDBDataContext idb = new InventoryDBDataContext();
 
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //The following code is used only once upon starting the program.  It gets the information from the files for
            //the images and submits those to the SQL database.
            controller = new InitializeOnce();
            if (controller.LockedGui)
            {
                emptyImage = pictureBox.Image;

                //Collect the images from a specific file, get their names, and use that to attach the Byte image object into
                //the database.
                string path = Path.Combine(Environment.CurrentDirectory, @"Assets\", "Idylwild's Armory");
                byte[] imgByte;
                string[] extensions = { ".jpg", ".jpeg", ".png" };

                //Collects all of the files in the directory specified by the path and the precedding extensions.  I had
                //to research this one online.  I had to have a way to collect the images that would be loaded into SQL.
                var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(s => extensions.Any(ext => ext == Path.GetExtension(s)));

                //Load Images from the Assets Folder.
                foreach (string file in files)
                {
                    itemImage = Image.FromFile(file);
                    string first = file.Substring(file.LastIndexOf('\\') + 1);
                    int last = first.LastIndexOf('.');
                    string fullFileName = first.Substring(0, last);
                    fileName.Add(first);
                    itemImages.Add(itemImage);

                    //Creates the means to get the object or row information from the Inventory class.
                    Inventory inv = idb.Inventories.FirstOrDefault(item => item.itemName == fullFileName);

                    //Initially sets the image to a picturebox for program runtime placement and makes
                    //adjustments after that.
                    pictureBox.Image = itemImage;
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                    //I guess that this provides the medium or flexibility for the byte data to transfer
                    //the data from SQL to the Application.
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        pictureBox.Image.Save(mStream, pictureBox.Image.RawFormat);
                        imgByte = mStream.ToArray();
                    }

                    //These place the uploaded images into the database and save them for runtime in
                    //both the Inventory and CharacterList classes.
                    inv.imageName = imgByte;
                    idb.SubmitChanges();

                }//**              End of Load Images area           **//

                controller.LockedGui = false;

            }//End of controller Decision Statement.

            //The following information is used for both the start of the program and also to
            //refresh list arrays when leaving and returning to this form from the Store form.
            players = player.getPlayers();
            foreach (var pl in players)
            {
                lblName.Text = pl.Name;
                lblHitPoints.Text = pl.HitPoints.ToString();
                lblAttack.Text = pl.Attack.ToString();
                lblArmorClass.Text = pl.Armor.ToString();
                lblEndurance.Text = pl.Endurance.ToString();
                lblPlatinum.Text = pl.Currency.ToString("n0");
            }

            if (db != null)
            {
                results = from CharacterItem in db.CharacterItems select CharacterItem;
            }

            //Fills the List array with Indexes and nothing else.
            for (int i = 0; i < 24; i++)
            {
                characterItemsList.Add(characterItem);
            }

            //This fills the Character List Array with values from the database if there is an item within
            //that slot, a.k.a. (equiped item).
            foreach (var result in results)
            {
                if (result.slot > 0)
                {
                    characterItemsList[result.slot - 1] = result;
                }
            }

            //Sorts the initial List Array correctly according to slot, the index number of the array is the slot number -1
            //for the pictures and objects and adds it to a listbox.
            initializeEvents();
            sortInventory(characterItemsList, 0);
            showStats(characterItemsList);

        }  //**           End of frmMain_Load Class            **//

        //This converts the rawformat images in SQL into Images format from the CharacterItem List Array.
        public Image convertToImage(int number)
        {           
            byte[] imageByte = (byte[])characterItemsList[number].imageName.ToArray();
            MemoryStream ms = new MemoryStream(imageByte);
            pictureBox.Image = new Bitmap(ms);
            convertedImage = pictureBox.Image;
            return convertedImage;
        }

        //If an item is added to the character inventory then it is adjusted in
        //the characterItemList Array.
        public void addToCharacterList(string name, decimal price)
        {
            int count = 0;
            int slotCount = 0;

            //check to make sure that the inventory locations are not full.
            foreach(var el in characterItemsList)
            {
                count++;

                if(el.slot == count && count < 9)
                {
                    slotCount++;
                }
            } 

            //If inventory is full, then divert to a notification.
            if (slotCount != 8)
            {
                int index = 0;
                int counter = 0;
                bool flag = false;

                players = player.getPlayers();
                players[0].Currency -= price;

                //Gets the lowest open slot position within the non-equiped slots to place the
                //new item.
                foreach (var elements in characterItemsList)
                {
                    counter++;

                    if (elements.slot == 0 && flag == false && counter < 9)
                    {
                        index = counter;
                        flag = true;
                    }
                }

                //Set the database row up correctly so that when we add to the list array, it has all of
                //the needed information.
                if (db != null)
                {
                    CharacterItem chr = db.CharacterItems.FirstOrDefault(item => item.itemName == name);

                    //Letting SQL know that the slot for the item has changed.
                    chr.slot = index;
                    db.SubmitChanges();

                    //Creating the new item in Character List.
                    characterItemsList[index - 1] = chr;
                }

                //Put the new item into the GUI.
                sortInventory(characterItemsList, index);

            } else
            {
                MessageBox.Show("Your inventory is Full!");
            }
        }

        //Remove item from CharacterItem List, send the infomration to the sortInventory method,
        //and adjust SQL for the modifications.
        public void removeFromCharacterList(string name, decimal price, object sender, EventArgs e)
        {
            if (db != null)
            {
                CharacterItem sell = db.CharacterItems.FirstOrDefault(item => item.itemName == name);

                if (sell != null)
                {
                    int index = sell.slot;

                    players = player.getPlayers();
                    players[0].Currency += price;

                    if (characterItemsList[index - 1] != null)
                    {
                        characterItemsList[index - 1] = characterItem;
                    }

                    db.CharacterItems.DeleteOnSubmit(sell);
                    db.SubmitChanges();
                }
            }
        }

        //This sorts the inventory List array for CharacterItem by the their current slot to
        //index numbers.  This must be sorted first by slot number.
        public void sortInventory(List<CharacterItem> sortListArray, int index)
        {
            foreach (var result in sortListArray)
            {
                switch (result.slot)
                {
                    case 1:
                        if (characterItemsList[0].quantity > 0)
                        {
                            picSlot1.Image = convertToImage(characterItemsList[0].slot - 1);
                        } else
                        {
                            picSlot1.Image = emptyImage;
                        }
                        break;

                    case 2:
                        if (characterItemsList[1].quantity > 0)
                        {
                            picSlot2.Image = convertToImage(characterItemsList[1].slot - 1);
                        }
                        else
                        {
                            picSlot2.Image = emptyImage;
                        }
                        break;

                    case 3:
                        if (characterItemsList[2].quantity > 0)
                        {
                            picSlot3.Image = convertToImage(characterItemsList[2].slot - 1);
                        }
                        else
                        {
                            picSlot3.Image = emptyImage;
                        }
                        break;

                    case 4:
                        if (characterItemsList[3].quantity > 0)
                        {
                           picSlot4.Image = convertToImage(characterItemsList[3].slot - 1);
                        }
                        else
                        {
                            picSlot4.Image = emptyImage;
                        }
                        break;

                    case 5:
                        if (characterItemsList[4].quantity > 0)
                        {
                            picSlot5.Image = convertToImage(characterItemsList[4].slot - 1);
                        }
                        else
                        {
                            picSlot5.Image = emptyImage;
                        }
                        break;

                    case 6:
                        if (characterItemsList[5].quantity > 0)
                        {
                            picSlot6.Image = convertToImage(characterItemsList[5].slot - 1);
                        }
                        else
                        {
                            picSlot6.Image = emptyImage;
                        }
                        break;

                    case 7:
                        if (characterItemsList[6].quantity > 0)
                        {
                            picSlot7.Image = convertToImage(characterItemsList[6].slot - 1);
                        }
                        else
                        {
                            picSlot7.Image = emptyImage;
                        }
                        break;

                    case 8:
                        if (characterItemsList[7].quantity > 0)
                        {
                            picSlot8.Image = convertToImage(characterItemsList[7].slot - 1);
                        }
                        else
                        {
                            picSlot8.Image = emptyImage;
                        }
                        break;

                    case 9:
                        if (characterItemsList[8].quantity > 0)
                        {
                            picSlot9.Image = convertToImage(characterItemsList[8].slot - 1);
                        }
                        else
                        {
                            picSlot9.Image = emptyImage;
                        }
                        break;

                    case 10:
                        if (characterItemsList[9].quantity > 0)
                        {
                            picSlot10.Image = convertToImage(characterItemsList[9].slot - 1);
                        }
                        else
                        {
                            picSlot10.Image = emptyImage;
                        }
                        break;

                    case 11:
                        if (characterItemsList[10].quantity > 0)
                        {
                            picSlot11.Image = convertToImage(characterItemsList[10].slot - 1);
                        }
                        else
                        {
                            picSlot11.Image = emptyImage;
                        }
                        break;

                    case 12:
                        if (characterItemsList[11].quantity > 0)
                        {
                            picSlot12.Image = convertToImage(characterItemsList[11].slot - 1);
                        }
                        else
                        {
                            picSlot12.Image = emptyImage;
                        }
                        break;

                    case 13:
                        if (characterItemsList[12].quantity > 0)
                        {
                            picSlot13.Image = convertToImage(characterItemsList[12].slot - 1);
                        }
                        else
                        {
                            picSlot13.Image = emptyImage;
                        }
                        break;

                    case 14:
                        if (characterItemsList[13].quantity > 0)
                        {
                            picSlot14.Image = convertToImage(characterItemsList[13].slot - 1);
                        }
                        else
                        {
                            picSlot14.Image = emptyImage;
                        }
                        break;

                    case 15:
                        if (characterItemsList[14].quantity > 0)
                        {
                            picSlot15.Image = convertToImage(characterItemsList[14].slot - 1);
                        }
                        else
                        {
                            picSlot15.Image = emptyImage;
                        }
                        break;

                    case 16:
                        if (characterItemsList[15].quantity > 0)
                        {
                            picSlot16.Image = convertToImage(characterItemsList[15].slot - 1);
                        }
                        else
                        {
                            picSlot16.Image = emptyImage;
                        }
                        break;

                    case 17:
                        if (characterItemsList[16].quantity > 0)
                        {
                            picSlot17.Image = convertToImage(characterItemsList[16].slot - 1);
                        }
                        else
                        {
                            picSlot17.Image = emptyImage;
                        }
                        break;

                    case 18:
                        if (characterItemsList[17].quantity > 0)
                        {
                            picSlot18.Image = convertToImage(characterItemsList[17].slot - 1);
                        }
                        else
                        {
                            picSlot18.Image = emptyImage;
                        }
                        break;

                    case 19:
                        if (characterItemsList[18].quantity > 0)
                        {
                            picSlot19.Image = convertToImage(characterItemsList[18].slot - 1);
                        }
                        else
                        {
                            picSlot19.Image = emptyImage;
                        }
                        break;

                    case 20:
                        if (characterItemsList[19].quantity > 0)
                        {
                            picSlot20.Image = convertToImage(characterItemsList[19].slot - 1);
                        }
                        else
                        {
                            picSlot20.Image = emptyImage;
                        }
                        break;

                    case 21:
                        if (characterItemsList[20].quantity > 0)
                        {
                            picSlot21.Image = convertToImage(characterItemsList[20].slot - 1);
                        }
                        else
                        {
                            picSlot21.Image = emptyImage;
                        }
                        break;

                    case 22:
                        if (characterItemsList[21].quantity > 0)
                        {
                            picSlot22.Image = convertToImage(characterItemsList[21].slot - 1);
                        }
                        else
                        {
                            picSlot22.Image = emptyImage;
                        }
                        break;

                    case 23:
                        if (characterItemsList[22].quantity > 0)
                        {
                            picSlot23.Image = convertToImage(characterItemsList[22].slot - 1);
                        }
                        else
                        {
                            picSlot23.Image = emptyImage;
                        }
                        break;

                    case 24:
                        if (characterItemsList[23].quantity > 0)
                        {
                            picSlot24.Image = convertToImage(characterItemsList[23].slot - 1);
                        }
                        else
                        {
                            picSlot24.Image = emptyImage;
                        }
                        break;

                    default:
                        break;
                }
            }
        }//**      End of SortList Method      **//

        //Returns the value of the CharacterItemsList on request.
        public List<CharacterItem> getCharacterList()
        {
            return characterItemsList;
        }

        //This is the generic method for processing the MouseDown Event.  It determines whether
        //the user is moving to or from the pictureBox.  It then initiallizes the MouseEnter
        //events.
        private void picSlot_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int slotLength = ((PictureBox)sender).Name.Length;

                if (slotLength == 8)
                {
                    previousPicture = Int32.Parse(((PictureBox)sender).Name.Substring(7, 1));
                } else
                {
                    previousPicture = Int32.Parse(((PictureBox)sender).Name.Substring(7, 2));
                }
                
                previousImage = ((PictureBox)sender).Image;

                this.picSlot1.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot2.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot3.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot4.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot5.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot6.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot7.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot8.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot9.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot10.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot11.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot12.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot13.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot14.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot15.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot16.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot17.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot18.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot19.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot20.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot21.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot22.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot23.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
                this.picSlot24.MouseEnter += new System.EventHandler(this.picSlot_MouseEnter);
            }   
        }

        //To indicate when the user enters a picture box.  It determines if the picture is
        //comming or leaving and assigns a value to it.
        private void picSlot_MouseEnter(object sender, EventArgs e)
        {
            int slotLength = ((PictureBox)sender).Name.Length;

            //currentPicture saves the second slot position to be swapped.
            if (slotLength == 8)
            {
                currentPicture = Int32.Parse(((PictureBox)sender).Name.Substring(7, 1));
            }
            else
            {
                currentPicture = Int32.Parse(((PictureBox)sender).Name.Substring(7, 2));
            }

            //Saves the image of the second picture to be moved.
            currentImage = ((PictureBox)sender).Image;

            this.picSlot1.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot2.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot3.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot4.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot5.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot6.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot7.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot8.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot9.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot10.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot11.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot12.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot13.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot14.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot15.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot16.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot17.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot18.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot19.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot20.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot21.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot22.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot23.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot24.MouseEnter -= new System.EventHandler(this.picSlot_MouseEnter);

            //Initializes the swap method, collects all needed information before it sends the
            //information to the swap method.
            swapPicture(previousPicture, currentPicture, previousImage, currentImage);
        }

        //This method swaps/switches list objects and their pictures with each other durring the user's
        //mouse events.  This is also run-time swap.
        private void swapPicture(int pp, int cp, Image pi, Image ci)
        {
            switch (previousPicture)
            {
                case 1:
                    picSlot1.Image = ci;
                    break;

                case 2:
                    picSlot2.Image = ci;
                    break;

                case 3:
                    picSlot3.Image = ci;
                    break;

                case 4:
                    picSlot4.Image = ci;
                    break;

                case 5:
                    picSlot5.Image = ci;
                    break;

                case 6:
                    picSlot6.Image = ci;
                    break;

                case 7:
                    picSlot7.Image = ci;
                    break;

                case 8:
                    picSlot8.Image = ci;
                    break;

                case 9:
                    picSlot9.Image = ci;
                    break;

                case 10:
                    picSlot10.Image = ci;
                    break;

                case 11:
                    picSlot11.Image = ci;
                    break;

                case 12:
                    picSlot12.Image = ci;
                    break;

                case 13:
                    picSlot13.Image = ci;
                    break;

                case 14:
                    picSlot14.Image = ci;
                    break;

                case 15:
                    picSlot15.Image = ci;
                    break;

                case 16:
                    picSlot16.Image = ci;
                    break;

                case 17:
                    picSlot17.Image = ci;
                    break;

                case 18:
                    picSlot18.Image = ci;
                    break;

                case 19:
                    picSlot19.Image = ci;
                    break;

                case 20:
                    picSlot20.Image = ci;
                    break;

                case 21:
                    picSlot21.Image = ci;
                    break;

                case 22:
                    picSlot22.Image = ci;
                    break;

                case 23:
                    picSlot23.Image = ci;
                    break;

                case 24:
                    picSlot24.Image = ci;
                    break;

                default:
                    break;
            }

            switch (currentPicture)
            {
                case 1:
                    picSlot1.Image = pi;
                    break;

                case 2:
                    picSlot2.Image = pi;
                    break;

                case 3:
                    picSlot3.Image = pi;
                    break;

                case 4:
                    picSlot4.Image = pi;
                    break;

                case 5:
                    picSlot5.Image = pi;
                    break;

                case 6:
                    picSlot6.Image = pi;
                    break;

                case 7:
                    picSlot7.Image = pi;
                    break;

                case 8:
                    picSlot8.Image = pi;
                    break;

                case 9:
                    picSlot9.Image = pi;
                    break;

                case 10:
                    picSlot10.Image = pi;
                    break;

                case 11:
                    picSlot11.Image = pi;
                    break;

                case 12:
                    picSlot12.Image = pi;
                    break;

                case 13:
                    picSlot13.Image = pi;
                    break;

                case 14:
                    picSlot14.Image = pi;
                    break;

                case 15:
                    picSlot15.Image = pi;
                    break;

                case 16:
                    picSlot16.Image = pi;
                    break;

                case 17:
                    picSlot17.Image = pi;
                    break;

                case 18:
                    picSlot18.Image = pi;
                    break;

                case 19:
                    picSlot19.Image = pi;
                    break;

                case 20:
                    picSlot20.Image = pi;
                    break;

                case 21:
                    picSlot21.Image = pi;
                    break;

                case 22:
                    picSlot22.Image = pi;
                    break;

                case 23:
                    picSlot23.Image = pi;
                    break;

                case 24:
                    picSlot24.Image = pi;
                    break;

                default:
                    break;
            }

            //This is supposed to get the row object in the characterItem table and changed the slot # to match the new
            //swapped slot.
            CharacterItem cha1 = db.CharacterItems.FirstOrDefault(item => item.slot == pp);
            CharacterItem cha2 = db.CharacterItems.FirstOrDefault(item => item.slot == cp);

            //This is the swap routine for the list array rows.
            if (cha1 != null && cha1.quantity != 0)
            {
                cha1.slot = cp;
                if (cha2 != null)
                {
                    cha2.slot = pp;
                }

                db.SubmitChanges();
                CharacterItem holder = characterItemsList[cp - 1];
                characterItemsList[cp - 1] = characterItemsList[pp - 1];
                characterItemsList[pp - 1] = holder;
            }

            //Follows up with sorting the images per picturebox container.
            //sortInventory(characterItemsList, pp);

            //This is the most logical place to collect the Character's Items combined bonuses/stats.
            showStats(characterItemsList);
        }

        //This method fills the shown stats for anything that is in an "Equiped slot," that is
        //anything that is not in the "bag" slots on the lower right corner of the GUI.
        private void showStats(List<CharacterItem> statList)
        {
            int armor = 0;
            int attack = 0;

            foreach(var result in statList)
            {
                if (result.slot > 8)
                {
                    armor += result.armorClass;
                    attack += result.attackValue;
                }
            }

            lblAttack.Text = attack.ToString();
            lblArmorClass.Text = armor.ToString();
        }

        //To open the store window.  Unfortunately I had to get desperate and use the Application.Restart()
        //to get the images to appear or dissappear durring run time.  Otherwise, they would only move with
        //external events.  I did eventually upgrade this to some extent as to not to have to use the Application
        //restart, but the cost was to seperate the start only items from the refresh statements in form load.
        private void btnStore_Click(object sender, EventArgs e)
        {
            frmStore store = new frmStore();
            store.ShowDialog();
            frmMain mainForm = new frmMain();          
            mainForm.ShowDialog();           
            this.Hide();
        }

        //These are all instantiated to keep the mouse down event from interferring with
        //other click events.
        private void picSlot1_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot2_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot3_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot4_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot5_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot6_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot7_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot8_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot9_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot10_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot11_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot12_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot13_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot14_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot15_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot16_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot17_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot18_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot19_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot20_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot21_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot22_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot23_Click(object sender, EventArgs e)
        {
            //
        }

        private void picSlot24_Click(object sender, EventArgs e)
        {
            //
        }

        public void initializeEvents()
        {
            //Mouse events to avoid the double click event from triggering or to pass it
            //as a MouseEnter event.
            this.picSlot1.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot2.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot3.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot4.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot5.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot6.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot7.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot8.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot9.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot10.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot11.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot12.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot13.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot14.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot15.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot16.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot17.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot18.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot19.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot20.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot21.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot22.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot23.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);
            this.picSlot24.DoubleClick += new System.EventHandler(this.picSlot_MouseEnter);

            //Mouse Events specifically for what picture is clicked on.
            this.picSlot1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot11.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot12.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot13.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot14.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot15.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot16.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot17.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot18.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot19.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot20.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot21.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot22.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot23.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
            this.picSlot24.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picSlot_MouseDown);
        }

        //Closes the application.
        private void btnExit_Click(object sender, EventArgs e)
        {
            frmMain mainForm = new frmMain();
            this.Close();
            mainForm.Close();
           
            Application.Exit();
        }
    }
}