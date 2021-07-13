using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace SubtitleTimeshift
{
    static class Constants
    {
        public const string timePattern = @"(\d+)[:,](\d+)[:,](\d+)[,.](\d+)(\b --> \b)(\d+)[:,](\d+)[:,](\d+)[,.](\d+)";
    }
    public class Shifter
    {
        async static public Task Shift(Stream input, Stream output, TimeSpan timeSpan, Encoding encoding, int bufferSize = 1024, bool leaveOpen = false)
        {
            try
            {
                using(StreamReader reader = new StreamReader(input)){
                    using(StreamWriter writer = new StreamWriter(output)){

                        string line = await reader.ReadLineAsync();
                        
                        while (line != null)
                        {
                            if(isTheTimeLine(line))
                            {
                                line = updateTime(line, timeSpan);
                            }
                            writer.WriteLine(line);
                            line = await reader.ReadLineAsync();
                        }
                    }
                }               
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed.");
                throw e;
            }
        }

        /// <summary>
        /// Checks if the line read is the one with the time.
        /// </summary>
        private static bool isTheTimeLine(string line)
        {
            return Regex.IsMatch(line, Constants.timePattern);
        }

        /// <summary>
        /// Updates the time with the time span.
        /// </summary>
        private static string updateTime(string line, TimeSpan timeSpan)
        {            
            var newLine = "";
            Regex pattern = new Regex(Constants.timePattern);
            Match match = pattern.Match(line);
            if (match.Success)
            {
                newLine = new TimeSpan(0, int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value),
                    int.Parse(match.Groups[4].Value)).Add(timeSpan).ToString(@"hh\:mm\:ss\.fff")
                    + match.Groups[5].Value
                    + new TimeSpan(0, int.Parse(match.Groups[6].Value), int.Parse(match.Groups[7].Value), int.Parse(match.Groups[8].Value),
                    int.Parse(match.Groups[9].Value)).Add(timeSpan).ToString(@"hh\:mm\:ss\.fff");
            }

            return newLine;
        }
    }
}
