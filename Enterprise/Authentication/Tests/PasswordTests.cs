﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
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
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;
using NUnit.Framework;
using System.Threading;

namespace ClearCanvas.Enterprise.Authentication.Tests
{
	[TestFixture]
	public class PasswordTests
	{
		private const string DefaultPassword = "password";
		private const string AlternatePassword = "clearcanvas";

		public PasswordTests()
		{
			// set the extension factory to special test factory
			Platform.SetExtensionFactory(new NullExtensionFactory());
		}

		[Test]
		public void Test_CreatePassword()
		{
			DateTime expiry = DateTime.Now + TimeSpan.FromDays(30);
			Password password = Password.CreatePassword(DefaultPassword, expiry);

			Assert.AreEqual(expiry, password.ExpiryTime);
			Assert.IsFalse(password.IsExpired(DateTime.Now));
            Assert.IsTrue(password.Verify(DefaultPassword));
		}

		[Test]
		public void Test_CreatePassword_NoExpiry()
		{
			Password password = Password.CreatePassword(DefaultPassword, null);

			Assert.AreEqual(null, password.ExpiryTime);
			Assert.IsFalse(password.IsExpired(DateTime.Now));

			Assert.IsFalse(password.IsExpired(DateTime.MinValue));	// never expires
			Assert.IsFalse(password.IsExpired(DateTime.MaxValue));	// never expires
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Test_CreatePassword_NullText()
		{
			Password.CreatePassword(null, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void Test_CreatePassword_EmptyText()
		{
			Password.CreatePassword("", null);
		}

		[Test]
		public void Test_CreateTemporaryPassword()
		{
			// should expire immediately
			Password password = Password.CreateTemporaryPassword(DefaultPassword);

			Assert.IsNotNull(password.ExpiryTime);

			Thread.Sleep(10);	// allow password to expire

			Assert.Less(password.ExpiryTime.Value, DateTime.Now);
			Assert.IsTrue(password.IsExpired(DateTime.Now));
			Assert.IsTrue(password.Verify(DefaultPassword));
		}

		[Test]
		public void Test_IsExpired()
		{
			DateTime now = DateTime.Now;
			DateTime expiry = now + TimeSpan.FromMilliseconds(100);	// expire in 100 msec
			Password password = Password.CreatePassword(DefaultPassword, expiry);

			Assert.IsFalse(password.IsExpired(now));
			Thread.Sleep(150);	// sleep longer than 100 msec
			Assert.IsTrue(password.IsExpired(DateTime.Now));
		}

		[Test]
		public void Test_Verify()
		{
			Password password = Password.CreatePassword(DefaultPassword, null);
			Assert.IsFalse(password.Verify(null));
			Assert.IsFalse(password.Verify(""));
			Assert.IsFalse(password.Verify(AlternatePassword));
			Assert.IsTrue(password.Verify(DefaultPassword));

			password = Password.CreatePassword(AlternatePassword, null);
			Assert.IsFalse(password.Verify(null));
			Assert.IsFalse(password.Verify(""));
			Assert.IsFalse(password.Verify(DefaultPassword));
			Assert.IsTrue(password.Verify(AlternatePassword));

		}

		[Test]
		public void Test_Verify_Expired()
		{
			// create an expired password
			Password password = Password.CreatePassword(AlternatePassword, DateTime.MinValue);
			Assert.IsTrue(password.IsExpired(DateTime.Now));

			// Verify behaviour is not affected by the expiry time - it can succeed even
			// for an expired password
			Assert.IsFalse(password.Verify(null));
			Assert.IsFalse(password.Verify(""));
			Assert.IsFalse(password.Verify(DefaultPassword));
			Assert.IsTrue(password.Verify(AlternatePassword));
		}
	}
}

#endif
