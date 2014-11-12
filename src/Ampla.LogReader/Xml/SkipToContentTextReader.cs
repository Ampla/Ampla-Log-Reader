using System;
using System.IO;
using System.Text;

namespace Ampla.LogReader.Xml
{
    /// <summary>
    ///     Xml Content Reader allows the reader to skip to the content section of a file 
    ///     given that some streams may have unwanted characters or lines at the start
    /// </summary>
    public class SkipToContentTextReader : TextReader
    {
        private readonly string seekLine;
        private readonly TextReader textReader;
        private int charsRead;
        private bool eof;
        private bool lineFound;
        private TextReader lineReader;

        public SkipToContentTextReader(string seekLine, Stream stream) : this(seekLine, new StreamReader(stream))
        {
        }

        public SkipToContentTextReader(string seekLine, TextReader textReader)
        {
            this.seekLine = seekLine;
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
            ReadUntilContent();

            if (!eof && lineReader != null)
            {
                int read = lineReader.Read(buffer, index, count);
                if (read > 0)
                {
                    return read;
                }
                if (read == 0)
                {
                    lineReader = null;
                }
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

        private void ReadUntilContent()
        {
            while (!eof && !lineFound)
            {
                string line = textReader.ReadLine();
                eof = line == null;
                if (line != null && string.Compare(line, seekLine, StringComparison.InvariantCulture) == 0)
                {
                    int peek = textReader.Peek();
                    lineFound = true;
                    string next = string.Concat(line, peek > 0 ? Environment.NewLine : "");
                    lineReader = new StringReader(next);
                }
            }
        }

        /// <summary>
        /// Reads the next character without changing the state of the reader or the character source. Returns the next available character without actually reading it from the input stream.
        /// </summary>
        /// <returns>
        /// An integer representing the next character to be read, or -1 if no more characters are available or the stream does not support seeking.
        /// </returns>
        public override int Peek()
        {
            ReadUntilContent();

            if (lineReader != null)
            {
                int peek = lineReader.Peek();
                if (peek >= 0)
                {
                    return peek;
                }
            }

            return !eof ? textReader.Peek() : base.Peek();
        }

        /// <summary>
        /// Reads the next character from the input stream and advances the character position by one character.
        /// </summary>
        /// <returns>
        /// The next character from the input stream, or -1 if no more characters are available. The default implementation returns -1.
        /// </returns>
        public override int Read()
        {
            char[] buffer = new char[1];
            int chars = Read(buffer, 0, 1);
            if (chars == 0)
            {
                return -1;
            }
            return buffer[0];
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