﻿using Build;
using Common;
using Parser;
using System.Collections.Generic;

namespace Crayon
{
    public class CompilationBundle
    {
        public ByteBuffer ByteCode { get; set; }
        public string ProjectID { get; set; }
        public string GuidSeed { get; set; }
        public ICollection<LibraryCompilationScope> LibraryScopesUsed { get; set; }
        public string IconPath { get; set; }
        public string LaunchScreenPath { get; set; }
        public string DefaultTitle { get; set; }
        public string JsFilePrefix { get; set; }
        public string IosBundlePrefix { get; set; }
        public string JavaPackage { get; set; }
        public int? WindowWidth { get; set; }
        public int? WindowHeight { get; set; }
        public string Orientations { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }

        public static CompilationBundle Compile(BuildContext buildContext)
        {
            using (new PerformanceSection("CompilationBundle.Compile"))
            {
                ParserContext parser = new ParserContext(buildContext);
                Parser.ParseTree.TopLevelConstruct[] resolvedParseTree = parser.ParseAllTheThings();

                ByteCodeCompiler bcc = new ByteCodeCompiler();
                ByteBuffer buffer = bcc.GenerateByteCode(parser, resolvedParseTree);

                string jsFilePrefix = buildContext.JsFilePrefix == null
                    ? ""
                    : ("/" + buildContext.JsFilePrefix.Trim('/') + "/");

                return new CompilationBundle()
                {
                    ByteCode = buffer,
                    LibraryScopesUsed = parser.LibraryManager.ImportedLibraries.ToArray(),
                    ProjectID = buildContext.ProjectID,
                    Version = buildContext.Version,
                    Description = buildContext.Description,
                    GuidSeed = buildContext.GuidSeed,
                    DefaultTitle = buildContext.DefaultTitle,
                    JsFilePrefix = jsFilePrefix,
                    IosBundlePrefix = buildContext.IosBundlePrefix,
                    JavaPackage = buildContext.JavaPackage,
                    IconPath = buildContext.IconFilePath,
                    LaunchScreenPath = buildContext.LaunchScreenPath,
                    WindowWidth = buildContext.WindowWidth,
                    WindowHeight = buildContext.WindowHeight,
                    Orientations = buildContext.Orientation,
                };
            }
        }
    }
}
