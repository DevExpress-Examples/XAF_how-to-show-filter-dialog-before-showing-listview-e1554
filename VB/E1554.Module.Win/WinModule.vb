Imports System
Imports System.ComponentModel

Imports DevExpress.ExpressApp
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.ExpressApp.Win.SystemModule

Namespace E1554.Module.Win
	<ToolboxItemFilter("Xaf.Platform.Win")>
	Public NotInheritable Partial Class E1554WindowsFormsModule
		Inherits ModuleBase

		Public Sub New()
			InitializeComponent()
		End Sub
		Private Sub Application_CreateCustomModelDifferenceStore(ByVal sender As Object, ByVal e As CreateCustomModelDifferenceStoreEventArgs)
			e.Store = New ModelDifferenceDbStore(DirectCast(sender, XafApplication), GetType(ModelDifference), True)
			e.Handled = True
		End Sub
		Private Sub Application_CreateCustomUserModelDifferenceStore(ByVal sender As Object, ByVal e As CreateCustomModelDifferenceStoreEventArgs)
			e.Store = New ModelDifferenceDbStore(DirectCast(sender, XafApplication), GetType(ModelDifference), False)
			e.Handled = True
		End Sub
		Public Overrides Sub Setup(ByVal application As XafApplication)
			MyBase.Setup(application)
			AddHandler application.ModelChanged, AddressOf application_ModelChanged
			AddHandler application.CreateCustomModelDifferenceStore, AddressOf Application_CreateCustomModelDifferenceStore
			AddHandler application.CreateCustomUserModelDifferenceStore, AddressOf Application_CreateCustomUserModelDifferenceStore
		End Sub

		Private Sub application_ModelChanged(ByVal sender As Object, ByVal e As EventArgs)
			Dim uiType As UIType = DirectCast(Application.Model.Options, IModelOptionsWin).UIType
			If uiType = UIType.StandardMDI Then
				Application.ShowViewStrategy = New MyMdiShowViewStrategy(Application, DevExpress.ExpressApp.Win.Templates.MdiMode.Standard)
			ElseIf uiType = UIType.TabbedMDI Then
				Application.ShowViewStrategy = New MyMdiShowViewStrategy(Application, DevExpress.ExpressApp.Win.Templates.MdiMode.Tabbed)
			End If
		End Sub
	End Class
End Namespace
