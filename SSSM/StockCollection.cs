//
// SSSM - 2015 - Daniele Faggi
//

using System;
using System.Collections;
using System.Collections.Generic;

namespace SSSM
{
    /// <summary>
    /// Collection where to store the quotes. It provides encapsulation of the operations to be carried out on the collection. 
    /// </summary>
    public class StockCollection : ICollection<GenericStock>
    {
        #region Fields

        // The base collection (a "normal" list)
        private List<GenericStock> m_StockList;
        #endregion

        #region Constructors/Finalizers

        // Standard Constructor
        public StockCollection()
        {
            m_StockList = new List<GenericStock>();
        }
        #endregion

        #region Accessors
        public List<GenericStock> BaseCollection
        {
            get { return m_StockList; }
        }
        #endregion

        #region Standard Collection Methods

        // Methods inherited from the collection interface

        public int Count
        {
            get
            {
                return m_StockList.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(GenericStock item)
        {
            m_StockList.Add(item);
        }

        public void Clear()
        {
            m_StockList.Clear();
        }

        public bool Contains(GenericStock item)
        {
            return m_StockList.Contains(item);
        }

        public void CopyTo(GenericStock[] array, int arrayIndex)
        {
            m_StockList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GenericStock> GetEnumerator()
        {
            return m_StockList.GetEnumerator();
        }

        public bool Remove(GenericStock item)
        {
            return m_StockList.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) m_StockList).GetEnumerator();
        }
        #endregion

        #region Operations

        /// <summary>
        /// Calculate the geometric mean of all stocks based on the price of last trade of each stock.
        /// </summary>
        /// <returns> The All Share Index (geometric mean) or NaN if there is an error </returns>
        public float GetGeometricMean()
        {

            // If there isn't any stock, exit returning NaN
            if (m_StockList.Count <= 0) return float.NaN;

            float mul = 1.0f;

            foreach(GenericStock stock in m_StockList)
            {
                if (!float.IsNaN(stock.LastPrice))
                {
                    mul *= stock.LastPrice;
                }
                else
                {
                    mul = float.NaN;
                    break;
                }
            }

            float result;

            if(!float.IsNaN(mul))
            {
                try
                {
                    float nsq = 1f / m_StockList.Count;
                    result = (float)Math.Pow(mul, nsq);
                }
                catch
                {
                    result = float.NaN;
                }
            }
            else
            {
                result = float.NaN;
            }

            return result;
        }
        #endregion
    }
}
