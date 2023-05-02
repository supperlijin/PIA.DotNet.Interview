using PIA.DotNet.Interview.Core.Database;
using PIA.DotNet.Interview.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace PIA.DotNet.Interview.Core.Repositories
{
    public class TaskSQLiteRepository : IDisposable
    {
        DbConnection connection;

        public TaskSQLiteRepository(string connectionString)
        {
            connection = new SQLiteConnection(connectionString);
            InsertMultipleWithTransaction();

        }

        private void OpenConnection()
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
        }

        private void CloseConnection()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
                connection.Close();
        }


        public async Task<int> DeleteFromTaskTable(Guid Id)
        {
            string sql = @"DELETE FROM Task where Id=@Id";
            OpenConnection();
            return await connection.ExecuteNonQuery(sql, new List<SQLiteParameter>
            {
                new SQLiteParameter("@Id",Id.ToString("N")),
            });
        }

        public async Task<int> InsertIntoTaskTable(Models.Task task)
        {
            string sql = @"INSERT INTO Task
                            (Id,Title, Description, IsFinished,Example)
                        VALUES
                            (@Id,@Title, @Description,@IsFinished,@Example)";
            OpenConnection();
            return await connection.ExecuteNonQuery(sql, new List<SQLiteParameter>
            {
                new SQLiteParameter("@Id",task.Id.ToString("N")),
                new SQLiteParameter("@Title",task.Title),
                new SQLiteParameter("@Description",task.Description),
                new SQLiteParameter("@IsFinished",task.IsFinished),
                new SQLiteParameter("@Example",task.Example),
            });
        }

        public async Task<int> UpdateTaskTable(Models.Task task)
        {
            string sql = @"Update Task Set
                    Title = @Title,
                    Description = @Description,
                    IsFinished=@IsFinished,
                    Example=@Example
                    Where
                    Id = @Id";
            OpenConnection();
            return await connection.ExecuteNonQuery(sql, new List<SQLiteParameter>
            {
                new SQLiteParameter("@Id",task.Id.ToString("N")),
                new SQLiteParameter("@Title",task.Title),
                new SQLiteParameter("@Description",task.Description),
                new SQLiteParameter("@IsFinished",task.IsFinished),
                new SQLiteParameter("@Example",task.Example),
            });
        }

        public async Task<int> CreateTaskTable()
        {
            string sql = @"
                        CREATE TABLE IF NOT EXISTS Task(
                            [Id] NVARCHAR(128)    NOT NULL
                            PRIMARY KEY,
                            [Title] NVARCHAR(128) NOT NULL,
                            [Description] NVARCHAR(250) NOT NULL,
                            [IsFinished] INTEGER NOT NULL,
                            [Example] NVARCHAR(128) NOT NULL
                        )";
            OpenConnection();
            return await connection.ExecuteNonQuery(sql);
        }

        public async Task<Models.Task>  GetTaskById(Guid Id)
        {
            try
            {
                string sql = @"SELECT * From Task where Id=@Id";
                OpenConnection();
                var result = await connection.ExecuteReader(sql, new List<SQLiteParameter>
                {
                    new SQLiteParameter("@Id", Id.ToString("N"))
                });
                if (result.Read())
                {
                    var t = new Models.Task
                    {
                        Id = result.GetGuid(0),
                        Title = result.GetString(1),
                        Description = result.GetString(2),
                        IsFinished = result.GetBoolean(3),
                        Example = result.GetString(4)
                    };
                    return t;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        public async Task<List<Models.Task>> GetTask()
        {
            try
            {
                var list = new List<Models.Task>();
                string sql = "SELECT * From Task";
                OpenConnection();
                var result =await connection.ExecuteReader(sql);
                while (result.Read())
                {
                    var t = new Models.Task
                    {
                        Id = result.GetGuid(0),
                        Title = result.GetString(1),
                        Description = result.GetString(2),
                        IsFinished = result.GetBoolean(3),
                        Example = result.GetString(4)
                    };
                    list.Add(t);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        // Transaction Demo
        public async void InsertMultipleWithTransaction()
        {
            try
            {
                await CreateTaskTable();
                var task=await GetTask();
                if (task.Count==0)
                {
                    var list = new List<Models.Task>();
                    list.Add(new Models.Task
                    {
                        Id = Guid.NewGuid(),
                        Title = "1 - Launch the solution from Visual Studio",
                        Description = "Well done! If you see this output in your browser, you have already completed the first task.<br />" +
                        "Now you can start with the tasks below and mark them as done later when you have created the functionality for them.<br />" +
                        "Make sure you follow the development guidelines in the Readme.md in the root directory of this solution.<br />" +
                        "Good luck!",
                        Example = ""
                    });

                    list.Add(new Models.Task
                    {
                        Id = Guid.NewGuid(),
                        Title = "2 - Editing functions - Mark task as done",
                        Description = "The 'Done' button is used to set the task processing status to Done or Not Done.<br />" +
                        "The customizations are to be done from the WebUI (frontend) to the database (backend).",
                        Example = "example_task2.JPG"
                    });

                    list.Add(new Models.Task
                    {
                        Id = Guid.NewGuid(),
                        Title = "3 - Central logging",
                        Description = "A logging mechanism is to be introduced for all services to ensure central logging.<br />",
                        Example = ""
                    });

                    list.Add(new Models.Task
                    {
                        Id = Guid.NewGuid(),
                        Title = "4 - Central configuration",
                        Description = "All services should be configurable via a central .ini file (http endpoint, database file, etc.).<br />",
                        Example = "example_task4.JPG"
                    });

                    list.Add(new Models.Task
                    {
                        Id = Guid.NewGuid(),
                        Title = "5 - UI extension",
                        Description = "Extend the user interface with another tab where you can display the number of completed tasks versus the number of open tasks in a pie chart.<br />",
                        Example = "example_task5.JPG"
                    });

                    list.Add(new Models.Task
                    {
                        Id = Guid.NewGuid(),
                        Title = "6 - Refactoring to Async",
                        Description = "Refactoring of the entire application, so that there are no more synchronous method calls to the database layer or in the rest interfaces.<br />",
                        Example = ""
                    });

                    list.Add(new Models.Task
                    {
                        Id = Guid.NewGuid(),
                        Title = " 7 - Use SQLite",
                        Description = "Use a SQLite database instead of the database.json file.<br />",
                        Example = ""
                    });

                    list.Add(new Models.Task
                    {
                        Id = Guid.NewGuid(),
                        Title = " 8 - Use another WebFramework",
                        Description = "The aim is to rebuild the current UI in another web framework (Angular or Blazor).<br />" +
                        "Hint: Use the <a href=\"http://localhost:5001/index.html\">Task-API</a><br />",
                        Example = ""
                    });

                    list.Add(new Models.Task
                    {
                        Id = Guid.NewGuid(),
                        Title = " 9 - Delete a Task",
                        Description = "The new frontend should be able to use the <a href=\"http://localhost:5001/index.html\">Task-API</a> hosted on PIA.DotNet.Interview.Backend to delete a task.<br />" +
                        "Hint: you need to add a delete button <br />",
                        Example = ""
                    });

                    foreach (var item in list)
                    {
                        int i =await InsertIntoTaskTable(item);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
