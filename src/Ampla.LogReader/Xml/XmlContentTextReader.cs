using System;
using System.IO;

namespace Ampla.LogReader.Xml
{
    public class XmlContentTextReader : TextReader
    {
        private readonly string seekLine;
        private readonly char[] seekChars;
        private readonly char[] seekWithNewLineChars;
        private char[] prefixChars;
        private readonly TextReader textReader;
        private int charsRead;
        private bool eof;
        private bool lineFound;

        public XmlContentTextReader(string seekLine, Stream stream) : this(seekLine, new StreamReader(stream))
        {
        }

        public XmlContentTextReader(string seekLine, TextReader textReader)
        {
            this.seekLine = seekLine;
            seekChars = seekLine.ToCharArray();
            seekWithNewLineChars = (seekLine + Environment.NewLine).ToCharArray();
            this.textReader = textReader;
        }

        /// <summary>
        /// Reads a maximum of <paramref name="count" /> characters from the current stream and writes the data to <paramref name="buffer" />, beginning at <paramref name="index" />.
        /// </summary>
        /// <param name="buffer">When this method returns, contains the specified character array with the values between <paramref name="index" /> and (<paramref name="index" /> + <paramref name="count" /> - 1) replaced by the characters read from the current source.</param>
        /// <param name="index">The place in <paramref name="buffer" /> at which to begin writing.</param>
        /// <param name="count">The maximum number of characters to read. If the end of the stream is reached before <paramref name="count" /> of characters is read into <paramref name="buffer" />, the current method returns.</param>
        /// <returns>
        /// The number of characters that have been read. The number will be less than or equal to <paramref name="count" />, depending on whether the data is available within the stream. This method returns zero if called when no more characters are left to read.
        /// </returns>
        public override int Read(char[] buffer, int index, int count)
        {
            while (!eof && !lineFound)
            {
                string line = textReader.ReadLine();
                eof = line == null;
                if (string.Compare(line, seekLine, StringComparison.InvariantCulture) == 0)
                {
                    lineFound = true;
                    int nextChar = textReader.Peek();

                    prefixChars = nextChar == -1 ? seekChars : seekWithNewLineChars;
                }
            }

            if (!eof && charsRead < prefixChars.Length)
            {
                // Prepend line to seek
                return ReadFromSource(prefixChars, buffer, index, count);
            }

            if (!eof)
            {
                // Normal reading operation
                int readChars = textReader.Read(buffer, index, count);
                if (readChars > 0) return readChars;

                // We've reached the end of the Stream
                eof = true;
                charsRead = 0;
            }
            return 0;
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