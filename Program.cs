using Figgle;
using System;
using System.IO;
using System.Diagnostics;

namespace Word_Array_Manager
{
    internal class Program
    {
        private static Node[] nodeArray = new Node[0];

        private static string currentSortingMethod = "None"; // Initialize with a default value

        static void MainTitle()
        {
            string titleText = "Word Array Manager";
            string asciiArt = FiggleFonts.Standard.Render(titleText);
            Console.Write(asciiArt + "COMP605 - Data Structures and Algorithms\n" + "v1.0\n" + "By Hamish Getty\n");

            PrintLineBreak();

            Console.Write("This application can:\n" +
                "- Load File: Insert words from a selected file into the node array.\n" +
                "- Sort using O(n^2) (Bubble Sort): Arrange words in the node array using the Bubble Sort algorithm.\n" +
                "- Sort using O(nlogn) (Quick Sort): Arrange words in the node array using the Quick Sort algorithm.\n" +
                "- Print Operations: Display information about the node array and the current sorting method.\n" +
                "- Exit: Close the application.\n\n");

            Console.Write("Press any key to get started: ");

            Console.ReadKey();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                PrintLineBreak();

                Console.Write("1. Load File\n" +
                    "2. Sort using O(n^2) (Bubble Sort)\n" +
                    "3. Sort using O(nlogn) (Quick Sort)\n" +
                    "4. Print Operations\n" +
                    "5. Exit\n" +
                    "Select an option: ");

                string choice = Console.ReadLine();

                Stopwatch stopwatch = new Stopwatch();

                switch (choice)
                {
                    case "1":
                        FileMenu();
                        break;

                    case "2":
                        Console.WriteLine("Sorting using BubbleSort...");
                        stopwatch.Start();

                        BubbleSort(nodeArray);

                        stopwatch.Stop();

                        currentSortingMethod = "Bubble Sort";

                        Console.WriteLine($"Array sorted using Bubble Sort.");
                        Console.WriteLine($"Time taken: {stopwatch.Elapsed.TotalMilliseconds} milliseconds");
                        Console.Write("Press any key to continue: ");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.WriteLine("Sorting using QuickSort...");

                        // Start the stopwatch before calling QuickSort
                        stopwatch.Start();

                        QuickSort(nodeArray, 0, nodeArray.Length - 1);

                        // Stop the stopwatch after QuickSort completes
                        stopwatch.Stop();

                        // Update the current sorting method variable
                        currentSortingMethod = "Quick Sort";

                        Console.WriteLine($"Array sorted using Quick Sort.");
                        Console.WriteLine($"Time taken: {stopwatch.Elapsed.TotalMilliseconds} milliseconds");
                        Console.Write("Press any key to continue: ");
                        Console.ReadKey();
                        break;

                    case "4":
                        PrintNodeArray(nodeArray, currentSortingMethod);
                        break;

                    case "5":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.Write("Press any key to continue: ");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void PrintLineBreak()
        {
            int consoleWidth = Console.WindowWidth;
            string dashes = new string('-', consoleWidth);
            Console.Write(dashes);
        }

        static void Main(string[] args)
        {
            MainTitle();

            while (true)
            {
                MainMenu();
            }
        }

        static void FileMenu()
        {
            while (true)
            {
                PrintLineBreak();

                Console.WriteLine("Choose a file to insert into the node array:");

                string[] files = Directory.GetFiles("random");

                for (int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {Path.GetFileName(files[i])}");
                }

                Console.Write("Select an option: ");
                string fileChoice = Console.ReadLine();

                int selectedFileIndex;
                if (int.TryParse(fileChoice, out selectedFileIndex) && selectedFileIndex >= 1 && selectedFileIndex <= files.Length)
                {
                    string filePath = files[selectedFileIndex - 1];
                    PrintLineBreak();

                    // Measure the time taken for the operation
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    Console.WriteLine($"Inserting words from {filePath} into the node array...");

                    ReadFile(filePath);

                    stopwatch.Stop();
                    TimeSpan elapsedTime = stopwatch.Elapsed;

                    Console.WriteLine($"Time taken: {elapsedTime.TotalMilliseconds} milliseconds");

                    break;
                }
                else
                {
                    PrintLineBreak();
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.Write("Press any key to continue: ");
                    Console.ReadKey();
                    MainMenu();
                }
            }
        }

        static void ReadFile(string filePath)
        {
            try
            {
                // Measure the time taken for the operation
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                string[] lines = File.ReadAllLines(filePath);

                Array.Resize(ref nodeArray, lines.Length);

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (!line.StartsWith("#") && !string.IsNullOrWhiteSpace(line))
                    {
                        Node node = new Node(line);

                        nodeArray[i] = node;

                        Console.WriteLine($"Word '{line}' inserted into the node array.");
                    }
                }

                currentSortingMethod = "None"; // Update the current sorting method variable

                stopwatch.Stop();
                TimeSpan elapsedTime = stopwatch.Elapsed;

                PrintLineBreak();
                Console.WriteLine($"{lines.Length} words inserted into the node array successfully.");
                Console.WriteLine($"Time taken: { elapsedTime.TotalMilliseconds} milliseconds");
                Console.Write("Press any key to continue: ");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void BubbleSort(Node[] nodeArray)
        {
            int n = nodeArray.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    // Check for null references before comparing
                    if (nodeArray[j] != null && nodeArray[j + 1] != null)
                    {
                        // Compare adjacent elements and swap if they are in the wrong order
                        if (nodeArray[j].Word.CompareTo(nodeArray[j + 1].Word) > 0)
                        {
                            // Swap
                            Node temp = nodeArray[j];
                            nodeArray[j] = nodeArray[j + 1];
                            nodeArray[j + 1] = temp;
                        }
                    }
                }
            }
        }

        static void QuickSort(Node[] nodeArray, int low, int high)
        {

            if (low < high)
            {
                // Find the partitioning index
                int partitionIndex = Partition(nodeArray, low, high);

                // Recursively sort the elements before and after the partition index
                QuickSort(nodeArray, low, partitionIndex - 1);
                QuickSort(nodeArray, partitionIndex + 1, high);
            }
        }

        static int Partition(Node[] nodeArray, int low, int high)
        {
            Node pivot = nodeArray[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                // Check for null references before comparing
                if (nodeArray[j] != null && pivot != null)
                {
                    if (nodeArray[j].Word.CompareTo(pivot.Word) <= 0)
                    {
                        i++;
                        // Swap nodeArray[i] and nodeArray[j]
                        Node temp = nodeArray[i];
                        nodeArray[i] = nodeArray[j];
                        nodeArray[j] = temp;
                    }
                }
            }

            // Swap nodeArray[i + 1] and pivot
            if (nodeArray[i + 1] != null && pivot != null)
            {
                Node tempPivot = nodeArray[i + 1];
                nodeArray[i + 1] = nodeArray[high];
                nodeArray[high] = tempPivot;
            }

            return i + 1;
        }

        static void PrintNodeArray(Node[] nodeArray, string sortingMethod = "")
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < nodeArray.Length; i++)
            {
                if (nodeArray[i] != null)
                {
                    Console.WriteLine($"Array Location: {i}");
                    Console.WriteLine($"Word: {nodeArray[i].Word}");
                    Console.WriteLine($"Length: {nodeArray[i].Length}");
                    Console.WriteLine();
                }
            }

            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;

            Console.WriteLine($"Printed all elements in the node array (Sorted using {sortingMethod}).");
            Console.WriteLine($"Time taken: {elapsedTime.TotalMilliseconds} milliseconds");

            Console.Write("Press any key to continue: ");
            Console.ReadKey();
        }

    }
}