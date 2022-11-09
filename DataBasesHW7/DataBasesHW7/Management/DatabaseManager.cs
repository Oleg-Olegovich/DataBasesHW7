namespace DataBasesHW7.Management;

using AppContext = DataBasesHW7.Database.AppContext;

public class DatabaseManager
{
    private static AppContext _appDatabase = null!;
    
    private static readonly string[] MainMenuText =
    {
        "You are welcomed by the Database manager!", "",
        "Choose option:",
        "0 - Viewing tables.",
        "1 - Adding 1 value to the Olympiad table.",
        "2 - Adding 1 value to the country table.",
        "3 - Adding 1 value to the event table.",
        "4 - Adding 1 value to the players table.",
        "5 - Adding 1 value to the results table.",
        "6 - Exit the program."
    };

    /// <summary>
    /// This array contains the delegates for the methods 
    /// that implement all of the user commands.
    /// </summary>
    private static readonly Action[] Commands =
    {
        ShowAllTables,
        AddOlympiad,
        AddCountry,
        AddEvent,
        AddSportsman,
        AddResult,
        Exit
    };

    public static void Launch()
    {
        using var db = new AppContext();

        if (db.Countries is null || !db.Countries.Any())
        {
            return;
        }
        
        Database.CreateDatabase(db);
        _appDatabase = db;
        
        Console.WriteLine("This program writes data to the database." + Environment.NewLine);
        ProcessMainMenu();
    }

    /// <summary>
    /// This method outputs an any error message.
    /// </summary>
    private static void PrintError(string errorMessage)
        => Console.WriteLine(errorMessage + "Try again. ");
    
    /// <summary>
    /// This method correctly processes the input data (for any type and validator).
    /// </summary>
    private static T? GetData<T>(string helpMessage, bool readOneChar, Action? afterCancelMethod = null,
        Predicate<T?>? dataValidator = null, string? helpErrorMessage = null)
    {
        Console.Write(helpMessage);
        if (afterCancelMethod != null)
        {
            Console.Write("Press \"Escape\" to cancel the input. ");
        }

        Console.WriteLine();
        var correctInput = false;
        T? result = default;
        while (correctInput == false)
        {
            try
            {
                //It is necessary for the implementation of the cancel input.
                var key = Console.ReadKey();
                if (afterCancelMethod != null && key.Key == ConsoleKey.Escape)
                {
                    afterCancelMethod();
                }

                // This is an attempt to convert the entered string to the desired type.
                result = (T) Convert.ChangeType(key.KeyChar
                                                + (readOneChar ? "" : Console.ReadLine()), typeof(T));
                /* If there are no additional data requirements or these requirements 
                 * are met, correctInput is set to true. Else program print error message.*/
                correctInput = dataValidator?.Invoke(result) ?? true;
                if (correctInput == false)
                {
                    PrintError("Input doesn't meet all conditions. ");
                    if (helpErrorMessage != null)
                    {
                        PrintError(helpErrorMessage);
                    }
                }

                Console.WriteLine();
            }
            catch
            {
                // Program catch an exception if the conversion fails.
                PrintError("Incorrect input. ");
                if (helpErrorMessage != null)
                {
                    PrintError(helpErrorMessage);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// This method correctly handles the input of a single 
    /// number in range [leftBorder; rightBorder].
    /// </summary>
    private static int GetIntInput(int leftBorder, int rightBorder, bool oneDigit, Action? afterCancelMethod)
        => GetData<int>($"Input number in the range: [{leftBorder}; {rightBorder}]. ",
            oneDigit, afterCancelMethod, x => leftBorder <= x && x <= rightBorder);

    /// <summary> 
    /// Method draw main menu and implements command selection.
    /// </summary>
    private static void ProcessMainMenu()
    {
        while (true)
        {
            // Show user interface.
            Drawer.DrawInterface(1, MainMenuText, false);
            var command = GetIntInput(0, 7, true, ProcessMainMenu);
            try
            {
                Commands[command]();
                if (command == Commands.Length)
                {
                    break;
                }
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Unfortunately, failed to execute the command. ");
                Thread.Sleep(1500);
            }
        }
    }

    private static void ShowAllTables()
    {
        Database.ShowAllTables(_appDatabase);
    }
    
    private static void AddOlympiad()
    {
        Database.AddOlympiad(_appDatabase);
    }
    
    private static void AddCountry()
    {
        Database.AddCountry(_appDatabase);
    }
    
    private static void AddEvent()
    {
        Database.AddEvent(_appDatabase);
    }
    
    private static void AddSportsman()
    {
        Database.AddSportsman(_appDatabase);
    }
    
    private static void AddResult()
    {
        Database.AddResult(_appDatabase);
    }

    /// <summary>
    /// This method calls exit screen.
    /// </summary>
    private static void Exit()
    {
        // Show user interface.
        Drawer.DrawInterface(Drawer.Height / 2, "All the best!", true);
    }
}