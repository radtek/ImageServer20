#region License

// Copyright (c) 2006-2007, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

#if UNIT_TESTS

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace ClearCanvas.Common.Utilities.Tests
{
    [TestFixture]
    public class CollectionUtilsTests
    {
        class Foo : IComparable<Foo>
        {
            private int _num;

            public Foo(int num)
            {
                _num = num;
            }

            public int Number
            {
                get { return _num; }
            }

            #region IComparable<Foo> Members

            public int CompareTo(Foo other)
            {
                if (other == null)
                    return 1;
                return this.Number.CompareTo(other.Number);
            }

            #endregion
        }


        public CollectionUtilsTests()
        {

        }

        [Test]
        public void TestMinMaxWithValueType()
        {
            int[] numbers = new int[] { 3, 1, 2 };

            Assert.AreEqual(1, CollectionUtils.Min<int>(numbers, -1));
            Assert.AreEqual(3, CollectionUtils.Max<int>(numbers, -1));
        }

        [Test]
        public void TestMinMaxWithReferenceType()
        {
            string[] strings = new string[] { "b", "a", "d", "c" };

            Assert.AreEqual("a", CollectionUtils.Min<string>(strings));
            Assert.AreEqual("d", CollectionUtils.Max<string>(strings));

            string[] stringsWithNull = new string[] { null, "b", "a", "d", "c" };
            string[] stringsWithTwoNulls = new string[] { null, "b", "a", "d", null, "c" };

            // nulls are treated as less than any other value
            Assert.AreEqual(null, CollectionUtils.Min<string>(stringsWithNull));
            Assert.AreEqual("d", CollectionUtils.Max<string>(stringsWithNull));
            Assert.AreEqual(null, CollectionUtils.Min<string>(stringsWithTwoNulls));
            Assert.AreEqual("d", CollectionUtils.Max<string>(stringsWithTwoNulls));
        }

        [Test]
        public void TestMinMaxWithNullableType()
        {
            int?[] numbers = new int?[] { 3, 1, 2, null };

            // nulls are treated as less than any other value
            Assert.AreEqual(null, CollectionUtils.Min<int?>(numbers, null));
            Assert.AreEqual(3, CollectionUtils.Max<int?>(numbers, null));
        }

        [Test]
        public void TestMinMaxWithIComparableType()
        {
            Foo foo1 = new Foo(1);
            Foo foo2 = new Foo(2);
            Foo foo3 = new Foo(3);

            Foo[] foos = new Foo[] { foo3, foo1, foo2 };

            Assert.AreEqual(foo1, CollectionUtils.Min<Foo>(foos));
            Assert.AreEqual(foo3, CollectionUtils.Max<Foo>(foos));

            Foo[] foosWithNull = new Foo[] { foo3, foo1, foo2, null };

            // nulls are treated as less than any other value
            Assert.AreEqual(null, CollectionUtils.Min<Foo>(foosWithNull));
            Assert.AreEqual(foo3, CollectionUtils.Max<Foo>(foosWithNull));
        }

        [Test]
        public void TestMinMaxWithCustomComparison()
        {
            Foo foo1 = new Foo(1);
            Foo foo2 = new Foo(2);
            Foo foo3 = new Foo(3);

            Foo[] foosWithNull = new Foo[] { foo3, foo1, foo2, null };

            Assert.AreEqual(null, CollectionUtils.Min<Foo>(foosWithNull, null, CompareTreatNullAsMin));
            Assert.AreEqual(foo1, CollectionUtils.Min<Foo>(foosWithNull, null, CompareTreatNullAsMax));

            Assert.AreEqual(foo3, CollectionUtils.Max<Foo>(foosWithNull, null, CompareTreatNullAsMin));
            Assert.AreEqual(null, CollectionUtils.Max<Foo>(foosWithNull, null, CompareTreatNullAsMax));
        }

        /// <summary>
        /// Comparison that treats a null as less than any other value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CompareTreatNullAsMin(Foo x, Foo y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            return x.Number.CompareTo(y.Number);
        }

        /// <summary>
        /// Comparison that treats a null as greater than any other value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CompareTreatNullAsMax(Foo x, Foo y)
        {
            if (x == null && y == null)
                return 0;
            if (x == null)
                return 1;
            if (y == null)
                return -1;
            return x.Number.CompareTo(y.Number);
        }
    }
}

#endif
