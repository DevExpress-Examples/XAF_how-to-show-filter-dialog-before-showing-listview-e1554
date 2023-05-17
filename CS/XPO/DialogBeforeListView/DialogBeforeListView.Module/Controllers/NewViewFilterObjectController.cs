using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;

namespace E1554.Module {
    public class NewViewFilterObjectController : ObjectViewController<ListView, ViewFilterObject> {
        public NewViewFilterObjectController() {
            TargetViewId = "ViewFilterObject_LookupListView";
        }
        protected override void OnActivated() {
            base.OnActivated();
            if (View.CollectionSource is PropertyCollectionSource && ((PropertyCollectionSource)View.CollectionSource).MasterObjectType == typeof(ViewFilterContainer)) {
                NewObjectViewController newObjectViewController = Frame.GetController<NewObjectViewController>();
                if (newObjectViewController != null) { 
                    newObjectViewController.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(ViewController1_ObjectCreated);
                }     
            }
        }
        void ViewController1_ObjectCreated(object sender, ObjectCreatedEventArgs e) {
            if (e.CreatedObject is ViewFilterObject) {
                ViewFilterObject newViewFilterObject = (ViewFilterObject)e.CreatedObject;
                PropertyCollectionSource pcs = (PropertyCollectionSource)View.CollectionSource;
                newViewFilterObject.ObjectType = ((ViewFilterContainer)pcs.MasterObject).ObjectType;
            }
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (View.CollectionSource is PropertyCollectionSource && ((PropertyCollectionSource)View.CollectionSource).MasterObjectType == typeof(ViewFilterContainer)) {
                NewObjectViewController newObjectViewController = Frame.GetController<NewObjectViewController>();
                if (newObjectViewController != null) { 
                    newObjectViewController.ObjectCreated -= new EventHandler<ObjectCreatedEventArgs>(ViewController1_ObjectCreated);
                }
            }
        }
    }
}
