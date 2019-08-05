using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CSharptoSQL {
    class Program {
        static void Main(string[] args) {
            var orderId = GetOrderbyId(2032);
            if (orderId != null) {
                Console.WriteLine($"{orderId.Id} | {orderId.Date}");
            } else {
                Console.WriteLine("No order found matching ID");
            }

            var cust = GetCustomerbyID(1028);
            if (cust == null) {
                Console.WriteLine("No customer found matching Id");
            } else {
                Console.WriteLine(cust.Name);
            }

            var sql = "select * from Orders;";
            var orders = SelectOrder(sql);
            foreach (var order in orders) {
                Console.WriteLine($"{order.Id} | {order.Date}");
            }
            
        }

        static List<Order> SelectOrder (string sql) {
            var connStr = @"server=localhost\sqlexpress;database=CustomerOrderDb;trusted_connection=true;";
            var connection = new SqlConnection(connStr);

            connection.Open();

            if (connection.State != System.Data.ConnectionState.Open) {
                throw new Exception("Conection Failed to open!"); 
            }
            var ordersList = new List<Order>();
            var cmd = new SqlCommand(sql, connection);
            var reader = cmd.ExecuteReader();

            while (reader.Read()) {
                var id = (int)reader["Id"];
                var date = (DateTime)reader["Date"];
                var note = reader.IsDBNull(reader.GetOrdinal("Note")) ?
                    null : reader["Note"].ToString();

                var customerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ?
                    null : (int?)reader["CustomerId"];

                var order = new Order(id, date, note, customerId);
                ordersList.Add(order);

            }
            connection.Close();
            return ordersList;
        }
        static List<Customer> SelectCustomer(string sql) {

            var connStr = @"server=localhost\sqlexpress;database=CustomerOrderDb;trusted_connection=true;";
            var connection = new SqlConnection(connStr);

            //Best Practice is a try catch on open due to exceptions
            connection.Open();

            if (connection.State != System.Data.ConnectionState.Open) {
                throw new Exception("Connection failed to open!");
            }
            var customersList = new List<Customer>();


            var cmd = new SqlCommand(sql, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                // Local Variables to values in the Db
                var id = (int)reader["Id"];
                var name = reader["Name"].ToString();
                var city = reader["City"].ToString();
                var state = reader["State"].ToString();
                var active = (bool)reader["Active"];

                // Ternary Operator
                var code = reader.IsDBNull(reader.GetOrdinal("Code"))
                    ? null : (string)reader["Code"].ToString();

                var customer = new Customer(id, name, city, state, active, code);
                customersList.Add(customer);

            }
            reader.Close();


            connection.Close();
                return customersList;
        }
        static Customer GetCustomerbyID(int pid) {
            var connStr = @"server=localhost\sqlexpress;database=CustomerOrderDb;trusted_connection=true;";
            var connection = new SqlConnection(connStr);

            //Best Practice is a try catch on open due to exceptions
            connection.Open();

            if (connection.State != System.Data.ConnectionState.Open) {
                throw new Exception("Connection failed to open!");
            }
            var sql = "Select * from Customers where id = @myid;";
            var cmd = new SqlCommand(sql, connection);
            var theId = new SqlParameter("@myid", pid);
            cmd.Parameters.Add(theId);
            
            var reader = cmd.ExecuteReader();
            Customer custRetrn = null;
            if (reader.Read()) {
                var id = (int)reader["Id"];
                var name = reader["Name"].ToString();
                var city = reader["City"].ToString();
                var state = reader["State"].ToString();
                var active = (bool)reader["Active"];

                // Ternary Operator
                var code = reader.IsDBNull(reader.GetOrdinal("Code"))
                    ? null : (string)reader["Code"].ToString();
                var customer = new Customer(id, name, city, state, active, code);
                custRetrn = customer;
            }
            reader.Close();
            connection.Close();

            return custRetrn;
        }
        static Order GetOrderbyId (int pid) {
            Order orderRetrn = null;
            var connStr = @"server=localhost\sqlexpress;database=CustomerOrderDb;trusted_connection=true;";
            var connection = new SqlConnection(connStr);

            connection.Open();
            if (connection.State != System.Data.ConnectionState.Open) {
                throw new Exception("Connection failed to open!");
            }

            string sql = "Select * from Orders where Id = @myId;";
            var cmd = new SqlCommand(sql, connection);
            var theId = new SqlParameter("@myId", pid);
            cmd.Parameters.Add(theId);

            var reader = cmd.ExecuteReader();

            if (reader.Read()) {
                var id = (int)reader["Id"];
                var date = (DateTime)reader["Date"];
                var note = reader.IsDBNull(reader.GetOrdinal("Note")) ?
                    null : reader["Note"].ToString();

                var customerId = reader.IsDBNull(reader.GetOrdinal("CustomerId")) ?
                    null : (int?)reader["CustomerId"];

                var order = new Order(id, date, note, customerId);
                orderRetrn = order;
            }
            reader.Close();
            connection.Close();

            return orderRetrn;
        }
    }
}
