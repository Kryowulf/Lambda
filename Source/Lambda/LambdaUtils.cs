using Lambda.AST;

namespace Lambda
{
    internal static class LambdaUtils
    {
        public static AbstractExpression MakeHeadFunction()
        {
            return new LambdaExpression("h",
                    new LambdaExpression("t",
                        new SymbolExpression("h", false)));
        }

        public static AbstractExpression MakeTailFunction()
        {
            return new LambdaExpression("h",
                    new LambdaExpression("t",
                        new SymbolExpression("t", false)));
        }

        // Converts a string into lambda form.
        // For example, "Hello" into:
        // ,f (f 'H' ,f (f 'e' ,f (f 'l' ,f (f 'l' ,f (f 'o' '')))))

        public static AbstractExpression StringToLambda(string str)
        {
            AbstractExpression result = new SymbolExpression("", true);

            for (int i = str.Length - 1; i >= 0; i--)
            {
                SymbolExpression chr = new SymbolExpression(str[i].ToString(), true);

                result = new LambdaExpression("f",
                                new CallExpression(
                                    new CallExpression(
                                        new SymbolExpression("f", false),
                                        chr),
                                    result));
            }

            return result;
        }
    }
}
