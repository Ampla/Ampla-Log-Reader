using System;
using System.Collections.Generic;
using System.IO;

namespace Ampla.LogReader.Xml
{
    /// <summary>
    ///     Text Reader that allows the underlying stream to be read in complete chunks
    /// </summary>
    public class TruncatedTextReader : TextReader
    {
        private readonly string chunkLine;
        private readonly TextReader textReader;
        private bool eof;
        private int offSet;
        private readonly List<string> lines = new List<string>();
        private ChunkedOffsetTextReader chunkedReader;

        public TruncatedTextReader(string chunkLine, Stream stream) : this(chunkLine, new StreamReader(stream))
        {
        }

        public TruncatedTextReader(string chunkLine, TextReader textReader)
        {
            this.chunkLine = chunkLine;
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
            while (!eof)
            {
                if (chunkedReader == null)
                {
                    ReadNextChunk();
                }

                if (chunkedReader != null)
                {
                    int lineRead = chunkedReader.Read(buffer, index, count);
                    if (lineRead > 0)
                    {
                        offSet += lineRead;
                        return lineRead;
                    }

                    if (lineRead == 0)
                    {
                        chunkedReader = null;
                    }
                }
            }
            return 0;
        }

        private void ReadNextChunk()
        {
            while (!eof && chunkedReader == null)
            {
                string line = textReader.ReadLine();
                eof = line == null;
                if (!eof)
                {
                    lines.Add(line);
                }
                if (string.Compare(line, chunkLine, StringComparison.InvariantCulture) == 0)
                {
                    int nextChar = textReader.Peek();

                    if (nextChar > -1)
                    {
                        lines.Add("");
                    }
                    if (lines.Count > 0)
                    {
                        chunkedReader = new ChunkedOffsetTextReader(offSet, lines);
                        lines.Clear();
                    }
                }
            }
        }
    }
}