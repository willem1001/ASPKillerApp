using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.ItemTests
{
    [TestClass()]
    public class ItemTests
    {
        [TestMethod()]
        public void ItemTest()
        {
            Item item = new Item(1, "Zwaard", "Weapon", 2, 25, 45);
            Assert.AreEqual(item.Id, 1, "Id");
            Assert.AreEqual(item.Name, "Zwaard", "Name");
            Assert.AreEqual(item.Type, "Weapon");
            Assert.AreEqual(item.Level, 2, "Level");
            Assert.AreEqual(item.GoldValue, 25, "Value");
            Assert.AreEqual(item.GoldCost, 45, "Cost");

            Item item2 = new Item(2, "Schild");
            Assert.AreEqual(item2.Id, 2, "Id2");
            Assert.AreEqual(item2.Name, "Schild", "Name2");

            Item item3 = new Item(3, "Pantser", "Armor", 3, 15, 25);
            Assert.AreEqual(item3.Id, 3, "Id3");
            Assert.AreEqual(item3.Name, "Pantser", "Name3");
            Assert.AreEqual(item3.Type, "Armor", "Type3");
            Assert.AreEqual(item3.Level, 3, "Level3");
            Assert.AreEqual(item3.GoldValue, 15, "Value3");
            Assert.AreEqual(item3.GoldCost, 25, "Cost3");
        }
    }
}