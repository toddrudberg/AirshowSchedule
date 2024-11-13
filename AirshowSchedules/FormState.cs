using Electroimpact.SettingsFormBuilderV2.Attributes;

namespace AirshowSchedules
{
    [Serializable]
    public class FormState
    {
        public string fnLastParsedFile = "";
        public string fnCurrentXMLDataBase = "";
        public string fnContactDataBase = "";
        [Display(DisplayName = "Choose the Region File:")]
        [FileBrowseDialog(OpenFileDialogFilter = "Region File (*.xml)|*.xml")]
        public string fnRegions = "";

        public static void SaveMe(FormState fs)
        {
            string fng = Electroimpact.XmlSerialization.Serializer.GenerateDefaultFilename("UndauntedAirshows", "AirshowScheduler-FormState");

            Electroimpact.XmlSerialization.Serializer.Save(fs, fng);
        }

        public static FormState LoadMe()
        {
            string fng = Electroimpact.XmlSerialization.Serializer.GenerateDefaultFilename("UndauntedAirshows", "AirshowScheduler-FormState");

            FormState fs = new FormState();

            try
            {
                if (System.IO.File.Exists(fng))
                    fs = Electroimpact.XmlSerialization.Serializer.Load<FormState>(fng);

                return fs;
            }
            catch { return fs; }
        }
    }
}
