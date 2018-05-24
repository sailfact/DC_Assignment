using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameData
{
    [DataContract]
    public class DataServerFault
    {
        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        public string ProblemType { get; set; }

        public DataServerFault(string op, string prob)
        {
            this.Operation = op;
            this.ProblemType = prob;
        }
    }
}
