using System.Xml.Serialization;

namespace AirshowSchedules
{
    [Serializable]
    public class AirshowGroup
    {
        //public cAirshowFileParserSetupTool FileSetup = new cAirshowFileParserSetupTool();
        public cAirshows Airshows = new cAirshows();
        public int AirshowYearOfInterest = 2025;
        public AirshowGroup()
        { }

        public class cAirshows
        {
            [XmlElement("Airshow")]
            public List<Airshow> myShows = new List<Airshow>();
        }

        public List<Airshow> GetAirshows()
        {
            return Airshows.myShows;
        }

        public List<Airshow> GetAirshowsForYear()
        {
            return Airshows.myShows.Where(x => x.Year == AirshowYearOfInterest).ToList();
        }
        
        public static void SaveMe(AirshowGroup airshowGroup, string FileName)
        {
            Electroimpact.XmlSerialization.Serializer.Save(airshowGroup, FileName);
        }
        public static AirshowGroup LoadMe(string FileName, out bool success)
        {
            try
            {
                if (System.IO.File.Exists(FileName))
                {
                    AirshowGroup asg = new AirshowGroup();
                    asg = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(FileName);
                    success = true;
                    return asg;
                }
                else
                {

                    success = false;
                    return new AirshowGroup();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                success = false;
                return new AirshowGroup();
            }
        }
    }
}
