using System;
using System.IO;

namespace Ampla.LogReader.Xml
{
    public class XmlFragmentTextReader : TextReader
    {
        private readonly char[] rootStart;
        private readonly char[] rootEnd;

        private readonly TextReader innerReader;
        private int charsRead;
        private bool eof;

        public XmlFragmentTextReader(string root, Stream stream) : this(root, new StreamReader(stream))
        {
        }

        public XmlFragmentTextReader(string root, TextReader innerReader)
        {
            rootStart = ("<" + root + ">").ToCharArray();
            rootEnd = ("</" + root + ">").ToCharArray();
            this.innerReader = innerReader;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            if (!eof && charsRead < rootStart.Length)
            {
                // Prepend root element
                return ReadFromSource(rootStart, buffer, index, count);
            }

            if (!eof)
            {
                // Normal reading operation
                int readChars = innerReader.Read(buffer, index, count);
                if (readChars > 0) return readChars;

                // We've reached the end of the Stream
                eof = true;
                charsRead = 0;
            }

            // Append root element end tag at the end of the Stream
            return ReadFromSource(rootEnd, buffer, index, count);
        }

        private int ReadFromSource(char[] source, char[] buffer, int offset, int count)
        {
            int length = Math.Min(source.Length - charsRead, count);
            Array.Copy(source, charsRead, buffer, offset, length);
            charsRead += length;
            return length;
        }
    }
}