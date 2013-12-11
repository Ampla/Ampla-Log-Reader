using System;
using System.IO;

namespace Ampla.LogReader.Xml
{
    /// <summary>
    ///     Xml Text Reader that allows documents without a root node to be read into an Xml Document.
    /// </summary>
    public class XmlFragmentTextReader : TextReader
    {
        private readonly char[] rootStart;
        private readonly char[] rootEnd;

        private readonly TextReader textReader;
        private int charsRead;
        private bool eof;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFragmentTextReader"/> class.
        /// </summary>
        /// <param name="root">The root node.</param>
        /// <param name="stream">The stream.</param>
        public XmlFragmentTextReader(string root, Stream stream) : this(root, new StreamReader(stream))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFragmentTextReader"/> class.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="textReader">The text reader.</param>
        public XmlFragmentTextReader(string root, TextReader textReader)
        {
            rootStart = ("<" + root + ">").ToCharArray();
            rootEnd = ("</" + root + ">").ToCharArray();
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
            if (!eof && charsRead < rootStart.Length)
            {
                // Prepend root element
                return ReadFromSource(rootStart, buffer, index, count);
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