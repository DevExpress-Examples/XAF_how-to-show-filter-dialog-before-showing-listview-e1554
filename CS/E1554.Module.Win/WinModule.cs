using System;
using System.ComponentModel;

using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Win.SystemModule;

namespace E1554.Module.Win {
    [ToolboxItemFilter("Xaf.Platform.Win")]
    public sealed partial class E1554WindowsFormsModule : ModuleBase {
        public E1554WindowsFormsModule() {
            InitializeComponent();
        }
        private void Application_CreateCustomModelDifferenceStore(Object sender, CreateCustomModelDifferenceStoreEventArgs e) {
            e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), true);
            e.Handled = true;
        }
        private void Application_CreateCustomUserModelDifferenceStore(Object sender, CreateCustomModelDifferenceStoreEventArgs e) {
            e.Store = new ModelDifferenceDbStore((XafApplication)sender, typeof(ModelDifference), false);
            e.Handled = true;
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            application.ModelChanged += new EventHandler(application_ModelChanged);
            application.CreateCustomModelDifferenceStore += Application_CreateCustomModelDifferenceStore;
            application.CreateCustomUserModelDifferenceStore += Application_CreateCustomUserModelDifferenceStore;
        }

        void application_ModelChanged(object sender, EventArgs e) {
            UIType uiType = ((IModelOptionsWin)Application.Model.Options).UIType;
            if (uiType == UIType.StandardMDI) {
                Application.ShowViewStrategy = new MyMdiShowViewStrategy(Application, DevExpress.ExpressApp.Win.Templates.MdiMode.Standard);
            } else if (uiType == UIType.TabbedMDI) {
                Application.ShowViewStrategy = new MyMdiShowViewStrategy(Application, DevExpress.ExpressApp.Win.Templates.MdiMode.Tabbed);
            }
        }
    }
}
