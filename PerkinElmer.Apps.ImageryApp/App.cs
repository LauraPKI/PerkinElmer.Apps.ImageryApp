using System;
using System.Runtime.Serialization;
using PerkinElmer.Signals.Analytics.AppCommon;
using PerkinElmer.Signals.Analytics.Components;
using Spotfire.Dxp.Framework.Persistence;

namespace PerkinElmer.Apps.ImageryApp
{
    [Serializable]
    [PersistenceVersion(1, 0)]
    [AppMetadata("PerkinElmer.Apps.ImageryApp", "PerkinElmer.Apps.ImageryApp")]
    public partial class App : BaseComponentsApp
    {

        #region Constructors
        public App(string name, string jsonAppStore)
            : base(name, jsonAppStore)
        {

        }
        protected App(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
        #endregion

        #region User Methods

        public override void ViewController()
        {
            // Configure here all the watchers you need for the app
        }





        #endregion
    }

}
