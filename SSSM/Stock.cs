//
// SSSM - 2015 - Daniele Faggi
//

namespace SSSM
{

    /// <summary>
    /// Definition of a generic stock. It describes a base class to hold common stock information.
    /// Add here common elements to all types of stocks handled.
    /// </summary>
    /// <remarks>
    /// It is an abstract class. Instancing may be allowed only in derived classes.
    /// Known subclasses: CommonStock, PreferredStock
    /// </remarks>
    public abstract class GenericStock
    {
        #region Fields
        // Generic fields
        private string m_Symbol;
        private float m_LastDividend;      
        private float m_ParValue;         
        private float m_LastPrice;

        // Collection of trades related to this stock
        private TradeCollection m_Trades;
        #endregion

        #region Constructors/Finalizers

        // Standard constructor
        protected GenericStock(string Symbol, float LastDividend, float ParValue)
        {
            m_Trades = new TradeCollection();
            m_LastPrice = float.NaN;

            m_Symbol = Symbol;
            m_LastDividend = LastDividend;
            m_ParValue = ParValue;
        }
        #endregion

        #region Public Accessors
        public string Symbol
        {
            get { return m_Symbol; }
            set { m_Symbol = value; }
        }

        public float LastDividend
        {
            get { return m_LastDividend; }
            set { m_LastDividend = value; }
        }

        public float ParValue
        {
            get { return m_ParValue; }
            set { m_ParValue = ParValue; }
        }

        public float LastPrice
        {
            get { return m_LastPrice; }
            set { m_LastPrice = value; }
        }

        public TradeCollection Trades
        {
            get { return m_Trades; }
        }
        #endregion

        #region Operations

        /// <summary>
        /// Abstract method to calculate Yield against a supplied Price. Must be implemented in
        /// subclasses in a way which is dependant to the type of stock taken in consideration.
        /// </summary>
        /// <param name="Price"> 
        /// Price against to calculate the Yield value, if isn't provided is used LastPrice value </param>
        /// <returns> The calculated Yield </returns>
        public abstract float GetDividendYield(float Price);
        public abstract float GetDividendYield();
        /// <summary>
        /// Abstract method to calculate P/E Ratio against a supplied Price. Must be implemented in
        /// subclasses in a way which is dependant to the type of stock taken in consideration.
        /// </summary>
        /// <param name="Price"> 
        /// Price against to calculate the P/E Ratio value, if isn't provided is used LastPrice value </param>
        /// <returns> The calculated P/E Ratio </returns>
        public abstract float GetPERatio(float Price);
        public abstract float GetPERatio();

        #endregion
    }

    /// <summary>
    /// Definition of a common stock. Add here implementation specific elements for this kind of stocks.  
    /// </summary>
    /// <remarks> It uses a protected field (m_LastDividend) of the base class </remarks>
    public class CommonStock : GenericStock
    {

        // Common stock has the same fields of a generic stock

        #region Constructors/Finalizers

        // Standard constructor
        public CommonStock(string Symbol, float LastDividend, float ParValue): 
            base(Symbol, LastDividend, ParValue)
        {
            
        }
        #endregion

        #region Operations

        /// <summary>
        /// Implementation specific calculation of dividend yield. It differs from a preferred stock.
        /// </summary>
        /// <param name="Price"> Price against to calculate the dividend yield </param>
        /// <returns> The dividend yield or NaN if there is an error </returns>
        public override float GetDividendYield(float Price)
        {
            float result;

            if(Price <= 0.0f || LastDividend < 0.0f)
            {
                // .Net specific (avoids overhead for exception handling)
                // and handling of wrong price values
                result = float.NaN;
            }
            else
            {
                try
                {
                    result = LastDividend / Price;
                }
                catch
                {
                    result = float.NaN;
                }
            }

            return result;
        }

        /// <summary>
        /// Implementation specific calculation of dividend yield against LastPrice. 
        /// It differs from whose of a preferred stock.
        /// </summary>
        /// <returns> The dividend yield or NaN if there is an error </returns>
        public override float GetDividendYield()
        {
            return GetDividendYield(LastPrice);
        }

        /// <summary>
        /// Implementation specific calculation of P/E ratio. At the moment it is the same of a preferred stock.
        /// </summary>
        /// <remarks> Check for correctness (calculation for both stock types is the same ? ) </remarks>
        /// <param name="Price"> Price against to calcolate the P/E ratio </param>
        /// <returns> The P/E ratio or NaN if there is an error </returns>
        public override float GetPERatio(float Price)
        {
            float result;

            if(LastDividend <= 0.0f || Price < 0.0f)
            {
                // .Net specific (avoids overhead for exception handling)
                // and handling of wrong price values
                result = float.NaN;
            }
            else
            {
                try
                {
                    result = Price / LastDividend;
                }
                catch
                {
                    result = float.NaN;
                }
            }

            return result;
        }

        /// <summary>
        /// Implementation specific calculation of P/E ratio against LastPrice. 
        /// At the moment it is the same of a preferred stock.
        /// </summary>
        /// <remarks> Check for correctness (calculation for both stock types is the same ? ) </remarks>
        /// <returns> The P/E ratio or NaN if there is an error </returns>
        public override float GetPERatio()
        {
            return GetPERatio(LastPrice);
        }

        #endregion
    }

    /// <summary>
    /// Definition of a preferred stock. Add here implementation specific elements for this kind of stocks.  
    /// </summary>
    /// <remarks> It uses some protected fields (m_Parvalue, m_LastDividend) of the base class </remarks>
    public class PreferredStock : GenericStock
    {
        #region "Fields"

        // Preferred stocks has also a Fixed Dividend Percentage (here expressed as ratio field)
        private float m_FixedDividend;  
        #endregion

        #region Constructor/Finalizers

        // Standard constructor
        public PreferredStock(string Symbol, float LastDividend, float FixedDividend, float ParValue): 
            base(Symbol, LastDividend, ParValue)
        {
            m_FixedDividend = FixedDividend;
        }
        #endregion

        #region "Public Accessors"
        public float FixedDividend
        {
            get { return m_FixedDividend; }
            set { m_FixedDividend = value; }
        }
        #endregion

        #region "Operations"

        /// <summary>
        /// Implementation specific calculation of dividend yield. It differs from a common stock.
        /// </summary>
        /// <param name="Price"> Price against to calculate the dividend yield </param>
        /// <returns> The dividend yield or NaN if there is an error </returns>
        public override float GetDividendYield(float Price)
        {
            float result;

            if (Price <= 0.0f || m_FixedDividend < 0.0f || ParValue < 0.0f)
            {
                // .Net specific (avoids overhead for exception handling)
                // and handling of wrong price values
                result = float.NaN;
            }
            else
            {
                try
                {
                    result = (m_FixedDividend * ParValue) / Price;
                }
                catch
                {
                    result = float.NaN;
                }
            }

            return result;
        }

        /// <summary>
        /// Implementation specific calculation of dividend yield against LastPrice. It differs from a common stock.
        /// </summary>
        /// <returns> The dividend yield or NaN if there is an error </returns>
        public override float GetDividendYield()
        {
            return GetDividendYield(LastPrice);
        }

        /// <summary>
        /// Implementation specific calculation of P/E ratio. At the moment it is the same of a common stock.
        /// </summary>
        /// <remarks> Check for correctness (calculation for both stock types is the same ? ) </remarks>
        /// <param name="Price"> Price against to calcolate the P/E ratio </param>
        /// <returns> The P/E ratio or NaN if there is an error </returns>
        public override float GetPERatio(float Price)
        {
            float result;

            if (LastDividend <= 0.0f || Price < 0.0f)
            {
                // .Net specific (avoids overhead for exception handling)
                // and handling of wrong price values
                result = float.NaN;
            }
            else
            {
                try
                {
                    result = Price / LastDividend;
                }
                catch
                {
                    result = float.NaN;
                }
            }

            return result;
        }

        /// <summary>
        /// Implementation specific calculation of P/E ratio against LastPrice. At the moment it is the same of a common stock.
        /// </summary>
        /// <remarks> Check for correctness (calculation for both stock types is the same ? ) </remarks>
        /// <returns> The P/E ratio or NaN if there is an error </returns>
        public override float GetPERatio()
        {
            return GetPERatio(LastPrice);
        }
        #endregion
    }
}
