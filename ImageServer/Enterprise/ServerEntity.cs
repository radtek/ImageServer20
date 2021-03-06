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

using System;
using ClearCanvas.Enterprise.Common;
using ClearCanvas.Enterprise.Core;

namespace ClearCanvas.ImageServer.Enterprise
{
    [Serializable] // TH (Oct 5, 2007): All entity objects should be serializable to use in ASP.NET app
    public abstract class ServerEntity : Entity
    {
        #region Private Members
        private readonly object _syncRoot = new object();
        private ServerEntityKey _key;
        private readonly String _name;

        #endregion

        #region Constructors

        public ServerEntity(String name)
        {
            _name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the ServerEntity object.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        public ServerEntityKey Key
        {
            get { return _key; }
        }

        protected object SyncRoot
        {
            get { return _syncRoot; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Set the primary key of the ServerEntity object.
        /// </summary>
        /// <param name="key"></param>
        public void SetKey(ServerEntityKey key)
        {
            _key = key;
        }

        /// <summary>
        /// Get the primary key of the ServerEntity object.
        /// </summary>
        /// <returns>A <see cref="ServerEntityKey"/> object representating the primary key.</returns>
        public ServerEntityKey GetKey()
        {
            if (_key == null)
                throw new InvalidOperationException("Cannot generate entity ref on transient entity");

            return _key;
        }

        /// <summary>
        /// Not supported by ServerEntity objects
        /// </summary>
        /// <returns></returns>
        public override EntityRef GetRef()
        {
            throw new InvalidOperationException("Not supported by ServerEntity");
        }

        #endregion
    }
}
