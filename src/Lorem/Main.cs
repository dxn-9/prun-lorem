using ManagedCommon;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Wox.Plugin;

using System.Diagnostics.Tracing;
using System.Windows.Shapes;
using System.Linq;

namespace Lorem
{
    public class Main : IPlugin
    {


        private PluginInitContext Context { get; set; }
        public string Name => "Lorem";

        public string Description => "Lorem Ipsum Generator";

        public List<Result> Query(Query query)
        {
       
            var results = new List<Result>(); 

            var strs = query.RawQuery.Split(" ");
            var nums = strs[1];

            if(long.TryParse(nums, out var result))
            {
                string lorem = GetLorem(result);

                if(lorem != null)
                {
                    results.Add(new Result
                    {
                        Title = lorem,
                        Action = e => {
                            Clipboard.SetText(lorem);
                            return true;
                        },
                    });
                }

            
            }

         

            return results;
        }

        public string? GetLorem(long WordsNumber)
        {
   
            try
            {
                // Get the directory where the current assembly (DLL) is located
                string assemblyDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // Specify the relative path to the file
                string relativeFilePath = "lorem.txt";

                // Combine the assembly directory with the relative file path to get the full path
                string fullPath = System.IO.Path.Combine(assemblyDirectory, relativeFilePath);

                // Check if the file exists
                if (File.Exists(fullPath))
                {
                    // Use FileStream to read the file as a stream
                    using FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                    // Read the file's content as a stream
                    using StreamReader streamReader = new StreamReader(fileStream);
                    // Read and display each line in the file
                    var wordsList = new List<string>();


                    long ReadWords = 0;
                    string line;
                    while ((line = streamReader.ReadLine()) != null && ReadWords != WordsNumber)
                    {

                        var words = line.Trim().Split(' ');
                        foreach (string word in words)
                        {
                            if (ReadWords < WordsNumber)
                            {
                                wordsList.Add(word);
                            }
                            ReadWords += 1;
                        }

                    }

                    return string.Join(" ", wordsList);
                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex)
            {
      
                return null;
            }
 
        }

    

        public void Init(PluginInitContext context)
        {
            Context = context;
            

    }


}
}
