using System.IO;
using System.Text.RegularExpressions;


namespace Notez
{
    class Program
    {
        static void Main(string[] args)
        {
            const string header = "/*************************************************************\n" +
                                  "|                                                            |\n" +
                                  "|                        Basil Notes                         |\n" +
                                  "|                                                            |\n" +
                                  "*************************************************************/\n";


            const string menu = "Welcome to Basil Notes!!\n\n" +
                "Please select one of the following options:\n" +
                "  1.  List Notebooks\n" +
                "  2.  Read Notes\n" +
                "  3.  Take Notes\n" +
                "  4.  Add Notebook\n" +
                "  5.  Delete Notebook\n" +
                "  99. Exit";

            const string notebookpath = @"./Notebooks/";

            void ListNotebooks()
            {
                IEnumerable<string> directory = Directory.EnumerateFiles(notebookpath);
                foreach (string file in directory)
                {
                    Console.WriteLine($"  {file.Replace(notebookpath, "").Replace(".ntbk", "")}");
                }

                return;
            }

            bool notebookExists(string notebookName)
            {
                IEnumerable<string> directory = Directory.EnumerateFiles(notebookpath);

                return directory.Contains(notebookpath + notebookName + ".ntbk");
            }

            void CreateNotebook()
            {
                do
                {
                    // TO-DO instead of if-else create try-catch
                    Console.WriteLine("Enter the name of the notebook below:");
                    string notebookname = Console.ReadLine() ?? "";
                    if (notebookExists(notebookname))
                    {
                        Console.WriteLine("That notebook already exists. Creating will overwrite the file.");   
                        Console.Write("Type CONTINUE to overwrite the file: ");
                        string answer = Console.ReadLine() ?? "";
                        if (answer != "CONTINUE")
                        {
                            continue;
                        }
                    }
                    if (notebookname != "")
                    {
                        FileStream file = File.Create(notebookpath + notebookname + ".ntbk");
                        file.Close();
                        Console.WriteLine($"Successfully created {notebookname}");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("You must enter a valid file name!");
                    }
                } while (true);
            }

            void DeleteNotebook()
            {
                Console.WriteLine("Delete Notebook:");
                ListNotebooks();
                do
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter the name of the notebook you would like to delete:");
                    string notebookname = Console.ReadLine() ?? "";
                    if (notebookExists(notebookname))
                    {
                        File.Delete(notebookpath + notebookname + ".ntbk");
                        Console.WriteLine($"Successfully deleted: {notebookname}");
                        Console.WriteLine("Press any key to return to main menu...");
                        Console.ReadKey();
                        return;
                    } else
                    {
                        Console.WriteLine("Could not find that notebook. Please try again.");
                    }
                } while (true);
            }

            void takeNotes()
            {
                //TO-DO add multi-line note functionality
                Console.WriteLine("Take Note:");
                ListNotebooks();
                do
                {
                    Console.WriteLine("Enter the name of the notebook you'd like to add a note to:");
                    string notebookname = Console.ReadLine() ?? "";
                    if (notebookExists(notebookname))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Enter your note below:");
                        string note = Console.ReadLine() ?? "";
                        do
                        {
                            if (note != "")
                            {
                                using (StreamWriter sw = File.AppendText(notebookpath + notebookname + ".ntbk"))
                                {
                                    DateTime now = DateTime.Now;
                                    sw.WriteLine($"{now.ToString("yyyy-MM-dd hh:mm:ss tt")} - {note}");
                                    Console.WriteLine("Note added! Press any key to return to the main menu...");
                                    Console.ReadKey();
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine("The note cannot be blank. Please try again.");
                            }
                        } while (true);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid notebook name!");
                    }
                } while (true);

            }

            void readNotes()
            {
                Console.WriteLine("Read Notes:");
                ListNotebooks();
                Console.WriteLine("Enter the name of the notebook you'd like to read.");
                string notebookname = Console.ReadLine() ?? "";
                do
                {
                    if (notebookExists(notebookname))
                    {
                        Console.Clear();
                        foreach (string line in File.ReadLines(notebookpath + notebookname + ".ntbk"))
                        {
                            Console.WriteLine(line);
                        }

                        Console.WriteLine();
                        Console.WriteLine("Notes loaded. Press any key to return to main menu...");
                        Console.ReadKey();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid notebook name!");
                    }
                } while (true);
            }


            bool exit = false;
            do
            {
                Console.Clear();
                Console.WriteLine(header);
                Console.WriteLine(menu);

                Console.Write("Enter number here: ");
                bool isParsed = int.TryParse(Console.ReadLine(), out int answer);

                if (isParsed)
                    switch (answer)
                    {
                        case 1:
                            Console.Clear();
                            Console.WriteLine("List Notebooks:");
                            ListNotebooks();
                            Console.WriteLine();
                            Console.WriteLine("Press any key to retun to main menu...");
                            Console.ReadKey();
                            break;
                        case 2:
                            Console.Clear();
                            readNotes();
                            break;
                        case 3:
                            Console.Clear();
                            takeNotes();
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("Add Notebook:");
                            CreateNotebook();
                            Console.WriteLine("Press any key to retun to main menu...");
                            Console.ReadKey();
                            break;
                        case 5:
                            Console.Clear();
                            DeleteNotebook();
                            break;
                        case 99:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("  Not a valid number option. Press any key to try again...");
                            Console.ReadKey();
                            break;
                    } else
                {
                    Console.WriteLine("  Not a valid number option. Press any key to try again...");
                    Console.ReadKey();
                }

            } while (!exit);
            Console.Clear();
            Console.WriteLine("Thank you, come again");
        }
    }
}
