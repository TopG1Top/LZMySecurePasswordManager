using MySecurePasswordManager.Services;
using System;

namespace MySecurePasswordManager
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== MySecurePasswordManager =====");

            // Master-Passwort setzen
            Console.Write("Master-Passwort eingeben (zum Entsperren): ");
            var masterPass = Console.ReadLine();
            MasterPasswordService.SetMasterPassword(masterPass!);

            // Repo mit Pfad initialisieren
            string filePath = "passwords.json";
            var repo = new PasswordRepository(filePath);

            while (true)
            {
                Console.WriteLine("\nVerfügbare Befehle:");
                Console.WriteLine("  add    => Neuen Eintrag hinzufügen");
                Console.WriteLine("  list   => Alle Einträge auflisten");
                Console.WriteLine("  show   => Ein Passwort anzeigen");
                Console.WriteLine("  remove => Ein Eintrag entfernen");
                Console.WriteLine("  exit   => Beenden");
                Console.Write("\nEingabe: ");
                var command = Console.ReadLine();

                switch (command?.ToLower())
                {
                    case "add":
                        HandleAdd(repo);
                        break;
                    case "list":
                        HandleList(repo);
                        break;
                    case "show":
                        HandleShow(repo);
                        break;
                    case "remove":
                        HandleRemove(repo);
                        break;
                    case "exit":
                        Console.WriteLine("Bye!");
                        return; // Programm beenden
                    default:
                        Console.WriteLine("Unbekannter Befehl");
                        break;
                }
            }
        }

        static void HandleAdd(PasswordRepository repo)
        {
            Console.Write("Titel: ");
            var title = Console.ReadLine();

            Console.Write("Username: ");
            var username = Console.ReadLine();

            Console.Write("Passwort: ");
            var pw = Console.ReadLine();

            repo.AddRecord(title!, username!, pw!);
            Console.WriteLine("Eintrag hinzugefügt!");
        }

        static void HandleList(PasswordRepository repo)
        {
            var records = repo.GetAllRecords();
            if (records.Count == 0)
            {
                Console.WriteLine("Keine Einträge vorhanden.");
                return;
            }

            Console.WriteLine("\nVorhandene Einträge:");
            foreach (var r in records)
            {
                Console.WriteLine($"- ID: {r.Id} | Title: {r.Title} | Username: {r.Username}");
            }
        }

        static void HandleShow(PasswordRepository repo)
        {
            Console.Write("Welche ID soll angezeigt werden? ");
            var id = Console.ReadLine();
            var pw = repo.DecryptPassword(id!);
            if (pw == null)
                Console.WriteLine("Keine Eintrag gefunden oder Fehler beim Entschlüsseln!");
            else
                Console.WriteLine($"Passwort: {pw}");
        }

        static void HandleRemove(PasswordRepository repo)
        {
            Console.Write("Welche ID soll entfernt werden? ");
            var id = Console.ReadLine();
            repo.RemoveRecord(id!);
            Console.WriteLine("Eintrag (falls vorhanden) entfernt.");
        }
    }
}
