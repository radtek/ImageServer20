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

public class DcmPersonName : DcmCharString {
  private IntPtr swigCPtr;

  internal DcmPersonName(IntPtr cPtr, bool cMemoryOwn) : base(DCMTKPINVOKE.DcmPersonNameUpcast(cPtr), cMemoryOwn) {
    swigCPtr = cPtr;
  }

  internal static IntPtr getCPtr(DcmPersonName obj) {
    return (obj == null) ? IntPtr.Zero : obj.swigCPtr;
  }

  protected DcmPersonName() : this(IntPtr.Zero, false) {
  }

  ~DcmPersonName() {
    Dispose();
  }

  public override void Dispose() {
    if(swigCPtr != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_DcmPersonName(swigCPtr);
    }
    swigCPtr = IntPtr.Zero;
    GC.SuppressFinalize(this);
    base.Dispose();
  }

  public DcmPersonName(DcmTag tag, uint len) : this(DCMTKPINVOKE.new_DcmPersonName__SWIG_0(DcmTag.getCPtr(tag), len), true) {
  }

  public DcmPersonName(DcmTag tag) : this(DCMTKPINVOKE.new_DcmPersonName__SWIG_1(DcmTag.getCPtr(tag)), true) {
  }

  public DcmPersonName(DcmPersonName old) : this(DCMTKPINVOKE.new_DcmPersonName__SWIG_2(DcmPersonName.getCPtr(old)), true) {
  }

  public override DcmEVR ident() {
    return (DcmEVR)DCMTKPINVOKE.DcmPersonName_ident(swigCPtr);
  }

  public override OFCondition getOFString(StringBuilder stringVal, uint pos, bool normalize) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getOFString__SWIG_0(swigCPtr, stringVal, pos, normalize), true);
  }

  public override OFCondition getOFString(StringBuilder stringVal, uint pos) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getOFString__SWIG_1(swigCPtr, stringVal, pos), true);
  }

  public OFCondition getNameComponents(StringBuilder lastName, StringBuilder firstName, StringBuilder middleName, StringBuilder namePrefix, StringBuilder nameSuffix, uint pos, uint componentGroup) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getNameComponents__SWIG_0(swigCPtr, lastName, firstName, middleName, namePrefix, nameSuffix, pos, componentGroup), true);
  }

  public OFCondition getNameComponents(StringBuilder lastName, StringBuilder firstName, StringBuilder middleName, StringBuilder namePrefix, StringBuilder nameSuffix, uint pos) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getNameComponents__SWIG_1(swigCPtr, lastName, firstName, middleName, namePrefix, nameSuffix, pos), true);
  }

  public OFCondition getNameComponents(StringBuilder lastName, StringBuilder firstName, StringBuilder middleName, StringBuilder namePrefix, StringBuilder nameSuffix) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getNameComponents__SWIG_2(swigCPtr, lastName, firstName, middleName, namePrefix, nameSuffix), true);
  }

  public OFCondition getFormattedName(StringBuilder formattedName, uint pos, uint componentGroup) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getFormattedName__SWIG_0(swigCPtr, formattedName, pos, componentGroup), true);
  }

  public OFCondition getFormattedName(StringBuilder formattedName, uint pos) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getFormattedName__SWIG_1(swigCPtr, formattedName, pos), true);
  }

  public OFCondition getFormattedName(StringBuilder formattedName) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getFormattedName__SWIG_2(swigCPtr, formattedName), true);
  }

  public OFCondition putNameComponents(string lastName, string firstName, string middleName, string namePrefix, string nameSuffix) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_putNameComponents(swigCPtr, lastName, firstName, middleName, namePrefix, nameSuffix), true);
  }

  public static OFCondition getNameComponentsFromString(string dicomName, StringBuilder lastName, StringBuilder firstName, StringBuilder middleName, StringBuilder namePrefix, StringBuilder nameSuffix, uint componentGroup) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getNameComponentsFromString__SWIG_0(dicomName, lastName, firstName, middleName, namePrefix, nameSuffix, componentGroup), true);
  }

  public static OFCondition getNameComponentsFromString(string dicomName, StringBuilder lastName, StringBuilder firstName, StringBuilder middleName, StringBuilder namePrefix, StringBuilder nameSuffix) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getNameComponentsFromString__SWIG_1(dicomName, lastName, firstName, middleName, namePrefix, nameSuffix), true);
  }

  public static OFCondition getFormattedNameFromString(string dicomName, StringBuilder formattedName, uint componentGroup) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getFormattedNameFromString__SWIG_0(dicomName, formattedName, componentGroup), true);
  }

  public static OFCondition getFormattedNameFromString(string dicomName, StringBuilder formattedName) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getFormattedNameFromString__SWIG_1(dicomName, formattedName), true);
  }

  public static OFCondition getFormattedNameFromComponents(string lastName, string firstName, string middleName, string namePrefix, string nameSuffix, StringBuilder formattedName) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getFormattedNameFromComponents(lastName, firstName, middleName, namePrefix, nameSuffix, formattedName), true);
  }

  public static OFCondition getStringFromNameComponents(string lastName, string firstName, string middleName, string namePrefix, string nameSuffix, StringBuilder dicomName) {
    return new OFCondition(DCMTKPINVOKE.DcmPersonName_getStringFromNameComponents(lastName, firstName, middleName, namePrefix, nameSuffix, dicomName), true);
  }

}

}
