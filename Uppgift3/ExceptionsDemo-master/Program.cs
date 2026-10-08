namespace ExceptionsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new ProcessingService();

            {
                Console.WriteLine("=== Start av programmet ===");

                // Exempel 1: try-catch-finally
                try
                {
                    Console.WriteLine("Försöker läsa fil och räkna...");
                    var path = Path.Combine(AppContext.BaseDirectory, "numbers.txt");
                    var result = service.ProcessFile(path);

                    Console.WriteLine($"\nResultat: {result}");
                }
                catch (FileNotFoundException ex)
                {
                    // Specifikt fel om filen inte finns
                    Console.WriteLine($"Filen hittades inte: {ex.Message}");
                }
                catch (FormatException ex)
                {
                    // Specifikt fel om texten inte kan tolkas som tal
                    Console.WriteLine($"Formatfel: {ex.Message}");
                }
                catch (DivideByZeroException ex)
                {
                    // Specifikt fel om nolldivision
                    Console.WriteLine($"Kan inte dividera med noll: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Fallback för alla övriga obekanta fel
                    Console.WriteLine($"Okänt fel: {ex.Message}");
                }
                finally
                {
                    // Körs ALLTID, även om det blev undantag
                    Console.WriteLine("Cleanup: Logging avslutat anrop.");
                }

                Console.WriteLine("Programmet avslutas normalt.");
            }
        }
    }
}

