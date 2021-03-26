Imports System
Imports System.Collections.Generic

Imports DevExpress.ExpressApp
Imports System.Reflection
Imports DevExpress.ExpressApp.Model
Imports System.ComponentModel


Namespace E1554.Module
	Public NotInheritable Partial Class E1554Dodule
		Inherits ModuleBase

		Public Sub New()
			InitializeComponent()
		End Sub
		Public Overrides Sub ExtendModelInterfaces(ByVal extenders As ModelInterfaceExtenders)
			MyBase.ExtendModelInterfaces(extenders)
			extenders.Add(Of IModelListView, IModelListViewAdditionalCriteria)()
		End Sub
	End Class
	Public Interface IModelListViewAdditionalCriteria
		Inherits IModelNode

		<DefaultValue("")>
		Property AdditionalCriteria() As String
	End Interface
End Namespace
