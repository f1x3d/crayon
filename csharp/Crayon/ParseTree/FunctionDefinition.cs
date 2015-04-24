﻿using System.Collections.Generic;
using System.Linq;

namespace Crayon.ParseTree
{
	internal class FunctionDefinition : Executable
	{
		public Token NameToken { get; private set; }
		public Token[] ArgNames { get; private set; }
		public Expression[] DefaultValues { get; private set; }
		public Executable[] Code { get; private set; }
		public Annotation[] ArgAnnotations { get; private set; }
		private Dictionary<string, Annotation> annotations;
		public VariableIdAllocator VariableIds { get; private set; }

		public FunctionDefinition(Token functionToken, Token nameToken, IList<Token> argNames, IList<Expression> argDefaultValues, IList<Annotation> argAnnotations, IList<Executable> code, IList<Annotation> functionAnnotations)
			: base(functionToken)
		{
			this.NameToken = nameToken;
			this.ArgNames = argNames.ToArray();
			this.DefaultValues = argDefaultValues.ToArray();
			this.ArgAnnotations = argAnnotations.ToArray();
			this.Code = code.ToArray();
			this.annotations = new Dictionary<string, Annotation>();
			foreach (Annotation annotation in functionAnnotations)
			{
				this.annotations[annotation.Type] = annotation;
			}
			this.VariableIds = new VariableIdAllocator();
		}

		public Annotation GetAnnotation(string type)
		{
			if (this.annotations.ContainsKey(type)) return this.annotations[type];
			return null;
		}

		public override IList<Executable> Resolve(Parser parser)
		{
			for (int i = 0; i < this.DefaultValues.Length; ++i)
			{
				if (this.DefaultValues[i] != null)
				{
					this.DefaultValues[i] = this.DefaultValues[i].Resolve(parser);
				}

				// Annotations not allowed in byte code mode
				if (parser.NullablePlatform == null && this.ArgAnnotations[i] != null)
				{
					throw new ParserException(this.ArgAnnotations[i].FirstToken, "Unexpected token: '@'");
				}
			}

			this.Code = Resolve(parser, this.Code).ToArray();

			if (this.Code.Length == 0 || !(this.Code[this.Code.Length - 1] is ReturnStatement))
			{
				List<Executable> newCode = new List<Executable>(this.Code);
				newCode.Add(new ReturnStatement(this.FirstToken, null));
				this.Code = newCode.ToArray();
			}

			foreach (Token arg in this.ArgNames)
			{
				this.VariableIds.RegisterVariable(arg.Value);
			}

			foreach (Executable ex in this.Code)
			{
				ex.AssignVariablesToIds(this.VariableIds);
			}

			return Listify(this);
		}

		public override void AssignVariablesToIds(VariableIdAllocator varIds)
		{
			varIds.RegisterVariable(this.NameToken.Value);
		}

		public override void GetAllVariableNames(Dictionary<string, bool> lookup)
		{
			foreach (Executable line in this.Code)
			{
				line.GetAllVariableNames(lookup);
			}
		}

		public IList<string> GetVariableDeclarationList()
		{
			HashSet<string> dontRedeclareThese = new HashSet<string>();
			foreach (Token argNameToken in this.ArgNames)
			{
				dontRedeclareThese.Add(argNameToken.Value);
			}

			Dictionary<string, bool> variableNamesDict = new Dictionary<string, bool>();
			this.GetAllVariableNames(variableNamesDict);
			foreach (string variableName in variableNamesDict.Keys.ToArray())
			{
				if (dontRedeclareThese.Contains(variableName))
				{
					variableNamesDict.Remove(variableName);
				}
			}

			return variableNamesDict.Keys.OrderBy<string, string>(s => s.ToLowerInvariant()).ToArray();
		}
	}
}
