using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace aCudaResearch.FpGrowth.Data.MsWeb
{
    public class MsDataBuilder : IDataBuilder<MsInstance<int>>
    {
        public MsInstance<int> BuildInstance(ExecutionSettings executionSettings)
        {
            var instance = new MsInstance<int>();
            if (!File.Exists(executionSettings.DataSourcePath))
            {
                return null;
            }

            var r = new StreamReader(executionSettings.DataSourcePath);
            string line, transactionId = null;
            var votes = new List<int>();

            while (!r.EndOfStream)
            {
                line = r.ReadLine();
                switch (line[0])
                {
                    case 'A':
                        instance.AddElement(line);
                        break;
                    case 'C':
                        if (transactionId != null)
                        {
                            instance.AddEntry(Convert.ToInt32(transactionId), votes.ToArray());
                            votes.Clear();
                        }
                        transactionId = line.Split(',')[2];
                        break;
                    case 'V':
                        votes.Add(Convert.ToInt32(line.Split(',')[1]));
                        break;
                }
            }
            if (transactionId != null)
            {
                instance.AddEntry(Convert.ToInt32(transactionId), votes.ToArray());
            }
            return instance;
        }
    }
}
