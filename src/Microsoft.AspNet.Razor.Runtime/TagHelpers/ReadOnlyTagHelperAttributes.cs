// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    public class ReadOnlyTagHelperAttributes<TAttributeType> : IReadOnlyList<TAttributeType>
        where TAttributeType : IReadOnlyTagHelperAttribute
    {
        protected readonly List<TAttributeType> _attributes;

        protected ReadOnlyTagHelperAttributes()
        {
            _attributes = new List<TAttributeType>();
        }

        public ReadOnlyTagHelperAttributes([NotNull] IEnumerable<TAttributeType> attributes)
        {
            _attributes = new List<TAttributeType>(attributes);
        }

        public TAttributeType this[int index]
        {
            get
            {
                return _attributes[index];
            }
        }

        public TAttributeType this[[NotNull] string key]
        {
            get
            {
                return _attributes.FirstOrDefault(attribute => KeyEquals(key, attribute));
            }
        }

        public int Count
        {
            get
            {
                return _attributes.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return _attributes.Select(attribute => attribute.Name);
            }
        }

        public IEnumerable<object> Values
        {
            get
            {
                return _attributes.Select(attribute => attribute.Value);
            }
        }

        public bool Contains([NotNull] TAttributeType item)
        {
            return _attributes.Contains(item);
        }

        public bool ContainsKey([NotNull] string key)
        {
            return _attributes.Any(attribute => KeyEquals(key, attribute));
        }

        public int IndexOf([NotNull] TAttributeType item)
        {
            return _attributes.IndexOf(item);
        }

        public bool TryGetValue([NotNull] string key, out TAttributeType value)
        {
            value = _attributes.LastOrDefault(attribute => KeyEquals(key, attribute));

            return value != null;
        }

        public bool TryGetValues([NotNull] string key, out IEnumerable<TAttributeType> values)
        {
            values = _attributes.Where(attribute => KeyEquals(key, attribute));

            return values.Any();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<TAttributeType> GetEnumerator()
        {
            return _attributes.GetEnumerator();
        }

        protected static bool KeyEquals(string key, [NotNull] TAttributeType attribute)
        {
            return string.Equals(key, attribute.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
}