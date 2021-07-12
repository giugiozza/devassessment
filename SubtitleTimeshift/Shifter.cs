using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleTimeshift
{
    public class Shifter
    {
        async static public Task Shift(Stream input, Stream output, TimeSpan timeSpan, Encoding encoding, int bufferSize = 1024, bool leaveOpen = false)
        {
            try
            {
                var reader = new StreamReader(input, encoding, false, bufferSize, leaveOpen);
                var writer = new StreamWriter(output, encoding, bufferSize, leaveOpen);

                var line = await reader.ReadLineAsync();

                while (line != null)
                {
                    if(isTheTimeLine(line))
                    {
                        updateTime(line, timeSpan);
                    }
                    else
                    {
                        writer.WriteLine(line);
                    }
                    line = await reader.ReadLineAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        private static bool isTheTimeLine(string line)
        {
            return false;
        }
        private static bool updateTime(string line, TimeSpan timeSpan)
        {
            return true;
        }
    }
}
