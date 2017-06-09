using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod()]
        public void PlayerTest()
        {
            Player player = new Player("Warrior", 0, 100, 1, 100, 100, 1, 5, 10, 10, 5, 3, "Willem", 1, 120, 100);
            Assert.AreEqual(player.Classes, "Warrior", "Classes");
            Assert.AreEqual(player.Experience, 0 , "Experience");
            Assert.AreEqual(player.Gold, 100, "Gold");
            Assert.AreEqual(player.Id, 1, "Id");
            Assert.AreEqual(player.Health, 100, "Health");
            Assert.AreEqual(player.Mana, 100, "Mana");
            Assert.AreEqual(player.HealthRegen, 1, "HealthRegen");
            Assert.AreEqual(player.ManaRegen, 5, "Manaregen");
            Assert.AreEqual(player.Attack, 10, "Attack");
            Assert.AreEqual(player.Defence, 10, "Defence");
            Assert.AreEqual(player.DodgeChance, 5, "Dodgechance");
            Assert.AreEqual(player.CritChance, 3, "Critchance");
            Assert.AreEqual(player.Name, "Willem", "Name");
            Assert.AreEqual(player.Level, 1, "level");
            Assert.AreEqual(player.MaxHealth, 120, "MaxHealth");
            Assert.AreEqual(player.MaxMana, 100, "Maxmana");

            Player player2 = new Player("Warrior", "Frank", 1);
            Assert.AreEqual(player2.Classes, "Warrior", "Classes2");
            Assert.AreEqual(player2.Name, "Frank", "Name2");
            Assert.AreEqual(player2.Id, 1, "Id2");
        }
    }
}