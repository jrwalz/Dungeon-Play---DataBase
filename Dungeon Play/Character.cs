using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Play
{
    public class Character
    {
        public static decimal _currency;

        public string Name { get; set; }
        public string Class { get; set; }
        public int Strength { get; set; }
        public int Constitution { get; set; }
        public int Dexterity { get; set; }

        public int HitPoints { get; set; }
        public int Attack { get; set; }
        public int Armor { get; set; }
        public int Endurance { get; set; }
        public int Weight { get; set; }

        public decimal Currency 
        {
            get { return _currency; }
            set { _currency = value; }
        }
        
        //Used to Create a psueudo player for template purposes.
        public static List<Character> players = new List<Character>();

        public Character() {}

        public Character(string nm, string cl, int st, int con, int dex, int hp, int att, int ar, int en, int wt, decimal cur)
        {
            Name = nm;
            Class = cl;
            Strength = st;
            Constitution = con;
            Dexterity = dex;
            HitPoints = hp;
            Attack = att;
            Armor = ar;
            Endurance = en;
            Weight = wt;
            Currency = cur;
        }

        public void firstMethod()
        {
            Character player = new Character();
            player = new Character("Ugh", "Barbarian", 18, 24, 13, 18, 0, 0, 18, 0, 1500 - getTotalCost());
            players.Add(player);
        }

        //Returns the total Cost of the Equipment purchased.
        public decimal getTotalCost()
        {
            CharacterItemDBDataContext db = new CharacterItemDBDataContext();
            var results = from CharacterItem in db.CharacterItems select CharacterItem;

            //Get data from Character Item Database
            results = from characterItem in db.CharacterItems select characterItem;

            decimal totalCost = 0;
            foreach (var result in results)
            {
                if (result.slot > 0 && result.quantity > 0)
                {
                    totalCost += result.price;
                }
            }
            return totalCost;
        }

        public void setCurrency(decimal totalCost)
        {           
            players[0].Currency = totalCost;
        }

        public List<Character> getPlayers()
        {
            return players;
        }
    }
}
