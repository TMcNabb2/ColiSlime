using AutoClass.Contexts;
using System;
using System.IO;
using System.Text;
using UnityEditor;

namespace AutoClass
{
    public class ClassBuilder : BuilderContext<ClassBuilder>,
        IClassDeclarable
    {
        private readonly string _fileName;
        private string[] _usings;
        private string _namespace;

        /// <summary>
        /// Creates a new class builder. Create a class by calling the "Class" method on this object.
        /// </summary>
        /// <param name="namespace">The namespace of the class file.</param>
        /// <param name="fileName">The name of the class file.</param>
        public ClassBuilder(string fileName) : base()
        {
            _fileName = fileName;
            _usings = new string[0];
            _namespace = string.Empty;
            Parent = this;
        }

        public ClassBuilder Using(params string[] usings)
        {
            _usings = usings;
            return this;
        }

        public ClassBuilder Namespace(string @namespace)
        {
            _namespace = @namespace;
            return this;
        }

        protected override void OnReturn()
        {
            // no-op
        }

        /// <summary>
        /// Save the class to a .cs file at the given <paramref name="directory"/>.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="createDirectory"></param>
        public void SaveClass(string directory, bool createDirectory = true)
        {
            if (!directory.StartsWith("Assets"))
                directory = Path.Combine("Assets", directory);

            if (createDirectory && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string path = Path.Combine(directory, string.Concat(_fileName, ".cs"));

            StringBuilder contents = new();

            if (_usings.Length > 0)
            {
                foreach (var line in _usings)
                {
                    contents.AppendLine($"using {line};");
                }
            }

            if (!string.IsNullOrEmpty(_namespace))
            {
                contents.AppendLine($"namespace {_namespace} {{")
                    .AppendLine(ToString())
                    .AppendLine("}");
            }
            else
            {
                contents.AppendLine(ToString());
            }

            File.WriteAllText(path, contents.ToString());

            AssetDatabase.Refresh();
        }
    }
}
