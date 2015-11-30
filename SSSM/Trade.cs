//
// SSSM - 2015 - Daniele Faggi
//

using System;

namespace SSSM
{
    /// <summary>
    /// Enumeration of trade operation. A trade can be "Buy" or "Sell".
    /// </summary>
    public enum TRADE_SIGN {Buy, Sell};

    /// <summary>
    /// Class describing a trade. 
    /// </summary>
    public class Trade
    {
        #region Fields

        // Fields of a trade: timestamp, quantity, sell/buy (sign), traded price
        private DateTime m_Timestamp;
        private int m_Quantity;
        private TRADE_SIGN m_Sign;
        private float m_TradedPrice;
        #endregion

        #region Constructors/Finalizers

        // Standard constructor
        public Trade (DateTime Timestamp, int Quantity, TRADE_SIGN Sign, float TradedPrice)
        {
            m_Timestamp = Timestamp;
            m_Quantity = Quantity;
            m_Sign = Sign;
            m_TradedPrice = TradedPrice;
        }
        #endregion

        #region Accessors
        public DateTime Timestamp {
            get { return m_Timestamp; }
            set { m_Timestamp = value; }              
        }
        public int Quantity
        {
            get { return m_Quantity; }
            set { m_Quantity = value; }
        }
        public TRADE_SIGN Sign
        {
            get { return m_Sign; }
            set { m_Sign = value; }
        }
        public float TradedPrice
        {
            get { return m_TradedPrice; }
            set { m_TradedPrice = value; }
        }
        #endregion
    }


}
