using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

using DBLayer.Models;

namespace DbLayer
{
    public class DBContext
    {
        string ConnectionString = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";

        public DBContext()
        {

           
        }
        public DBContext(string connection) {
        
        ConnectionString = connection;
        }

        public List<Test> testCall1()
        {
            List<Test>employeeList = new List<Test>();
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";

            using (MySqlConnection con = new MySqlConnection(CS))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                cmd.CommandType = CommandType.Text;
                con.Open(); 

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var employee = new Test();
                    employee.text = Convert.ToString(rdr["text"]);
                    employee.disc = Convert.ToString(rdr["disc"]);
                    employee.id = Convert.ToInt32(rdr["id"]);
                    employee.imgpath = Convert.ToString(rdr["ImagePath"]);
                    employeeList.Add(employee);
                }
                con.Close();
            }
            return employeeList;
        }

        public bool TryCreateTest(string text, string disc, string fileName)
        {
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";

            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                    if(fileName != null)
                    {
                        cmd.CommandText = "INSERT INTO test(text,disc,ImagePath) VALUES(@text, @disc, @file)";
                        cmd.Parameters.AddWithValue("@text", text);
                        cmd.Parameters.AddWithValue("@disc", disc);
                        cmd.Parameters.AddWithValue("@file", fileName);
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO test(text,disc) VALUES(@text, @disc)";
                        cmd.Parameters.AddWithValue("@text", text);
                        cmd.Parameters.AddWithValue("@disc", disc);
                    }

                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

                    con.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                //return false;
                throw;
            }
        }

        public bool TryDeleteTest(int ID)
        {
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";

            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                    cmd.CommandText = "DELETE FROM test WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

                    con.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                //return false;
                throw;
            }

        }
    }
}
