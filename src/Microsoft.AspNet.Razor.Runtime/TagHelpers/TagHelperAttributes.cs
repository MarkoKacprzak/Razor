// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    public class TagHelperAttributes
        : ReadOnlyTagHelperAttributes<TagHelperAttribute>, IList<TagHelperAttribute>
    {
        public new TagHelperAttribute this[int index]
        {
            get
            {
                return base[index];
            }
            [param: NotNull]
            set
            {
                _attributes[index] = value;
            }
        }

        public new TagHelperAttribute this[[NotNull] string key]
        {
            get
            {
                return base[key];
            }
            [param: NotNull]
            set
            {
                // Name will be null if user attempts to set the attribute via an implicit conversion:
                // output.Attributes["someName"] = "someValue"
                if (value.Name == null)
                {
                    value.Name = key;
                }

                var attributeReplaced = false;

                for (var i = 0; i < _attributes.Count; i++)
                {
                    if (KeyEquals(key, _attributes[i]))
                    {
                        // We replace the first attribute with the provided value, remove all the rest.
                        if (!attributeReplaced)
                        {
                            // We replace the first attribute we find with the same key.
                            _attributes[i] = value;
                            attributeReplaced = true;
                        }
                        else
                        {
                            _attributes.RemoveAt(i--);
                        }
                    }
                }

                // If we didn't replace an attribute value we should add a new entry.
                if (!attributeReplaced)
                {
                    Add(value);
                }
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        // Internal for testing
        internal void Add([NotNull] string name, object value)
        {
            _attributes.Add(new TagHelperAttribute(name, value));
        }

        public void Add([NotNull] TagHelperAttribute attribute)
        {
            _attributes.Add(attribute);
        }

        public void Insert(int index, [NotNull] TagHelperAttribute item)
        {
            _attributes.Insert(index, item);
        }

        public void CopyTo([NotNull] TagHelperAttribute[] attributes, int index)
        {
            _attributes.CopyTo(attributes, index);
        }

        public bool RemoveAll([NotNull] string key)
        {
            return _attributes.RemoveAll(attribute => KeyEquals(key, attribute)) > 0;
        }

        public bool Remove([NotNull] TagHelperAttribute item)
        {
            return _attributes.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _attributes.RemoveAt(index);
        }

        public void Clear()
        {
            _attributes.Clear();
        }
    }
}