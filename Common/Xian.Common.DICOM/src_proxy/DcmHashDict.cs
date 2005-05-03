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

public class DcmHashDict : IDisposable {
  private IntPtr swigCPtr;
  protected bool swigCMemOwn;

  internal DcmHashDict(IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(DcmHashDict obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  ~DcmHashDict() {
    Dispose();
  }

  public virtual void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_DcmHashDict(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
  }

  public DcmHashDict(int hashTabLen) : this(DCMTKPINVOKE.new_DcmHashDict__SWIG_0(hashTabLen), true) {
  }

  public DcmHashDict() : this(DCMTKPINVOKE.new_DcmHashDict__SWIG_1(), true) {
  }

  public int size() {
    return DCMTKPINVOKE.DcmHashDict_size(swigCPtr);
  }

  public void clear() {
    DCMTKPINVOKE.DcmHashDict_clear(swigCPtr);
  }

  public void put(DcmDictEntry e) {
    DCMTKPINVOKE.DcmHashDict_put(swigCPtr, DcmDictEntry.getCPtr(e));
  }

  public DcmDictEntry get(DcmTagKey key, string privCreator) {
    IntPtr cPtr = DCMTKPINVOKE.DcmHashDict_get(swigCPtr, DcmTagKey.getCPtr(key), privCreator);
    return (cPtr == IntPtr.Zero) ? null : new DcmDictEntry(cPtr, false);
  }

  public void del(DcmTagKey k, string privCreator) {
    DCMTKPINVOKE.DcmHashDict_del(swigCPtr, DcmTagKey.getCPtr(k), privCreator);
  }

  public DcmHashDictIterator begin() {
    return new DcmHashDictIterator(DCMTKPINVOKE.DcmHashDict_begin(swigCPtr), true);
  }

  public DcmHashDictIterator end() {
    return new DcmHashDictIterator(DCMTKPINVOKE.DcmHashDict_end(swigCPtr), true);
  }

  public SWIGTYPE_p_ostream loadSummary(SWIGTYPE_p_ostream outStream) {
    return new SWIGTYPE_p_ostream(DCMTKPINVOKE.DcmHashDict_loadSummary(swigCPtr, SWIGTYPE_p_ostream.getCPtr(outStream)), false);
  }

}

}
