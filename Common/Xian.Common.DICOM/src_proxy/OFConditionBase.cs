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

public class OFConditionBase : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal OFConditionBase(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(OFConditionBase obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  protected OFConditionBase() : this(IntPtr.Zero, false) {
  }

  ~OFConditionBase() {
    Dispose();
  }

  public virtual void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_OFConditionBase(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
  }

  public virtual OFConditionBase clone() {
    IntPtr cPtr = DCMTKPINVOKE.OFConditionBase_clone(swigCPtr);
    return (cPtr == IntPtr.Zero) ? null : new OFConditionBase(cPtr, false);
  }

  public virtual uint codeAndModule() {
    return DCMTKPINVOKE.OFConditionBase_codeAndModule(swigCPtr);
  }

  public virtual OFStatus status() {
    return (OFStatus)DCMTKPINVOKE.OFConditionBase_status(swigCPtr);
  }

  public virtual string text() {
    return DCMTKPINVOKE.OFConditionBase_text(swigCPtr);
  }

  public virtual bool deletable() {
    return DCMTKPINVOKE.OFConditionBase_deletable(swigCPtr);
  }

  public ushort module() {
    return DCMTKPINVOKE.OFConditionBase_module(swigCPtr);
  }

  public ushort code() {
    return DCMTKPINVOKE.OFConditionBase_code(swigCPtr);
  }

}

}
