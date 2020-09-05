using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SocialMediaApp.Models
{
    public class dbUtils
    {
        public string connString = @"Server=tcp:ql7fq5xxow.database.windows.net,1433;Initial Catalog=MESSENGER_DB;Persist Security Info=False;User ID=db_abdallah_elkhayat;Password='2232122321Aa';MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public ArrayList[] fetch(string sql, int NofCols)
        {
            ArrayList[] data = new ArrayList[NofCols];
            for (int i = 0; i < NofCols; i++)
            {
                data[i] = new ArrayList();
            }

            SqlConnection cnn;
            cnn = new SqlConnection(connString);
            cnn.Open();
            SqlCommand c;
            SqlDataReader dr;
            c = new SqlCommand(sql, cnn);
            dr = c.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    for (int i = 0; i < NofCols; i++)
                    {
                        data[i].Add(dr.GetValue(i));
                    }
                }
            }
            cnn.Close();
            return data;
        }

        public void create(String sql)
        {
            SqlConnection cnn;
            cnn = new SqlConnection(connString);
            cnn.Open();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            command = new SqlCommand(sql, cnn);
            adapter.InsertCommand = command;
            try
            {
                adapter.InsertCommand.ExecuteNonQuery();

            }
            catch (Exception)
            {

            }
            command.Dispose();
            cnn.Close();
        }
        public void create(String sql, ArrayList[] param)
        {
            SqlConnection cnn;
            cnn = new SqlConnection(connString);
            cnn.Open();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            command = new SqlCommand(sql, cnn);
            int ind = 0;
            foreach (object o in param[0])
            {
                command.Parameters.AddWithValue(o.ToString(), param[1][ind].ToString());
                ind++;
            }
            adapter.InsertCommand = command;
            try
            {
                adapter.InsertCommand.ExecuteNonQuery();

            }
            catch (Exception)
            {

            }
            command.Dispose();
            cnn.Close();
        }

        public void Update(string sql)
        {
            SqlConnection cnn;

            cnn = new SqlConnection(connString);

            cnn.Open();

            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            command = new SqlCommand(sql, cnn);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }
            command.Dispose();
            cnn.Close();
        }
    }
}