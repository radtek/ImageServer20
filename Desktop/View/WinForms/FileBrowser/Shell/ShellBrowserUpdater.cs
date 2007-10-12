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

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ClearCanvas.Desktop.View.WinForms.FileBrowser.ShellDll
{
    internal class ShellBrowserUpdater : NativeWindow
    {
        #region Fields

        private ShellBrowser br;
        private uint notifyId = 0;

        #endregion

        public ShellBrowserUpdater(ShellBrowser br)
        {
            this.br = br;
            CreateHandle(new CreateParams());

            ShellAPI.SHChangeNotifyEntry entry = new ShellAPI.SHChangeNotifyEntry();
            entry.pIdl = br.DesktopItem.PIDLRel.Ptr;
            entry.Recursively = true;

            notifyId = ShellAPI.SHChangeNotifyRegister(
                this.Handle,
                ShellAPI.SHCNRF.InterruptLevel | ShellAPI.SHCNRF.ShellLevel,
                ShellAPI.SHCNE.ALLEVENTS | ShellAPI.SHCNE.INTERRUPT,
                ShellAPI.WM.SH_NOTIFY,
                1,
                new ShellAPI.SHChangeNotifyEntry[] { entry });
        }

        ~ShellBrowserUpdater()
        {
            if (notifyId > 0)
            {
                ShellAPI.SHChangeNotifyDeregister(notifyId);
                GC.SuppressFinalize(this);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)ShellAPI.WM.SH_NOTIFY)
            {
                ShellAPI.SHNOTIFYSTRUCT shNotify =
                    (ShellAPI.SHNOTIFYSTRUCT)Marshal.PtrToStructure(m.WParam, typeof(ShellAPI.SHNOTIFYSTRUCT));

                //Console.Out.WriteLine("Event: {0}", (ShellAPI.SHCNE)m.LParam);
                //if (shNotify.dwItem1 != IntPtr.Zero)
                    //PIDL.Write(shNotify.dwItem1);
                //if (shNotify.dwItem2 != IntPtr.Zero)
                    //PIDL.Write(shNotify.dwItem2);

                switch ((ShellAPI.SHCNE)m.LParam)
                {
                    #region File Changes

                    case ShellAPI.SHCNE.CREATE:
                        #region Create Item
                        {
                            if (!PIDL.IsEmpty(shNotify.dwItem1))
                            {
                                IntPtr parent, child, relative;
                                PIDL.SplitPidl(shNotify.dwItem1, out parent, out child);

                                PIDL parentPIDL = new PIDL(parent, false);
                                ShellItem parentItem = br.GetShellItem(parentPIDL);
                                if (parentItem != null && parentItem.FilesExpanded && !parentItem.SubFiles.Contains(child))
                                {
                                    ShellAPI.SHGetRealIDL(
                                        parentItem.ShellFolder,
                                        child,
                                        out relative);
                                    parentItem.AddItem(new ShellItem(br, parentItem, relative));
                                }
                                    
                                Marshal.FreeCoTaskMem(child);
                                parentPIDL.Free();
                            }
                        }
                        #endregion
                        break;

                    case ShellAPI.SHCNE.RENAMEITEM:
                        #region Rename Item
                        {
                            if (!PIDL.IsEmpty(shNotify.dwItem1) && !PIDL.IsEmpty(shNotify.dwItem2))
                            {
                                ShellItem item = br.GetShellItem(new PIDL(shNotify.dwItem1, true));
                                if (item != null)
                                    item.Update(shNotify.dwItem2, ShellItemUpdateType.Renamed);
                            }
                        }
                        #endregion
                        break;

                    case ShellAPI.SHCNE.DELETE:
                        #region Delete Item
                        {
                            if (!PIDL.IsEmpty(shNotify.dwItem1))
                            {
                                IntPtr parent, child;
                                PIDL.SplitPidl(shNotify.dwItem1, out parent, out child);

                                PIDL parentPIDL = new PIDL(parent, false);
                                ShellItem parentItem = br.GetShellItem(parentPIDL);
                                if (parentItem != null && parentItem.SubFiles.Contains(child))
                                    parentItem.RemoveItem(parentItem.SubFiles[child]);

                                Marshal.FreeCoTaskMem(child);
                                parentPIDL.Free();
                            }
                        }
                        #endregion
                        break;

                    case ShellAPI.SHCNE.UPDATEITEM:
                        #region Update Item
                        {
                            if (!PIDL.IsEmpty(shNotify.dwItem1))
                            {
                                ShellItem item = br.GetShellItem(new PIDL(shNotify.dwItem1, true));
                                if (item != null)
                                {
                                    Console.Out.WriteLine("Item: {0}", item);
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.Updated);
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.IconChange);
                                }
                            }
                        }
                        #endregion
                        break;

                    #endregion

                    #region Folder Changes

                    case ShellAPI.SHCNE.MKDIR:                        
                    case ShellAPI.SHCNE.DRIVEADD:
                    case ShellAPI.SHCNE.DRIVEADDGUI:                        
                        #region Make Directory
                        {
                            if (!PIDL.IsEmpty(shNotify.dwItem1))
                            {
                                IntPtr parent, child, relative;
                                PIDL.SplitPidl(shNotify.dwItem1, out parent, out child);

                                PIDL parentPIDL = new PIDL(parent, false);
                                ShellItem parentItem = br.GetShellItem(parentPIDL);
                                if (parentItem != null && parentItem.FoldersExpanded && !parentItem.SubFolders.Contains(child))
                                {
                                    ShellAPI.SHGetRealIDL(
                                        parentItem.ShellFolder,
                                        child,
                                        out relative);

                                    IntPtr shellFolderPtr;
                                    if (parentItem.ShellFolder.BindToObject(
                                                relative,
                                                IntPtr.Zero,
                                                ref ShellAPI.IID_IShellFolder,
                                                out shellFolderPtr) == ShellAPI.S_OK)
                                    {
                                        parentItem.AddItem(new ShellItem(br, parentItem, relative, shellFolderPtr));
                                    }
                                    else
                                        Marshal.FreeCoTaskMem(relative);
                                }

                                Marshal.FreeCoTaskMem(child);
                                parentPIDL.Free();
                            }
                        }
                        #endregion
                        break;

                    case ShellAPI.SHCNE.RENAMEFOLDER:
                        #region Rename Directory
                        {
                            if (!PIDL.IsEmpty(shNotify.dwItem1) && !PIDL.IsEmpty(shNotify.dwItem2))
                            {
                                ShellItem item = br.GetShellItem(new PIDL(shNotify.dwItem1, false));
                                if (item != null)
                                {
                                    //Console.Out.WriteLine("Update: {0}", item);
                                    item.Update(shNotify.dwItem2, ShellItemUpdateType.Renamed);
                                }
                            }
                        }
                        #endregion
                        break;

                    case ShellAPI.SHCNE.RMDIR:
                    case ShellAPI.SHCNE.DRIVEREMOVED:
                        #region Remove Directory
                        {
                            if (!PIDL.IsEmpty(shNotify.dwItem1))
                            {
                                IntPtr parent, child;
                                PIDL.SplitPidl(shNotify.dwItem1, out parent, out child);

                                PIDL parentPIDL = new PIDL(parent, false);
                                ShellItem parentItem = br.GetShellItem(parentPIDL);
                                if (parentItem != null && parentItem.SubFolders.Contains(child))
                                    parentItem.RemoveItem(parentItem.SubFolders[child]);

                                Marshal.FreeCoTaskMem(child);
                                parentPIDL.Free();
                            }
                        }
                        #endregion
                        break;

                    case ShellAPI.SHCNE.UPDATEDIR:
                    case ShellAPI.SHCNE.ATTRIBUTES:
                        #region Update Directory
                        {
                            if (!PIDL.IsEmpty(shNotify.dwItem1))
                            {
                                ShellItem item = br.GetShellItem(new PIDL(shNotify.dwItem1, true));
                                if (item != null)
                                {
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.Updated);
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.IconChange);
                                }
                            }
                        }
                        #endregion
                        break;

                    case ShellAPI.SHCNE.MEDIAINSERTED:
                    case ShellAPI.SHCNE.MEDIAREMOVED:
                        #region Media Change
                        {
                            if (!PIDL.IsEmpty(shNotify.dwItem1))
                            {
                                ShellItem item = br.GetShellItem(new PIDL(shNotify.dwItem1, true));
                                if (item != null)
                                    item.Update(IntPtr.Zero, ShellItemUpdateType.MediaChange);
                            }
                        }
                        #endregion
                        break;

                    #endregion

                    #region Other Changes

                    case ShellAPI.SHCNE.ASSOCCHANGED:
                        #region Update Images
                        {
                            
                        }
                        #endregion
                        break;

                    case ShellAPI.SHCNE.NETSHARE:
                    case ShellAPI.SHCNE.NETUNSHARE:
                        break;

                    case ShellAPI.SHCNE.UPDATEIMAGE:
                        UpdateRecycleBin();
                        break;

                    #endregion
                }
            }

            base.WndProc(ref m);
        }

        private void UpdateRecycleBin()
        {
            ShellItem recycleBin = br.DesktopItem["Recycle Bin"];
            if (recycleBin != null)
                recycleBin.Update(IntPtr.Zero, ShellItemUpdateType.IconChange);
        }
    }
}
