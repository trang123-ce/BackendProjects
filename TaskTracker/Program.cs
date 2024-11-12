using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;

public class Task
{
    public int Id { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TaskManager
{
    private const string TasksFile = "tasks.json";

    public static List<Task> LoadTasks()
    {
        if (File.Exists(TasksFile))
        {
            var json = File.ReadAllText(TasksFile);
            return JsonSerializer.Deserialize<List<Task>>(json);
        }
        return new List<Task>();
    }

    public static void SaveTasks(List<Task> tasks)
    {
        var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(TasksFile, json);
    }

    public static void AddTask(string description)
    {
        var tasks = LoadTasks();
        var task = new Task
        {
            Id = tasks.Count + 1,
            Description = description,
            Status = "todo",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        tasks.Add(task);
        SaveTasks(tasks);
        Console.WriteLine($"Task added successfully (ID: {task.Id})");
    }

    public static void UpdateTask(int id, string description)
    {
        var tasks = LoadTasks();
        var task = tasks.Find(t => t.Id == id);
        if (task != null)
        {
            task.Description = description;
            task.UpdatedAt = DateTime.Now;
            SaveTasks(tasks);
            Console.WriteLine($"Task {id} updated successfully");
        }
        else
        {
            Console.WriteLine($"Task {id} not found");
        }
    }

    public static void DeleteTask(int id)
    {
        var tasks = LoadTasks();
        tasks.RemoveAll(t => t.Id == id);
        SaveTasks(tasks);
        Console.WriteLine($"Task {id} deleted successfully");
    }

    public static void MarkInProgress(int id)
    {
        UpdateStatus(id, "in-progress");
    }

    public static void MarkDone(int id)
    {
        UpdateStatus(id, "done");
    }

    public static void UpdateStatus(int id, string status)
    {
        var tasks = LoadTasks();
        var task = tasks.Find(t => t.Id == id);
        if (task != null)
        {
            task.Status = status;
            task.UpdatedAt = DateTime.Now;
            SaveTasks(tasks);
            Console.WriteLine($"Task {id} marked as {status}");
        }
        else
        {
            Console.WriteLine($"Task {id} not found");
        }
    }

    public static void ListTasks(string status = null)
    {
        var tasks = LoadTasks();
        foreach (var task in tasks)
        {
            if (status == null || task.Status == status)
            {
                Console.WriteLine($"ID: {task.Id}, Description: {task.Description}, Status: {task.Status}, Created At: {task.CreatedAt}, Updated At: {task.UpdatedAt}");
            }
        }
    }
}

class Program
{
    static int Main(string[] args)
    {
        var rootCommand = new RootCommand();

        var addCommand = new Command("add", "Add a new task")
        {
            new Argument<string>("description")
        };
        addCommand.Handler = CommandHandler.Create<string>((description) =>
        {
            TaskManager.AddTask(description);
        });

        var updateCommand = new Command("update", "Update an existing task")
        {
            new Argument<int>("id"),
            new Argument<string>("description")
        };
        updateCommand.Handler = CommandHandler.Create<int, string>((id, description) =>
        {
            TaskManager.UpdateTask(id, description);
        });

        var deleteCommand = new Command("delete", "Delete a task")
        {
            new Argument<int>("id")
        };
        deleteCommand.Handler = CommandHandler.Create<int>((id) =>
        {
            TaskManager.DeleteTask(id);
        });

        var markInProgressCommand = new Command("mark-in-progress", "Mark a task as in progress")
        {
            new Argument<int>("id")
        };
        markInProgressCommand.Handler = CommandHandler.Create<int>((id) =>
        {
            TaskManager.MarkInProgress(id);
        });

        var markDoneCommand = new Command("mark-done", "Mark a task as done")
        {
            new Argument<int>("id")
        };
        markDoneCommand.Handler = CommandHandler.Create<int>((id) =>
        {
            TaskManager.MarkDone(id);
        });

        var listCommand = new Command("list", "List tasks")
        {
            new Argument<string>("status", () => null, "Status of tasks to list (optional)")
        };
        listCommand.Handler = CommandHandler.Create<string>((status) =>
        {
            TaskManager.ListTasks(status);
        });

        rootCommand.AddCommand(addCommand);
        rootCommand.AddCommand(updateCommand);
        rootCommand.AddCommand(deleteCommand);
        rootCommand.AddCommand(markInProgressCommand);
        rootCommand.AddCommand(markDoneCommand);
        rootCommand.AddCommand(listCommand);

        return rootCommand.InvokeAsync(args).Result;
    }
}
