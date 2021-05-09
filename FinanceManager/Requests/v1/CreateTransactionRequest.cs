﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Requests.v1
{
    public class CreateTransactionRequest
    {
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
    }
}
