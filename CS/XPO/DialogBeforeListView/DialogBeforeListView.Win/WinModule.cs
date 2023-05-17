using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using System.Collections.Generic;
using System;
using DevExpress.ExpressApp.Win.Templates;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.SystemModule;

namespace DialogBeforeListView.Win;

[ToolboxItemFilter("Xaf.Platform.Win")]
// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
public sealed class DialogBeforeListViewWinModule : ModuleBase {
    public DialogBeforeListViewWinModule() {
        DevExpress.ExpressApp.Editors.FormattingProvider.UseMaskSettings = true;
    }
    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
        return ModuleUpdater.EmptyModuleUpdaters;
    }
    public override void Setup(XafApplication application) {
        base.Setup(application);
        application.ModelChanged += new EventHandler(application_ModelChanged);
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

public class MyMdiShowViewStrategy : MdiShowViewStrategy {
    public MyMdiShowViewStrategy(XafApplication application, MdiMode mdiMode) : base(application, mdiMode) { }
    public DevExpress.ExpressApp.View FindExistingView(DevExpress.ExpressApp.View view) {
        WinWindow window = FindWindowByView(view);
        return window == null ? null : window.View;
    }
}
