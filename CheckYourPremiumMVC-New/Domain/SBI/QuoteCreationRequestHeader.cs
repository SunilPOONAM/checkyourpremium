using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.SBI
{
    public class QuoteCreationRequestHeader
    {
        public string requestId { get; set; }
        public string action { get; set; }
        public string transactionTimestamp { get; set; }
        public string channel { get; set; }
        public string username { get; set; }
        public string userRole { get; set; }
        public User user { get; set; }
    }
}
