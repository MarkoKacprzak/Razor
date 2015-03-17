﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Microsoft.AspNet.Razor.Runtime.TagHelpers
{
    public class TagHelperOutputTest
    {
        [Fact]
        public void TagName_CanSetToNullInCtor()
        {
            // Arrange & Act
            var tagHelperOutput = new TagHelperOutput(null);

            // Assert
            Assert.Null(tagHelperOutput.TagName);
        }

        [Fact]
        public void TagName_CanSetToNull()
        {
            // Arrange & Act
            var tagHelperOutput = new TagHelperOutput("p")
            {
                TagName = null
            };

            // Assert
            Assert.Null(tagHelperOutput.TagName);
        }

        [Fact]
        public void PreContent_SetContent_ChangesValue()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p", attributes:
                new Dictionary<string, string>
                {
                    { "class", "btn" },
                    { "something", "   spaced    " }
                });

            // Act
            var output = tagHelperOutput.GenerateStartTag();

            // Assert
            Assert.Equal("<p class=\"btn\" something=\"   spaced    \">", output.ToString());
        }

        [Fact]
        public void GenerateStartTag_ReturnsNoAttributeStartTag()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p");
            tagHelperOutput.PreContent.SetContent("Hello World");

            // Act
            var output = tagHelperOutput.GenerateStartTag();

            // Assert
            Assert.Equal("<p>", output.ToString());
        }

        [Fact]
        public void Content_SetContent_ChangesValue()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p",
                attributes: new Dictionary<string, string>
                {
                    { "class", "btn" },
                    { "something", "   spaced    " }
                });

            tagHelperOutput.SelfClosing = true;

            // Act
            var output = tagHelperOutput.GenerateStartTag();

            // Assert
            Assert.Equal("<p class=\"btn\" something=\"   spaced    \" />", output.ToString());
        }

        [Fact]
        public void GenerateStartTag_ReturnsSelfClosingStartTag_Attributes()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p",
                attributes: new Dictionary<string, string>
                {
                    { "class", "btn" },
                    { "something", "   spaced    " }
                });

            tagHelperOutput.SelfClosing = true;

            // Act
            var output = tagHelperOutput.GenerateStartTag();

            // Assert
            Assert.Equal("<p class=\"btn\" something=\"   spaced    \" />", output.ToString());
        }

        [Fact]
        public void GenerateStartTag_ReturnsSelfClosingStartTag_NoAttributes()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p");
            tagHelperOutput.Content.SetContent("Hello World");
            tagHelperOutput.SelfClosing = true;

            // Act
            var output = tagHelperOutput.GenerateStartTag();

            // Assert
            Assert.Equal("<p />", output.ToString());
        }

        [Fact]
        public void GenerateStartTag_UsesProvidedHtmlEncoder()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p",
                attributes: new Dictionary<string, string>
                {
                    { "hello", "world" },
                });

            tagHelperOutput.SelfClosing = true;

            // Act
            var output = tagHelperOutput.GenerateStartTag();

            var writer = new StringWriter();
            output.WriteTo(writer, new PseudoHtmlEncoder());

            // Assert
            Assert.Equal("<p hello=\"HtmlEncode[[world]]\" />", writer.ToString());
        }

        [Fact]
        public void GenerateStartTag_ReturnsNothingIfWhitespaceTagName()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("  ",
                attributes: new Dictionary<string, string>
                {
                    { "class", "btn" },
                    { "something", "   spaced    " }
                })
            {
                SelfClosing = true
            };

            // Act
            var output = tagHelperOutput.GenerateStartTag();

            // Assert
            Assert.Empty(output.ToString());
        }


        [Fact]
        public void GenerateEndTag_ReturnsNothingIfWhitespaceTagName()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput(" ");
            tagHelperOutput.Content.Append("Hello World");

            // Act
            var output = tagHelperOutput.GenerateEndTag();

            // Assert
            Assert.Empty(output.ToString());
        }

        [Fact]
        public void GeneratePreContent_ReturnsPreContent()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p");
            tagHelperOutput.PostContent.SetContent("Hello World");

            // Act & Assert
            Assert.NotNull(tagHelperOutput.PreContent);
            Assert.NotNull(tagHelperOutput.Content);
            Assert.NotNull(tagHelperOutput.PostContent);
            Assert.Equal("Hello World", tagHelperOutput.PostContent.GetContent());
        }

        [Fact]
        public void GenerateEndTag_ReturnsEndTag()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p");

            // Act
            var output = tagHelperOutput.GenerateEndTag();

            // Assert
            Assert.Equal("</p>", output.ToString());
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData("\t", true )]
        [InlineData(null, false)]
        [InlineData("\t", false)]
        public void GeneratePostContent_ReturnsPostContentIfTagNameIsNullOrWhitespace(string tagName, bool selfClosing)
        {
            // Arrange
            var expectedContent = "Hello World";

            var tagHelperOutput = new TagHelperOutput(tagName)
            {
                SelfClosing = selfClosing
            };
            tagHelperOutput.PostContent.Append(expectedContent);

            // Act
            var output = tagHelperOutput.GeneratePostContent();

            // Assert
            var result = Assert.IsType<DefaultTagHelperContent>(output);
            Assert.Equal(expectedContent, result.GetContent());
        }

        [Fact]
        public void GenerateEndTag_ReturnsNothingIfSelfClosing()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p")
            {
                SelfClosing = true
            };

            // Act
            var output = tagHelperOutput.GenerateEndTag();

            // Assert
            Assert.Empty(output.ToString());
        }

        [Fact]
        public void SuppressOutput_Sets_TagName_Content_PreContent_PostContent_ToNull()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p");
            tagHelperOutput.PreContent.Append("Pre Content");
            tagHelperOutput.Content.Append("Content");
            tagHelperOutput.PostContent.Append("Post Content");

            // Act
            tagHelperOutput.SuppressOutput();

            // Assert
            Assert.Null(tagHelperOutput.TagName);
            Assert.NotNull(tagHelperOutput.PreContent);
            Assert.Empty(tagHelperOutput.PreContent.GetContent());
            Assert.NotNull(tagHelperOutput.Content);
            Assert.Empty(tagHelperOutput.Content.GetContent());
            Assert.NotNull(tagHelperOutput.PostContent);
            Assert.Empty(tagHelperOutput.PostContent.GetContent());
        }

        [Fact]
        public void SuppressOutput_PreventsTagOutput()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p",
                attributes: new Dictionary<string, string>
                {
                    { "class", "btn" },
                    { "something", "   spaced    " }
                });
            tagHelperOutput.PreContent.Append("Pre Content");
            tagHelperOutput.Content.Append("Content");
            tagHelperOutput.PostContent.Append("Post Content");

            // Act
            tagHelperOutput.SuppressOutput();

            // Assert
            Assert.Empty(tagHelperOutput.GenerateStartTag().ToString());
            var result = Assert.IsType<DefaultTagHelperContent>(tagHelperOutput.GeneratePreContent());
            Assert.Empty(result.GetContent());
            result = Assert.IsType<DefaultTagHelperContent>(tagHelperOutput.GenerateContent());
            Assert.Empty(result.GetContent());
            result = Assert.IsType<DefaultTagHelperContent>(tagHelperOutput.GeneratePostContent());
            Assert.Empty(result.GetContent());
            Assert.Empty(tagHelperOutput.GenerateEndTag().ToString());
        }

        [Theory]
        [InlineData("class", "ClASs")]
        [InlineData("CLaSs", "class")]
        [InlineData("cLaSs", "cLasS")]
        public void Attributes_IgnoresCase(string originalName, string updateName)
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("p",
                attributes: new Dictionary<string, string>
                {
                    { originalName, "btn" },
                });

            // Act
            tagHelperOutput.Attributes[updateName] = "super button";

            // Assert
            var attribute = Assert.Single(tagHelperOutput.Attributes);
            Assert.Equal(new KeyValuePair<string, string>(originalName, "super button"), attribute);
        }
    }
}
