
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.Templates;
using DialogBeforeListViewEF.Module;
using DialogBeforeListViewEF.Win;
using System;

namespace E1554.Module.Win {


    public class WinShowFilterDialogController : WindowController {
        public WinShowFilterDialogController() {
            TargetWindowType = WindowType.Main;
        }

        ShowNavigationItemController showNavigationItemController;
        ListView oldListView;

        protected override void OnActivated() {
            base.OnActivated();

            showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            if (showNavigationItemController != null) {
                showNavigationItemController.CustomShowNavigationItem += ShowNavigationItemController_CustomShowNavigationItem;
            }
        }
        private void ShowNavigationItemController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e) {
            ViewShortcut shortcut = e.ActionArguments.SelectedChoiceActionItem.Data as ViewShortcut;
            if (shortcut != null) {
                oldListView = Application.ProcessShortcut(shortcut) as ListView;
                if (oldListView != null) {
                    e.Handled = true;
                    NonPersistentObjectSpace nonPersistentObjectSpace = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(ViewFilterContainer));
                    IObjectSpace persistentObjectSpace = Application.CreateObjectSpace(typeof(ViewFilterObject));
                    nonPersistentObjectSpace.AdditionalObjectSpaces.Add(persistentObjectSpace);
                    ViewFilterContainer newViewFilterContainer = nonPersistentObjectSpace.CreateObject<ViewFilterContainer>();
                    newViewFilterContainer.ObjectType = oldListView.ObjectTypeInfo.Type;
                    newViewFilterContainer.Filter = GetFilterObject(persistentObjectSpace, ((IModelListViewAdditionalCriteria)oldListView.Model).AdditionalCriteria, newViewFilterContainer.ObjectType);
                    DetailView filterDetailView = Application.CreateDetailView(nonPersistentObjectSpace, newViewFilterContainer);
                    filterDetailView.Caption = String.Format("Filter for the {0} ListView", oldListView.Caption);
                    filterDetailView.ViewEditMode = ViewEditMode.Edit;
                    e.ActionArguments.ShowViewParameters.CreatedView = filterDetailView;
                    e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    DialogController dialogCotnroller = Application.CreateController<DialogController>();
                    dialogCotnroller.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(dialogCotnroller_Accepting);
                    dialogCotnroller.Cancelling += dialogCotnroller_Cancelling;
                    dialogCotnroller.ViewClosed += dialogCotnroller_ViewClosed;
                    e.ActionArguments.ShowViewParameters.Controllers.Add(dialogCotnroller);
                }
            }
        }
        void dialogCotnroller_Cancelling(object sender, EventArgs e) {
            oldListView.Dispose();
        }
        void dialogCotnroller_Accepting(object sender, DialogControllerAcceptingEventArgs e) {
            ViewFilterContainer currentViewFilterContainer = (ViewFilterContainer)e.AcceptActionArgs.CurrentObject;
            ListView targetView = GetTargetView();
            ((IModelListViewAdditionalCriteria)targetView.Model).AdditionalCriteria = currentViewFilterContainer.Criteria;
            targetView.CollectionSource.Criteria["ByViewFilterObject"] = CriteriaEditorHelper.GetCriteriaOperator(currentViewFilterContainer.Criteria, currentViewFilterContainer.ObjectType, targetView.ObjectSpace);
            oldListView = null;
            ShowView(targetView, e.ShowViewParameters);
        }
        void dialogCotnroller_ViewClosed(object sender, EventArgs e) {
            oldListView = null;
        }
        protected virtual void ShowViewCore(DevExpress.ExpressApp.View targetView, ShowViewParameters showViewParameters) {
            Window.SetView(targetView);
        }
        protected void ShowView(DevExpress.ExpressApp.View targetView, ShowViewParameters showViewParameters) {
            if (Application.ShowViewStrategy is MdiShowViewStrategy) {
                showViewParameters.CreatedView = targetView;
                showViewParameters.TargetWindow = TargetWindow.NewWindow;
                showViewParameters.NewWindowTarget = NewWindowTarget.MdiChild;
                return;
            }
            ShowViewCore(targetView, showViewParameters);
        }
        protected ListView GetTargetView() {
            ListView targetView = oldListView;
            if (Application.ShowViewStrategy is MyMdiShowViewStrategy) {
                ListView existingView = ((MyMdiShowViewStrategy)Application.ShowViewStrategy).FindExistingView(targetView) as ListView;
                return existingView ?? targetView;
            } else {
                return targetView;
            }
        }
        private ViewFilterObject GetFilterObject(IObjectSpace objectSpace, string listViewCriteria, Type objectType) {
            CriteriaOperator criteria = CriteriaOperator.Parse("Criteria = ? and ObjectType = ?", listViewCriteria, objectType);
            ViewFilterObject filterObject = objectSpace.FindObject<ViewFilterObject>(criteria);
            if (filterObject == null) {
                filterObject = objectSpace.CreateObject<ViewFilterObject>();
                filterObject.ObjectType = objectType;
                filterObject.Criteria = listViewCriteria;
                filterObject.FilterName = "Default";
            }
            return filterObject;
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (showNavigationItemController != null) {
                showNavigationItemController.CustomShowNavigationItem -= ShowNavigationItemController_CustomShowNavigationItem;
                showNavigationItemController = null;
            }
            oldListView = null;
        }
    }



 
}
