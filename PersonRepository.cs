﻿using SQLite;
using PeopleEjercicio.Models;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PeopleEjercicio;

public class PersonRepository
{
    string _dbPath;

    public string StatusMessage { get; set; }

    // TODO: Add variable for the SQLite connection

    private SQLiteAsyncConnection conn;
    private ObservableCollection<User> userList = new ObservableCollection<User>();


    private async Task Init()
    {
        if (conn != null)
            return;

        conn = new SQLiteAsyncConnection(_dbPath);

        await conn.CreateTableAsync<Person>();
    }

    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;                        
    }

    public async Task AddNewPerson(string name)
    {
        int result = 0;
        try
        {
            // Call Init()
            await Init();

            // basic validation to ensure a name was entered
            if (string.IsNullOrEmpty(name))
                throw new Exception("Valid name required");

            result = await conn.InsertAsync(new Person { Name = name });

            StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
        }
    }

    public async Task<List<Person>> GetAllPeople()
    {
        try
        {
            await Init();
            return await conn.Table<Person>().ToListAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Person>();
    }

    public async Task AddAllUsersAsync()
    {
        List<User> users = await conn.Table<User>().ToListAsync();
        // Must be on UI thread here!
        foreach (var u in users)
            userList.Add(u);
    }
}