using System.IO;

namespace Assignment2C2P.Controllers
{
    internal class CsvReader
    {
        private StringReader textReader;

        public CsvReader(StringReader textReader)
        {
            this.textReader = textReader;
        }
    }
}