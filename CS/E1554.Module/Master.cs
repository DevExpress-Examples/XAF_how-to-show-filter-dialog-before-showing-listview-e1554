using System;

using DevExpress.Xpo;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Data.Filtering;

namespace E1554.Module {
    [DefaultClassOptions]
    public class Master : BaseObject {
        public Master(Session session) : base(session) { }
        private string _MasterName;
        public string MasterName {
            get { return _MasterName; }
            set { SetPropertyValue("MasterName", ref _MasterName, value); }
        }
        [Association("Master-Details")]
        public XPCollection<Detail> Details {
            get {
                return GetCollection<Detail>("Details");
            }
        }
    }

}