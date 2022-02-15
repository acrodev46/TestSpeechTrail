// See https://aka.ms/new-console-template for more information
using Microsoft.CognitiveServices.Speech;
using System.Text.RegularExpressions;

ContinousSpeechRecognitionAsync().Wait();
Console.ReadKey();

static async Task ContinousSpeechRecognitionAsync()
{
    var config = SpeechConfig.FromSubscription("d4cf255084d642118b77921b5ac1cc73", "francecentral");
    using (var recognizer = new SpeechRecognizer(config, "fr-FR"))
    {
        recognizer.Recognizing += (sender, args) => { Console.WriteLine($"RECOGNIZING: {args.Result.Text}"); };
        recognizer.Recognized += (sender, args) =>
        {
            var result = args.Result;
            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                Console.WriteLine($"Final statement: {result.Text}");
                var dossards = Regex.Split(result.Text, @"\D+"); ;
                foreach (var dossard in dossards)
                {
                    if (int.TryParse(dossard, out _))
                    {
                        Console.WriteLine($"Dossard: {dossard}");
                    }
                }
            }
        };

        recognizer.SessionStarted += (sender, args) => { Console.WriteLine("\n Session has started. You can start speaking..."); };
        recognizer.SessionStopped += (sender, args) => { Console.WriteLine("\n Session ended."); };

        await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
        do
        {
            Console.WriteLine("Press enter to stop");

        } while (Console.ReadKey().Key != ConsoleKey.Enter);

        await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
    }
}