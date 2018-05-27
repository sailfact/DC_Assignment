﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DistributedGameData
{
    /// <summary>
    /// DataServerFault
    /// used for throwing exceptions across tiers 
    /// defines the operation where the error occurred 
    /// the problem type and error message
    /// </summary>
    [DataContract]
    public class DataServerFault
    {
        [DataMember]
        public string Operation { get; set; }

        [DataMember]
        public string ProblemType { get; set; }

        [DataMember]
        public string Message { get; set; }

        public DataServerFault(string op, string prob, string msg)
        {
            this.Operation = op;
            this.ProblemType = prob;
            this.Message = msg;
        }
    }
}
