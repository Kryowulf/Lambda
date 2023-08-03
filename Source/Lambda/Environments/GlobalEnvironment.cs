using Lambda.AST;
using Lambda.Objects;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lambda.Environments
{
    internal class GlobalEnvironment : IEnvironment
    {
        private Dictionary<string, Thunk> _definitions = new();
        private Dictionary<string, ModuleInstance> _modules = new();

        public Thunk this[string name]
        {
            get
            {
                if (!_definitions.ContainsKey(name))
                    _definitions[name] = new Thunk(new SymbolObject(name));

                return _definitions[name];
            }
            set
            {
                _definitions[name] = value;
            }
        }

        public ModuleInstance this[ModulePathExpression path]
        { 
            get
            {
                return _modules[path.SymbolicName];
            }
        }

        public bool Contains(ModulePathExpression path)
        {
            return _modules.ContainsKey(path.SymbolicName);
        }

        public ModuleInstance Register(Module module)
        {
            if (_definitions.ContainsKey(module.Path.SymbolicName))
                throw new InvalidOperationException("Module's symbolic name already registered with global environment.");

            ModuleEnvironment menv = new ModuleEnvironment(this);

            foreach (var def in module.Definitions)
                menv[def.Name] = new Thunk(def.Value, menv);

            ModuleInstance mobj = new ModuleInstance(menv, module);
            _definitions[module.Path.SymbolicName] = new Thunk(mobj);
            _modules[module.Path.SymbolicName] = mobj;
            return mobj;
        }
    }
}
