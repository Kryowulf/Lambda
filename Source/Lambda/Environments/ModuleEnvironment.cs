using Lambda.Objects;
using System.Collections.Generic;

namespace Lambda.Environments
{
    internal class ModuleEnvironment : IEnvironment
    {
        private Dictionary<string, Thunk> _importedDefinitions = new();
        private Dictionary<string, Thunk> _localDefinitions = new();

        public GlobalEnvironment GlobalEnvironment { get; private set; }
        public IReadOnlyDictionary<string, Thunk> ImportedDefinitions => _importedDefinitions;
        public IReadOnlyDictionary<string, Thunk> LocalDefinitions => _localDefinitions;

        public Thunk this[string key]
        {
            get
            {
                Thunk? value;

                if (this.LocalDefinitions.TryGetValue(key, out value))
                    return value;

                if (this.ImportedDefinitions.TryGetValue(key, out value))
                    return value;

                return this.GlobalEnvironment[key];
            }
            set
            {
                _localDefinitions[key] = value;
            }
        }
            
        public ModuleEnvironment(GlobalEnvironment genv)
        {
            this.GlobalEnvironment = genv;
        }

        public void Import(ModuleEnvironment other)
        {
            foreach (string key in other.LocalDefinitions.Keys)
                _importedDefinitions[key] = other.LocalDefinitions[key];
        }
    }
}
