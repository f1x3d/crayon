﻿using Platform;
using System.Collections.Generic;

namespace Crayon
{
    class PlatformProvider : IPlatformProvider
    {
        private Dictionary<string, AbstractPlatform> platforms;
        public AbstractPlatform GetPlatform(string name)
        {
            if (platforms == null)
            {
                platforms = new Dictionary<string, AbstractPlatform>();
                foreach (System.Reflection.Assembly assembly in GetRawAssemblies())
                {
                    AbstractPlatform platform = this.GetPlatformInstance(assembly);
                    platform.PlatformProvider = this;
                    string key = platform.Name;
                    if (platforms.ContainsKey(key))
                    {
                        throw new System.InvalidOperationException("Multiple platforms with the same ID: '" + key + "'");
                    }
                    platforms[key] = platform;
                }
            }

            if (name != null && platforms.ContainsKey(name))
            {
                return platforms[name];
            }

            return null;
        }

        private AbstractPlatform GetPlatformInstance(System.Reflection.Assembly assembly)
        {
            foreach (System.Type type in assembly.GetExportedTypes())
            {
                // TODO: check to make sure it inherits from AbstractPlatform
                // Perhaps make that the only qualification instead of going by name?
                if (type.Name == "PlatformImpl")
                {
                    return (AbstractPlatform)assembly.CreateInstance(type.FullName);
                }
            }
            throw new System.InvalidOperationException("This assembly does not define a PlatformImpl type: " + assembly.FullName);
        }

        private static System.Reflection.Assembly[] GetRawAssemblies()
        {
            // TODO: create a dev Crayon csproj that has a strong project reference to the platforms
            // and a release csproj that does not and then ifdef out the implementation of this function.
            return new System.Reflection.Assembly[] {
                typeof(CApp.PlatformImpl).Assembly,
                typeof(CSharpApp.PlatformImpl).Assembly,
                typeof(JavaApp.PlatformImpl).Assembly,
                typeof(JavaAppAndroid.PlatformImpl).Assembly,
                typeof(JavaScriptAppChrome.PlatformImpl).Assembly,
                typeof(JavaScriptApp.PlatformImpl).Assembly,
                typeof(JavaScriptAppIos.PlatformImpl).Assembly,
                typeof(LangC.PlatformImpl).Assembly,
                typeof(LangCSharp.PlatformImpl).Assembly,
                typeof(LangJava.PlatformImpl).Assembly,
                typeof(LangJavaScript.PlatformImpl).Assembly,
                typeof(LangPython.PlatformImpl).Assembly,
                typeof(PythonApp.PlatformImpl).Assembly,
            };
        }
    }
}
