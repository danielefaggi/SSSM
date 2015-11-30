//
// SSSM - 2015 - Daniele Faggi
//

//
// Notes on implementation of SSSM
// ===============================
//
// The documentation doesn't state where to place the price of a stock. Probably the best place is a field
// with public accessors and allow the user to change it (see the LastPrice property).
//
// In this implementation, instead of handling exceptions, I preferred to use float.NaN values to handle
// numeric errors inside methods (it's cleaner, faster -at least in .Net- and compatible with output and 
// string formatting).
// Eventually it's up to the caller to decide if an exception should be thrown when a float.NaN is detected
// (even if it lose a bit of information on the root causes of the problem: divide by 0 ? "Overflow" ? 
// wrong parameter ? etc.).
//
// The parameters and variables aren't extensively controlled if they are inside the allowable range.  
//
// All the prices are expressed as float numbers, quantities as int numbers.
//
// I hope you like it.
//
using System;

namespace SSSM
{
    /// <summary>
    /// Main Class of the Program 
    /// </summary>
    class Program
    {
        #region Fields

        private StockCollection Stocks;         // Main collection of stocks
        #endregion


        #region Constructors/Finalizers

        Program()
        {
            // Initialize collection
            Stocks = new StockCollection();

            // Populate "database"
            Stocks.Add(new CommonStock("TEA", 0, 100));
            Stocks.Add(new CommonStock("POP", 8, 100));
            Stocks.Add(new CommonStock("ALE", 23, 60));
            Stocks.Add(new PreferredStock("GIN", 8, .02f, 100));
            Stocks.Add(new CommonStock("JOE", 13, 250));

        }
        #endregion

        #region Operations

        /// <summary>
        /// Show to the user the list of stocks in memory
        /// </summary>
        private void PrintoutStocks()
        {

            Console.WriteLine("Stock in memory:");

            Console.WriteLine("Symbol\tType      \tL. Div\tF. Div\tPar V.\tPrice\tTrades");

            foreach (GenericStock stock in Stocks)
            {

                if(stock is CommonStock)
                {
                    Console.WriteLine("{0,3}\tCommon   \t{1:#0}\t\t{2:#0}\t{3:#0}\t{4:#0}", 
                        stock.Symbol, stock.LastDividend, stock.ParValue, stock.LastPrice, stock.Trades.Count);
                }
                else if(stock is PreferredStock)
                {
                    Console.WriteLine("{0,3}\tPreferred\t{1:#0}\t{2:#0}%\t{3:#0}\t{4:#0}\t{5:#0}", 
                        stock.Symbol, stock.LastDividend, ((PreferredStock)stock).FixedDividend*100, stock.ParValue,
                        stock.LastPrice, stock.Trades.Count);
                }
                else
                {
                    throw new NotImplementedException("Unknown stock type");
                }
            }

        }

        /// <summary>
        /// Accepts input from the user
        /// </summary>
        private void MainMenu()
        {
            bool exit = false;

            do
            {
                switch (QueryUser())
                {
                    case "0":
                        PrintoutStocks();
                        break;
                    case "1":
                        CalcYield();
                        break;
                    case "2":
                        CalcPE();
                        break;
                    case "3":
                        RecordATrade();
                        break;
                    case "4":
                        CalcAllShares();
                        break;
                    case "5":
                        ShowTrades();
                        break;
                    case "6":
                        SetLastPrice();
                        break;
                    case "q":
                    case "Q":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            } while (!exit);
        }

        /// <summary>
        /// Shows to the user the possible actions to be performed and waits for a choice
        /// </summary>
        /// <returns></returns>
        private string QueryUser()
        {

            Console.WriteLine("Choices:");
            Console.WriteLine("0 - Display Stock List");
            Console.WriteLine("1 - Calculate Yield");
            Console.WriteLine("2 - Calculate P/E");
            Console.WriteLine("3 - Record a Trade");
            Console.WriteLine("4 - Calculate All Share Index");
            Console.WriteLine("5 - Show trades on a Stock (and Volume Weighted Price within last 15 min)");
            Console.WriteLine("6 - Set Last Price for a Stock");
            Console.WriteLine("q - Quit");
            Console.Write(">");

            string line = Console.ReadLine();

            return line;
        }

        /// <summary>
        /// Performs a Yield calculation on a stock selected by the user
        /// </summary>
        private void CalcYield()
        {
            Console.WriteLine("Calculate Yield of which Symbol ?");
            string symbol = Console.ReadLine();

            bool found = false;
            foreach(GenericStock stock in Stocks)
            {
                if(stock.Symbol == symbol.ToUpper())
                {
                    found = true;

                    Console.WriteLine("At which price ? (or leave empty to use stock last price)");
                    string sprice = Console.ReadLine();
                    if (sprice.Trim() == string.Empty)
                    {
                        Console.WriteLine("Symbol: {0,3}\tYield:{1:#0.00}", stock.Symbol, stock.GetDividendYield());
                    }
                    else
                    {
                        float price = float.NaN;
                        if (float.TryParse(sprice, out price))
                        {
                            Console.WriteLine("Symbol: {0,3}\tYield:{1:#0.00}", stock.Symbol, stock.GetDividendYield(price));
                        }
                        else
                        {
                            Console.WriteLine("Invalid number!");
                        }
                    }
                }
            }

            if (!found) Console.WriteLine("Symbol not found!");
        }

        /// <summary>
        /// Performs a P/E calculation on a stock selected by the user
        /// </summary>
        private void CalcPE()
        {
            Console.WriteLine("Calculate PE of which Symbol ?");
            string symbol = Console.ReadLine();

            bool found = false;
            foreach (GenericStock stock in Stocks)
            {
                if (stock.Symbol == symbol.ToUpper())
                {
                    found = true;

                    Console.WriteLine("At which price ? (or leave empty to use stock last price)");
                    string sprice = Console.ReadLine();
                    if (sprice.Trim() == string.Empty)
                    {
                        Console.WriteLine("Symbol: {0,3}\tP/E:{1:#0.00}", stock.Symbol, stock.GetPERatio());
                    }
                    else
                    {
                        float price = float.NaN;
                        if (float.TryParse(sprice, out price))
                        {
                            Console.WriteLine("Symbol: {0,3}\tP/E:{1:#0.00}", stock.Symbol, stock.GetPERatio(price));
                        }
                        else
                        {
                            Console.WriteLine("Invalid number!");
                        }
                    }
                }
            }

            if (!found) Console.WriteLine("Symbol not found!");
        }

        /// <summary>
        /// Set Last Price of a Stock
        /// </summary>
        private void SetLastPrice()
        {
            Console.WriteLine("Set Last Price related to which Symbol ?");
            string symbol = Console.ReadLine();

            GenericStock workingStock = null;

            foreach (GenericStock stock in Stocks)
            {
                if (stock.Symbol == symbol.ToUpper())
                    workingStock = stock;
            }

            if (workingStock == null)
            {
                Console.WriteLine("Symbol not found!");
                return;
            }

            Console.WriteLine("Insert last Price:");
            string sprice = Console.ReadLine();

            float price;

            if(!float.TryParse(sprice, out price))
            {
                Console.WriteLine("Invalid numeric value");
                return;
            }

            workingStock.LastPrice = price;
        }

        /// <summary>
        /// Method to record a trade
        /// </summary>
        private void RecordATrade()
        {
            Console.WriteLine("Record a trade related to which Symbol ?");
            string symbol = Console.ReadLine();

            GenericStock workingStock = null;

            foreach (GenericStock stock in Stocks)
            {
                if (stock.Symbol == symbol.ToUpper())
                    workingStock = stock;
            }

            if(workingStock == null)
            {
                Console.WriteLine("Symbol not found!");
                return;
            }

            Console.WriteLine("Related to which date/time ?");
            string stimestamp = Console.ReadLine();

            DateTime timestamp = DateTime.Now;
            if(!DateTime.TryParse(stimestamp, out timestamp))
            {
                Console.WriteLine("Invalid date/time");
                return;
            }

            Console.WriteLine("B - Buy or S - Sell ?");
            string sbs = Console.ReadLine();

            TRADE_SIGN sign;
            if(sbs.ToUpper() == "B")
            {
                sign = TRADE_SIGN.Buy;
            }
            else if(sbs.ToUpper() == "S")
            {
                sign = TRADE_SIGN.Sell;
            }
            else
            {
                Console.WriteLine("Invalid Operation!");
                return;
            }

            Console.WriteLine("Quantity ?");
            string sqty = Console.ReadLine();

            int qty;
            if (!int.TryParse(sqty, out qty))
            {
                Console.WriteLine("Invalid Quantity!");
                return;
            }

            Console.WriteLine("Price (in pennies) ?");
            string sprice = Console.ReadLine();

            float price;
            if (!float.TryParse(sprice, out price))
            {
                Console.WriteLine("Invalid price!");
                return;
            }

            Trade trade = new Trade(timestamp, qty, sign, price);
            PrintTrade(trade);
            Console.WriteLine("Add to Stock {0} ? (Y to confirm or any other input to cancel)", workingStock.Symbol);
            string answer = Console.ReadLine();
            if(answer.ToUpper() == "Y")
            {
                workingStock.Trades.Add(trade);
            }
            else 
            {
                Console.WriteLine("Operation cancelled!");
            }
            
        }

        /// <summary>
        /// Method to show the trade list of a stock
        /// </summary>
        private void ShowTrades()
        {

            Console.WriteLine("Show trades related to which Symbol ?");
            string symbol = Console.ReadLine();

            GenericStock workingStock = null;

            foreach (GenericStock stock in Stocks)
            {
                if (stock.Symbol == symbol.ToUpper())
                    workingStock = stock;
            }

            if (workingStock == null)
            {
                Console.WriteLine("Symbol not found!");
                return;
            }

            if (workingStock.Trades.Count > 0)
            {
                Console.WriteLine("{0,20}\tB/S\tQty\tPrice", "Timestamp");
                foreach (Trade trade in workingStock.Trades)
                {
                    PrintTrade(trade);
                }
            }
            else
            {
                Console.WriteLine("No trades recorded!");
            }

            Console.WriteLine("Last 15 minutes Volume Weighted Price: {0:#0.00}",
                workingStock.Trades.GetVolumeWeightedPrice(new TimeSpan(0, 15, 0)));
        }

        /// <summary>
        /// Shows a trade on screen
        /// </summary>
        /// <param name="trade"></param>
        private void PrintTrade(Trade trade)
        {
            Console.WriteLine("{0,20:g}\t{1}\t{2}\t{3}", trade.Timestamp,
                trade.Sign == TRADE_SIGN.Buy ? "Buy" : "Sell", trade.Quantity, trade.TradedPrice);
        }

        /// <summary>
        /// Shows the All Share index
        /// </summary>
        private void CalcAllShares()
        {
            float allshares = Stocks.GetGeometricMean();

            if (float.IsNaN(allshares))
            {
                Console.WriteLine("Some stocks don't have last price!");
            }
            else
            {
                Console.WriteLine("AllShare Index, based on last prices, is {0:#0.00}", allshares);
            }
        }
        #endregion

        #region Entry Point

        /// <summary>
        /// Main entry Point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Program p = new Program();      // Main class instancing

            p.MainMenu();                   // Start from main menu

        }
        #endregion

    }
}
