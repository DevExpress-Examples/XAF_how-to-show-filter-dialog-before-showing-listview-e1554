Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Editors
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Utils

Namespace E1554.Module
	<DefaultProperty("FilterName")> _
	Public Class ViewFilterObject
		Inherits BaseObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _ObjectType As Type
		<ValueConverter(GetType(TypeToStringConverter)), MemberDesignTimeVisibility(False)> _
		Public Property ObjectType() As Type
			Get
				Return _ObjectType
			End Get
			Set(ByVal value As Type)
				If SetPropertyValue("ObjectType", _ObjectType, value) AndAlso (Not IsLoading) Then
					Criteria = String.Empty
				End If
			End Set
		End Property
		Private _Criteria As String
		<CriteriaOptions("ObjectType")> _
		Public Property Criteria() As String
			Get
				Return _Criteria
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Criteria", _Criteria, value)
			End Set
		End Property
		Private _FilterName As String
		Public Property FilterName() As String
			Get
				Return _FilterName
			End Get
			Set(ByVal value As String)
				SetPropertyValue("FilterName", _FilterName, value)
			End Set
		End Property
	End Class
End Namespace
