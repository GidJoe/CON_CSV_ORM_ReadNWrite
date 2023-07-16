using System;


namespace CON_CSVReadNWrite
{

    /* CSV und ORM 
    *  v 1.1 von Marc Winter IT42+
    *  2021-10-14
    *   - ORM Beispiele hinzugefügt
    *
    * Hier sind ein paar Beispiele, wie mann mit CSV Datein arbeiten kann.
    * Im Grunde haben wir hier dauerhafte Stringmanipulationen
    * Durch das Speichern in einer List<String> haben wir die möglichkeit in Rows/Collums zu arbeiten
    *   
    * - Der CSVHandler kann CSV Datein einlesen und in eine List<String> speichern
    *   - Beispielhafte Methoden sind:
    *       - LadeCSV(string filePath) -> Lädt eine CSV Datei und gibt sie als List<String> zurück
    *       - ShowCollumByPosition(List<string[]> data) -> Gibt eine ganze Collum aus, anhand einer Position.
    *       - ShowRowByPosition(List<string[]> data, int rowIndex) -> Gibt eine Row aus anhand einer Position.
    *       - WriteCompleteCSVtoConsole(List<string[]> data) -> Gibt die komplette CSV in die Konsole aus
    *       - SearchForFirstRowByString(List<string[]> data, string searchString) -> Sucht nach einem String in der CSV und gibt die erste Row aus in der der String vorkommt
    *       - SearchAllRowsByString(List<string[]> data, string searchString) -> Sucht nach einem String in der CSV und gibt alle Rows aus in denen der String vorkommt
    *       - SpeicherCSV(string filePath, List<string[]> data) -> Speichert die CSV Datei
    *     
    *       
    *   
    *   - Der CSVMapper gibt einfache Besipiele für ORM (Object Relational Mapping)
    *       - Hier wird aus der CSV Datei eine Liste von Objekten erstellt
    *           - Beispielhafte Methoden sind:
    *           - ReadCSV(string filePath) -> Lädt eine CSV Datei und gibt sie als List<Person> zurück
    *           - GetAllObjects(List<Person> people) -> Gibt alle Objekte aus
    *           - GetObjectByName(List<Person> people, string name) -> Gibt ein Objekt aus anhand des Namens
    *           - Durchschnittsalter(List<Person> people) -> Gibt das Durchschnittsalter aller Personen aus
    * 
    */



    class Program
    {
        // Pfad zur CSV Datei
        static string path = "C:\\Users\\MWB\\OneDrive\\CloudRepos\\CON_CSVReadNWrite\\CON_CSVReadNWrite\\testcsv.csv";

        /*
         * Pfad zum Desktop des ausführenden Users
         * static string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\testcsv.csv";
        */




        static void Main()
        {
            /* CSV Methoden Beispiele
            * Zeigt eine komplette Row über Position aus
            * csvHandler.ShowRowByPosition(csvData, 1);
            *             
            * Durchsucht die CSV und gibt die erste ROW aus in dem der übergebende String vorkommt
            * csvHandler.SearchForFirstRowByString(csvData, "Bob Johnson");
            * 
            * Durchsucht die CSV und gibt alle ROWs aus in denen der übergebede String vorkommt
            * csvHandler.SearchAllRowsByString(csvData, "Bob Johnson");
            * 
            * Schreibt die komplette CSV in die Konsole
            * csvHandler.WriteCompleteCSVtoConsole(csvData);
            * 
            * 
            *
            */


            CsvHandler csvHandler = new CsvHandler();
            CSVMapper csvMapper = new CSVMapper();

            // Einelsen der CSV Datei als Liste von String Arrays
            List<string[]> csvData = csvHandler.LadeCSV(path);
            List<Person> personen = csvMapper.ReadCSV(path);



            //csvMapper.GetAllObjects(personen);
            //csvMapper.GetObjectsByString(personen, "David Clark");
            //csvMapper.SearchAllObjects(personen, "42");



            int anzahlPersonen;
            double alterGesammt;

            csvHandler.WriteCompleteCSVtoConsole(csvData);
            csvMapper.BerechneAnzahlUndDurchschnittsalter(personen, out anzahlPersonen, out alterGesammt);
            Console.WriteLine("____________________________________________________________________________" + "\n");
            Console.WriteLine("Anzahl aller Personen: " + anzahlPersonen);
            Console.WriteLine("Durchschnittsalter: " + (alterGesammt / anzahlPersonen));
            AnyKeyToExit();

        }

        static void AnyKeyToExit()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }


        #region CSVHandler
        class CsvHandler
        {
            public List<string[]> LadeCSV(string filePath)
            {
                List<string[]> rows = new List<string[]>();

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        // Delimiter ist ein Komma, kann angepasst werden.
                        string[] fields = line.Split(',');

                        rows.Add(fields);
                    }
                }

                return rows;
            }


            public void ShowCollumByPosition(List<string[]> data)
            {
                foreach (string[] collum in data)
                {
                    string phoneNumber = collum[2];
                    Console.WriteLine(phoneNumber);
                }
            }

            public void ShowRowByPosition(List<string[]> data, int rowIndex)
            {
                if (rowIndex >= 0 && rowIndex < data.Count)
                {
                    string[] row = data[rowIndex];
                    string line = string.Join("\t\t", row);
                    Console.WriteLine(line);
                }
                else
                {
                    Console.WriteLine("Falscher Zeilenindex.");
                }
            }

            public void WriteCompleteCSVtoConsole(List<string[]> data)
            {
                // Bestimte die maximale weite für jede Spalte
                int[] columnWidths = new int[data[0].Length];
                for (int i = 0; i < data[0].Length; i++)
                {
                    columnWidths[i] = data.Max(row => row[i].Length);
                }

                foreach (string[] row in data)
                {
                    string line = "";
                    for (int i = 0; i < row.Length; i++)
                    {
                        line += row[i].PadRight(columnWidths[i] + 2); // PadRight füllt auf
                    }
                    Console.WriteLine(line);
                }
            }


            public void SearchAllRowsByString(List<string[]> data, string entry)
            {
                bool found = false;

                Console.WriteLine("Folgende Reihe mit " + entry + "wurden gefunden");
                Console.WriteLine("\n");
                foreach (string[] row in data)
                {
                    foreach (string field in row)
                    {
                        if (field.Equals(entry, StringComparison.OrdinalIgnoreCase))
                        {
                            string line = string.Join("\t\t\t", row);
                            Console.WriteLine(line);
                            found = true;
                            break;
                        }
                    }

                }

                if (!found)
                {
                    Console.WriteLine("Eintrag nicht gefunden");
                }
            }

            public void SearchForFirstRowByString(List<string[]> data, string entry)
            {
                bool found = false;

                Console.WriteLine("Folgende Reihe mit " + entry + "wurden gefunden");
                Console.WriteLine("\n");
                foreach (string[] row in data)
                {
                    foreach (string field in row)
                    {
                        if (field.Equals(entry, StringComparison.OrdinalIgnoreCase))
                        {
                            string line = string.Join(",", row);
                            Console.WriteLine(line);
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        break;
                    }
                }

                if (!found)
                {
                    Console.WriteLine("Entry not found.");
                }
            }

            public void SpeicherCSV(string filePath, List<string[]> data)
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string[] row in data)
                    {
                        string line = string.Join(",", row);
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine("CSV erfolgreich bearbeitet und gespeichert.");
                Console.WriteLine("Speicherort: " + filePath);
            }
        }
        #endregion


        #region CSV Mapper und Methoden
        public class CSVMapper
        {
            public List<Person> ReadCSV(string csvData)
            {
                List<Person> people = new List<Person>();

                try
                {
                    using (StreamReader reader = new StreamReader(csvData))
                    {
                        string header = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string[] fields = line.Split(',');

                            Person person = new Person
                            {
                                Name = fields[0],
                                Email = fields[1],
                                PhoneNumber = fields[2],
                                Age = int.Parse(fields[3])
                            };

                            people.Add(person);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Es gab einen fehler beim lesen der CSV file: " + ex.Message);
                }

                return people;
            }


            public void GetObjectsByString(List<Person> data, string name)
            {
                foreach (Person person in data)
                {
                    if (person.Name.Equals(name))
                    {
                        Console.WriteLine("Name: " + person.Name);
                        Console.WriteLine("Email: " + person.Email);
                        Console.WriteLine("Phone Number: " + person.PhoneNumber);
                        Console.WriteLine("Age: " + person.Age);
                        Console.WriteLine();
                    }

                }
            }

            public void GetAllObjects(List<Person> data)
            {
                foreach (Person person in data)
                {
                    Console.WriteLine("Name: " + person.Name);
                    Console.WriteLine("Email: " + person.Email);
                    Console.WriteLine("Phone Number: " + person.PhoneNumber);
                    Console.WriteLine("Age: " + person.Age);
                    Console.WriteLine();
                }
            }

            public void SearchAllObjects(List<Person> data, string search)
            {
                foreach (Person person in data)
                {
                    if (person.Name.Contains(search) || person.Email.Contains(search) || person.PhoneNumber.Contains(search) || Convert.ToString(person.Age) == (search))
                    {


                        Console.WriteLine("Name: " + person.Name);
                        Console.WriteLine("Email: " + person.Email);
                        Console.WriteLine("Phone Number: " + person.PhoneNumber);
                        Console.WriteLine("Age: " + person.Age);
                        Console.WriteLine();
                    }

                }
            }


            public void BerechneAnzahlUndDurchschnittsalter(List<Person> people, out int anzahlPersonen, out double alterGesammt)
            {
                anzahlPersonen = 0;
                alterGesammt = 0;

                foreach (Person person in people)
                {
                    if (person.Name.Any())
                    {
                        anzahlPersonen++;
                    }

                    if (person.Age > 0)
                    {
                        alterGesammt += person.Age;
                    }
                }
            }
        }
        #endregion
    }
}
