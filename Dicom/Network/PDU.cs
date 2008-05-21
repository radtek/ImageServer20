#region License

// Copyright (c) 2006-2008, ClearCanvas Inc.
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

#region mDCM License
// mDCM: A C# DICOM library
//
// Copyright (c) 2008  Colby Dillion
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
// Author:
//    Colby Dillion (colby.dillion@gmail.com)
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using ClearCanvas.Dicom.IO;

namespace ClearCanvas.Dicom.Network
{
    #region Raw PDU
    public class RawPDU
    {
        #region Private members
        private readonly byte _type;
        private readonly Stream _is;
        private MemoryStream _ms;
        private BinaryReader _br;
        private readonly BinaryWriter _bw;
        private readonly Stack<long> _m16;
        private readonly Stack<long> _m32;
        #endregion

        #region Public Constructors
        public RawPDU(byte type)
        {
            _type = type;
            _ms = new MemoryStream();
            _bw = EndianBinaryWriter.Create(_ms, Endian.Big);
            _m16 = new Stack<long>();
            _m32 = new Stack<long>();
        }
        public RawPDU(Stream s)
        {
            BinaryReader br = EndianBinaryReader.Create(s, Endian.Big);

            _type = br.ReadByte();	// PDU-Type
            _is = s;
        }
        public RawPDU(byte[] d)
        {
            _is = new MemoryStream(d);

            BinaryReader br = EndianBinaryReader.Create(_is, Endian.Big);
            _type = br.ReadByte();
        }
        #endregion

        #region Public Properties
        public byte Type
        {
            get { return _type; }
        }
        public uint Length
        {
            get { return (uint)_ms.Length; }
        }
        #endregion

        #region Private Methods
        public void ReadPDU()
        {
            _ms = new MemoryStream();
            BinaryReader br = EndianBinaryReader.Create(_is, Endian.Big);

            br.ReadByte();
            uint len = br.ReadUInt32();	// PDU-Length

            byte[] data = br.ReadBytes((int)len);
            _ms = new MemoryStream(data, false);
            _br = EndianBinaryReader.Create(_ms, Endian.Big);
        }

        public void WritePDU(Stream s)
        {
            // Ran into a problem when the BinaryWriter was writing
            // directly to the stream where we would send just the "type" of the
            // PDU in its own packet, and an implementation was aborting the connection
            // after receiving this one packet w/ one byte.  Buffering up the data
            // caused it to be written at least in 6 bytes and it eliminated the problems.
            MemoryStream ms = new MemoryStream();

            BinaryWriter bw = EndianBinaryWriter.Create(ms, Endian.Big);
            bw.Write(_type);
            bw.Write((byte)0);
            bw.Write((uint)_ms.Length);

            ms.WriteTo(s);
            _ms.WriteTo(s);
            s.Flush();
        }
        #endregion

        #region Public Methods
        public void Save(String file)
        {
            FileInfo f = new FileInfo(file);
            DirectoryInfo d = f.Directory;

            if (!d.Exists)
            {
                d.Create();
            }
            using (FileStream fs = f.OpenWrite())
            {
                WritePDU(fs);
            }
        }

        public override String ToString()
        {
            return String.Format("Pdu[type={0:X2}, length={1}]", Type, Length);
        }

        public void Reset()
        {
            _ms.Seek(0, SeekOrigin.Begin);
        }

        #region Read Methods
        private void CheckOffset(int bytes, String name)
        {
            if ((_ms.Position + bytes) > _ms.Length)
            {
                String msg = String.Format("{0} (offset={1}, bytes={2}, field=\"{3}\") Requested offset out of range!", ToString(),
                    _ms.Position + 6, bytes, name);
                throw new NetworkException(msg);
            }
        }

        public byte ReadByte(String name)
        {
            CheckOffset(1, name);
            return _br.ReadByte();
        }

        public byte[] ReadBytes(String name, int count)
        {
            CheckOffset(count, name);
            return _br.ReadBytes(count);
        }

        public ushort ReadUInt16(String name)
        {
            CheckOffset(2, name);
            return _br.ReadUInt16();
        }

        public uint ReadUInt32(String name)
        {
            CheckOffset(4, name);
            return _br.ReadUInt32();
        }

        private readonly char[] trimChars = { ' ', '\0' };
        public String ReadString(String name, int count)
        {
            CheckOffset(count, name);
            char[] c = _br.ReadChars(count);
            return new String(c).Trim(trimChars);
        }

        public void SkipBytes(String name, int count)
        {
            CheckOffset(count, name);
            _ms.Seek(count, SeekOrigin.Current);
        }
        #endregion

        #region Write Methods
        public void Write(String name, byte v)
        {
            _bw.Write(v);
        }

        public void Write(String name, byte v, int count)
        {
            for (int i = 0; i < count; i++)
                _bw.Write(v);
        }

        public void Write(String name, byte[] v)
        {
            _bw.Write(v);
        }

        public void Write(String name, ushort v)
        {
            _bw.Write(v);
        }

        public void Write(String name, uint v)
        {
            _bw.Write(v);
        }

        public void Write(String name, String v)
        {
            _bw.Write(v.ToCharArray());
        }

        public void Write(String name, String v, int count, char pad)
        {
            _bw.Write(ToCharArray(v, count, pad));
        }

        public void MarkLength16(String name)
        {
            _m16.Push(_ms.Position);
            _bw.Write((ushort)0);
        }

        public void WriteLength16()
        {
            long p1 = _m16.Pop();
            long p2 = _ms.Position;
            _ms.Position = p1;
            _bw.Write((ushort)(p2 - p1 - 2));
            _ms.Position = p2;
        }

        public void MarkLength32(String name)
        {
            _m32.Push(_ms.Position);
            _bw.Write((uint)0);
        }

        public void WriteLength32()
        {
            long p1 = _m32.Pop();
            long p2 = _ms.Position;
            _ms.Position = p1;
            //DicomLogger.LogInfo(" 32 bit PDV length: {0}, end position: {1} start position: {2}", (uint)(p2-(p1+4)),p2, p1 );
            _bw.Write((uint)(p2 - (p1 + 4)));
            _ms.Position = p2;
        }

        private static char[] ToCharArray(String s, int l, char p)
        {
            char[] c = new char[l];
            for (int i = 0; i < l; i++)
            {
                if (i < s.Length)
                    c[i] = s[i];
                else
                    c[i] = p;
            }
            return c;
        }
        #endregion
        #endregion
    }
    #endregion

    #region PDU Interface
    public interface IPDU
    {
        RawPDU Write();
        void Read(RawPDU raw);
    }
    #endregion

    #region A-Associate-RQ
    public class AAssociateRQ : IPDU
    {
        private readonly AssociationParameters _assoc;

        public AAssociateRQ(AssociationParameters assoc)
        {
            _assoc = assoc;
        }

        #region Write
        public RawPDU Write()
        {
            RawPDU pdu = new RawPDU((byte)0x01);

            pdu.Write("Version", (ushort)0x0001);
            pdu.Write("Reserved", 0x00, 2);
            pdu.Write("Called AE", _assoc.CalledAE, 16, ' ');
            pdu.Write("Calling AE", _assoc.CallingAE, 16, ' ');
            pdu.Write("Reserved", 0x00, 32);

            // Application Context
            pdu.Write("Item-Type", (byte)0x10);
            pdu.Write("Reserved", (byte)0x00);
            pdu.MarkLength16("Item-Length");
            pdu.Write("Application Context Name", DicomUids.DICOMApplicationContextName.UID);
            pdu.WriteLength16();

            if (_assoc.GetPresentationContexts().Count == 0)
                throw new DicomException("No presentation contexts set for association");

            foreach (DicomPresContext pc in _assoc.GetPresentationContexts())
            {
                // Presentation Context
                pdu.Write("Item-Type", (byte)0x20);
                pdu.Write("Reserved", (byte)0x00);
                pdu.MarkLength16("Item-Length");
                pdu.Write("Presentation Context ID", (byte)pc.ID);
                pdu.Write("Reserved", (byte)0x00, 3);

                // Abstract Syntax
                pdu.Write("Item-Type", (byte)0x30);
                pdu.Write("Reserved", (byte)0x00);
                pdu.MarkLength16("Item-Length");
                pdu.Write("Abstract Syntax UID", pc.AbstractSyntax.Uid);
                pdu.WriteLength16();

                if (pc.GetTransfers().Count == 0)
                    throw new DicomException("No transfer syntaxes set for presentation context " + pc.AbstractSyntax.Name );

                // Transfer Syntax
                foreach (TransferSyntax ts in pc.GetTransfers())
                {
                    pdu.Write("Item-Type", (byte)0x40);
                    pdu.Write("Reserved", (byte)0x00);
                    pdu.MarkLength16("Item-Length");
                    pdu.Write("Transfer Syntax UID", ts.DicomUid.UID);
                    pdu.WriteLength16();
                }

                pdu.WriteLength16();
            }

            // User Data Fields
            pdu.Write("Item-Type", 0x50);
            pdu.Write("Reserved", 0x00);
            pdu.MarkLength16("Item-Length");

            // Maximum PDU
            pdu.Write("Item-Type", 0x51);
            pdu.Write("Reserved", 0x00);
            pdu.Write("Item-Length", (ushort)0x0004);
            pdu.Write("Max PDU Length", _assoc.LocalMaximumPduLength);

            // Asychronous Window
            /*
            pdu.Write("Item-Type", (byte)0x53);
            pdu.Write("Reserved", (byte)0x00);
            pdu.Write("Item-Length", (ushort)0x0004);
            pdu.Write("Max Operations Invoked", (ushort)_assoc.MaximumOperationsInvoked);
            pdu.Write("Max Operations Invoked", (ushort)_assoc.MaximumOperationsPerformed);
             */

            // SCU / SCP Role Selection


            // Implementation Class UID
            pdu.Write("Item-Type", (byte)0x52);
            pdu.Write("Reserved", (byte)0x00);
            pdu.MarkLength16("Item-Length");
            pdu.Write("Implementation Class UID", DicomImplementation.ClassUID.UID);
            pdu.WriteLength16();

            // Implementation Version
            pdu.Write("Item-Type", (byte)0x55);
            pdu.Write("Reserved", (byte)0x00);
            pdu.MarkLength16("Item-Length");
            pdu.Write("Implementation Version", DicomImplementation.Version);
            pdu.WriteLength16();

            pdu.WriteLength16();

            return pdu;
        }
        #endregion

        #region Read
        public void Read(RawPDU raw)
        {
            uint l = raw.Length;

            raw.ReadUInt16("Version");
            raw.SkipBytes("Reserved", 2);
            _assoc.CalledAE = raw.ReadString("Called AE", 16);
            _assoc.CallingAE = raw.ReadString("Calling AE", 16);
            raw.SkipBytes("Reserved", 32);
            l -= 2 + 2 + 16 + 16 + 32;

            while (l > 0)
            {
                byte type = raw.ReadByte("Item-Type");
                raw.SkipBytes("Reserved", 1);
                ushort il = raw.ReadUInt16("Item-Length");

                l -= 4 + (uint)il;

                if (type == 0x10)
                {
                    // Application Context
                    raw.SkipBytes("Application Context", il);
                }
                else

                    if (type == 0x20)
                    {
                        // Presentation Context
                        byte id = raw.ReadByte("Presentation Context ID");
                        raw.SkipBytes("Reserved", 3);
                        il -= 4;

                        while (il > 0)
                        {
                            byte pt = raw.ReadByte("Presentation Context Item-Type");
                            raw.SkipBytes("Reserved", 1);
                            ushort pl = raw.ReadUInt16("Presentation Context Item-Length");
                            string sx = raw.ReadString("Presentation Context Syntax UID", pl);
                            if (pt == 0x30)
                            {
                                SopClass sopClass = SopClass.GetSopClass(sx);
                                if (sopClass == null)
                                    sopClass = new SopClass("Private SOP Class", sx, false);
                                _assoc.AddPresentationContext(id, sopClass);
                            }
                            else if (pt == 0x40)
                            {
                                TransferSyntax transferSyntax = TransferSyntax.GetTransferSyntax(sx);
                                if (transferSyntax == null)
                                    transferSyntax = new TransferSyntax("Private Syntax", sx, true, false, true, false, false, false);
                                _assoc.AddTransferSyntax(id, transferSyntax);
                            }
                            il -= (ushort)(4 + pl);
                        }
                    }
                    else

                        if (type == 0x50)
                        {
                            // User Information
                            while (il > 0)
                            {
                                byte ut = raw.ReadByte("User Information Item-Type");
                                raw.SkipBytes("Reserved", 1);
                                ushort ul = raw.ReadUInt16("User Information Item-Length");
                                il -= (ushort)(4 + ul);
                                if (ut == 0x51)
                                {
                                    _assoc.RemoteMaximumPduLength = raw.ReadUInt32("Max PDU Length");
                                }
                                else if (ut == 0x52)
                                {
                                    _assoc.ImplementationClass = new DicomUid(raw.ReadString("Implementation Class UID", ul), "Implementation Class UID", UidType.Unknown);
                                }
                                else if (ut == 0x53)
                                {
                                    _assoc.MaxOperationsInvoked = raw.ReadUInt16("Max Operations Invoked");
                                    _assoc.MaxOperationsPerformed = raw.ReadUInt16("Max Operations Performed");
                                }
                                else if (ut == 0x55)
                                {
                                    _assoc.ImplementationVersion = raw.ReadString("Implementation Version", ul);
                                }
                                else if (ut == 0x54)
                                {
                                    raw.SkipBytes("SCU/SCP Role Selection", ul);
                                    /*
                                    ushort rsul = raw.ReadUInt16();
                                    if ((rsul + 4) != ul) {
                                        throw new DicomNetworkException("SCU/SCP role selection length (" + ul + " bytes) does not match uid length (" + rsul + " + 4 bytes)");
                                    }
                                    raw.ReadChars(rsul);	// Abstract Syntax
                                    raw.ReadByte();		// SCU role
                                    raw.ReadByte();		// SCP role
                                    */
                                }
                                else
                                {
                                    DicomLogger.LogError("Unhandled user item: 0x{0:x2} ({1} + 4 bytes)", ut, ul);
                                    raw.SkipBytes("Unhandled User Item", ul);
                                }
                            }
                        }
            }
        }
        #endregion
    }
    #endregion

    #region A-Associate-AC
    public class AAssociateAC : IPDU
    {
        private readonly AssociationParameters _assoc;

        public AAssociateAC(AssociationParameters assoc)
        {
            _assoc = assoc;
        }

        #region Write
        public RawPDU Write()
        {
            RawPDU pdu = new RawPDU((byte)0x02);

            pdu.Write("Version", (ushort)0x0001);
            pdu.Write("Reserved", 0x00, 2);
            pdu.Write("Called AE", _assoc.CalledAE, 16, ' ');
            pdu.Write("Calling AE", _assoc.CallingAE, 16, ' ');
            pdu.Write("Reserved", 0x00, 32);

            // Application Context
            pdu.Write("Item-Type", (byte)0x10);
            pdu.Write("Reserved", (byte)0x00);
            pdu.MarkLength16("Item-Length");
            pdu.Write("Application Context Name", DicomUids.DICOMApplicationContextName.UID);
            pdu.WriteLength16();

            foreach (DicomPresContext pc in _assoc.GetPresentationContexts())
            {
                // Presentation Context
                pdu.Write("Item-Type", (byte)0x21);
                pdu.Write("Reserved", (byte)0x00);
                pdu.MarkLength16("Item-Length");
                pdu.Write("Presentation Context ID", (byte)pc.ID);
                pdu.Write("Reserved", (byte)0x00);
                pdu.Write("Result", (byte)pc.Result);
                pdu.Write("Reserved", (byte)0x00);

                // Transfer Syntax
                pdu.Write("Item-Type", (byte)0x40);
                pdu.Write("Reserved", (byte)0x00);
                pdu.MarkLength16("Item-Length");
                if (pc.AcceptedTransferSyntax!=null) // 
                    pdu.Write("Transfer Syntax UID", pc.AcceptedTransferSyntax.DicomUid.UID);
                pdu.WriteLength16();

                pdu.WriteLength16();
            }

            // User Data Fields
            pdu.Write("Item-Type", (byte)0x50);
            pdu.Write("Reserved", (byte)0x00);
            pdu.MarkLength16("Item-Length");

            // Maximum PDU
            pdu.Write("Item-Type", (byte)0x51);
            pdu.Write("Reserved", (byte)0x00);
            pdu.Write("Item-Length", (ushort)0x0004);
            pdu.Write("Max PDU Length", (uint)_assoc.LocalMaximumPduLength);

            // Implementation Class UID
            pdu.Write("Item-Type", (byte)0x52);
            pdu.Write("Reserved", (byte)0x00);
            pdu.MarkLength16("Item-Length");
            pdu.Write("Implementation Class UID", DicomImplementation.ClassUID.UID);
            pdu.WriteLength16();

            // Implementation Version
            pdu.Write("Item-Type", (byte)0x55);
            pdu.Write("Reserved", (byte)0x00);
            pdu.MarkLength16("Item-Length");
            pdu.Write("Implementation Version", DicomImplementation.Version);
            pdu.WriteLength16();

            pdu.WriteLength16();

            return pdu;
        }
        #endregion

        #region Read
        public void Read(RawPDU raw)
        {
            uint l = raw.Length;

            raw.ReadUInt16("Version");
            raw.SkipBytes("Reserved", 2);
            raw.SkipBytes("Reserved", 16);
            raw.SkipBytes("Reserved", 16);
            raw.SkipBytes("Reserved", 32);
            l -= 68;

            while (l > 0)
            {
                byte type = raw.ReadByte("Item-Type");
                l -= 1;

                if (type == 0x10)
                {
                    // Application Context
                    raw.SkipBytes("Reserved", 1);
                    ushort c = raw.ReadUInt16("Item-Length");
                    raw.SkipBytes("Value", (int)c);
                    l -= 3 + (uint)c;
                }
                else

                    if (type == 0x21)
                    {
                        // Presentation Context
                        raw.ReadByte("Reserved");
                        ushort pl = raw.ReadUInt16("Presentation Context Item-Length");
                        byte id = raw.ReadByte("Presentation Context ID");
                        raw.ReadByte("Reserved");
                        byte res = raw.ReadByte("Presentation Context Result/Reason");
                        raw.ReadByte("Reserved");
                        l -= (uint)pl + 3;
                        pl -= 4;

                        // Presentation Context Transfer Syntax
                        raw.ReadByte("Presentation Context Item-Type (0x40)");
                        raw.ReadByte("Reserved");
                        ushort tl = raw.ReadUInt16("Presentation Context Item-Length");
                        string tx = raw.ReadString("Presentation Context Syntax UID", tl);
                        pl -= (ushort)(tl + 4);

                        _assoc.SetPresentationContextResult(id, (DicomPresContextResult)res);
                        TransferSyntax acceptedSyntax = TransferSyntax.GetTransferSyntax(tx);
                        if (acceptedSyntax != null)
                            _assoc.SetAcceptedTransferSyntax(id, acceptedSyntax);
                        else 
                            _assoc.GetPresentationContext(id).ClearTransfers();
                    }
                    else

                        if (type == 0x50)
                        {
                            // User Information
                            raw.ReadByte("Reserved");
                            ushort il = raw.ReadUInt16("User Information Item-Length");
                            l -= (uint)(il + 3);
                            while (il > 0)
                            {
                                byte ut = raw.ReadByte("User Item-Type");
                                raw.ReadByte("Reserved");
                                ushort ul = raw.ReadUInt16("User Item-Length");
                                il -= (ushort)(ul + 4);
                                if (ut == 0x51)
                                {
                                    _assoc.RemoteMaximumPduLength = raw.ReadUInt32("Max PDU Length");
                                }
                                else if (ut == 0x52)
                                {
                                    _assoc.ImplementationClass = DicomUids.Lookup(raw.ReadString("Implementation Class UID", ul));
                                }
                                else if (ut == 0x55)
                                {
                                    _assoc.ImplementationVersion = raw.ReadString("Implementation Version", ul);
                                }
                                else
                                {
                                    raw.SkipBytes("User Item Value", (int)ul);
                                }
                            }
                        }

                        else
                        {
                            raw.SkipBytes("Reserved", 1);
                            ushort il = raw.ReadUInt16("User Item-Length");
                            raw.SkipBytes("Unknown User Item", il);
                            l -= (uint)(il + 3);
                        }
            }
        }
        #endregion
    }
    #endregion

    #region A-Associate-RJ
    /// <summary>
    /// An enumerated value to represent if the association is being rejected for permanent or transient reasons.
    /// </summary>
    public enum DicomRejectResult
    {
        Permanent = 1,
        Transient = 2
    }

    /// <summary>
    /// An enumerated value to represent the source of an association being rejected.
    /// </summary>
    public enum DicomRejectSource
    {
        ServiceUser = 1,
        ServiceProviderACSE = 2,
        ServiceProviderPresentation = 3
    }

    /// <summary>
    /// An enumerated value represneting the reason for an association being rejected.
    /// </summary>
    public enum DicomRejectReason
    {
        // Service User
        NoReasonGiven = 1,
        ApplicationContextNotSupported = 2,
        CallingAENotRecognized = 3,
        CalledAENotRecognized = 7,

        // Service Provider ACSE
        ProtocolVersionNotSupported = 1,

        // Service Provider Presentation
        TemporaryCongestion = 1,
        LocalLimitExceeded = 2
    }

    public class AAssociateRJ : IPDU
    {
        private DicomRejectResult _rt = DicomRejectResult.Permanent;
        private DicomRejectSource _so = DicomRejectSource.ServiceUser;
        private DicomRejectReason _rn = DicomRejectReason.NoReasonGiven;

        public AAssociateRJ()
        {
        }
        public AAssociateRJ(DicomRejectResult rt, DicomRejectSource so, DicomRejectReason rn)
        {
            _rt = rt;
            _so = so;
            _rn = rn;
        }

        public DicomRejectResult Result
        {
            get { return _rt; }
        }
        public DicomRejectSource Source
        {
            get { return _so; }
        }
        public DicomRejectReason Reason
        {
            get { return _rn; }
        }

        public RawPDU Write()
        {
            RawPDU pdu = new RawPDU((byte)0x03);
            pdu.Write("Reserved", (byte)0x00);
            pdu.Write("Result", (byte)_rt);
            pdu.Write("Source", (byte)_so);
            pdu.Write("Reason", (byte)_rn);
            return pdu;
        }

        public void Read(RawPDU raw)
        {
            raw.ReadByte("Reserved");
            _rt = (DicomRejectResult)raw.ReadByte("Result");
            _so = (DicomRejectSource)raw.ReadByte("Source");
            _rn = (DicomRejectReason)raw.ReadByte("Reason");
        }
    }
    #endregion

    #region A-Release-RQ
    public class AReleaseRQ : IPDU
    {
        public RawPDU Write()
        {
            RawPDU pdu = new RawPDU((byte)0x05);
            pdu.Write("Reserved", (uint)0x00000000);
            return pdu;
        }

        public void Read(RawPDU raw)
        {
            raw.ReadUInt32("Reserved");
        }
    }
    #endregion

    #region A-Release-RP
    public class AReleaseRP : IPDU
    {
        public RawPDU Write()
        {
            RawPDU pdu = new RawPDU((byte)0x06);
            pdu.Write("Reserved", (uint)0x00000000);
            return pdu;
        }

        public void Read(RawPDU raw)
        {
            raw.ReadUInt32("Reserved");
        }
    }
    #endregion

    #region A-Abort
    public enum DicomAbortSource
    {
        Unknown = 0,
        ServiceUser = 1,
        ServiceProvider = 2
    }
    public enum DicomAbortReason
    {
        NotSpecified = 0,
        UnrecognizedPDU = 1,
        UnexpectedPDU = 2,
        UnrecognizedPDUParameter = 4,
        UnexpectedPDUParameter = 5,
        InvalidPDUParameter = 6
    }

    public class AAbort : IPDU
    {
        private DicomAbortSource _s = DicomAbortSource.ServiceUser;
        public DicomAbortSource Source
        {
            get { return _s; }
        }

        private DicomAbortReason _r = DicomAbortReason.NotSpecified;
        public DicomAbortReason Reason
        {
            get { return _r; }
        }

        public AAbort()
        {
        }
        public AAbort(DicomAbortSource s, DicomAbortReason r)
        {
            _s = s; _r = r;
        }

        #region Write
        public RawPDU Write()
        {
            RawPDU pdu = new RawPDU((byte)0x07);
            pdu.Write("Reserved", (byte)0x00);
            pdu.Write("Reserved", (byte)0x00);
            pdu.Write("Source", (byte)_s);
            pdu.Write("Reason", (byte)_r);
            return pdu;
        }
        #endregion

        #region Read
        public void Read(RawPDU raw)
        {
            raw.ReadByte("Reserved");
            raw.ReadByte("Reserved");
            _s = (DicomAbortSource)raw.ReadByte("Source");
            _r = (DicomAbortReason)raw.ReadByte("Reason");
        }
        #endregion
    }
    #endregion

    #region P-Data-TF
    public class PDataTF : IPDU
    {
        public PDataTF()
        {
        }

        private readonly List<PDV> _pdvs = new List<PDV>();
        public List<PDV> PDVs
        {
            get { return _pdvs; }
        }

        public uint GetLengthOfPDVs()
        {
            uint len = 0;
            foreach (PDV pdv in _pdvs)
            {
                len += pdv.PDVLength;
            }
            return len;
        }

        #region Write
        public RawPDU Write()
        {
            RawPDU pdu = new RawPDU(0x04);
            foreach (PDV pdv in _pdvs)
            {
                pdv.Write(pdu);
            }
            return pdu;
        }
        #endregion

        #region Read
        public void Read(RawPDU raw)
        {
            uint len = raw.Length;
            uint read = 0;
            while (read < len)
            {
                PDV pdv = new PDV();
                read += pdv.Read(raw);
                _pdvs.Add(pdv);
            }
        }
        #endregion
    }
    #endregion

    #region PDV
    public class PDV
    {
        private byte _pcid;
        private byte[] _value = new byte[0];
        private bool _command = false;
        private bool _last = false;

        public PDV(byte pcid, byte[] value, bool command, bool last)
        {
            _pcid = pcid;
            _value = value;
            _command = command;
            _last = last;
        }
        public PDV()
        {
        }

        public byte PCID
        {
            get { return _pcid; }
            set { _pcid = value; }
        }
        public byte[] Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public bool IsCommand
        {
            get { return _command; }
            set { _command = value; }
        }
        public bool IsLastFragment
        {
            get { return _last; }
            set { _last = value; }
        }

        /// <summary>
        /// Returns entire length of PDV (the data length + 6 byte header).
        /// </summary>
        public uint PDVLength
        {
            get { return (uint)_value.Length + 6; }
        }

        #region Write
        public void Write(RawPDU pdu)
        {
            byte mch = (byte)((_last ? 2 : 0) + (_command ? 1 : 0));
            pdu.MarkLength32("PDV-Length");
            pdu.Write("Presentation Context ID", _pcid);
            pdu.Write("Message Control Header", mch);
            pdu.Write("PDV Value", _value);
            pdu.WriteLength32();
        }
        #endregion

        #region Read
        public uint Read(RawPDU raw)
        {
            uint len = raw.ReadUInt32("PDV-Length");
            _pcid = raw.ReadByte("Presentation Context ID");
            byte mch = raw.ReadByte("Message Control Header");
            _value = raw.ReadBytes("PDV Value", (int)len - 2);
            _command = (mch & 0x01) != 0;
            _last = (mch & 0x02) != 0;
            return len + 4;
        }
        #endregion
    }
    #endregion
}
