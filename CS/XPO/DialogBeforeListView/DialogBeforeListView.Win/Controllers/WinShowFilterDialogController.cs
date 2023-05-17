using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.Templates;


namespace E1554.Module.Win {
    public class WinShowFilterDialogController : ShowFilterDialogController {
        protected override ListView GetTargetView() {
            ListView targetView = base.GetTargetView();
            if(Application.ShowViewStrategy is MyMdiShowViewStrategy) {
                ListView existingView = ((MyMdiShowViewStrategy)Application.ShowViewStrategy).FindExistingView(targetView) as ListView;
                return existingView ?? targetView;
            }
            else {
                return targetView;
            }
        }
        protected override void ShowView(DevExpress.ExpressApp.View targetView, ShowViewParameters showViewParameters) {
            if(Application.ShowViewStrategy is MdiShowViewStrategy) {
                showViewParameters.CreatedView = targetView;
                showViewParameters.TargetWindow = TargetWindow.NewWindow;
                showViewParameters.NewWindowTarget = NewWindowTarget.MdiChild;
                return;
            }
            base.ShowView(targetView, showViewParameters);
        }
    }
    public class MyMdiShowViewStrategy : MdiShowViewStrategy {
        public MyMdiShowViewStrategy(XafApplication application, MdiMode mdiMode) : base(application, mdiMode) { }
        public DevExpress.ExpressApp.View FindExistingView(DevExpress.ExpressApp.View view) {
            WinWindow window = FindWindowByView(view);
            return window == null ? null : window.View;
        }
    }
}
