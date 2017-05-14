﻿using System;
using System.Collections.Generic;
using Common;
using Pastel.Nodes;
using Platform;

namespace JavaAppAndroid
{
    public class PlatformImpl : Platform.AbstractPlatform
    {
        public PlatformImpl() : base()
        {
        }

        public override string InheritsFrom { get { return "lang-java"; } }
        public override string Name { get { return "java-app-android"; } }
        public override string NL { get { return "\n"; } }

        public override Dictionary<string, FileOutput> ExportProject(IList<VariableDeclaration> globals, IList<StructDefinition> structDefinitions, IList<FunctionDefinition> functionDefinitions, IList<LibraryForExport> libraries, ResourceDatabase resourceDatabase, Options options, ILibraryNativeInvocationTranslatorProvider libraryNativeInvocationTranslatorProviderForPlatform)
        {
            Dictionary<string, FileOutput> output = new Dictionary<string, FileOutput>();

            output["foo.txt"] = new FileOutput()
            {
                Type = FileOutputType.Text,
                TextContent = "TODO: export android project.",
            };

            return output;
        }

        public override Dictionary<string, FileOutput> ExportStandaloneVm(IList<VariableDeclaration> globals, IList<StructDefinition> structDefinitions, IList<FunctionDefinition> functionDefinitions, IList<LibraryForExport> everyLibrary, ILibraryNativeInvocationTranslatorProvider libraryNativeInvocationTranslatorProviderForPlatform)
        {
            throw new NotImplementedException();
        }

        public override string GenerateCodeForFunction(AbstractTranslator translator, FunctionDefinition funcDef)
        {
            throw new NotImplementedException();
        }

        public override string GenerateCodeForGlobalsDefinitions(AbstractTranslator translator, IList<VariableDeclaration> globals)
        {
            throw new NotImplementedException();
        }

        public override string GenerateCodeForStruct(StructDefinition structDef)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<string, string> GenerateReplacementDictionary(Options options, ResourceDatabase resDb)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<string, object> GetConstantFlags()
        {
            return new Dictionary<string, object>()
            {
                {  "IS_JAVA", true },
            };
        }
    }
}
