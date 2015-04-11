// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Internal.Web.Utils;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    public class TagHelperAttribute : IReadOnlyTagHelperAttribute
    {
        public TagHelperAttribute()
        {
        }

        public TagHelperAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public object Value { get; set; }

        public static implicit operator TagHelperAttribute(string value)
        {
            return new TagHelperAttribute
            {
                Value = value
            };
        }

        public bool Equals(IReadOnlyTagHelperAttribute other)
        {
            return
                other != null &&
                string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase) &&
                Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            var other = obj as IReadOnlyTagHelperAttribute;

            if (other != null)
            {
                return Equals(other);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCodeCombiner
                .Start()
                .Add(Name, StringComparer.OrdinalIgnoreCase)
                .Add(Value)
                .CombinedHash;
        }
    }
}