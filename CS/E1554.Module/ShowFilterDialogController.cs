using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Data.Filtering;

namespace E1554.Module {
    public class ShowFilterDialogController : WindowController {
        public ShowFilterDialogController() {
            TargetWindowType = WindowType.Main;
        }

        ShowNavigationItemController showNavigationItemController;
        ListView oldListView;

        protected override void OnActivated() {
            base.OnActivated();
            showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            if (showNavigationItemController != null) {
                showNavigationItemController.ShowNavigationItemAction.Execute += ShowNavigationItemAction_Execute;
                showNavigationItemController.CustomUpdateSelectedItem += showNavigationItemController_CustomUpdateSelectedItem;
            }
        }

        void ShowNavigationItemAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e) {
            if (e.ShowViewParameters.CreatedView is ListView) {
                oldListView = (ListView)e.ShowViewParameters.CreatedView;
                NonPersistentObjectSpace nonPersistentObjectSpace = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(ViewFilterContainer));
                IObjectSpace persistentObjectSpace = Application.CreateObjectSpace(typeof(ViewFilterObject));
                nonPersistentObjectSpace.AdditionalObjectSpaces.Add(persistentObjectSpace);
                ViewFilterContainer newViewFilterContainer = nonPersistentObjectSpace.CreateObject<ViewFilterContainer>();
                newViewFilterContainer.ObjectType = oldListView.ObjectTypeInfo.Type;
                newViewFilterContainer.Filter = GetFilterObject(persistentObjectSpace, ((IModelListViewAdditionalCriteria)oldListView.Model).AdditionalCriteria, newViewFilterContainer.ObjectType);
                DetailView filterDetailView = Application.CreateDetailView(nonPersistentObjectSpace, newViewFilterContainer);
                filterDetailView.Caption = String.Format("Filter for the {0} ListView", oldListView.Caption);
                filterDetailView.ViewEditMode = ViewEditMode.Edit;
                e.ShowViewParameters.CreatedView = filterDetailView;
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                DialogController dialogCotnroller = Application.CreateController<DialogController>();
                dialogCotnroller.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(dialogCotnroller_Accepting);
                dialogCotnroller.ViewClosed += dialogCotnroller_ViewClosed;
                e.ShowViewParameters.Controllers.Add(dialogCotnroller);
            }
        }

        void dialogCotnroller_Accepting(object sender, DialogControllerAcceptingEventArgs e) {
            ViewFilterContainer currentViewFilterContainer = (ViewFilterContainer)e.AcceptActionArgs.CurrentObject;
            ListView targetView = GetTargetView();
            ((IModelListViewAdditionalCriteria)targetView.Model).AdditionalCriteria = currentViewFilterContainer.Criteria;
            targetView.CollectionSource.Criteria["ByViewFilterObject"] = CriteriaEditorHelper.GetCriteriaOperator(currentViewFilterContainer.Criteria, currentViewFilterContainer.ObjectType, targetView.ObjectSpace);
            ShowViewParameters parameters = new ShowViewParameters(targetView);
            parameters.TargetWindow = TargetWindow.Current;
            parameters.Context = TemplateContext.View;
            ShowViewSource source = new ShowViewSource(Frame, showNavigationItemController.ShowNavigationItemAction);
            Application.ShowViewStrategy.ShowView(parameters, source);
            oldListView = null;
        }

        protected virtual ListView GetTargetView() {
            return oldListView;
        }

        void dialogCotnroller_ViewClosed(object sender, EventArgs e) {
            oldListView = null;
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

        void showNavigationItemController_CustomUpdateSelectedItem(object sender, CustomUpdateSelectedItemEventArgs e) {
            if (oldListView != null) {
                e.ProposedSelectedItem = showNavigationItemController.FindNavigationItemByViewShortcut(oldListView.CreateShortcut());
                e.Handled = true;
            }
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            if (showNavigationItemController != null) {
                showNavigationItemController.ShowNavigationItemAction.Execute -= ShowNavigationItemAction_Execute;
                showNavigationItemController.CustomUpdateSelectedItem -= showNavigationItemController_CustomUpdateSelectedItem;
                showNavigationItemController = null;
            }
            oldListView = null;
        }
    }
}
