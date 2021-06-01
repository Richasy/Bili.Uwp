// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;

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
                return element != default; // TODO UNO
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
