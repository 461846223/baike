using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
using Baike.Entity;

namespace Baike.Entity{
	 	//Attribute
    public class Attribute :BaseEntity
	{      
		/// <summary>
		/// Name
        /// </summary>		
		private string _name;
        public string Name
        {
            get{ return _name; }
            set{ _name = value; }
        }        
		/// <summary>
		/// Description
        /// </summary>		
		private string _description;
        public string Description
        {
            get{ return _description; }
            set{ _description = value; }
        }        
		   
	}
}

