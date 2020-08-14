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
using LicenseHeaderManager.Utils;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LicenseHeaderManager.Test
{
  [TestFixture]
  internal class JsonBoolConverterTest
  {
    [TestCase ("\"\"")]
    [TestCase ("\"True\"")]
    [TestCase ("\"tRuE\"")]
    [TestCase ("\"true\"")]
    [TestCase ("\"False\"")]
    [TestCase ("\"false\"")]
    [TestCase ("\"fALsE\"")]
    public void Test_Deserialize_String_Throws (string stringLiteral)
    {
      Assert.Throws<JsonReaderException> (() => JsonConvert.DeserializeObject<bool> (stringLiteral, new JsonBoolConverter()));
    }

    [TestCase ("0")]
    [TestCase ("-0")]
    [TestCase ("1")]
    [TestCase ("5")]
    [TestCase ("-1")]
    [TestCase ("-13")]
    public void Test_Deserialize_Number_Throws (string number)
    {
      Assert.Throws<JsonReaderException> (() => JsonConvert.DeserializeObject<bool> (number, new JsonBoolConverter()));
    }

    [Test]
    public void Test_Deserialize_False_Is_False ()
    {
      var deserializeObject = JsonConvert.DeserializeObject<bool> ("false", new JsonBoolConverter());
      Assert.That (deserializeObject, Is.False);
    }

    [Test]
    public void Test_Deserialize_Null_Throws ()
    {
      var ex = Assert.Throws<JsonReaderException> (() => JsonConvert.DeserializeObject<bool> ("null", new JsonBoolConverter()));
      Assert.That (ex.Message, Contains.Substring (JsonBoolConverter.NullLiteral));
    }

    [Test]
    public void Test_Deserialize_True_Is_True ()
    {
      var deserializeObject = JsonConvert.DeserializeObject<bool> ("true", new JsonBoolConverter());
      Assert.That (deserializeObject, Is.True);
    }

    [Test]
    public void Test_Serialize_False_is_False ()
    {
      var serializeObject = JsonConvert.SerializeObject (false, new JsonBoolConverter());
      Assert.That (serializeObject, Is.EqualTo ("false"));
    }

    [Test]
    public void Test_Serialize_True_is_True ()
    {
      var serializeObject = JsonConvert.SerializeObject (true, new JsonBoolConverter());
      Assert.That (serializeObject, Is.EqualTo ("true"));
    }
  }
}