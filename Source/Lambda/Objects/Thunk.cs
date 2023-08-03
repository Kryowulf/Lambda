using Lambda.AST;
using Lambda.Commands;
using Lambda.Environments;

namespace Lambda.Objects
{
    internal class Thunk
    {
        private AbstractExpression? _expression;
        private IEnvironment? _environment;
        private ResultCommand? _cachedResult;

        public Thunk(AbstractExpression expression, IEnvironment environment)
        {
            _expression = expression;
            _environment = environment;
        }

        public Thunk(AbstractObject result)
        {
            _cachedResult = new ResultCommand(result);
        }

        public ICommand Evaluate()
        {
            if (_cachedResult != null)
                return _cachedResult;

            AggregateCommand buffer = new AggregateCommand();

            buffer.Push(_expression!.Evaluate(_environment!));

            buffer.Push(new UnaryFunctionCommand((obj) =>
            {
                _cachedResult = new ResultCommand(obj);
                return _cachedResult;
            }));

            return buffer;
        }
    }
}
