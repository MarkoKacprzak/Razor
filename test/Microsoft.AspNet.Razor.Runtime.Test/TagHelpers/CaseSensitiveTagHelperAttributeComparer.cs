﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Internal.Web.Utils;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers.Test
{
    public class CaseSensitiveTagHelperAttributeComparer : IEqualityComparer<IReadOnlyTagHelperAttribute>
    {
        public readonly static CaseSensitiveTagHelperAttributeComparer Default =
            new CaseSensitiveTagHelperAttributeComparer();

        private CaseSensitiveTagHelperAttributeComparer()
        {
        }

        public bool Equals(
            [NotNull] IReadOnlyTagHelperAttribute attributeX,
            [NotNull] IReadOnlyTagHelperAttribute attributeY)
        {
            return
                // Normal comparer doesn't care about the Key case, in tests we do.
                string.Equals(attributeX.Key, attributeY.Key, StringComparison.Ordinal) &&
                Equals(attributeX.Value, attributeY.Value);
        }

        public int GetHashCode([NotNull] IReadOnlyTagHelperAttribute attribute)
        {
            return HashCodeCombiner
                .Start()
                .Add(attribute.Key, StringComparer.Ordinal)
                .Add(attribute.Value)
                .CombinedHash;
        }
    }
}