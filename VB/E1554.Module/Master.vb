Imports System

Imports DevExpress.Xpo

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Persistent.Validation
Imports DevExpress.Data.Filtering

Namespace E1554.Module
	<DefaultClassOptions>
	Public Class Master
		Inherits BaseObject

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub
		Private _MasterName As String
		Public Property MasterName() As String
			Get
				Return _MasterName
			End Get
			Set(ByVal value As String)
				SetPropertyValue("MasterName", _MasterName, value)
			End Set
		End Property
		<Association("Master-Details")>
		Public ReadOnly Property Details() As XPCollection(Of Detail)
			Get
				Return GetCollection(Of Detail)("Details")
			End Get
		End Property
	End Class

End Namespace