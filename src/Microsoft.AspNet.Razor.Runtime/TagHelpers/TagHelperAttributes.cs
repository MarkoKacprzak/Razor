// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    public class TagHelperAttributes : ReadOnlyTagHelperAttributes<TagHelperAttribute>, IList<TagHelperAttribute>
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
                var attributeReplaced = false;

                for (var i = _attributes.Count - 1; i >= 0; i++)
                {
                    if (SameKey(key, _attributes[i]))
                    {
                        // We replace the last attribute with the provided value, remove all the rest.
                        if (!attributeReplaced)
                        {
                            attributeReplaced = true;
                            _attributes[i] = value;
                        }
                        else
                        {
                            _attributes.RemoveAt(i);
                        }
                    }
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
        internal void Add([NotNull] string key, object value)
        {
            _attributes.Add(new TagHelperAttribute(key, value));
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

        public bool Remove([NotNull] string key)
        {
            return _attributes.RemoveAll(attribute => SameKey(key, attribute)) > 0;
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