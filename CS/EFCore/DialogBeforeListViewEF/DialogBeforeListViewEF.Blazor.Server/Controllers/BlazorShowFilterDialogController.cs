using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp;
using E1554.Module;
using DialogBeforeListViewEF.Module;

namespace DialogBeforeListView.Blazor.Server.Controllers {
    public class BlazorShowFilterDialogController : ViewController<ListView> {
        SimpleAction showFilterDialogAction;
        public BlazorShowFilterDialogController() {
            TargetViewNesting = Nesting.Root;
            showFilterDialogAction = new SimpleAction(this, "ShowFilterDialog", "Filters");
            showFilterDialogAction.Execute += ShowFilterDialogAction_Execute;
        }
        protected override void OnActivated() {
            base.OnActivated();
            if (Frame is Window && ((Window)Frame).IsMain) {
                showFilterDialogAction.Active["IsMainWindow"] = true;
                View.CollectionSource.Criteria[nameof(BlazorShowFilterDialogController)] = CollectionSourceBase.EmptyCollectionCriteria;
                ShowFilterDialog(View);
            } else {
                showFilterDialogAction.Active["IsMainWindow"] = false;
            }
        }
        private void ShowFilterDialogAction_Execute(object sender, SimpleActionExecuteEventArgs e) {
            ShowFilterDialog(View);
        }
        protected void ShowFilterDialog(ListView listView) {
            NonPersistentObjectSpace nonPersistentObjectSpace = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(ViewFilterContainer));
            IObjectSpace persistentObjectSpace = Application.CreateObjectSpace(typeof(ViewFilterObject));
            nonPersistentObjectSpace.AdditionalObjectSpaces.Add(persistentObjectSpace);
            ViewFilterContainer newViewFilterContainer = nonPersistentObjectSpace.CreateObject<ViewFilterContainer>();
            newViewFilterContainer.ObjectType = View.ObjectTypeInfo.Type;
            newViewFilterContainer.Filter = GetFilterObject(persistentObjectSpace, ((IModelListViewAdditionalCriteria)View.Model).AdditionalCriteria, newViewFilterContainer.ObjectType);
            DetailView filterDetailView = Application.CreateDetailView(nonPersistentObjectSpace, newViewFilterContainer);
            filterDetailView.Caption = String.Format("Filter for the {0} ListView", View.Caption);
            filterDetailView.ViewEditMode = ViewEditMode.Edit;
            Application.ShowViewStrategy.ShowViewInPopupWindow(filterDetailView, () => FilterDetailView_OK(filterDetailView));
        }
        void FilterDetailView_OK(DetailView filterDetailView) {
            filterDetailView.ObjectSpace.CommitChanges();
            ViewFilterContainer currentViewFilterContainer = (ViewFilterContainer)filterDetailView.CurrentObject;
            ((IModelListViewAdditionalCriteria)View.Model).AdditionalCriteria = currentViewFilterContainer.Criteria;
            View.CollectionSource.Criteria[nameof(BlazorShowFilterDialogController)] = CriteriaEditorHelper.GetCriteriaOperator(currentViewFilterContainer.Criteria, currentViewFilterContainer.ObjectType, ObjectSpace);
        }
        private ViewFilterObject GetFilterObject(IObjectSpace objectSpace, string listViewCriteria, Type objectType) {
            ViewFilterObject filterObject = objectSpace.FirstOrDefault<ViewFilterObject>(fo => fo.Criteria == listViewCriteria && fo.ObjectType == objectType);
            if (filterObject == null) {
                filterObject = objectSpace.FirstOrDefault<ViewFilterObject>(fo => fo.FilterName == "Default");
                if (filterObject == null) {
                    using (IObjectSpace creatingObjectSpace = Application.CreateObjectSpace(typeof(ViewFilterObject))) {
                        ViewFilterObject newFilterObject = creatingObjectSpace.CreateObject<ViewFilterObject>();
                        newFilterObject.FilterName = "Default";
                        creatingObjectSpace.CommitChanges();
                        filterObject = objectSpace.GetObject<ViewFilterObject>(newFilterObject);
                    }
                }
                filterObject.ObjectType = objectType;
                filterObject.Criteria = listViewCriteria;
            }
            return filterObject;
        }
    }
}
