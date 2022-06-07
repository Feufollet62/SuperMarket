using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Adobe.Substance.Editor
{
    public static class NamingExtensions
    {
        public static string GetAssociatedAssetPath(this SubstanceGraphSO graph, string name, string extension)
        {
            var fileName = Path.GetFileNameWithoutExtension(graph.AssetPath);
            return Path.Combine(graph.OutputPath, $"{fileName}_{name}.{extension}");
        }

        public static string GetAssetFileName(this SubstanceGraphSO graph)
        {
            return Path.GetFileNameWithoutExtension(graph.AssetPath);
        }
    }
}