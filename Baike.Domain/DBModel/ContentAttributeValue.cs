using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace Baike.Entity{
	 	//ContentAttributeValue
    public class ContentAttributeValue : BaseEntity
	{
       
		/// <summary>
		/// ContentId
        /// </summary>		
		private int _contentid;
        public int ContentId
        {
            get{ return _contentid; }
            set{ _contentid = value; }
        }        
		/// <summary>
		/// AttributeValueId
        /// </summary>		
		private int _attributevalueid;
        public int AttributeValueId
        {
            get{ return _attributevalueid; }
            set{ _attributevalueid = value; }
        }        
		   
	}
}

