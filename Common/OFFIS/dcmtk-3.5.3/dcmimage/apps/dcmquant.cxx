/*
 *
 *  Copyright (C) 2001-2003, OFFIS
 *
 *  This software and supporting documentation were developed by
 *
 *    Kuratorium OFFIS e.V.
 *    Healthcare Information and Communication Systems
 *    Escherweg 2
 *    D-26121 Oldenburg, Germany
 *
 *  THIS SOFTWARE IS MADE AVAILABLE,  AS IS,  AND OFFIS MAKES NO  WARRANTY
 *  REGARDING  THE  SOFTWARE,  ITS  PERFORMANCE,  ITS  MERCHANTABILITY  OR
 *  FITNESS FOR ANY PARTICULAR USE, FREEDOM FROM ANY COMPUTER DISEASES  OR
 *  ITS CONFORMITY TO ANY SPECIFICATION. THE ENTIRE RISK AS TO QUALITY AND
 *  PERFORMANCE OF THE SOFTWARE IS WITH THE USER.
 *
 *  Module:  dcmimage
 *
 *  Authors: Marco Eichelberg
 *
 *  Purpose: Convert DICOM color images palette color
 *
 *  Last Update:      $Author: joergr $
 *  Update Date:      $Date: 2003/12/05 10:50:52 $
 *  CVS/RCS Revision: $Revision: 1.10 $
 *  Status:           $State: Exp $
 *
 *  CVS/RCS Log at end of file
 *
 */


#include "osconfig.h"    /* make sure OS specific configuration is included first */

#define INCLUDE_CSTDIO
#define INCLUDE_CSTRING
#include "ofstdinc.h"

#ifdef HAVE_GUSI_H
#include <GUSI.h>
#endif

#include "dctk.h"          /* for various dcmdata headers */
#include "dcdebug.h"       /* for SetDebugLevel */
#include "cmdlnarg.h"      /* for prepareCmdLineArgs */
#include "dcuid.h"         /* for dcmtk version name */
#include "dcmimage.h"      /* for DicomImage */

#include "ofconapp.h"      /* for OFConsoleApplication */
#include "ofcmdln.h"       /* for OFCommandLine */

#include "diregist.h"      /* include to support color images */
#include "diquant.h"       /* for DcmQuant */

#ifdef BUILD_WITH_DCMJPEG_SUPPORT
#include "djdecode.h"      /* for dcmjpeg decoders */
#include "dipijpeg.h"      /* for dcmimage JPEG plugin */
#endif

#ifdef WITH_ZLIB
#include <zlib.h>          /* for zlibVersion() */
#endif

#define OFFIS_CONSOLE_DESCRIPTION "Convert DICOM color images to palette color"

#ifdef BUILD_WITH_DCMJPEG_SUPPORT
#define OFFIS_CONSOLE_APPLICATION "dcmjquan"
#else
#define OFFIS_CONSOLE_APPLICATION "dcmquant"
#endif

static char rcsid[] = "$dcmtk: " OFFIS_CONSOLE_APPLICATION " v"
  OFFIS_DCMTK_VERSION " " OFFIS_DCMTK_RELEASEDATE " $";

#define SHORTCOL 3
#define LONGCOL 21


// ********************************************

int main(int argc, char *argv[])
{
    OFConsoleApplication app(OFFIS_CONSOLE_APPLICATION, OFFIS_CONSOLE_DESCRIPTION, rcsid);
    OFCommandLine cmd;

    OFBool opt_verbose = OFFalse;
    OFBool opt_iDataset = OFFalse;
    OFBool opt_oDataset = OFFalse;
    E_TransferSyntax opt_ixfer = EXS_Unknown;
    E_TransferSyntax opt_oxfer = EXS_Unknown;
    E_GrpLenEncoding opt_oglenc = EGL_recalcGL;
    E_EncodingType opt_oenctype = EET_ExplicitLength;
    E_PaddingEncoding opt_opadenc = EPD_noChange;
    OFCmdUnsignedInt opt_filepad = 0;
    OFCmdUnsignedInt opt_itempad = 0;

    unsigned long       opt_compatibilityMode = CIF_MayDetachPixelData;
                                                          /* default: pixel data may detached if no longer needed */
    OFCmdUnsignedInt    opt_frame = 1;                    /* default: first frame */
    OFCmdUnsignedInt    opt_frameCount = 0;               /* default: all frames */

    OFBool              opt_palette_ow = OFFalse;
    OFBool              opt_entries_word = OFFalse;
    OFBool              opt_palette_fs = OFFalse;
    OFCmdUnsignedInt    opt_palette_col = 256;

    DcmLargestDimensionType opt_largeType = DcmLargestDimensionType_default;
    DcmRepresentativeColorType opt_repType = DcmRepresentativeColorType_default;

    OFBool              opt_secondarycapture = OFFalse;
    OFBool              opt_uidcreation = OFTrue;


#ifdef BUILD_WITH_DCMJPEG_SUPPORT
    // JPEG parameters
    OFCmdUnsignedInt    opt_quality = 90;                 /* default: 90% JPEG quality */
    E_SubSampling       opt_sampling = ESS_422;           /* default: 4:2:2 sub-sampling */
    E_DecompressionColorSpaceConversion opt_decompCSconversion = EDC_photometricInterpretation;
#endif

    const char *        opt_ifname = NULL;
    const char *        opt_ofname = NULL;

    SetDebugLevel((0));
    DicomImageClass::setDebugLevel(DicomImageClass::DL_Warnings | DicomImageClass::DL_Errors);

    prepareCmdLineArgs(argc, argv, OFFIS_CONSOLE_APPLICATION);
    cmd.setOptionColumns(LONGCOL, SHORTCOL);

    cmd.addParam("dcmfile-in",  "DICOM input filename to be converted");
    cmd.addParam("dcmfile-out", "DICOM output filename to be written");

    cmd.addGroup("general options:", LONGCOL, SHORTCOL + 2);
     cmd.addOption("--help",                "-h",      "print this help text and exit" /*, OFTrue is set implicitly */);
     cmd.addOption("--version",                        "print version information and exit", OFTrue /* exclusive */);
     cmd.addOption("--verbose",             "-v",      "verbose mode, print processing details");

    cmd.addGroup("input options:");

     cmd.addSubGroup("input file format:");
      cmd.addOption("--read-file",          "+f",      "read file format or data set (default)");
      cmd.addOption("--read-dataset",       "-f",      "read data set without file meta information");

     cmd.addSubGroup("input transfer syntax (only with --read-dataset):");
      cmd.addOption("--read-xfer-auto",     "-t=",     "use TS recognition (default)");
      cmd.addOption("--read-xfer-little",   "-te",     "read with explicit VR little endian TS");
      cmd.addOption("--read-xfer-big",      "-tb",     "read with explicit VR big endian TS");
      cmd.addOption("--read-xfer-implicit", "-ti",     "read with implicit VR little endian TS");

    cmd.addGroup("image processing and encoding options:");
     cmd.addSubGroup("frame selection:");
      cmd.addOption("--frame",              "+fr",  1, "[n]umber : integer",
                                                       "select specified frame");
      cmd.addOption("--all-frames",         "+fa",     "select all frames (default)");

#ifdef BUILD_WITH_DCMJPEG_SUPPORT
     cmd.addSubGroup("color space conversion options (compressed images only):");
      cmd.addOption("--conv-photometric",   "+cp",     "convert if YCbCr photom. interpr. (default)");
      cmd.addOption("--conv-lossy",         "+cl",     "convert YCbCr to RGB if lossy JPEG");
      cmd.addOption("--conv-always",        "+ca",     "always convert YCbCr to RGB");
      cmd.addOption("--conv-never",         "+cn",     "never convert color space");
#endif

     cmd.addSubGroup("compatibility options:");
      cmd.addOption("--accept-palettes",    "+Mp",     "accept incorrect palette attribute tags\n(0028,111x) and (0028,121x)");

     cmd.addSubGroup("median cut dimension selection options:");
      cmd.addOption("--mc-dimension-rgb",   "+Dr",     "max dimension from RGB range (default)");
      cmd.addOption("--mc-dimension-lum",   "+Dl",     "max dimension from luminance");

     cmd.addSubGroup("median cut representative color selection options:");
      cmd.addOption("--mc-color-avgbox",   "+Cb",     "average colors in box (default)");
      cmd.addOption("--mc-color-avgpixel", "+Cp",     "average pixels in box");
      cmd.addOption("--mc-color-center",   "+Cc",     "select center of box");

     cmd.addSubGroup("color palette creation options:");
      cmd.addOption("--write-ow",           "+pw",     "write Palette LUT as OW instead of US");
      cmd.addOption("--lut-entries-word",   "+pe",     "write Palette LUT with 16-bit entries");
      cmd.addOption("--floyd-steinberg",    "+pf",     "use Floyd-Steinberg error diffusion");
      cmd.addOption("--colors",             "+pc",  1, "number of colors: 2..65536 (default 256)",
                                                       "number of colors to quantize to");

    cmd.addSubGroup("SOP Class UID options:");
     cmd.addOption("--class-default",      "+cd",     "keep SOP Class UID (default)");
     cmd.addOption("--class-sc",           "+cs",     "convert to Secondary Capture Image\n(implies --uid-always)");

    cmd.addSubGroup("SOP Instance UID options:");
     cmd.addOption("--uid-always",         "+ua",     "always assign new UID (default)");
     cmd.addOption("--uid-never",          "+un",     "never assign new UID");

  cmd.addGroup("output options:");
    cmd.addSubGroup("output file format:");
      cmd.addOption("--write-file",             "+F",        "write file format (default)");
      cmd.addOption("--write-dataset",          "-F",        "write data set without file meta information");
    cmd.addSubGroup("output transfer syntax:");
      cmd.addOption("--write-xfer-same",        "+t=",       "write with same TS as input (default)");
      cmd.addOption("--write-xfer-little",      "+te",       "write with explicit VR little endian TS");
      cmd.addOption("--write-xfer-big",         "+tb",       "write with explicit VR big endian TS");
      cmd.addOption("--write-xfer-implicit",    "+ti",       "write with implicit VR little endian TS");
    cmd.addSubGroup("post-1993 value representations:");
      cmd.addOption("--enable-new-vr",          "+u",        "enable support for new VRs (UN/UT) (default)");
      cmd.addOption("--disable-new-vr",         "-u",        "disable support for new VRs, convert to OB");
    cmd.addSubGroup("group length encoding:");
      cmd.addOption("--group-length-recalc",    "+g=",       "recalculate group lengths if present (default)");
      cmd.addOption("--group-length-create",    "+g",        "always write with group length elements");
      cmd.addOption("--group-length-remove",    "-g",        "always write without group length elements");
    cmd.addSubGroup("length encoding in sequences and items:");
      cmd.addOption("--length-explicit",        "+e",        "write with explicit lengths (default)");
      cmd.addOption("--length-undefined",       "-e",        "write with undefined lengths");
    cmd.addSubGroup("data set trailing padding (not with --write-dataset):");
      cmd.addOption("--padding-retain",         "-p=",       "do not change padding\n(default if not --write-dataset)");
      cmd.addOption("--padding-off",            "-p",        "no padding (implicit if --write-dataset)");
      cmd.addOption("--padding-create",         "+p",    2,  "[f]ile-pad [i]tem-pad: integer",
                                                             "align file on multiple of f bytes\nand items on multiple of i bytes");

    if (app.parseCommandLine(cmd, argc, argv))
    {
      /* check exclusive options first */

      if (cmd.getParamCount() == 0)
      {
          if (cmd.findOption("--version"))
          {
              app.printHeader(OFTrue /*print host identifier*/);          // uses ofConsole.lockCerr()
              CERR << endl << "External libraries used:";
#if !defined(WITH_ZLIB) && !defined(BUILD_WITH_DCMJPEG_SUPPORT)
              CERR << " none" << endl;
#else
              CERR << endl;
#endif
#ifdef WITH_ZLIB
              CERR << "- ZLIB, Version " << zlibVersion() << endl;
#endif
#ifdef BUILD_WITH_DCMJPEG_SUPPORT
              CERR << "- " << DiJPEGPlugin::getLibraryVersionString() << endl;
#endif
              return 0;
          }
      }

      /* command line parameters */

      cmd.getParam(1, opt_ifname);
      cmd.getParam(2, opt_ofname);

      if (cmd.findOption("--verbose")) opt_verbose = OFTrue;

      cmd.beginOptionBlock();
      if (cmd.findOption("--read-file")) opt_iDataset = OFFalse;
      if (cmd.findOption("--read-dataset")) opt_iDataset = OFTrue;
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--read-xfer-auto"))
      {
          if (! opt_iDataset) app.printError("--read-xfer-auto only allowed with --read-dataset");
          opt_ixfer = EXS_Unknown;
      }
      if (cmd.findOption("--read-xfer-little"))
      {
          if (! opt_iDataset) app.printError("--read-xfer-little only allowed with --read-dataset");
          opt_ixfer = EXS_LittleEndianExplicit;
      }
      if (cmd.findOption("--read-xfer-big"))
      {
          if (! opt_iDataset) app.printError("--read-xfer-big only allowed with --read-dataset");
          opt_ixfer = EXS_BigEndianExplicit;
      }
      if (cmd.findOption("--read-xfer-implicit"))
      {
          if (! opt_iDataset) app.printError("--read-xfer-implicit only allowed with --read-dataset");
          opt_ixfer = EXS_LittleEndianImplicit;
      }
      cmd.endOptionBlock();

      if (cmd.findOption("--accept-palettes"))
          opt_compatibilityMode |= CIF_WrongPaletteAttributeTags;

      cmd.beginOptionBlock();
      if (cmd.findOption("--frame"))
      {
          app.checkValue(cmd.getValueAndCheckMin(opt_frame, 1));
          opt_frameCount = 1;
      }
      if (cmd.findOption("--all-frames"))
      {
          opt_frame = 1;
          opt_frameCount = 0;
      }
      cmd.endOptionBlock();

      if (cmd.findOption("--write-ow")) opt_palette_ow = OFTrue;
      if (cmd.findOption("--lut-entries-word")) opt_entries_word = OFTrue;
      if (cmd.findOption("--floyd-steinberg")) opt_palette_fs = OFTrue;
      if (cmd.findOption("--colors")) cmd.getValueAndCheckMinMax(opt_palette_col, 2, 65536);

      cmd.beginOptionBlock();
      if (cmd.findOption("--mc-dimension-rgb")) opt_largeType = DcmLargestDimensionType_default;
      if (cmd.findOption("--mc-dimension-lum")) opt_largeType = DcmLargestDimensionType_luminance;
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--mc-color-avgbox")) opt_repType = DcmRepresentativeColorType_default;
      if (cmd.findOption("--mc-color-avgpixel")) opt_repType = DcmRepresentativeColorType_averagePixels;
      if (cmd.findOption("--mc-color-center")) opt_repType = DcmRepresentativeColorType_centerOfBox;
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--class-default")) opt_secondarycapture = OFFalse;
      if (cmd.findOption("--class-sc")) opt_secondarycapture = OFTrue;
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--uid-always")) opt_uidcreation = OFTrue;
      if (cmd.findOption("--uid-never")) opt_uidcreation = OFFalse;
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--write-file")) opt_oDataset = OFFalse;
      if (cmd.findOption("--write-dataset")) opt_oDataset = OFTrue;
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--write-xfer-same")) opt_oxfer = EXS_Unknown;
      if (cmd.findOption("--write-xfer-little")) opt_oxfer = EXS_LittleEndianExplicit;
      if (cmd.findOption("--write-xfer-big")) opt_oxfer = EXS_BigEndianExplicit;
      if (cmd.findOption("--write-xfer-implicit")) opt_oxfer = EXS_LittleEndianImplicit;
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--enable-new-vr"))
      {
          dcmEnableUnknownVRGeneration.set(OFTrue);
          dcmEnableUnlimitedTextVRGeneration.set(OFTrue);
      }
      if (cmd.findOption("--disable-new-vr"))
      {
          dcmEnableUnknownVRGeneration.set(OFFalse);
          dcmEnableUnlimitedTextVRGeneration.set(OFFalse);
      }
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--group-length-recalc")) opt_oglenc = EGL_recalcGL;
      if (cmd.findOption("--group-length-create")) opt_oglenc = EGL_withGL;
      if (cmd.findOption("--group-length-remove")) opt_oglenc = EGL_withoutGL;
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--length-explicit")) opt_oenctype = EET_ExplicitLength;
      if (cmd.findOption("--length-undefined")) opt_oenctype = EET_UndefinedLength;
      cmd.endOptionBlock();

      cmd.beginOptionBlock();
      if (cmd.findOption("--padding-retain"))
      {
          if (opt_oDataset) app.printError("--padding-retain not allowed with --write-dataset");
          opt_opadenc = EPD_noChange;
      }
      if (cmd.findOption("--padding-off")) opt_opadenc = EPD_withoutPadding;
      if (cmd.findOption("--padding-create"))
      {
          if (opt_oDataset) app.printError("--padding-create not allowed with --write-dataset");
          app.checkValue(cmd.getValueAndCheckMin(opt_filepad, 0));
          app.checkValue(cmd.getValueAndCheckMin(opt_itempad, 0));
          opt_opadenc = EPD_withPadding;
      }
      cmd.endOptionBlock();
    }

    /* make sure data dictionary is loaded */
    if (!dcmDataDict.isDictionaryLoaded())
    {
        CERR << "Warning: no data dictionary loaded, "
             << "check environment variable: "
             << DCM_DICT_ENVIRONMENT_VARIABLE << endl;
    }

#ifdef BUILD_WITH_DCMJPEG_SUPPORT
    // register global decompression codecs
    DJDecoderRegistration::registerCodecs(opt_decompCSconversion, EUC_default, EPC_default, OFFalse);
#endif

    // ======================================================================
    // read input file

    if ((opt_ifname == NULL) || (strlen(opt_ifname) == 0))
    {
        CERR << "Error: invalid filename: <empty string>" << endl;
        return 1;
    }

    DcmFileFormat fileformat;
    DcmDataset *dataset = fileformat.getDataset();

    OFCondition error = fileformat.loadFile(opt_ifname, opt_ixfer, EGL_noChange, DCM_MaxReadLength, opt_iDataset);
    if (error.bad())
    {
        CERR << "Error: " << error.text()
             << ": reading file: " <<  opt_ifname << endl;
        return 1;
    }

    if (opt_verbose)
        COUT << "load all data into memory" << endl;

    /* make sure that pixel data is loaded before output file is created */
    dataset->loadAllDataIntoMemory();

    // select uncompressed output transfer syntax.
    // this will implicitly decompress the image if necessary.

    if (opt_oxfer == EXS_Unknown)
    {
        if (opt_verbose)
            COUT << "set output transfer syntax to input transfer syntax" << endl;
        opt_oxfer = dataset->getOriginalXfer();
    }

    if (opt_verbose)
        COUT << "check if new output transfer syntax is possible" << endl;

    DcmXfer opt_oxferSyn(opt_oxfer);
    dataset->chooseRepresentation(opt_oxfer, NULL);

    if (dataset->canWriteXfer(opt_oxfer))
    {
        if (opt_verbose)
            COUT << "output transfer syntax " << opt_oxferSyn.getXferName()
                 << " can be written" << endl;
    } else {
        CERR << "Error: no conversion to transfer syntax " << opt_oxferSyn.getXferName()
             << " possible!" << endl;
        return 1;
    }

    // ======================================================================
    // image processing starts here

    if (opt_verbose)
        CERR << "preparing pixel data." << endl;

    // create DicomImage object
    DicomImage di(dataset, opt_oxfer, opt_compatibilityMode, opt_frame - 1, opt_frameCount);
    if (di.getStatus() != EIS_Normal) app.printError(DicomImage::getString(di.getStatus()));

    if (di.isMonochrome())
    {
      app.printError("cannot convert monochrome image to palette color");
    }

    OFString derivationDescription;

    // create palette color image
    error = DcmQuant::createPaletteColorImage(
      di, *dataset, opt_palette_ow, opt_entries_word, opt_palette_fs, opt_palette_col,
      derivationDescription, opt_verbose, opt_largeType, opt_repType);

    // update image type
    if (error.good()) error = DcmQuant::updateImageType(dataset);

    // update derivation description
    if (error.good()) error = DcmQuant::updateDerivationDescription(dataset, derivationDescription.c_str());

    // create new SOP instance UID
    if (error.good() && (opt_secondarycapture || opt_uidcreation)) error = DcmQuant::newInstance(dataset);

    // convert to Secondary Capture if requested by user.
    // This method creates a new SOP class UID, so it should be executed
    // after the call to newInstance() which creates a Source Image Sequence.
    if (error.good() && opt_secondarycapture) error = DcmQuant::convertToSecondaryCapture(dataset);

    if (error.bad())
    {
        CERR << "Error: " << error.text()
             << ": converting image: " <<  opt_ifname << endl;
        return 1;
    }

    // ======================================================================
    // write back output file

    if (opt_verbose)
        COUT << "write converted DICOM file" << endl;

    error = fileformat.saveFile(opt_ofname, opt_oxfer, opt_oenctype, opt_oglenc, opt_opadenc,
        OFstatic_cast(Uint32, opt_filepad), OFstatic_cast(Uint32, opt_itempad), opt_oDataset);

    if (error.bad())
    {
        CERR << "Error: " << error.text()
             << ": writing file: " <<  opt_ofname << endl;
        return 1;
    }

    if (opt_verbose)
        COUT << "conversion successful" << endl;

#ifdef BUILD_WITH_DCMJPEG_SUPPORT
    // deregister global decompression codecs
    DJDecoderRegistration::cleanup();
#endif

    return 0;
}


/*
 * CVS/RCS Log:
 * $Log: dcmquant.cc,v $
 * Revision 1.10  2003/12/05 10:50:52  joergr
 * Adapted type casts to new-style typecast operators defined in ofcast.h.
 *
 * Revision 1.9  2003/05/20 09:27:22  joergr
 * Removed unused helper functions (dcutils.*).
 *
 * Revision 1.8  2003/04/25 13:15:54  joergr
 * Fixed inconsistency regarding the default option for frame selection.
 *
 * Revision 1.7  2002/11/27 14:16:53  meichel
 * Adapted module dcmimage to use of new header file ofstdinc.h
 *
 * Revision 1.6  2002/11/26 08:44:56  meichel
 * Replaced all includes for "zlib.h" with <zlib.h>
 *   to avoid inclusion of zlib.h in the makefile dependencies.
 *
 * Revision 1.5  2002/09/23 18:01:19  joergr
 * Added new command line option "--version" which prints the name and version
 * number of external libraries used (incl. preparation for future support of
 * 'config.guess' host identifiers).
 *
 * Revision 1.4  2002/08/27 17:19:07  meichel
 * Added options --frame and --all-frames
 *
 * Revision 1.3  2002/08/20 12:20:21  meichel
 * Adapted code to new loadFile and saveFile methods, thus removing direct
 *   use of the DICOM stream classes.
 *
 * Revision 1.2  2002/01/25 17:50:34  joergr
 * Corrected wrong table spacing in the syntax output of the dcmquant tool.
 *
 * Revision 1.1  2002/01/25 13:32:01  meichel
 * Initial release of new color quantization classes and
 *   the dcmquant tool in module dcmimage.
 *
 *
 */
