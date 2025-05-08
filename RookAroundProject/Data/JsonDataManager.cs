using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RookAroundProject
{
    public class JsonDataManager : IDataManager
    {
        private static readonly string playersFilePath = "Database/players.json";
        private static readonly string matchesFilePath = "Database/matches.json";
        private static readonly string managerFilePath = "Database/tempmanager.json";

        public void AddPlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void AddMatch(Match match)
        {
            throw new NotImplementedException();
        }

        public void AddManager(Manager manager)
        {
            var managerList = LoadManagers();
            managerList.Add(manager);
            SaveToFile(managerFilePath, managerList);
            Console.WriteLine($"Manager {manager.Username} added.");
        }

        public void UpdateAccountInfo(Player player)
        {
            throw new NotImplementedException();
        }

        public void UpdateManagerInfo(Manager manager)
        {
            var managerList = LoadManagers();
            var existingManager = managerList.Find(s => s.Username == manager.Username);
            
            if (existingManager != null)
            {
                //existingManager points to managerList so changing existingManager changes managerlist
                existingManager.Pwd = manager.Pwd;
                SaveToFile(managerFilePath, managerList);
                Console.WriteLine($"Manager {manager.Username} updated.");
            } else
            {
                Console.WriteLine($"Manager {manager.Username} not found.");
            }
        }
        public void RemovePlayer(Player player)
        {
            throw new NotImplementedException();
        }

        public void RemoveMatch(Match match)
        {
            throw new NotImplementedException();
        }

        public void RemoveManager(Manager manager)
        {
            var managerList = LoadManagers();
            managerList.RemoveAll(s => s.Username == manager.Username);
            SaveToFile(managerFilePath, managerList);
            Console.WriteLine($"Manager {manager.Username} removed.");
        }

        public bool VerifyUser(string username, string pwd)
        {
            throw new NotImplementedException();
        }

        public Manager? GetManagerByUsername(string username)
        {
            var managerList = LoadManagers();
            return managerList.FirstOrDefault(manager => manager.Username == username);
        }


        public List<Player> LoadPlayers()
        {
            throw new NotImplementedException();
        }

        public List<Tournament> LoadTournaments()
        {
            throw new NotImplementedException();
        }

        public List<Manager> LoadManagers()
        {
            return LoadFromFile<List<Manager>>(managerFilePath);
        }

        private void SaveToFile<T>(string filePath, T data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, json);
        }

        private T LoadFromFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return default;

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json) ?? Activator.CreateInstance<T>();
        }

        public List<Venue> LoadVenues()
        {
            throw new NotImplementedException();
        }
        public List<Resource> LoadResources()
        {
            throw new NotImplementedException();
        }
        public void AddResources(List<Resource> resources)
        {
            throw new NotImplementedException();
        }
        public void AddVenue(Venue venue)
        {
            throw new NotImplementedException();
        }
        public void AddTournament(Tournament tournament)
        {
            throw new NotImplementedException();
        }

        public List<Match> LoadMatches()
        {
            return LoadFromFile<List<Match>>(matchesFilePath);
        }
        public List<Manager> LoadManager()
        {
            return LoadFromFile<List<Manager>>(managerFilePath);
        }

        public void ClearDatabase(){
            // Clear the data files
            File.WriteAllText(playersFilePath, string.Empty);
            File.WriteAllText(matchesFilePath, string.Empty);
            File.WriteAllText(managerFilePath, string.Empty);
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void RemoveTournament(Tournament tournament)
        {
            throw new NotImplementedException();
        }
    }
}
