Imports Microsoft.VisualBasic
Imports DevExpress.ExpressApp
Imports System

Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Data.Filtering

Namespace E1554.Module
	Public Class Updater
		Inherits ModuleUpdater
		Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
			MyBase.New(objectSpace, currentDBVersion)
		End Sub
		Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
			MyBase.UpdateDatabaseAfterUpdateSchema()
			CreateMaster("Master 1")
			CreateMaster("Master 2")
			CreateMaster("Master 3")
			CreateDetail("Detail 1")
			CreateDetail("Detail 2")
			CreateDetail("Detail 3")
		End Sub
		Private Sub CreateMaster(ByVal name As String)
			Dim master As Master = ObjectSpace.FindObject(Of Master)(New BinaryOperator("MasterName", name))
			If master Is Nothing Then
				master = ObjectSpace.CreateObject(Of Master)()
				master.MasterName = name
			End If
		End Sub
		Private Sub CreateDetail(ByVal name As String)
			Dim detail As Detail = ObjectSpace.FindObject(Of Detail)(New BinaryOperator("DetailName", name))
			If detail Is Nothing Then
				detail = ObjectSpace.CreateObject(Of Detail)()
				detail.DetailName = name
			End If
		End Sub
	End Class
End Namespace
