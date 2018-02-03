using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base64Library
{
    public static class Base64Converter
    {
        private static char[] base64TableChar;
        private static byte[] base64TableBytes;
        private static StringBuilder encodeString;

        static Base64Converter()
        {
            base64TableChar = new char[] {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d',
            'e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
            '8','9','+','/' };

            base64TableBytes = new byte[256];

            for(int i = 0; i < base64TableChar.Length; i++)
            {
                base64TableBytes[base64TableChar[i]] = (byte)i;
            }

            encodeString = new StringBuilder();
        }

        #region Encode
        public static string Encode(byte[] data, Base64FormattingOptions option = Base64FormattingOptions.None)
        {
            encodeString.Clear();

            byte[] correctedData;
            int numberOfPadding;

            if(!IsCorrectData(data, out numberOfPadding))
            {
                correctedData = GenerateCorrectData(data, numberOfPadding);
            }
            else
            {
                correctedData = data;
            }

            byte[] block = new byte[3];

            for(int i = 0; i < correctedData.Length; i += 3)
            {
                for(int j = 0; j < 3; j++)
                {
                    block[j] = correctedData[i + j];
                }

                Encode24Bit(block);
            }

            for(int j = 0; j < numberOfPadding; j++)
            {
                encodeString[encodeString.Length - 1 - j] = '=';
            }

            if (option == Base64FormattingOptions.InsertLineBreaks)
            {
                AddNewLineInEncodeString(76);
            }

            return encodeString.ToString();
        }

        private static bool IsCorrectData(byte[] data, out int numberOfPadding)
        {
            if (data.Length % 3 == 0)
            {
                numberOfPadding = 0;
                return true;
            }
            else
            {
                numberOfPadding = 3 - (data.Length % 3);
                return false;
            }
        }

        private static byte[] GenerateCorrectData(byte[] data, int numberOfPadding)
        {
            byte[] correctedData = new byte[data.Length + numberOfPadding];
            Array.Copy(data, correctedData, data.Length);

            for (int i = 0; i < numberOfPadding; i++)
            {
                correctedData[data.Length + i] = 0;
            }

            return correctedData;
        }

        private static void Encode24Bit(byte[] block)
        {
            int block24 = 0;

            for (int i = 0; i < block.Length; i++)
            {
                block24 += block[i];
                if (i + 1 < block.Length)
                {
                    block24 = block24 << 8;
                }
            }

            char[] blockChar = new char[4];

            for (int j = 0; j < 4; j++)
            {
                byte rightChar = (byte)(block24 & 0x3F);
                block24 = block24 >> 6;
                blockChar[j] = base64TableChar[rightChar];
            }

            Array.Reverse(blockChar);

            encodeString.Append(blockChar);
        }

        private static void AddNewLineInEncodeString(int sizeLine)
        {
            if ((encodeString.Length / sizeLine) != 0)
            {
                string temp = encodeString.ToString();
                encodeString.Replace(temp,
                    string.Join(Environment.NewLine, Enumerable.Range(0, (temp.Length + sizeLine - 1) / sizeLine)
                .Select(i => temp.Substring(i * sizeLine, (temp.Length - i * sizeLine) < sizeLine ? (temp.Length - i * sizeLine) : sizeLine))));
            }
        }
        #endregion

        #region Decode
        public static byte[] Decode(string data)
        {
            data = data.Replace("\r", "");
            data = data.Replace("\n", "");
            int numberOfPadding = data.Substring(data.Length-2).Count<char>(c => c == '=');

            List<byte> decodeBytes = new List<byte>();

            for(int i = 0; i < data.Length; i += 4)
            {
                int block = 0;
                for(int j = 0; j < 4; j++)
                {
                    block = block << 6;
                    if (numberOfPadding > 0 && i + 4 == data.Length && j + numberOfPadding >= 4)
                    {
                        block += 0;
                    }
                    else
                    {
                        block += base64TableBytes[data[i + j]];
                    }
                }

                if (numberOfPadding > 0 && i + 4 == data.Length)
                {
                    Decode24(block, decodeBytes, true, numberOfPadding);
                }
                else
                {
                    Decode24(block, decodeBytes);
                }
            }

            return decodeBytes.ToArray();
        }

        private static void Decode24(int block, List<byte> decodeBytes, bool isPaddingBlock = false, int numberOfPadding = 0)
        {
            byte[] block8 = new byte[3-numberOfPadding];
            for (int i = 0; i < 3; i++)
            {
                if (!isPaddingBlock)
                {
                    block8[i] = (byte)(block & 0xFF);
                }
                else
                {
                    if(numberOfPadding <= i)
                    {
                        block8[i-numberOfPadding] = (byte)(block & 0xFF);
                    }
                }
                block = block >> 8;
            }

            Array.Reverse(block8);

            decodeBytes.AddRange(block8);
        }
        #endregion
    }
}