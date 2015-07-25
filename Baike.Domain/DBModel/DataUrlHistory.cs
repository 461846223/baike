using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Baike.Entity.DBModel
{
    /// <summary>
    /// The data url history.
    /// </summary>
    public class DataUrlHistory:BaseEntity
    {
        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the inser time.
        /// </summary>
        public DateTime InserTime { get; set; }
    }
}
