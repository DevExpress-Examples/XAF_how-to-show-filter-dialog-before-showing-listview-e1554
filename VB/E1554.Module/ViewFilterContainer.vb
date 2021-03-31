Imports System
Imports DevExpress.ExpressApp.Editors
Imports System.ComponentModel
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.DC
Imports System.Collections.Generic

Namespace E1554.Module
	<DomainComponent>
	Public Class ViewFilterContainer
		Implements IObjectSpaceLink

		Private _Filter As ViewFilterObject
		<DataSourceProperty("Filters")>
		<ImmediatePostData>
		Public Property Filter() As ViewFilterObject
			Get
				Return _Filter
			End Get
			Set(ByVal value As ViewFilterObject)
				_Filter = value
			End Set
		End Property
		Private _Filters As IList(Of ViewFilterObject)
		<Browsable(False)>
		Public ReadOnly Property Filters() As IList(Of ViewFilterObject)
			Get
				If _Filters Is Nothing AndAlso ObjectType IsNot Nothing Then
					_Filters = _ObjectSpace.GetObjects(Of ViewFilterObject)(New BinaryOperator("ObjectType", ObjectType))
				End If
				Return _Filters
			End Get
		End Property
		<CriteriaOptions("ObjectType")>
		<ImmediatePostData>
		Public Property Criteria() As String
			Get
				Return If(Filter IsNot Nothing, Filter.Criteria, String.Empty)
			End Get
			Set(ByVal value As String)
				If Filter IsNot Nothing Then
					Filter.Criteria = value
				End If
			End Set
		End Property
		Private _ObjectType As Type
		<Browsable(False)>
		Public Property ObjectType() As Type
			Get
				Return _ObjectType
			End Get
			Set(ByVal value As Type)
				_ObjectType = value
			End Set
		End Property
		Private _ObjectSpace As IObjectSpace
		Private Property IObjectSpaceLink_ObjectSpace() As IObjectSpace Implements IObjectSpaceLink.ObjectSpace
			Get
				Return _ObjectSpace
			End Get
			Set(ByVal value As IObjectSpace)
				_ObjectSpace = value
			End Set
		End Property
	End Class
End Namespace
