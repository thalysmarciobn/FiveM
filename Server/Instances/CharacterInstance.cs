using Shared.Models.Database;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Server.Instances
{
    public class CharacterInstance
    {
        private ConcurrentDictionary<string, AccountCharacterModel> Characters = new ConcurrentDictionary<string, AccountCharacterModel>();

        private static CharacterInstance s_Instance { get; set; }

        public static CharacterInstance Instance { get
            {
                if (s_Instance == null)
                    s_Instance = new CharacterInstance();
                return s_Instance;
            }
        }

        public void AddCharacter(string license, AccountCharacterModel model) => Characters.AddOrUpdate(license, model, (key, value) => value = model);

        public bool RemoveCharacter(string license, out AccountCharacterModel model) => Characters.TryRemove(license, out model);

        public bool GetCharacter(string license, out AccountCharacterModel model) => Characters.TryGetValue(license, out model);
    }
}
