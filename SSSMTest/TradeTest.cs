//
// SSSM - 2015 - Daniele Faggi
//

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SSSM;

namespace SSSMTest
{
    [TestClass]
    public class TradeTest
    {
        [TestMethod]
        public void TestTradeCollectionAddTrades()
        {
            // Collection Order Test

            TradeCollection trades = new TradeCollection();

            Trade trade1 = new Trade(new DateTime(2015, 10, 2), 1, TRADE_SIGN.Buy, 100);
            Trade trade2 = new Trade(new DateTime(2015, 10, 3), 1, TRADE_SIGN.Buy, 100);
            Trade trade3 = new Trade(new DateTime(2015, 10, 1), 1, TRADE_SIGN.Buy, 100);
            Trade trade4 = new Trade(new DateTime(2015, 10, 4), 1, TRADE_SIGN.Buy, 100);
            Trade trade5 = new Trade(new DateTime(2015, 09, 1), 1, TRADE_SIGN.Buy, 100);

            trades.Add(trade1);
            trades.Add(trade2);
            trades.Add(trade3);
            trades.Add(trade4);
            trades.Add(trade5);

            Assert.AreEqual(5, trades.Count);

            Assert.AreSame(trades.ElementAt(0), trade4);
            Assert.AreSame(trades.ElementAt(1), trade2);
            Assert.AreSame(trades.ElementAt(2), trade1);
            Assert.AreSame(trades.ElementAt(3), trade3);
            Assert.AreSame(trades.ElementAt(4), trade5);
        }

        [TestMethod]
        public void TestTradeCollectionWeightedMean()
        {
            TradeCollection trades = new TradeCollection();

            TimeSpan span1 = new TimeSpan(0, 5, 0);   // 5 minutes
            TimeSpan span2 = new TimeSpan(0, 10, 0);  // 10 minutes
            TimeSpan span3 = new TimeSpan(0, 20, 0);  // 20 minutes

            float vwp;

            // no trades => no result
            vwp = trades.GetVolumeWeightedPrice(new TimeSpan(0, 15, 0));
            Assert.AreEqual(float.NaN, vwp);

            int qty1 = 10;
            float price1 = 100;
            int qty2 = 5;
            float price2 = 50;

            // Mean based on these
            trades.Add(new Trade(DateTime.Now.Subtract(span1), qty1, TRADE_SIGN.Sell, price1));
            trades.Add(new Trade(DateTime.Now.Subtract(span2), qty2, TRADE_SIGN.Buy, price2));
            trades.Add(new Trade(DateTime.Now.Subtract(span2), qty2, TRADE_SIGN.Sell, price2));
            // This must be excluded
            trades.Add(new Trade(DateTime.Now.Subtract(span3), 100, TRADE_SIGN.Sell, 1000));

            vwp = trades.GetVolumeWeightedPrice(new TimeSpan(0, 15, 0));
            Assert.AreEqual((qty1 * price1 + qty2 * price2 * 2) / (qty1 + qty2 * 2), vwp);

        }
    }
}
