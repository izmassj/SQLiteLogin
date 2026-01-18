using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    private static DatabaseManager _instance;
    public static DatabaseManager Instance { get { return _instance; } }

    private string connectionString;
    private string dbPath;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
    }

    private void InitializeDatabase()
    {
        dbPath = Path.Combine(Application.persistentDataPath, "users.db");
        connectionString = "URI=file:" + dbPath;

        CreateUsersTable();
        Debug.Log("Database initialized at: " + dbPath);
    }

    private void CreateUsersTable()
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Users (UserID INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT UNIQUE NOT NULL, Password TEXT NOT NULL, CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP)";
                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }
    }

    // Método para registrar nuevo usuario
    public bool RegisterUser(string username, string password)
    {
        if (password.Length < 8)
        {
            return false;
        }

        try
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Users (Username, Password) VALUES (@username, @password)";
                    cmd.Parameters.Add(new SqliteParameter("@username", username));
                    cmd.Parameters.Add(new SqliteParameter("@password", password));
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            return true;
        }
        catch (SqliteException ex)
        {
            Debug.LogError("Registration error: " + ex.Message);
            return false;
        }
    }

    public (bool success, int userId) LoginUser(string username, string password)
    {
        try
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT UserID FROM Users WHERE Username = @username AND Password = @password";
                    cmd.Parameters.Add(new SqliteParameter("@username", username));
                    cmd.Parameters.Add(new SqliteParameter("@password", password));

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int userId = reader.GetInt32(0);
                            return (true, userId);
                        }
                    }
                }

                conn.Close();
            }
            return (false, -1);
        }
        catch (SqliteException ex)
        {
            Debug.LogError("Login error: " + ex.Message);
            return (false, -1);
        }
    }

    public bool UserExists(string username)
    {
        using (var conn = new SqliteConnection(connectionString))
        {
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                cmd.Parameters.Add(new SqliteParameter("@username", username));

                int count = System.Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();

                return count > 0;
            }
        }
    }
}