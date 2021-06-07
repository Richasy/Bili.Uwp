// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Richasy.Bili.App.Controls
{
    internal static partial class CollectionExtensions
    {
        public static bool TryGetElementAt<T>(this IList<T> collection, int index, out T element)
            where T : class
        {
            if (index < collection.Count)
            {
                element = collection[index];
                return element != default;
            }
            else
            {
                element = default;
                return false;
            }
        }

        public static void AddOrInsert<T>(this IList<T> collection, int index, T element)
        {
            if (index >= collection.Count)
            {
                collection.Add(element);
            }
            else
            {
                collection.Insert(index, element);
            }
        }
    }
}
