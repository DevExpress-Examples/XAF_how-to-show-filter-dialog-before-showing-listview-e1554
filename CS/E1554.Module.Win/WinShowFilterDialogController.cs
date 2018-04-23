using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.Templates;

namespace E1554.Module.Win {
    public class WinShowFilterDialogController : ShowFilterDialogController {
        protected override ListView GetTargetView() {
            ListView targetView = base.GetTargetView();
            if (Application.ShowViewStrategy is MyMdiShowViewStrategy) {
                ListView existingView = ((MyMdiShowViewStrategy)Application.ShowViewStrategy).FindExistingView(targetView) as ListView;
                return existingView ?? targetView;
            } else {
                return targetView;
            }
        }
    }
    public class MyMdiShowViewStrategy : MdiShowViewStrategy {
        public MyMdiShowViewStrategy(XafApplication application, MdiMode mdiMode) : base(application, mdiMode) { }
        public View FindExistingView(View view) {
            WinWindow window = FindWindowByView(view);
            return window == null ? null : window.View;
        }
    }
}
