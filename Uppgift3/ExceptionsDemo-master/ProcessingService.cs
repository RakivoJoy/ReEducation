namespace ExceptionsDemo
{
    /// <summary>
    /// Service class that handles file processing and exception handling demonstration
    /// </summary>
    public class ProcessingService
    {
        /// <summary>
        /// Processes a file containing a number and calculates 100.0 divided by that number.
        /// </summary>

        public double ProcessFile(string fileName)
        {
            // Om filnamnet är tomt: logiskt fel vi vill signalera
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Filnamn får inte vara tomt eller null.", nameof(fileName));
            }

            StreamReader? reader = null;
            try
            {
                reader = new StreamReader(fileName);

                string? line = reader.ReadLine();
                if (line == null)
                    throw new InvalidOperationException("Filen är tom.");

                // Försöker omvandla text till tal
                int number = int.Parse(line); // Kan ge FormatException

                // Division: kan ge DivideByZeroException
                return 100.0 / number;
            }
            catch (FormatException ex)
            {
                // Vi kan logga eller omformulera felet
                Console.WriteLine($"Formatfel i ProcessFile: {ex.Message}");
                // Vi kan välja att låta metoden "kasta upp" felet
                throw; // När du i `catch` bara vill logga/analysera,
                       // men låta anroparen (t.ex. en högre nivå i applikationen)
                       // bestämma hur man ska återhämta sig.
            }
            catch (Exception ex)
            {
                // Om vi vill ge en mer meningsfull feltyp till anroparen
                throw new InvalidOperationException(
                    "Det gick inte att processa filen.",
                    ex); // InnerException = ursprunglig fel
            }
            finally
            {
                // Garanterad stängning av resurs
                reader?.Close();
                Console.WriteLine("finally i ProcessFile: StreamReader stängd.");
            }
        }
    }
}
