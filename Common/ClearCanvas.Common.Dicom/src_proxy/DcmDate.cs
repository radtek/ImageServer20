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

public class DcmDate : DcmByteString {
  private HandleRef swigCPtr;

  internal DcmDate(IntPtr cPtr, bool cMemoryOwn) : base(DCMTKPINVOKE.DcmDateUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(DcmDate obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~DcmDate() {
    Dispose();
  }

  public override void Dispose() {
    if(swigCPtr.Handle != IntPtr.Zero && swigCMemOwn) {
      swigCMemOwn = false;
      DCMTKPINVOKE.delete_DcmDate(swigCPtr);
    }
    swigCPtr = new HandleRef(null, IntPtr.Zero);
    GC.SuppressFinalize(this);
    base.Dispose();
  }

  public DcmDate(DcmTag tag, uint len) : this(DCMTKPINVOKE.new_DcmDate__SWIG_0(DcmTag.getCPtr(tag), len), true) {
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
  }

  public DcmDate(DcmTag tag) : this(DCMTKPINVOKE.new_DcmDate__SWIG_1(DcmTag.getCPtr(tag)), true) {
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
  }

  public DcmDate(DcmDate old) : this(DCMTKPINVOKE.new_DcmDate__SWIG_2(DcmDate.getCPtr(old)), true) {
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
  }

  public override DcmEVR ident() {
    DcmEVR ret = (DcmEVR)DCMTKPINVOKE.DcmDate_ident(swigCPtr);
    return ret;
  }

  public override OFCondition getOFString(StringBuilder stringVal, uint pos, bool normalize) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getOFString__SWIG_0(swigCPtr, stringVal, pos, normalize), true);
    return ret;
  }

  public override OFCondition getOFString(StringBuilder stringVal, uint pos) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getOFString__SWIG_1(swigCPtr, stringVal, pos), true);
    return ret;
  }

  public OFCondition setCurrentDate() {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_setCurrentDate(swigCPtr), true);
    return ret;
  }

  public OFCondition setOFDate(OFDate dateValue) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_setOFDate(swigCPtr, OFDate.getCPtr(dateValue)), true);
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public OFCondition getOFDate(OFDate dateValue, uint pos, bool supportOldFormat) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getOFDate__SWIG_0(swigCPtr, OFDate.getCPtr(dateValue), pos, supportOldFormat), true);
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public OFCondition getOFDate(OFDate dateValue, uint pos) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getOFDate__SWIG_1(swigCPtr, OFDate.getCPtr(dateValue), pos), true);
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public OFCondition getOFDate(OFDate dateValue) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getOFDate__SWIG_2(swigCPtr, OFDate.getCPtr(dateValue)), true);
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public OFCondition getISOFormattedDate(StringBuilder formattedDate, uint pos, bool supportOldFormat) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getISOFormattedDate__SWIG_0(swigCPtr, formattedDate, pos, supportOldFormat), true);
    return ret;
  }

  public OFCondition getISOFormattedDate(StringBuilder formattedDate, uint pos) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getISOFormattedDate__SWIG_1(swigCPtr, formattedDate, pos), true);
    return ret;
  }

  public OFCondition getISOFormattedDate(StringBuilder formattedDate) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getISOFormattedDate__SWIG_2(swigCPtr, formattedDate), true);
    return ret;
  }

  public static OFCondition getCurrentDate(StringBuilder dicomDate) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getCurrentDate(dicomDate), true);
    return ret;
  }

  public static OFCondition getDicomDateFromOFDate(OFDate dateValue, StringBuilder dicomDate) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getDicomDateFromOFDate(OFDate.getCPtr(dateValue), dicomDate), true);
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static OFCondition getOFDateFromString(string dicomDate, OFDate dateValue, bool supportOldFormat) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getOFDateFromString__SWIG_0(dicomDate, OFDate.getCPtr(dateValue), supportOldFormat), true);
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static OFCondition getOFDateFromString(string dicomDate, OFDate dateValue) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getOFDateFromString__SWIG_1(dicomDate, OFDate.getCPtr(dateValue)), true);
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static OFCondition getISOFormattedDateFromString(string dicomDate, StringBuilder formattedDate, bool supportOldFormat) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getISOFormattedDateFromString__SWIG_0(dicomDate, formattedDate, supportOldFormat), true);
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public static OFCondition getISOFormattedDateFromString(string dicomDate, StringBuilder formattedDate) {
    OFCondition ret = new OFCondition(DCMTKPINVOKE.DcmDate_getISOFormattedDateFromString__SWIG_1(dicomDate, formattedDate), true);
    if (DCMTKPINVOKE.SWIGPendingException.Pending) throw DCMTKPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
