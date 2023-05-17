using System;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Editors;
using System.ComponentModel;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace E1554.Module {
    [DefaultProperty("FilterName")]
    public class ViewFilterObject : BaseObject {

        private Type fDataType;
        [ImmediatePostData]
        public virtual Type ObjectType {
            get { return fDataType; }
            set {
                if (fDataType == value) return;
                fDataType = value;
                Criteria = string.Empty;
                
            }
        }

        [CriteriaOptions("ObjectType")]
        public virtual string Criteria { get; set; }

        public virtual string FilterName { get; set; }
    }
}
