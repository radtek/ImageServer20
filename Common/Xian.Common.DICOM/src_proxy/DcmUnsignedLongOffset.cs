/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 1.3.24
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace Xian.Common.DICOM {

using System;
using System.Text;

public class DcmUnsignedLongOffset : DcmUnsignedLong {
  private IntPtr swigCPtr;

  internal DcmUnsignedLongOffset(IntPtr cPtr, bool cMemoryOwn) : base(DCMTKPINVOKE.DcmUnsignedLongOffsetUpcast(cPtr), cMemoryOwn) {
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(DcmUnsignedLongOffset obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  protected DcmUnsignedLongOffset() : this(IntPtr.Zero, false) {
  }

  ~DcmUnsignedLongOffset() {
    Dispose();
  }

  public override void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_DcmUnsignedLongOffset(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
    base.Dispose();
  }

  public DcmUnsignedLongOffset(DcmTag tag, uint len) : this(DCMTKPINVOKE.new_DcmUnsignedLongOffset__SWIG_0(DcmTag.getCPtr(tag), len), true) {
  }

  public DcmUnsignedLongOffset(DcmTag tag) : this(DCMTKPINVOKE.new_DcmUnsignedLongOffset__SWIG_1(DcmTag.getCPtr(tag)), true) {
  }

  public DcmUnsignedLongOffset(DcmUnsignedLongOffset old) : this(DCMTKPINVOKE.new_DcmUnsignedLongOffset__SWIG_2(DcmUnsignedLongOffset.getCPtr(old)), true) {
  }

  public override DcmEVR ident() {
    return (DcmEVR)DCMTKPINVOKE.DcmUnsignedLongOffset_ident(swigCPtr);
  }

  public override OFCondition clear() {
    return new OFCondition(DCMTKPINVOKE.DcmUnsignedLongOffset_clear(swigCPtr), true);
  }

  public virtual DcmObject getNextRecord() {
    IntPtr cPtr = DCMTKPINVOKE.DcmUnsignedLongOffset_getNextRecord(swigCPtr);
    return (cPtr == IntPtr.Zero) ? null : new DcmObject(cPtr, false);
  }

  public virtual DcmObject setNextRecord(DcmObject record) {
    IntPtr cPtr = DCMTKPINVOKE.DcmUnsignedLongOffset_setNextRecord(swigCPtr, DcmObject.getCPtr(record));
    return (cPtr == IntPtr.Zero) ? null : new DcmObject(cPtr, false);
  }

  public override OFCondition verify(bool autocorrect) {
    return new OFCondition(DCMTKPINVOKE.DcmUnsignedLongOffset_verify__SWIG_0(swigCPtr, autocorrect), true);
  }

  public override OFCondition verify() {
    return new OFCondition(DCMTKPINVOKE.DcmUnsignedLongOffset_verify__SWIG_1(swigCPtr), true);
  }

}

}
