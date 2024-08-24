using TapAndRun.Tools.Reactivity;

namespace TapAndRun.MVP.Settings.Model
{
    public interface ISettingsModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; }

        void Initialize();
    }

    public interface ISelfSettingsModel
    {
        SimpleReactiveProperty<bool> IsDisplaying { get; }
        
        bool AudioStatus { get; set; }
        bool VibroStatus { get; set; }
        string Language { get; set; }
    }

    public class SettingsModel : ISettingsModel, ISelfSettingsModel
    {
        public SimpleReactiveProperty<bool> IsDisplaying { get; set; }
        
        public bool AudioStatus { get; set; }
        public bool VibroStatus { get; set; }
        public string Language { get; set; }
        
        public SettingsModel()
        {
        }

        public void Initialize()
        {
            IsDisplaying = new SimpleReactiveProperty<bool>(false);
            
            //TODO Try Load Data

            AudioStatus = true;
            VibroStatus = true;
            Language = "en";
        }
    }
}