using System;
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.ImageServer.Dicom
{
    /// <summary>
    /// Default Specific Character Set parser for the DICOM library
    /// </summary>
    public class SpecificCharacterSetParser : IDicomCharacterSetParser
    {
        public SpecificCharacterSetParser()
        {
        }

        #region IDicomCharacterSetParser Members

        public byte[] Encode(string dataInUnicode, string specificCharacterSet)
        {
            byte[] rawBytes;

            if (null == specificCharacterSet || "" == specificCharacterSet)
            {
                rawBytes = Encoding.GetEncoding("Windows-1252").GetBytes(dataInUnicode);
                return rawBytes;
            }

            CharacterSetInfo defaultRepertoire;
            Dictionary<string, CharacterSetInfo> extensionRepertoires;
            GetRepertoires(specificCharacterSet, out defaultRepertoire, out extensionRepertoires);

            // TODO: here's where the hack starts
            // pick the first one and use that for decoding
            foreach (CharacterSetInfo info in extensionRepertoires.Values)
            {
                Encode(dataInUnicode, info, out rawBytes);
                return rawBytes;
            }

            // if nothing happened with extension repertoires, use default repertoire
            if (null != defaultRepertoire)
            {
                Encode(dataInUnicode, defaultRepertoire, out rawBytes);
                return rawBytes;
            }

            rawBytes = Encoding.GetEncoding("Windows-1252").GetBytes(dataInUnicode);
            return rawBytes;
        }

        public string Decode(byte[] rawData, string specificCharacterSet)
        {
            if (null == specificCharacterSet || "" == specificCharacterSet)
            {
                // this takes the raw bytes, and converts it into a Unicode string
                // represention of the original raw bytes
                return System.Text.Encoding.GetEncoding("Windows-1252").GetString(rawData);
            }

            CharacterSetInfo defaultRepertoire;
            Dictionary<string, CharacterSetInfo> extensionRepertoires;
            GetRepertoires(specificCharacterSet, out defaultRepertoire, out extensionRepertoires);

            // TODO: here's where the hack starts
            // pick the first one and use that for decoding
            foreach (CharacterSetInfo info in extensionRepertoires.Values)
            {
                return Decode(rawData, info);
            }

            // if nothing happened with extension repertoires, use default repertoire
            if (null != defaultRepertoire)
            {
                return Decode(rawData, defaultRepertoire);
            }
            // this takes the raw bytes, and converts it into a Unicode string
            // represention of the original raw bytes
            return System.Text.Encoding.GetEncoding("Windows-1252").GetString(rawData);
        }

        #endregion

        #region Static Methods
        static SpecificCharacterSetParser()
        {
            _characterSetInfo = new Dictionary<string, CharacterSetInfo>();
            _characterSetInfo.Add("ISO_IR 100", new CharacterSetInfo("ISO_IR 100", 28591, "", "", "Latin Alphabet No. 1 Unextended"));
            _characterSetInfo.Add("ISO_IR 101", new CharacterSetInfo("ISO_IR 101", 28592, "", "", "Latin Alphabet No. 2 Unextended"));
            _characterSetInfo.Add("ISO_IR 109", new CharacterSetInfo("ISO_IR 109", 28593, "", "", "Latin Alphabet No. 3 Unextended"));
            _characterSetInfo.Add("ISO_IR 110", new CharacterSetInfo("ISO_IR 110", 28594, "", "", "Latin Alphabet No. 4 Unextended"));
            _characterSetInfo.Add("ISO_IR 144", new CharacterSetInfo("ISO_IR 144", 28595, "", "", "Cyrillic Unextended"));
            _characterSetInfo.Add("ISO_IR 127", new CharacterSetInfo("ISO_IR 127", 28596, "", "", "Arabic Unextended"));
            _characterSetInfo.Add("ISO_IR 126", new CharacterSetInfo("ISO_IR 126", 28597, "", "", "Greek Unextended"));
            _characterSetInfo.Add("ISO_IR 138", new CharacterSetInfo("ISO_IR 138", 28598, "", "", "Hebrew Unextended"));
            _characterSetInfo.Add("ISO_IR 148", new CharacterSetInfo("ISO_IR 148", 28599, "", "", "Latin Alphabet No. 5 (Turkish) Unextended"));
            _characterSetInfo.Add("ISO_IR 13", new CharacterSetInfo("ISO_IR 13", 932, "", "", "JIS X 0201 (Shift JIS) Unextended"));
            _characterSetInfo.Add("ISO_IR 166", new CharacterSetInfo("ISO_IR 166", 874, "", "", "TIS 620-2533 (Thai) Unextended"));
            _characterSetInfo.Add("ISO_IR 192", new CharacterSetInfo("ISO_IR 192", 65001, "", "", "Unicode in UTF-8"));
            _characterSetInfo.Add("ISO 2022 IR 6", new CharacterSetInfo("ISO 2022 IR 6", 28591, "\x1b\x28\x42", "", "Default"));
            _characterSetInfo.Add("ISO 2022 IR 100", new CharacterSetInfo("ISO 2022 IR 100", 28591, "\x1b\x28\x42", "\x1b\x2d\x41", "Latin Alphabet No. 1 Extended"));
            _characterSetInfo.Add("ISO 2022 IR 101", new CharacterSetInfo("ISO 2022 IR 101", 28592, "\x1b\x28\x42", "\x1b\x2d\x42", "Latin Alphabet No. 2 Extended"));
            _characterSetInfo.Add("ISO 2022 IR 109", new CharacterSetInfo("ISO 2022 IR 109", 28593, "\x1b\x28\x42", "\x1b\x2d\x43", "Latin Alphabet No. 3 Extended"));
            _characterSetInfo.Add("ISO 2022 IR 110", new CharacterSetInfo("ISO 2022 IR 110", 28594, "\x1b\x28\x42", "\x1b\x2d\x44", "Latin Alphabet No. 4 Extended"));
            _characterSetInfo.Add("ISO 2022 IR 144", new CharacterSetInfo("ISO 2022 IR 144", 28595, "\x1b\x28\x42", "\x1b\x2d\x4c", "Cyrillic Extended"));
            _characterSetInfo.Add("ISO 2022 IR 127", new CharacterSetInfo("ISO 2022 IR 127", 28596, "\x1b\x28\x42", "\x1b\x2d\x47", "Arabic Extended"));
            _characterSetInfo.Add("ISO 2022 IR 126", new CharacterSetInfo("ISO 2022 IR 126", 28597, "\x1b\x28\x42", "\x1b\x2d\x46", "Greek Extended"));
            _characterSetInfo.Add("ISO 2022 IR 138", new CharacterSetInfo("ISO 2022 IR 138", 28598, "\x1b\x28\x42", "\x1b\x2d\x48", "Hebrew Extended"));
            _characterSetInfo.Add("ISO 2022 IR 148", new CharacterSetInfo("ISO 2022 IR 148", 28599, "\x1b\x28\x42", "\x1b\x2d\x4d", "Latin Alphabet No. 5 (Turkish) Extended"));
            _characterSetInfo.Add("ISO 2022 IR 13", new CharacterSetInfo("ISO 2022 IR 13", 50222, "\x1b\x28\x4a", "\x1b\x29\x49", "JIS X 0201 (Shift JIS) Extended"));
            _characterSetInfo.Add("ISO 2022 IR 166", new CharacterSetInfo("ISO 2022 IR 166", 874, "\x1b\x28\x42", "\x1b\x2d\x54", "TIS 620-2533 (Thai) Extended"));
            _characterSetInfo.Add("ISO 2022 IR 87", new CharacterSetInfo("ISO 2022 IR 87", 50222, "\x1b\x24\x42", "", "JIS X 0208 (Kanji) Extended"));
            _characterSetInfo.Add("ISO 2022 IR 159", new CharacterSetInfo("ISO 2022 IR 159", 50222, "\x1b\x24\x28\x44", "", "JIS X 0212 (Kanji) Extended"));
            _characterSetInfo.Add("ISO 2022 IR 149", new CharacterSetInfo("ISO 2022 IR 149", 20949, "", "\x1b\x24\x29\x43", "KS X 1001 (Hangul and Hanja) Extended"));
            _characterSetInfo.Add("GB18030", new CharacterSetInfo("GB18030", 54936, "", "", "Chinese (Simplified) Extended"));
        }


        public static Encoding GetEncoding(string specificCharacterSet)
        {
            CharacterSetInfo defaultRepertoire;
            Dictionary<string, CharacterSetInfo> extensionRepertoires;
            GetRepertoires(specificCharacterSet, out defaultRepertoire, out extensionRepertoires);

            return Encoding.GetEncoding(defaultRepertoire.MicrosoftCodePage);
        }

        private static void GetRepertoires(string specificCharacterSet, out CharacterSetInfo defaultRepertoire, out Dictionary<string, CharacterSetInfo> extensionRepertoires)
        {
            // TODO:
            // Specific Character Set may have up to n values if 
            // Code Extensions are used. We accomodate for that here
            // by parsing out all the different possible defined terms.
            // At this point, however, we're not going to handle escaping
            // between character sets from different code pages within
            // a single string. For example, DICOM implies that you should
            // be able to have JIS-encoded Japanese, ISO European characters,
            // Thai characters and Korean characters on the same line, using
            // Code Extensions (escape sequences). (Chinese is not included
            // since the only support for Chinese is through GB18030 and
            // UTF-8, both of which do not support Code Extensions.)
            string[] specificCharacterSetValues = specificCharacterSet.Split('\\');
            defaultRepertoire = null;

            // set the default repertoire from Value 1 
            if (specificCharacterSetValues.GetUpperBound(0) >= 0)
            {
                if (SpecificCharacterSetParser.CharacterSetDatabase.ContainsKey(specificCharacterSetValues[0]))
                    defaultRepertoire = SpecificCharacterSetParser.CharacterSetDatabase[specificCharacterSetValues[0]];
                else
                    // we put in the default repertoire. Technically, it may
                    // not be ISO 2022 IR 6, but ISO_IR 6, but the information
                    // we want to use is the same
                    defaultRepertoire = SpecificCharacterSetParser.CharacterSetDatabase["ISO 2022 IR 6"];
            }

            // Here we are accounting for cases where the same character sets are repeated, so
            // we need to select out the unique ones.  It should never really happen, but it 
            // does happen with a particular dataset when querying JDicom.
            List<string> uniqueExtensionRepertoireDefinedTerms = new List<string>();
            for (int i = 1; i < specificCharacterSetValues.Length; ++i)
            {
                string value = specificCharacterSetValues[i];
                if (value != defaultRepertoire.DefinedTerm && !uniqueExtensionRepertoireDefinedTerms.Contains(value))
                    uniqueExtensionRepertoireDefinedTerms.Add(value);
            }

            // parse out the extension repertoires
            extensionRepertoires = new Dictionary<string, CharacterSetInfo>();
            foreach (string value in uniqueExtensionRepertoireDefinedTerms)
            {
                if (SpecificCharacterSetParser.CharacterSetDatabase.ContainsKey(value) && !extensionRepertoires.ContainsKey(value))
                {
                    // special robustness handling of GB18030 and UTF-8
                    if ("GB18030" == value || "ISO_IR 192" == value)
                    {
                        // these two character sets can't use code extensions, so there should really only be 1
                        // character set in the repertoire
                        extensionRepertoires.Clear();
                        extensionRepertoires.Add(value, SpecificCharacterSetParser.CharacterSetDatabase[value]);
                        break;
                    }

                    extensionRepertoires.Add(value, SpecificCharacterSetParser.CharacterSetDatabase[value]);
                }
                else if (!extensionRepertoires.ContainsKey("ISO 2022 IR 6"))
                {
                    // we put in the default repertoire. Technically, it may
                    // not be ISO 2022 IR 6, but ISO_IR 6, but the information
                    // we want to use is the same
                    extensionRepertoires.Add(value, SpecificCharacterSetParser.CharacterSetDatabase["ISO 2022 IR 6"]);
                }
            }
        }

        private static string Encode(string unicodeData, CharacterSetInfo repertoire)
        {
            byte[] rawBytes;
            Encode(unicodeData, repertoire, out rawBytes);
            char[] rawCharacters = Encoding.GetEncoding("Windows-1252").GetChars(rawBytes);
            return new string(rawCharacters);
        }

        private static void Encode(string unicodeData, CharacterSetInfo repertoire, out byte[] encoded)
        {
            byte[] rawBytes = Encoding.GetEncoding(repertoire.MicrosoftCodePage).GetBytes(unicodeData);
            encoded = rawBytes;
        }

        private static string Decode(byte[] rawData, CharacterSetInfo repertoire)
        {
            Encoding rawEncoding = Encoding.GetEncoding(repertoire.MicrosoftCodePage);
            string rawDataDecoded = new string(rawEncoding.GetChars(rawData));

            // get rid of any escape sequences, if they appear in the decoded string,
            // like the case of Korean, using code page 20949 for some reason
            if ("" != repertoire.G1Sequence)
                return rawDataDecoded.Replace(repertoire.G1Sequence, "");
            else
                return rawDataDecoded;
        }

        private static string Decode(string rawData, CharacterSetInfo repertoire)
        {
            // get it back to byte array form using a character set that includes 
            // both GR and GL areas (characters up to \xff in binary value)
            // and it seems Windows-1252 works better than ISO-8859-1
            byte[] rawBytes = Encoding.GetEncoding("Windows-1252").GetBytes(rawData);
            return Decode(rawBytes, repertoire);
        }
        #endregion


        protected class CharacterSetInfo
        {
            public CharacterSetInfo(string definedTerm, int codePage, string g0Sequence, string g1Sequence, string description)
            {
                _definedTerm = definedTerm;
                _microsoftCodePage = codePage;
                _g0Sequence = g0Sequence;
                _g1Sequence = g1Sequence;
                _description = description;
            }


            #region Properties
            private string _definedTerm;
            private int _microsoftCodePage;
            private string _description;
            private string _g0Sequence;
            private string _g1Sequence;

            public string G1Sequence
            {
                get { return _g1Sequence; }
                set { _g1Sequence = value; }
            }

            public string G0Sequence
            {
                get { return _g0Sequence; }
                set { _g0Sequence = value; }
            }

            public string Description
            {
                get { return _description; }
                set { _description = value; }
            }


            public int MicrosoftCodePage
            {
                get { return _microsoftCodePage; }
                set { _microsoftCodePage = value; }
            }

            public string DefinedTerm
            {
                get { return _definedTerm; }
                set { _definedTerm = value; }
            }

            #endregion
        }

        #region Properties
        // private string _specificCharacterSet;
        //public string SpecificCharacterSet
        //{
        //    get { return _specificCharacterSet; }
        //    set { _specificCharacterSet = value; }
        //}	

        protected static Dictionary<string, CharacterSetInfo> CharacterSetDatabase
        {
            get { return SpecificCharacterSetParser._characterSetInfo; }
        }

        protected static string DefaultCharacterSet
        {
            get { return "ISO 2022 IR 6"; }    // this is the default    
        }

        #endregion

        #region Private fields
        private static Dictionary<string, CharacterSetInfo> _characterSetInfo;
        #endregion

  
    }
    

}
