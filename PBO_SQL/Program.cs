using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace PBO_SQL
{
    internal class Program
    {
        class Helper
        {
            public void DBConn(ref DataSet ds, string query, NpgsqlParameter[] param)
            {
                string dsn = "Host=localhost;Username=postgres;Password=admin;Database=postgres;Port=5432";
                var conn = new NpgsqlConnection(dsn);
                var cmd = new NpgsqlCommand(query, conn);

                try
                {
                    // Perulangan untuk menyisipkan nilai yang ada pada parameter ke query
                    foreach (var p in param)
                    {
                        cmd.Parameters.Add(p);
                    }

                    cmd.Connection.Open();

                    // Mengisi ds dengan data yang didapatkan dari database
                    new NpgsqlDataAdapter(cmd).Fill(ds);
                    Console.WriteLine("Berhasil");
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    cmd.Connection.Close();
                }

            }
        }

        class User
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Title { get; set; }
            public string Phone_number { get; set; }
            public int Status { get; set; }
            public string Email { get; set; }
        }

        class UserManager
        {
            Helper helper;
            DataSet ds;
            NpgsqlParameter[] param;

            // Query ke Database
            string query;
            public UserManager()
            {
                helper = new Helper();
                ds = new DataSet();
                param = new NpgsqlParameter[] { };
                query = "";
            }

            // Method untuk mendapatkan semua data 
            public void GetAllData()
            {
                ds = new DataSet();
                param = new NpgsqlParameter[] { };
                // Query Select
                query = "SELECT * FROM employee.employees;";
                helper.DBConn(ref ds, query, param);
                // List of User untuk menampung data user
                List<User> users = new List<User>();
                // Mengambil value dari tabel di index 0
                var data = ds.Tables[0];
                // Perulangan untuk mengambil instance tiap baris dari tabel
                foreach (DataRow u in data.Rows)
                {
                    User user = new User();
                    // Mengisi id dan username dari object user dengan nilai dari database
                    user.Id = u.Field<Int32>(data.Columns[0]);
                    user.Username = u.Field<string>(data.Columns[1]);
                    user.Title = u.Field<string>(data.Columns[2]);
                    user.Phone_number = u.Field<string>(data.Columns[3]);
                    user.Status = u.Field<int>(data.Columns[4]);
                    user.Email = u.Field<string>(data.Columns[5]);
                    // Menambahkan user ke users (List of User)
                    users.Add(user);
                }
                // Perulangan untuk menampilkan semua data User yang ada pada users
                foreach (User user in users)
                {
                    Console.WriteLine($"ID: {user.Id} -- Username: {user.Username} -- Title: {user.Title} -- Birth Date: " +
                        $"-- Phone Number: {user.Phone_number} -- Status: {user.Status} -- Email: {user.Email}");
                }
            }

            public void GetUserById(int id)
            {
                ds = new DataSet();
                param = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@id", id)
                };
                query = "SELECT * FROM employee.employees WHERE employee_id = @id;";
                helper.DBConn(ref ds, query, param);
                List<User> users = new List<User>();
                var data = ds.Tables[0];

                foreach (DataRow u in data.Rows)
                {
                    User user = new User();
                    user.Id = u.Field<Int32>(data.Columns[0]);
                    user.Username = u.Field<string>(data.Columns[1]);
                    user.Title = u.Field<string>(data.Columns[2]);
                    user.Phone_number = u.Field<string>(data.Columns[3]);
                    user.Status = u.Field<int>(data.Columns[4]);
                    user.Email = u.Field<string>(data.Columns[5]);
                    users.Add(user);
                }

                foreach (User user in users)
                {
                    Console.WriteLine($"ID: {user.Id} -- Username: {user.Username} -- Title: {user.Title} -- Birth Date:" +
                        $" -- Phone Number: {user.Phone_number} -- Status: {user.Status} -- Email: {user.Email}");
                }
            }

            public void InsertUser(User user)
            {
                ds = new DataSet();
                param = new NpgsqlParameter[] {
                    new NpgsqlParameter("@id", user.Id),
                    new NpgsqlParameter("@username", user.Username),
                    new NpgsqlParameter("@title", user.Title),
                    new NpgsqlParameter("@phonenumber", user.Phone_number),
                    new NpgsqlParameter("@status", user.Status),
                    new NpgsqlParameter("@email", user.Email)
                };

                query = "INSERT INTO employee.employees VALUES (@id, @username, @title, @phonenumber, @status, @email);";
                helper.DBConn(ref ds, query, param);
            }

            public void UpdateName(User user)
            {
                ds = new DataSet();
                param = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@id", user.Id),
                    new NpgsqlParameter("@username", user.Username),
                };

                query = "UPDATE employee.employees SET name = @username WHERE employee_id = @id;";
                helper.DBConn(ref ds, query, param);
            }

            public void UpdateTitle(User user)
            {
                ds = new DataSet();
                param = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@id", user.Id),
                    new NpgsqlParameter("@title", user.Title)
                };

                query = "UPDATE employee.employees SET title = @title WHERE employee_id = @id;";
                helper.DBConn(ref ds, query, param);
            }

            public void UpdatePhoneNumber(User user)
            {
                ds = new DataSet();
                param = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@id", user.Id),
                    new NpgsqlParameter("@phonenumber", user.Phone_number),
                };

                query = "UPDATE employee.employees SET phone_number = @phonenumber WHERE employee_id = @id;";
                helper.DBConn(ref ds, query, param);
            }
            public void UpdateStatus(User user)
            {
                ds = new DataSet();
                param = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@id", user.Id),
                    new NpgsqlParameter("@status", user.Status)
                };

                query = "UPDATE employee.employees SET status = @status WHERE employee_id = @id;";
                helper.DBConn(ref ds, query, param);
            }

            public void UpdateEmail(User user)
            {
                ds = new DataSet();
                param = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@id", user.Id),
                    new NpgsqlParameter("@email", user.Email)
                };

                query = "UPDATE employee.employees SET email = @email WHERE employee_id = @id;";
                helper.DBConn(ref ds, query, param);
            }

            public void DeleteUser(int id)
            {
                ds = new DataSet();
                param = new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@id", id)
                };

                query = "DELETE FROM employee.employees WHERE employee_id = @id;";
                helper.DBConn(ref ds, query, param);
            }
        }


        static void Main(string[] args)
        {
            UserManager userManager = new UserManager();
        }
    }
}