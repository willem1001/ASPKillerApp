using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.MagicTests
{
    [TestClass()]
    public class MagicTests
    {
        [TestMethod()]
        public void MagicTest()
        {
            Magic magic = new Magic(1, "Heal", 0, 100, 30);
            Assert.AreEqual(magic.Id, 1, "Id");
            Assert.AreEqual(magic.Name, "Heal", "Name");
            Assert.AreEqual(magic.Damage, 0, "Damage");
            Assert.AreEqual(magic.HealthRestore, 100, "Heal");
            Assert.AreEqual(magic.Manacost, 30, "Manacost");
        }
    }
}