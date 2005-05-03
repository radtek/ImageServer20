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

public class T_DIMSE_C_MoveRQ : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal T_DIMSE_C_MoveRQ(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(T_DIMSE_C_MoveRQ obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~T_DIMSE_C_MoveRQ() {
    Dispose();
  }

  public virtual void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_T_DIMSE_C_MoveRQ(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
  }

  public ushort MessageID {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_MoveRQ_MessageID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_C_MoveRQ_MessageID(swigCPtr);
    } 
  }

  public string AffectedSOPClassUID {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_MoveRQ_AffectedSOPClassUID(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_C_MoveRQ_AffectedSOPClassUID(swigCPtr);
    } 
  }

  public T_DIMSE_Priority Priority {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_MoveRQ_Priority(swigCPtr, (int)value);
    } 
    get {
      return (T_DIMSE_Priority)DCMTKPINVOKE.get_T_DIMSE_C_MoveRQ_Priority(swigCPtr);
    } 
  }

  public T_DIMSE_DataSetType DataSetType {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_MoveRQ_DataSetType(swigCPtr, (int)value);
    } 
    get {
      return (T_DIMSE_DataSetType)DCMTKPINVOKE.get_T_DIMSE_C_MoveRQ_DataSetType(swigCPtr);
    } 
  }

  public string MoveDestination {
    set {
      DCMTKPINVOKE.set_T_DIMSE_C_MoveRQ_MoveDestination(swigCPtr, value);
    } 
    get {
      return DCMTKPINVOKE.get_T_DIMSE_C_MoveRQ_MoveDestination(swigCPtr);
    } 
  }

  public T_DIMSE_C_MoveRQ() : this(DCMTKPINVOKE.new_T_DIMSE_C_MoveRQ(), true) {
  }

}

}
