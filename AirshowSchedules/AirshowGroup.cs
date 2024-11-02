using System.Xml.Serialization;

namespace AirshowSchedules
{
    [Serializable]
    public class AirshowGroup
    {
        //public cAirshowFileParserSetupTool FileSetup = new cAirshowFileParserSetupTool();
        public cAirshows Airshows = new cAirshows();
        public int AirshowYearOfInterest = 2023;
        public AirshowGroup()
        { }

        #region Wrapper Classes
        public class cAirshows
        {
            [XmlElement("Airshow")]
            public List<Airshow> myShows = new List<Airshow>();
        }
        #endregion


        public static void SaveMe(List<Airshow> AirShows, string FileName)
        {
            AirshowGroup myGroup = new AirshowGroup();
            myGroup.Airshows.myShows = AirShows;
            Electroimpact.XmlSerialization.Serializer.Save(AirShows, FileName);
        }
        public static AirshowGroup LoadMe(string FileName)
        {
            try
            {
                if (System.IO.File.Exists(FileName))
                {
                    AirshowGroup asg = new AirshowGroup();
                    asg = Electroimpact.XmlSerialization.Serializer.Load<AirshowGroup>(FileName);
                    return asg;
                }
                else
                {
                    return new AirshowGroup();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return new AirshowGroup();
            }
        }
    }
}
