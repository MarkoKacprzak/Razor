// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    public class TagHelperAttribute : IReadOnlyTagHelperAttribute
    {
        public TagHelperAttribute([NotNull] string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key
        {
            get;
            [param: NotNull]
            set;
        }

        public object Value { get; set; }

        public bool Equals(IReadOnlyTagHelperAttribute other)
        {
            return
                other != null &&
                string.Equals(Key, other.Key, StringComparison.OrdinalIgnoreCase) &&
                Equals(Value, other.Value);
        }
    }
}