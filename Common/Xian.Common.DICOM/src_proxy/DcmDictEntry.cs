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

public class DcmDictEntry : DcmTagKey {
  private IntPtr swigCPtr;

  internal DcmDictEntry(IntPtr cPtr, bool cMemoryOwn) : base(DCMTKPINVOKE.DcmDictEntryUpcast(cPtr), cMemoryOwn) {
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(DcmDictEntry obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  protected DcmDictEntry() : this(IntPtr.Zero, false) {
  }

  ~DcmDictEntry() {
    Dispose();
  }

  public override void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_DcmDictEntry(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
    base.Dispose();
  }

  public DcmDictEntry(ushort g, ushort e, DcmVR vr, string nam, int vmMin, int vmMax, string vers, bool doCopyStrings, string pcreator) : this(DCMTKPINVOKE.new_DcmDictEntry__SWIG_0(g, e, DcmVR.getCPtr(vr), nam, vmMin, vmMax, vers, doCopyStrings, pcreator), true) {
  }

  public DcmDictEntry(ushort g, ushort e, ushort ug, ushort ue, DcmVR vr, string nam, int vmMin, int vmMax, string vers, bool doCopyStrings, string pcreator) : this(DCMTKPINVOKE.new_DcmDictEntry__SWIG_1(g, e, ug, ue, DcmVR.getCPtr(vr), nam, vmMin, vmMax, vers, doCopyStrings, pcreator), true) {
  }

  public DcmDictEntry(DcmDictEntry e) : this(DCMTKPINVOKE.new_DcmDictEntry__SWIG_2(DcmDictEntry.getCPtr(e)), true) {
  }

  public DcmVR getVR() {
    return new DcmVR(DCMTKPINVOKE.DcmDictEntry_getVR(swigCPtr), true);
  }

  public DcmEVR getEVR() {
    return (DcmEVR)DCMTKPINVOKE.DcmDictEntry_getEVR(swigCPtr);
  }

  public string getStandardVersion() {
    return DCMTKPINVOKE.DcmDictEntry_getStandardVersion(swigCPtr);
  }

  public string getTagName() {
    return DCMTKPINVOKE.DcmDictEntry_getTagName(swigCPtr);
  }

  public string getPrivateCreator() {
    return DCMTKPINVOKE.DcmDictEntry_getPrivateCreator(swigCPtr);
  }

  public int privateCreatorMatch(string c) {
    return DCMTKPINVOKE.DcmDictEntry_privateCreatorMatch__SWIG_0(swigCPtr, c);
  }

  public int privateCreatorMatch(DcmDictEntry arg) {
    return DCMTKPINVOKE.DcmDictEntry_privateCreatorMatch__SWIG_1(swigCPtr, DcmDictEntry.getCPtr(arg));
  }

  public int getVMMin() {
    return DCMTKPINVOKE.DcmDictEntry_getVMMin(swigCPtr);
  }

  public int getVMMax() {
    return DCMTKPINVOKE.DcmDictEntry_getVMMax(swigCPtr);
  }

  public bool isFixedSingleVM() {
    return DCMTKPINVOKE.DcmDictEntry_isFixedSingleVM(swigCPtr);
  }

  public bool isFixedRangeVM() {
    return DCMTKPINVOKE.DcmDictEntry_isFixedRangeVM(swigCPtr);
  }

  public bool isVariableRangeVM() {
    return DCMTKPINVOKE.DcmDictEntry_isVariableRangeVM(swigCPtr);
  }

  public void setUpper(DcmTagKey key) {
    DCMTKPINVOKE.DcmDictEntry_setUpper(swigCPtr, DcmTagKey.getCPtr(key));
  }

  public void setUpperGroup(ushort ug) {
    DCMTKPINVOKE.DcmDictEntry_setUpperGroup(swigCPtr, ug);
  }

  public void setUpperElement(ushort ue) {
    DCMTKPINVOKE.DcmDictEntry_setUpperElement(swigCPtr, ue);
  }

  public ushort getUpperGroup() {
    return DCMTKPINVOKE.DcmDictEntry_getUpperGroup(swigCPtr);
  }

  public ushort getUpperElement() {
    return DCMTKPINVOKE.DcmDictEntry_getUpperElement(swigCPtr);
  }

  public DcmTagKey getKey() {
    return new DcmTagKey(DCMTKPINVOKE.DcmDictEntry_getKey(swigCPtr), true);
  }

  public DcmTagKey getUpperKey() {
    return new DcmTagKey(DCMTKPINVOKE.DcmDictEntry_getUpperKey(swigCPtr), true);
  }

  public int isRepeatingGroup() {
    return DCMTKPINVOKE.DcmDictEntry_isRepeatingGroup(swigCPtr);
  }

  public int isRepeatingElement() {
    return DCMTKPINVOKE.DcmDictEntry_isRepeatingElement(swigCPtr);
  }

  public int isRepeating() {
    return DCMTKPINVOKE.DcmDictEntry_isRepeating(swigCPtr);
  }

  public DcmDictRangeRestriction getGroupRangeRestriction() {
    return (DcmDictRangeRestriction)DCMTKPINVOKE.DcmDictEntry_getGroupRangeRestriction(swigCPtr);
  }

  public void setGroupRangeRestriction(DcmDictRangeRestriction rr) {
    DCMTKPINVOKE.DcmDictEntry_setGroupRangeRestriction(swigCPtr, (int)rr);
  }

  public DcmDictRangeRestriction getElementRangeRestriction() {
    return (DcmDictRangeRestriction)DCMTKPINVOKE.DcmDictEntry_getElementRangeRestriction(swigCPtr);
  }

  public void setElementRangeRestriction(DcmDictRangeRestriction rr) {
    DCMTKPINVOKE.DcmDictEntry_setElementRangeRestriction(swigCPtr, (int)rr);
  }

  public int contains(DcmTagKey key, string privCreator) {
    return DCMTKPINVOKE.DcmDictEntry_contains__SWIG_0(swigCPtr, DcmTagKey.getCPtr(key), privCreator);
  }

  public int contains(string name) {
    return DCMTKPINVOKE.DcmDictEntry_contains__SWIG_1(swigCPtr, name);
  }

  public int subset(DcmDictEntry e) {
    return DCMTKPINVOKE.DcmDictEntry_subset(swigCPtr, DcmDictEntry.getCPtr(e));
  }

  public int setEQ(DcmDictEntry e) {
    return DCMTKPINVOKE.DcmDictEntry_setEQ(swigCPtr, DcmDictEntry.getCPtr(e));
  }

}

}
