﻿/* Copyright (c) rubicon IT GmbH
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 */

using System;
using System.Collections.Generic;

namespace Core
{
  /// <summary>
  ///   Encapsulates the information required for the <see cref="LicenseHeaderReplacer" /> when updating license headers
  ///   based on the contents of input files.
  /// </summary>
  /// <seealso cref="LicenseHeaderReplacer" />
  public class LicenseHeaderContentInput : LicenseHeaderInput
  {
    /// <summary>
    ///   Initializes a new <see cref="LicenseHeaderContentInput" /> instance.
    /// </summary>
    /// <param name="documentContent">The content of the file whose headers are to be modified.</param>
    /// <param name="documentPath">The path of the file whose headers are to be modified.</param>
    /// <param name="headers">
    ///   The header definitions of the file whose headers are to be modified. Keys are language
    ///   extensions, values represent the definition for the respective language - one line per array index. Value should be
    ///   null if headers are only to be removed.
    /// </param>
    /// <param name="additionalProperties">
    ///   Additional properties that cannot be expanded by the Core whose tokens should be
    ///   replaced by their values.
    /// </param>
    public LicenseHeaderContentInput (
        string documentContent,
        string documentPath,
        IDictionary<string, string[]> headers,
        IEnumerable<AdditionalProperty> additionalProperties = null)
        : base (headers, additionalProperties, documentPath)
    {
      DocumentContent = documentContent;
    }

    /// <summary>
    ///   The content of the file whose headers are to be modified.
    /// </summary>
    public string DocumentContent { get; }

    internal override LicenseHeaderInputMode InputMode => LicenseHeaderInputMode.Content;
  }
}