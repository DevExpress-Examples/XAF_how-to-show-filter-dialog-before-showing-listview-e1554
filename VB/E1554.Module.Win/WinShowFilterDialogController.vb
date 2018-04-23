Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Win
Imports DevExpress.ExpressApp.Win.Templates

Namespace E1554.Module.Win
	Public Class WinShowFilterDialogController
		Inherits ShowFilterDialogController
		Protected Overrides Function GetTargetView() As ListView
			Dim targetView As ListView = MyBase.GetTargetView()
			If TypeOf Application.ShowViewStrategy Is MyMdiShowViewStrategy Then
				Dim existingView As ListView = TryCast((CType(Application.ShowViewStrategy, MyMdiShowViewStrategy)).FindExistingView(targetView), ListView)
				Return If(existingView, targetView)
			Else
				Return targetView
			End If
		End Function
	End Class
	Public Class MyMdiShowViewStrategy
		Inherits MdiShowViewStrategy
		Public Sub New(ByVal application As XafApplication, ByVal mdiMode As MdiMode)
			MyBase.New(application, mdiMode)
		End Sub
		Public Function FindExistingView(ByVal view As View) As View
			Dim window As WinWindow = FindWindowByView(view)
			Return If(window Is Nothing, Nothing, window.View)
		End Function
	End Class
End Namespace
