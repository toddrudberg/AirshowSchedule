using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirshowSchedules
{
    [Serializable]
    public class Regions
    {
        //Electroimpact.XmlSerialization.SerializableDictionary<string, string> Regions = new Electroimpact.XmlSerialization.SerializableDictionary<string, string>();
        public Dictionary<string, string> myRegions = new Dictionary<string, string>();

        public class cValuePair
        {
            public string sState;
            public string sRegion;
        }

        public void BuildFile(string tsvPaste)
        {
            string[] lines = tsvPaste.Split('\n');
            foreach (string s in lines)
            {
                string[] ss = s.Split('\t');
                if (ss.Length > 1)
                    myRegions.Add(ss[0], ss[1].Trim());
            }
        }

        public List<string> GetRegionList()
        {
            List<string> regions = myRegions.Values.ToList();
            regions.Sort();
            string lastRegion = "";
            List<string> uniqueRegions = new List<string>();

            foreach (string rgn in regions)
            {
                if (lastRegion != rgn)
                {
                    uniqueRegions.Add(rgn);
                    lastRegion = rgn;
                }
            }

            return uniqueRegions;
        }

        public static Regions LoadMe(string FileName)
        {
            string fng = FileName;
            Regions fs = new Regions();

            List<cValuePair> getthedata = new List<cValuePair>();
            try
            {
                if (System.IO.File.Exists(fng))
                {
                    getthedata = Electroimpact.XmlSerialization.Serializer.Load<List<cValuePair>>(fng);
                }

                foreach (cValuePair pair in getthedata)
                {
                    fs.myRegions.Add(pair.sState, pair.sRegion);
                }

                return fs;
            }
            catch { return fs; }
        }

        public static void SaveMe(Regions fs, string FileName)
        {
            List<cValuePair> pairs = new List<cValuePair>();
            foreach (KeyValuePair<string, string> pair in fs.myRegions)
            {
                cValuePair cp = new cValuePair();
                cp.sState = pair.Key;
                cp.sRegion = pair.Value;
                pairs.Add(cp);
            }

            Electroimpact.XmlSerialization.Serializer.Save(pairs, FileName);
        }
    }
}
