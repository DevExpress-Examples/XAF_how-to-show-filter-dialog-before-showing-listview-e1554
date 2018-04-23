using DevExpress.ExpressApp;
using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Data.Filtering;

namespace E1554.Module {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            CreateMaster("Master 1");
            CreateMaster("Master 2");
            CreateMaster("Master 3");
            CreateDetail("Detail 1");
            CreateDetail("Detail 2");
            CreateDetail("Detail 3");
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
    }
}
