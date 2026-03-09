using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    private static DatabaseManager _instance;
    public static DatabaseManager Instance { get { return _instance; } }

    [Header("Database")]
    [SerializeField] private string databaseName = "users.db";

    private SQLiteConnection _connection;
    private string _databasePath;

    public string DatabasePath { get { return _databasePath; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        _databasePath = Path.Combine(Application.persistentDataPath, databaseName);
        _connection = new SQLiteConnection(_databasePath, false);
        _connection.CreateTable<User>();

        Debug.Log("Database initialized at: " + _databasePath);
    }

    public bool RegisterUser(string username, string password)
    {
        username = username.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        if (password.Length < 8)
        {
            return false;
        }

        if (UserExists(username))
        {
            return false;
        }

        try
        {
            User newUser = new User();
            newUser.Username = username;
            newUser.Password = password;
            newUser.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            _connection.Insert(newUser);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Register error: " + ex.Message);
            return false;
        }
    }

    public (bool success, int userId) LoginUser(string username, string password)
    {
        try
        {
            User user = _connection.Table<User>()
                .FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user != null)
            {
                return (true, user.UserID);
            }

            return (false, -1);
        }
        catch (Exception ex)
        {
            Debug.LogError("Login error: " + ex.Message);
            return (false, -1);
        }
    }

    public bool UserExists(string username)
    {
        try
        {
            User user = _connection.Table<User>()
                .FirstOrDefault(x => x.Username == username);

            return user != null;
        }
        catch (Exception ex)
        {
            Debug.LogError("UserExists error: " + ex.Message);
            return false;
        }
    }

    public List<User> GetAllUsers()
    {
        try
        {
            return _connection.Table<User>()
                .OrderBy(x => x.UserID)
                .ToList();
        }
        catch (Exception ex)
        {
            Debug.LogError("GetAllUsers error: " + ex.Message);
            return new List<User>();
        }
    }

    public bool DeleteUser(int userId)
    {
        try
        {
            User user = _connection.Find<User>(userId);

            if (user == null)
            {
                return false;
            }

            _connection.Delete(user);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("DeleteUser error: " + ex.Message);
            return false;
        }
    }

    private void OnDestroy()
    {
        if (_connection != null)
        {
            _connection.Dispose();
            _connection = null;
        }
    }
}
