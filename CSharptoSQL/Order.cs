using System;
using System.Collections.Generic;
using System.Text;

namespace CSharptoSQL {
    public class Order {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int? CustomerId { get; set; }

        public Order(int id, DateTime date, string note, int? customerId) {
            Id = id;
            Date = date;
            Note = note;
            CustomerId = customerId;
        }
    }
}
