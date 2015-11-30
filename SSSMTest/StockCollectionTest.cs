//
// SSSM - 2015 - Daniele Faggi
//

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SSSM;

namespace SSSMTest
{
    [TestClass]
    public class StockCollectionTest
    {
        [TestMethod]
        public void TestStockCollectionGeometricMean()
        {
            StockCollection stocks = new StockCollection();

            GenericStock stock1 = new CommonStock("TS1", 5, 100);
            GenericStock stock2 = new CommonStock("TS2", 10, 100);
            GenericStock stock3 = new CommonStock("TS3", 15, 100);
            stocks.Add(stock1);
            stocks.Add(stock2);
            stocks.Add(stock3);

            float gm;

            // If no any prices => no result
            gm = stocks.GetGeometricMean();
            Assert.AreEqual(float.NaN, gm);

            float price1 = 110;
            float price2 = 50;
            float price3 = 10;

            stock1.LastPrice = price1;
            stock2.LastPrice = price2;

            // If there is at least one stock w/o price => no result
            gm = stocks.GetGeometricMean();
            Assert.AreEqual(float.NaN, gm);

            stock3.LastPrice = price3;

            // result is the 3-sq of price1*price2*price3
            gm = stocks.GetGeometricMean();
            float mul = 1.0f;
            mul *= price1;
            mul *= price2;
            mul *= price3;
            float nsq = 1f / 3;
            Assert.AreEqual(Math.Pow(mul, nsq), gm, 0.0001f);

        }
    }
}
