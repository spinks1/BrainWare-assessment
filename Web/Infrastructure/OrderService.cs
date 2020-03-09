using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    using System.Data;
    using Models;
    using System.Configuration;

    public class OrderService
    {
        IDatabase dbcon = new Database();

        public List<Order> GetOrdersForCompany(int CompanyId)
        {
            var values = GetCompanyOrders(CompanyId);

            var values2 = GetOrderedProducts();

            return ConstructCompanyOrderedProducts(values, values2);
        }

        public List<Order> GetCompanyOrders(int CompanyId)
        {
            var values = new List<Order>();
            try
            {
                //Get the company orders              
                //Need to convert this to a stored proc
                var sql1 = ConfigurationManager.AppSettings["sql1"];
                var reader1 = dbcon.ExecuteReader(sql1);

                while (reader1.Read())
                {
                    var record1 = (IDataRecord)reader1;

                    values.Add(new Order()
                    {
                        CompanyName = record1.GetString(0),
                        Description = record1.GetString(1),
                        OrderId = record1.GetInt32(2),
                        OrderProducts = new List<OrderProduct>()
                    });
                }
                reader1.Close();
            }
            catch (Exception ex)
            {
                throw;//this would need to be setup to write to a db or a flat file.
            }
            return values;
        }

        public List<OrderProduct> GetOrderedProducts()
        {
            var values2 = new List<OrderProduct>();
            try
            {
                //Get the order products 
                //Need to convert this to a stored proc
                var sql2 = ConfigurationManager.AppSettings["sql2"];
                var reader2 = dbcon.ExecuteReader(sql2);

                while (reader2.Read())
                {
                    var record2 = (IDataRecord)reader2;

                    values2.Add(new OrderProduct()
                    {
                        OrderId = record2.GetInt32(1),
                        ProductId = record2.GetInt32(2),
                        Price = record2.GetDecimal(0),
                        Quantity = record2.GetInt32(3),
                        Product = new Product()
                        {
                            Name = record2.GetString(4),
                            Price = record2.GetDecimal(5)
                        }
                    });
                }
                reader2.Close();
            }
            catch (Exception ex)
            {
                throw;//this would need to be setup to write to a db or a flat file.
            }
            return values2;
        }

        public List<Order> ConstructCompanyOrderedProducts(List<Order> values, List<OrderProduct> values2)
        {
            try
            {
                foreach (var order in values)
                {
                    foreach (var orderproduct in values2)
                    {
                        if (orderproduct.OrderId != order.OrderId)
                        {
                            continue;
                        }
                        order.OrderProducts.Add(orderproduct);
                        order.OrderTotal = order.OrderTotal + (orderproduct.Price * orderproduct.Quantity);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;//this would need to be setup to write to a db or a flat file.
            }
            return values;
        }
    }
}