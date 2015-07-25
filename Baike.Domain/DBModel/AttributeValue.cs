using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Baike.Entity;

namespace Baike.Entity
{
    /// <summary>
    /// The attribute value.
    /// </summary>
    public class AttributeValue : BaseEntity
    {
        /// <summary>
        /// ValueName
        /// </summary>		
        private string _valuename;

        public string ValueName
        {
            get
            {
                return _valuename;
            }
            set
            {
                _valuename = value;
            }
        }

        /// <summary>
        /// AttributeId
        /// </summary>		
        private int _attributeid;

        public int AttributeId
        {
            get
            {
                return _attributeid;
            }
            set
            {
                _attributeid = value;
            }
        }

        /// <summary>
        /// Status
        /// </summary>		
        private bool _status;

        public bool Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

    }
}

