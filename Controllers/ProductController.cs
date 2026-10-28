using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Practical8.Models;

namespace Practical8.Controllers
{
    public class ProductController : Controller
    {
        // Database connection
        string cs = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TextDB;Integrated Security=True;Encrypt=False";


        // GET: Product
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT ProductId, ProductName,
                                        Quantity, Price
                                 FROM Products";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product p = new Product
                            {
                                ProductId = Convert.ToInt32(
                                    reader["ProductId"]),

                                ProductName = reader["ProductName"].ToString(),

                                Quantity = Convert.ToInt32(
                                    reader["Quantity"]),

                                Price = Convert.ToDecimal(
                                    reader["Price"])
                            };

                            products.Add(p);
                        }
                    }
                }
            }

            return View(products);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Products
                                (ProductName, Quantity, Price)
                                VALUES
                                (@ProductName, @Quantity, @Price)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ProductName", SqlDbType.VarChar, 100)
                        .Value = p.ProductName;

                    cmd.Parameters.Add("@Quantity", SqlDbType.Int)
                        .Value = p.Quantity;

                    SqlParameter priceParameter =
                        cmd.Parameters.Add("@Price", SqlDbType.Decimal);

                    priceParameter.Precision = 10;
                    priceParameter.Scale = 2;
                    priceParameter.Value = p.Price;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            Product p = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT ProductId, ProductName,
                                        Quantity, Price
                                 FROM Products
                                 WHERE ProductId = @ProductId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = id;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            p = new Product
                            {
                                ProductId = Convert.ToInt32(
                                    reader["ProductId"]),

                                ProductName = reader["ProductName"].ToString(),

                                Quantity = Convert.ToInt32(
                                    reader["Quantity"]),

                                Price = Convert.ToDecimal(
                                    reader["Price"])
                            };
                        }
                    }
                }
            }

            if (p == null)
                return HttpNotFound();

            return View(p);
        }

        // POST: Product/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product p)
        {
            if (!ModelState.IsValid)
                return View(p);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE Products
                                 SET ProductName = @ProductName,
                                     Quantity = @Quantity,
                                     Price = @Price
                                 WHERE ProductId = @ProductId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ProductId", SqlDbType.Int)
                        .Value = p.ProductId;

                    cmd.Parameters.Add("@ProductName", SqlDbType.VarChar, 100)
                        .Value = p.ProductName;

                    cmd.Parameters.Add("@Quantity", SqlDbType.Int)
                        .Value = p.Quantity;

                    SqlParameter priceParameter =
                        cmd.Parameters.Add("@Price", SqlDbType.Decimal);

                    priceParameter.Precision = 10;
                    priceParameter.Scale = 2;
                    priceParameter.Value = p.Price;

                    con.Open();

                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                        return HttpNotFound();
                }
            }

            return RedirectToAction("Index");
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            Product p = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT ProductId, ProductName,
                                        Quantity, Price
                                 FROM Products
                                 WHERE ProductId = @ProductId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = id;

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            p = new Product
                            {
                                ProductId = Convert.ToInt32(
                                    reader["ProductId"]),

                                ProductName = reader["ProductName"].ToString(),

                                Quantity = Convert.ToInt32(
                                    reader["Quantity"]),

                                Price = Convert.ToDecimal(
                                    reader["Price"])
                            };
                        }
                    }
                }
            }

            if (p == null)
                return HttpNotFound();

            return View(p);
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"DELETE FROM Products
                                 WHERE ProductId = @ProductId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@ProductId", SqlDbType.Int).Value = id;

                    con.Open();

                    int rows = cmd.ExecuteNonQuery();

                    if (rows == 0)
                        return HttpNotFound();
                }
            }

            return RedirectToAction("Index");
        }
    }
}