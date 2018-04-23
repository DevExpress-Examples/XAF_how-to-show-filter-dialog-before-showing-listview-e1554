using System;
using DevExpress.ExpressApp.Editors;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;

namespace E1554.Module {
    [DomainComponent]
    public class ViewFilterContainer : IObjectSpaceLink {
        private ViewFilterObject _Filter;
        [DataSourceProperty("Filters")]
        [ImmediatePostData]
        public ViewFilterObject Filter {
            get { return _Filter; }
            set { _Filter = value; }
        }
        private IList<ViewFilterObject> _Filters;
        [Browsable(false)]
        public IList<ViewFilterObject> Filters {
            get {
                if (_Filters == null && ObjectType != null) {
                    _Filters = _ObjectSpace.GetObjects<ViewFilterObject>(new BinaryOperator("ObjectType", ObjectType));
                }
                return _Filters;
            }
        }
        [CriteriaOptions("ObjectType")]
        [ImmediatePostData]
        public string Criteria {
            get { return Filter != null ? Filter.Criteria : String.Empty; }
            set {
                if (Filter != null) {
                    Filter.Criteria = value;
                }
            }
        }
        private Type _ObjectType;
        [Browsable(false)]
        public Type ObjectType {
            get { return _ObjectType; }
            set { _ObjectType = value; }
        }
        private IObjectSpace _ObjectSpace;
        IObjectSpace IObjectSpaceLink.ObjectSpace {
            get {
                return _ObjectSpace;
            }
            set {
                _ObjectSpace = value;
            }
        }
    }
}
