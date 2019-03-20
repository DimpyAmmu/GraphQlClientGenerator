﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQlClientGenerator.Console
{
    internal static class GraphQlCSharpFileHelper
    {
        public static async Task GenerateGraphQlClient(string url, string targetFileName, string @namespace)
        {
            var schema = await GraphQlGenerator.RetrieveSchema(url);

            var builder = new StringBuilder();

            GraphQlGenerator.GenerateQueryBuilder(schema, builder);

            builder.AppendLine();
            builder.AppendLine();

            GraphQlGenerator.GenerateDataClasses(schema, builder);

            using (var writer = File.CreateText(targetFileName))
            {
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using System.Globalization;");
                writer.WriteLine("using System.Linq;");
                writer.WriteLine("using System.Reflection;");
                writer.WriteLine("using System.Runtime.Serialization;");
                writer.WriteLine("using System.Text;");
                writer.WriteLine();

                writer.WriteLine($"namespace {@namespace}");
                writer.WriteLine("{");

                var indentedLines =
                    builder
                        .ToString()
                        .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                        .Select(l => $"    {l}");

                foreach (var line in indentedLines)
                    writer.WriteLine(line);

                writer.WriteLine("}");
            }
        }
    }
}