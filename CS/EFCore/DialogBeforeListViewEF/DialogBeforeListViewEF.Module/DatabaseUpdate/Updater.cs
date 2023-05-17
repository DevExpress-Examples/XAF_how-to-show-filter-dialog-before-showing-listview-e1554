using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.EF;
using DevExpress.Persistent.BaseImpl.EF;
using E1554.Module;

namespace DialogBeforeListViewEF.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        CreateMaster("Master 1");
        CreateMaster("Master 2");
        CreateMaster("Master 3");
        CreateDetail("Detail 1");
        CreateDetail("Detail 2");
        CreateDetail("Detail 3");
        ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
    }
    private void CreateMaster(string name) {
        Master master = ObjectSpace.FindObject<Master>(new BinaryOperator("MasterName", name));
        if (master == null) {
            master = ObjectSpace.CreateObject<Master>();
            master.MasterName = name;
        }
    }
    private void CreateDetail(string name) {
        Detail detail = ObjectSpace.FindObject<Detail>(new BinaryOperator("DetailName", name));
        if (detail == null) {
            detail = ObjectSpace.CreateObject<Detail>();
            detail.DetailName = name;
        }
    }
    public override void UpdateDatabaseBeforeUpdateSchema() {
        base.UpdateDatabaseBeforeUpdateSchema();
    }
}
