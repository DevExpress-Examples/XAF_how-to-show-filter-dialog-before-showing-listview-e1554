using System;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl.EF;
using System.Collections.ObjectModel;

namespace E1554.Module {
    [DefaultClassOptions]
    public class Master : BaseObject {
        public virtual string MasterName { get; set; }
        public virtual IList<Detail> Details { get; set; } = new ObservableCollection<Detail>();
    }

}