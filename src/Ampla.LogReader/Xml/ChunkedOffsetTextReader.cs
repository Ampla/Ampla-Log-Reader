using System;
using System.Collections.Generic;
using System.IO;

namespace Ampla.LogReader.Xml
{
    public class ChunkedOffsetTextReader : TextReader
    {
        private readonly StringReader streamReader;
        private readonly int streamOffset;

        private int length;

        public ChunkedOffsetTextReader(int offSet, IEnumerable<string> lines)
        {
            streamReader = new StringReader(string.Join(Environment.NewLine, lines));
            streamOffset = offSet;
            length = 0;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            int offset = Math.Max(index - streamOffset, 0);
            int read = streamReader.Read(buffer, offset, count);
            length += read;
            return read;
        }

        public int Length
        {
            get { return length; }
        }

        public int Offset
        {
            get { return streamOffset; }
        }
    }
}