using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphApp.GraphService;
using GraphService.Model;
using GraphX.PCL.Common.Models;

namespace GraphApp
{
    public class DataEdge : EdgeBase<DataVertex>
    {
        public DataEdge(DataVertex source, DataVertex target, double weight = 1)
			: base(source, target, weight)
		{
        }
        /// <summary>
        /// Default parameterless constructor (for serialization compatibility)
        /// </summary>
        public DataEdge()
            : base(null, null, 1)
        {
        }

        /// <summary>
        /// Custom string property for example
        /// </summary>
        public string Text { get; set; }

        #region GET members
        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
