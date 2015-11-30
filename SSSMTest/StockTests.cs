//
// SSSM - 2015 - Daniele Faggi
//

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SSSM;

namespace SSSMTest
{
    [TestClass]
    public class StockTests
    {
        [TestMethod]
        public void TestCommonStockYield()
        {
            // Test Common Stock

            CommonStock stock; 
                
            stock = new CommonStock("TST", 0, 100);

            float yield;
            
            // Test 0/50 => 0
            yield = stock.GetDividendYield(50);
            Assert.AreEqual(0, yield, float.Epsilon);

            // Test 10/10 => 1
            stock.LastDividend = 10;
            yield = stock.GetDividendYield(10);
            Assert.AreEqual(1, yield, float.Epsilon);

            // Test 10/0 => NaN
            yield = stock.GetDividendYield(0);
            Assert.AreEqual(float.NaN, yield);

            // LastPrice

            stock = new CommonStock("TST", 0, 100);

            // Test 0/50 => 0
            stock.LastPrice = 50;
            yield = stock.GetDividendYield();
            Assert.AreEqual(0, yield, float.Epsilon);

            // Test 10/10 => 1
            stock.LastPrice = 10;
            stock.LastDividend = 10;
            yield = stock.GetDividendYield();
            Assert.AreEqual(1, yield, float.Epsilon);

            // Test 10/0 => NaN
            stock.LastPrice = 0;
            yield = stock.GetDividendYield();
            Assert.AreEqual(float.NaN, yield);


        }

        [TestMethod]
        public void TestCommonStockPERatio()
        {
            CommonStock stock;

            stock = new CommonStock("TST", 10, 100);

            float pe;

            // Test 0/10 => 0
            pe = stock.GetPERatio(0);
            Assert.AreEqual(0, pe, float.Epsilon);

            // Test 10/10 => 1
            pe = stock.GetPERatio(10);
            Assert.AreEqual(1, pe, float.Epsilon);

            // Test 10/0 => NaN
            stock.LastDividend = 0;
            pe = stock.GetPERatio(10);
            Assert.AreEqual(float.NaN, pe);

            // LastPrice
            stock = new CommonStock("TST", 10, 100);

            // Test 0/10 => 0
            stock.LastPrice = 0;
            pe = stock.GetPERatio();
            Assert.AreEqual(0, pe, float.Epsilon);

            // Test 10/10 => 1
            stock.LastPrice = 10;
            pe = stock.GetPERatio();
            Assert.AreEqual(1, pe, float.Epsilon);

            // Test 10/0 => NaN
            stock.LastPrice = 10;
            stock.LastDividend = 0;
            pe = stock.GetPERatio();
            Assert.AreEqual(float.NaN, pe);

        }

        [TestMethod]
        public void TestPreferredStockYield()
        {
            // Test Preferred Stock

            PreferredStock stock;

            stock = new PreferredStock("TST", 10, 0, 100);

            float yield;

            // Test 0*100/50 => 0
            yield = stock.GetDividendYield(50);
            Assert.AreEqual(0, yield, float.Epsilon);

            // Test 0.1*100/10 => 1
            stock.FixedDividend = 0.1f;
            yield = stock.GetDividendYield(10);
            Assert.AreEqual(1, yield, float.Epsilon);

            // Test 0.1*100/0 => NaN
            yield = stock.GetDividendYield(0);
            Assert.AreEqual(float.NaN, yield);

            // LastPrice
            stock = new PreferredStock("TST", 10, 0, 100);

            // Test 0*100/50 => 0
            stock.LastPrice = 50;
            yield = stock.GetDividendYield();
            Assert.AreEqual(0, yield, float.Epsilon);

            // Test 0.1*100/10 => 1
            stock.LastPrice = 10;
            stock.FixedDividend = 0.1f;
            yield = stock.GetDividendYield();
            Assert.AreEqual(1, yield, float.Epsilon);

            // Test 0.1*100/0 => NaN
            stock.LastPrice = 0;
            yield = stock.GetDividendYield();
            Assert.AreEqual(float.NaN, yield);

        }


        [TestMethod]
        public void TestPreferredStockPERatio()
        {

            // Test Preferred Stock

            PreferredStock stock;

            stock = new PreferredStock("TST", 10, 1, 100);

            float pe;

            // Test 0/10 => 0
            pe = stock.GetPERatio(0);
            Assert.AreEqual(0, pe, float.Epsilon);

            // Test 10/10 => 1
            pe = stock.GetPERatio(10);
            Assert.AreEqual(1, pe, float.Epsilon);

            // Test 10/0 => NaN
            stock.LastDividend = 0;
            pe = stock.GetPERatio(10);
            Assert.AreEqual(float.NaN, pe);

            // LastPrice
            stock = new PreferredStock("TST", 10, 1, 100);

            // Test 0/10 => 0
            stock.LastPrice = 0;
            pe = stock.GetPERatio();
            Assert.AreEqual(0, pe, float.Epsilon);

            // Test 10/10 => 1
            stock.LastPrice = 10;
            pe = stock.GetPERatio();
            Assert.AreEqual(1, pe, float.Epsilon);

            // Test 10/0 => NaN
            stock.LastPrice = 10;
            stock.LastDividend = 0;
            pe = stock.GetPERatio();
            Assert.AreEqual(float.NaN, pe);

        }


    }
}
