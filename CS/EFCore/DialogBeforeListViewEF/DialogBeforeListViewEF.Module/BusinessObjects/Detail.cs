using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

namespace E1554.Module {
    [DefaultClassOptions]
    public class Detail : BaseObject {
        public virtual string DetailName { get; set; }

        public virtual Master Master { get; set; }
    }
}
