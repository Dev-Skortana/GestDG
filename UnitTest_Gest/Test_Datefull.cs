using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gest.Helpers;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Linq;

namespace UnitTest_Gest
{
    [TestClass]
    public class TestDatefull
    {

        [Ignore]
        private IEnumerable<Date_Part> BuildListDatepart()
        {
            return new List<Date_Part>() 
            {
                new Date_Part(5, 8, 0, 22, 0),
                new Date_Part(5, 6, 11, 10, 1),
                new Date_Part(5, 6, 7, 13, 2),
                new Date_Part(3, 18, 20, 30, 3),
                new Date_Part(1, 8, 21, 14, 4),
                new Date_Part(11, 19, 18, 47, 5),
                new Date_Part(7, 29, 9, 16, 6),
                new Date_Part(5, 23, 21, 35, 7),
                new Date_Part(4, 26, 5, 37, 8),
                new Date_Part(4, 8, 6, 18, 9),
                new Date_Part(3, 18, 7, 15, 10)
            };
        }
        [Ignore]
        private DateTime BuildDateComplet(List<Date_Part> list_dates_part,int index)
        {
            return Datefull.getdate_visite_full(list_dates_part,list_dates_part[index]);
        }
        [TestMethod]  
        public void TestGetDateVisiteFullCaseDateHaveExactly1YearOrMoreThan1Year()
        {
            List<Date_Part> liste_parties_date = BuildListDatepart().ToList();
            DateTime dates_complets_generer=BuildDateComplet(liste_parties_date,8);
            Assert.AreEqual(new DateTime(month: 4, day: 26, year: 2019, hour: 5, minute: 37, second: 0), dates_complets_generer);
        }

        [TestMethod]
        public void TestGetDateVisiteFullCaseDateLessThan1Year()
        {
            List<Date_Part> liste_parties_date = BuildListDatepart().ToList();
            DateTime dates_complets_generer = BuildDateComplet(liste_parties_date, 3);
            Assert.AreEqual(new DateTime(month: 3, day: 18, year: 2020, hour: 20, minute: 30, second: 0), dates_complets_generer);
        }

        [TestMethod]
        public void TestGetDateVisiteFullCaseDateLessThan1ButOnYearDifferent()
        {
            List<Date_Part> liste_parties_date = BuildListDatepart().ToList();
            DateTime dates_complets_generer = BuildDateComplet(liste_parties_date, 5);
            Assert.AreEqual(new DateTime(month: 11, day: 19, year: 2019, hour: 18, minute: 47, second: 0), dates_complets_generer);
        }
    }
}
