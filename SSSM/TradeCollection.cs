//
// SSSM - 2015 - Daniele Faggi
//

using System;
using System.Collections.Generic;
using System.Collections;

namespace SSSM
{
    /// <summary>
    /// Collection where to store the trades. It provides an Add method to add a trade keeping an order based on the
    /// timestamp of the trade object and encapsulation of the operations to be carried out on the collection. 
    /// </summary>
    public class TradeCollection : ICollection<Trade>
    {
        #region Fields

        // The base collection (a "normal" list)
        private List<Trade> m_TradeList;
        #endregion

        #region Constructors/Finalizers

        // Standard costructor
        public TradeCollection()
        {
            m_TradeList = new List<Trade>();
        }
        #endregion

        #region Accessors

        public List<Trade> BaseCollection
        {
            get { return m_TradeList; }
        }
        #endregion

        #region "Standard Collection Methods"

        // Methods inherited from the collection interface

        public int Count
        {
            get
            {
                return m_TradeList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Add the trade in the ordered position inside the list. The trades are ordered from the most recent
        /// to the least. If trades will be loaded in other ways or timestamp is changed after addition, order
        /// won't be longer kept.
        /// </summary>
        /// <param name="item"> Trade to be added </param>
        public void Add(Trade item)
        {

            IEnumerator<Trade> e = m_TradeList.GetEnumerator();

            int n = 0;

            while(e.MoveNext())
            {
                if(item.Timestamp > e.Current.Timestamp)
                {
                    m_TradeList.Insert(n, item);
                    // Enumerator is invalidated
                    break;
                }
                n++;
            }

            if(n > m_TradeList.Count - 1)
                m_TradeList.Add(item);
        }

        public void Clear()
        {
            m_TradeList.Clear();
        }

        public bool Contains(Trade item)
        {
            return m_TradeList.Contains(item);
        }

        public void CopyTo(Trade[] array, int arrayIndex)
        {
            m_TradeList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Trade> GetEnumerator()
        {
            return m_TradeList.GetEnumerator();
        }

        public bool Remove(Trade item)
        {
            return m_TradeList.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_TradeList).GetEnumerator();
        }
        #endregion

        #region Operations

        /// <summary>
        /// Method to calculate the volume weighted price of the trades done within the last interval of time.
        /// </summary>
        /// <param name="TimeFrame"> Interval duration </param>
        /// <returns> The volume weighted price of last trades </returns>
        public float GetVolumeWeightedPrice(TimeSpan TimeFrame)
        {
            float num = 0.0f;
            float den = 0.0f;

            DateTime TimeLimit = DateTime.Now.Subtract(TimeFrame);

            foreach (Trade trade in m_TradeList)
            {
                if (trade.Timestamp > TimeLimit)
                {
                    num += trade.Quantity * trade.TradedPrice;
                    den += trade.Quantity;
                }
            }

            float result;

            if (den <= 0.0f)
            {
                // .Net specific (avoids overhead for exception handling)
                result = float.NaN;
            }
            else
            {
                // "Normal" processing
                try
                {
                    result = num / den;
                }
                catch
                {
                    result = float.NaN;
                }
            }

            return result;
        }
        #endregion
    }
}
