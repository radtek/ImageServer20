/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 1.3.25
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace ClearCanvas.Common.Dicom {

using System;
using System.Text;
using System.Runtime.InteropServices;

public class DcmShortText : DcmCharString {
  private HandleRef swigCPtr;

  internal DcmShortText(IntPtr cPtr, bool cMemoryOwn) : base(DCMTKPINVOKE.DcmShortTextUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(DcmShortText obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~DcmShortText() {
    Dispose();
  }

  public override void Dispose() {
    if(swigCPtr.Handle != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_DcmShortText(swigCPtr);
    }
    swigCPtr = new HandleRef(null, IntPtr.Zero);
    GC.SuppressFinalize(this);
    base.Dispose();
  }

  public DcmShortText(DcmTag tag, uint len) : this(DCMTKPINVOKE.new_DcmShortText__SWIG_0(DcmTag.getCPtr(tag), len), true) {
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
  }

  public DcmShortText(DcmTag tag) : this(DCMTKPINVOKE.new_DcmShortText__SWIG_1(DcmTag.getCPtr(tag)), true) {
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
  }

  public DcmShortText(DcmShortText old) : this(DCMTKPINVOKE.new_DcmShortText__SWIG_2(DcmShortText.getCPtr(old)), true) {
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
  }

  public override DcmEVR ident() {
    DcmEVR ret = (DcmEVR)DCMTKPINVOKE.DcmShortText_ident(swigCPtr);
    return ret;
  }

  public override uint getVM() {
    uint ret = DCMTKPINVOKE.DcmShortText_getVM(swigCPtr);
    return ret;
  }

  public override OFCondition getOFString(StringBuilder stringVal, uint pos, bool normalize) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmShortText_getOFString__SWIG_0(swigCPtr, stringVal, pos, normalize), true);
    return ret;
  }

  public override OFCondition getOFString(StringBuilder stringVal, uint pos) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmShortText_getOFString__SWIG_1(swigCPtr, stringVal, pos), true);
    return ret;
  }

  public override OFCondition getOFStringArray(StringBuilder stringVal, bool normalize) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmShortText_getOFStringArray__SWIG_0(swigCPtr, stringVal, normalize), true);
    return ret;
  }

  public override OFCondition getOFStringArray(StringBuilder stringVal) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmShortText_getOFStringArray__SWIG_1(swigCPtr, stringVal), true);
    return ret;
  }

}

}
