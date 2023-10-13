using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

using DBLayer.Models;
using DbLayer.Models;

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

        public List<Test> testCall1(List<int> TagIds)
        {
            List<Test> employeeList = new List<Test>();
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";

            using (MySqlConnection con = new MySqlConnection(CS))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                string FinalCommand = "SELECT DISTINCT test.id,test.text, test.disc, test.ImagePath FROM test INNER JOIN test_tag ON test_tag.test_id = test.id INNER JOIN tag ON tag.id = test_tag.tag_id WHERE ( ";

                bool first = true;

                foreach (int id in TagIds)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        FinalCommand += " OR ";
                    }

                    FinalCommand += "tag.id = @Tag_" + id;
                    cmd.Parameters.AddWithValue("@Tag_"+ id, id);
                }
                FinalCommand += ")";

                
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = FinalCommand;
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

        public List<TagDB> GetAllTags()
        {
            List<TagDB> TagList = new List<TagDB>();
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";

            using (MySqlConnection con = new MySqlConnection(CS))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tag", con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var tag = new TagDB();
                    tag.name = Convert.ToString(rdr["TagName"]);
                    tag.id = Convert.ToInt32(rdr["id"]);
                    TagList.Add(tag);
                }
                con.Close();
            }
            return TagList;
        }
        public List<TagDB> GetPostTags(int id)
        {
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";
            List<TagDB> TagList = new List<TagDB>();
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);

                    cmd.CommandText = "SELECT tag.id as tagId, tag.TagName as tagName FROM ((test INNER JOIN test_tag ON test_tag.test_id = test.id) INNER JOIN tag ON test_tag.tag_id = tag.id) WHERE test.id = @Postid";
                    cmd.Parameters.AddWithValue("@Postid", id);
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        TagDB tag = new TagDB();

                        tag.name = Convert.ToString(rdr["TagName"]);
                        tag.id = Convert.ToInt32(rdr["tagId"]);

                        TagList.Add(tag);
                    }

                    con.Close();
                    return TagList;
                }
            }
            catch (Exception)
            {
                //return false;
                throw;
            }
        }

        public Test? GetById(int id)
        {
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";
            Test test = new Test();
            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);

                    cmd.CommandText = "SELECT * FROM test WHERE id = @id LIMIT 1";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        
                        test.text = Convert.ToString(rdr["text"]);
                        test.disc = Convert.ToString(rdr["disc"]);
                        test.id = Convert.ToInt32(rdr["id"]);
                        test.imgpath = Convert.ToString(rdr["ImagePath"]);
                    }

                    con.Close();
                    return test;
                }
            }
            catch (Exception)
            {
                //return false;
                throw;
            }
        }

        public bool DoesConnectionExist(int postID, int tagID)
        {
            List<Test> employeeList = new List<Test>();
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";

            bool returnVal = false;

            using (MySqlConnection con = new MySqlConnection(CS))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM test_tag WHERE(test_id = @testId && tag_id = @tagId)", con);
                cmd.Parameters.AddWithValue("@testId", postID);
                cmd.Parameters.AddWithValue("@tagId", tagID);
                cmd.CommandType = CommandType.Text;
                con.Open();

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnVal = true;
                }
                con.Close();
                return returnVal;

            }

        }

        public bool TryCreateTagConnection(int postID, int tagID)
        {
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";

            if (DoesConnectionExist(postID, tagID))
            {
                return true;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                    cmd.CommandText = "INSERT INTO test_tag(test_id,tag_id) VALUES(@testId, @tagId)";
                    cmd.Parameters.AddWithValue("@testId", postID);
                    cmd.Parameters.AddWithValue("@tagId", tagID);

                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

                    con.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool TryRemoveTagConnection(int postID, int tagID)
        {
            string CS = "SERVER=127.0.0.1;UID=root;PASSWORD=;DATABASE=testdb";

            if (!DoesConnectionExist(postID, tagID))
            {
                return true;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(CS))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM test", con);
                    cmd.CommandText = "DELETE FROM test_tag WHERE(test_id = @testId && tag_id = @tagId)";
                    cmd.Parameters.AddWithValue("@testId", postID);
                    cmd.Parameters.AddWithValue("@tagId", tagID);

                    cmd.ExecuteNonQuery();
                    cmd.CommandType = CommandType.Text;

                    con.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
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
