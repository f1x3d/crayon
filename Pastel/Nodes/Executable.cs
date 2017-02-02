﻿using System;
using System.Collections.Generic;

namespace Pastel.Nodes
{
    public abstract class Executable
    {
        public Token FirstToken { get; set; }

        public Executable(Token firstToken)
        {
            this.FirstToken = firstToken;
        }

        public abstract IList<Executable> ResolveNamesAndCullUnusedCode(PastelCompiler compiler);

        protected IList<Executable> Listify(Executable ex)
        {
            return new Executable[] { ex };
        }

        internal static IList<Executable> ResolveNamesAndCullUnusedCodeForBlock(IList<Executable> code, PastelCompiler compiler)
        {
            List<Executable> output = new List<Executable>();
            for (int i = 0; i < code.Count; ++i)
            {
                output.AddRange(code[i].ResolveNamesAndCullUnusedCode(compiler));
            }

            for (int i = 0; i < output.Count - 1; i++)
            {
                Executable ex = output[i];
                if (ex is ReturnStatement || ex is BreakStatement)
                {
                    throw new ParserException(output[i + 1].FirstToken, "Unreachable code detected");
                }

                if (ex is ExpressionAsExecutable)
                {
                    Expression innerExpression = ((ExpressionAsExecutable)ex).Expression;
                    if (!(innerExpression is FunctionInvocation))
                    {
                        throw new ParserException(ex.FirstToken, "This expression isn't allowed here.");
                    }
                }
            }
            return output;
        }

        internal static void ResolveTypes(Executable[] executables, VariableScope varScope, PastelCompiler compiler)
        {
            for (int i = 0; i < executables.Length; ++i)
            {
                executables[i].ResolveTypes(varScope, compiler);
            }
        }

        internal abstract void ResolveTypes(VariableScope varScope, PastelCompiler compiler);
    }
}
