using System;
using System.Security.Cryptography;
using System.Text;


/*
 * This class represents an executable that retrieves a key,
 * checks user input against the key, and performs some action based on the result.
 */
internal class Executable
{
    // The hash of the primary key
    protected internal byte[] primaryKeyHash;

    // Indicates whether the key retrieval process has completed
    protected internal bool generationCompleted;

    // Indicates whether the check process has completed
    protected internal bool checkCompleted;


    // The main entry point of the program
    private static void Main()
    {
        // Clear the console
        Console.Clear();

        // Create a new instance of the Executable class
        var localInstance = new Executable();

        // Retrieve the key
        localInstance.Retrieve_Key();

        // Prompt the user to enter the key
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("Please Enter the Key to Continue");
        Console.ForegroundColor = ConsoleColor.White;
        var userInput = Console.ReadLine();

        // Check the user's input against the key
        localInstance.Check(userInput, out var status);

        // Ensure the check process has completed
        localInstance.Check2();

        // Display the result of the check
        switch (status)
        {
            case InputStatus.Incorrect:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong Passcode, Sorry");
                Console.ReadLine();
                break;
            case InputStatus.Succes:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Congrats you got the Passcode!");
                Console.ReadLine();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // Exit the program
        Environment.Exit(0);
    }


    // Ensures the check process has completed, and exits the program if not
    protected internal void Check2()
    {
        if (checkCompleted) return;
        Environment.Exit(11);
    }


    /*
     * Checks the user's input against security criteria.
     * Sets the status to InputStatus.Incorrect if the input doesn't meet the criteria,
     * or InputStatus.Succes if it does.
     */
    protected internal void Check(string userInput, out InputStatus status)
    {
        // Initialize the user's hash
        byte[] userHash;

        // Pre-check: Ensure the user's input is long enough and the key has been generated
        if (userInput.Length < 10 || !generationCompleted)
        {
            status = InputStatus.Incorrect;
            checkCompleted = true;
            return;
        }

        // Compute the hash of the user's input
        using (var sha = SHA256.Create())
            userHash = sha.ComputeHash(Encoding.UTF8.GetBytes(userInput));

        // Check 1: Ensure the user's input matches the primary key
        if (userInput != Environment.GetEnvironmentVariable("key")) {
            status = InputStatus.Incorrect;
            checkCompleted = true;
            return;
        }

        // Check 2: Ensure the user's input matches the auxiliary key
        if (userInput != Environment.GetEnvironmentVariable("key2")) {
            status = InputStatus.Incorrect;
            checkCompleted = true;
            return;
        }

        // Check 3: Ensure the hash of the user's input matches the hash of the primary key
        var userHashString = BitConverter.ToString(userHash).Replace("-", "");
        var checkHashString = BitConverter.ToString(primaryKeyHash).Replace("-", "");
        if (userHashString != checkHashString) {
            status = InputStatus.Incorrect;
            checkCompleted = true;
            return;
        }

        // If all checks pass, set the status to success
        status = InputStatus.Succes;
        checkCompleted = true;
        return;
    }


    protected internal virtual void Retrieve_Key()
    {
        // Generate a Random Number (64 Bytes)
        var randomNumber = new byte[64];
        using (var rng = new RNGCryptoServiceProvider())
            rng.GetBytes(randomNumber);

        // Convert and set the main Key and the auxillary Key
        Environment.SetEnvironmentVariable("key", BitConverter.ToString(randomNumber).Replace("-", ""));
        Environment.SetEnvironmentVariable("key2", BitConverter.ToString(randomNumber).Replace("-", ""));

        // Load the Envoirment variables (Keys) to Compare
        var primaryKeyString = Environment.GetEnvironmentVariable("key");
        var auxillaryKeyString = Environment.GetEnvironmentVariable("key2");

        using (var sha256 = SHA256.Create())
        {
            // Calculate the Hash for the Main Key
            if (primaryKeyString != null) primaryKeyHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(primaryKeyString));
            if (auxillaryKeyString != null)
            {
                // Calculate the Hash for the Auxillary Key
                var auxiallaryKeyHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(auxillaryKeyString));

                // Compare the hashes of the keys
                var compare1 = BitConverter.ToString(primaryKeyHash).Replace("-", "");
                var compare2 = BitConverter.ToString(auxiallaryKeyHash).Replace("-", "");

                // If the hashes don't match, exit the program
                if (compare1 != compare2) Environment.Exit(10);
            }
        }

        //Confirm the Function has been executed and return to the Main fuction
        generationCompleted = true;
        return;
    }


    // Represents the possible statuses of the input check
    internal enum InputStatus
    {
        Succes,
        Incorrect
    }
}
