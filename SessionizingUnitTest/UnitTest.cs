using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Sessionizing;
using Sessionizing.QueryExcecutors;
using System;

namespace UnitTestSessionizing
{
    [TestClass]
    public class UnitTest
    {
        static string csvsDirectory = Directory.GetCurrentDirectory() + "\\csvs";
        string threeColumnsCsvDir = csvsDirectory + "\\three_columns";
        string fiveColumnsCsvDir = csvsDirectory + "\\five_columns";
        string emptyFieldCsvDir = csvsDirectory + "\\empty_field";
        string validCsvDir = csvsDirectory + "\\valid";
        string emptyDir = csvsDirectory + "\\empty";
        string badTimestampCsvDir = csvsDirectory + "\\bad_timestamp";
        string originalCsvDataDir = csvsDirectory + "\\original_valid_csvs";

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ThreeColumnsCsv()
        {
            new DataManager(threeColumnsCsvDir);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FiveColumnsCsv()
        {
            new DataManager(fiveColumnsCsvDir);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void EmptyFieldCsv()
        {
            new DataManager(emptyFieldCsvDir);
        }

        [TestMethod]
        public void ValidCsv()
        {
            new DataManager(validCsvDir);
            // doesn't throw an exception
        }

        [TestMethod]
        public void NoCsvFileFound()
        {
            DataManager dataManager = new DataManager(emptyDir);
            Assert.IsTrue(dataManager.IsDataEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void BadTimeStamp()
        {
            new DataManager(badTimestampCsvDir);
        }

        [TestMethod]
        public void TestExpectedResultNumOfSessions()
        {
            NumOfSessionsExcecutor excecutor = new NumOfSessionsExcecutor();
            DataManager dataManager = new DataManager(originalCsvDataDir);
            string s1 = "www.s_1.com";
            double actual, expected = 3684;
            actual = excecutor.ExcecuteQuery(dataManager.Data, s1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestExpectedResultMedianSessionLength()
        {
            MedianSessionLengthExcecutor excecutor = new MedianSessionLengthExcecutor();
            DataManager dataManager = new DataManager(originalCsvDataDir);
            string s1 = "www.s_1.com";
            double actual, expected = 1353;
            actual = excecutor.ExcecuteQuery(dataManager.Data, s1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestExpectedResultNumOfUniqueVisitedSites()
        {
            NumOfUniqueVisitedSitesExcecutor excecutor = new NumOfUniqueVisitedSitesExcecutor();
            DataManager dataManager = new DataManager(originalCsvDataDir);
            string visitor1 = "visitor_1";
            double actual, expected = 3;
            actual = excecutor.ExcecuteQuery(dataManager.Data, visitor1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSiteMissingMedianSessionLength()
        {
            MedianSessionLengthExcecutor excecutor = new MedianSessionLengthExcecutor();
            DataManager dataManager = new DataManager(originalCsvDataDir);
            string abc = "www.abc.com";
            bool isQueryLegal = excecutor.IsQueryInputLegal(dataManager.Data, abc);
            Assert.IsFalse(isQueryLegal);
        }
    }
}
