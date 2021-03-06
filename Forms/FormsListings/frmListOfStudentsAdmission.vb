﻿
Imports MySql.Data.MySqlClient

Public Class frmListOfStudentsAdmission

#Region "Database"

    Public Sub GatStudentsAdmittedOnCurrentTermYear()
        Dim dtaTable As New DataTable

        dtaTable = GetStudentsAdmissionByTermYear(ConnectionString, "GatStudentsAdmittedOnCurrentTermYear", 4, CurrentTerm, CurrentSchoolYear)
        Me.dvgList.DataSource = dtaTable
        ArrangeDatagridView(Me.dvgList)

    End Sub

    Public Sub GatStudentsAdmittedAll()
        Dim dtaTable As New DataTable

        dtaTable = GetStudentsAdmissionByTermYear(ConnectionString, "GatStudentsAdmittedAll", 4, CurrentTerm, CurrentSchoolYear)
        Me.dvgList.DataSource = dtaTable
        ArrangeDatagridView(Me.dvgList)

    End Sub

    Public Sub GatStudentsByLastName(ByVal strSearch As String, ByVal strSFor As String)
        Dim dtaTable As New DataTable

        dtaTable = GetDataAndReturnDataSetSearch(ConnectionString, "GatStudentsByLastName", 4, strSearch & "%")
        Me.dvgList.DataSource = dtaTable
        ArrangeDatagridView(Me.dvgList)

    End Sub

#End Region

#Region "Helper"

    Private Sub ArrangeDatagridView(ByVal TheGridView As DataGridView)
        Try
            With TheGridView
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                .Columns(0).Visible = False
                .Columns(1).Visible = False
                .Columns(2).Visible = False
                .Columns(11).Visible = False
                .Columns(12).Visible = False
                .Columns(13).Visible = False

                .Columns(3).HeaderText = "EDP Number"
                .Columns(4).HeaderText = "Course"
                .Columns(5).HeaderText = "Name"
                .Columns(6).HeaderText = "Civil Status"
                .Columns(7).HeaderText = "Sex"
                .Columns(8).HeaderText = "Date of Birth"
                .Columns(9).HeaderText = "Home Address"
                .Columns(10).HeaderText = "Present Address"

                .Columns(3).Width = "100"
                .Columns(4).Width = "100"
                .Columns(5).Width = "255"
                .Columns(6).Width = "100"
                .Columns(8).Width = "100"
                .Columns(9).Width = "200"
                .Columns(10).Width = "200"

                .Columns(8).DefaultCellStyle.Format = "MMM/dd/yyyy"
                '.Columns(5).DefaultCellStyle.Format = "MMM/dd/yyyy"
                .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                '.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                '.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

            End With
        Catch ex As Exception

        End Try
       
    End Sub

#End Region

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub frmListOfDepartments_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.ToolStripStatusLabel1.Text = SetWindowHeader(Me, "List of Admissions - " & CurrentTerm & ", " & CurrentSchoolYear)
        'Me.Text = "List of Departments"
        Me.cmdRefresh.PerformClick()

    End Sub

    Private Sub cmdAddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddNew.Click
        Dim newfrm As New frmAddEditAdmission

        With newfrm
            .IsAdding = True
            .FormHeading = "Add New Student"
            .ShowDialog()
            If .MustUpdate = True Then
                Me.cmdRefresh.PerformClick()
                Me.txtSearch.Text = .txtLastName.Text & ", " & .txtFirstName.Text
            End If
        End With
    End Sub

    Private Sub cmdRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRefresh.Click
        GatStudentsAdmittedOnCurrentTermYear()
    End Sub

    Private Sub cmdModify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdModify.Click
        Dim newfrm As New frmAddEditAdmission

        With newfrm
            .IsAdding = False
            .FormHeading = "Modify Student"
            .CurrentStudeID = Me.dvgList.CurrentRow.Cells(0).Value
            If IsDBNull(Me.dvgList.CurrentRow.Cells(2).Value) = True Then
                .CurrentCOurseID = -1
            Else
                .CurrentCOurseID = Me.dvgList.CurrentRow.Cells(2).Value
            End If

            .txtEDPNumber.Text = Me.dvgList.CurrentRow.Cells(3).Value.ToString
            .txtLastName.Text = Me.dvgList.CurrentRow.Cells(11).Value.ToString
            .txtFirstName.Text = Me.dvgList.CurrentRow.Cells(12).Value.ToString
            .txtMiddle.Text = Me.dvgList.CurrentRow.Cells(13).Value.ToString
            .cboCivilStatus.Text = Me.dvgList.CurrentRow.Cells(6).Value.ToString
            .cboGender.Text = Me.dvgList.CurrentRow.Cells(7).Value.ToString
            .dtpBirth.Value = Me.dvgList.CurrentRow.Cells(8).Value.ToString
            .txtPAddress.Text = Me.dvgList.CurrentRow.Cells(10).Value.ToString
            .txtHAdress.Text = Me.dvgList.CurrentRow.Cells(9).Value.ToString
            .txtContactPersonName.Text = Me.dvgList.CurrentRow.Cells(15).Value.ToString
            .txtContactPersonNumber.Text = Me.dvgList.CurrentRow.Cells(16).Value.ToString
            .ShowDialog()
            If .MustUpdate = True Then
                Me.cmdRefresh.PerformClick()
                Me.txtSearch.Text = .txtLastName.Text & ", " & .txtFirstName.Text
            End If
        End With
    End Sub

    Private Sub txtSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.Click

    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                GatStudentsByLastName(Me.txtSearch.Text, "Last Name")
        End Select
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged

        Dim i As Integer = 0

        With Me
            For i = 0 To .dvgList.Rows.Count - 1
                If .dvgList.Rows(i).Cells(5).Value.ToString.StartsWith(txtSearch.Text, StringComparison.InvariantCultureIgnoreCase) Then
                    .dvgList.Rows(i).Selected = True
                    .dvgList.CurrentCell = .dvgList(5, i)
                    .dvgList.FirstDisplayedScrollingRowIndex = .dvgList.CurrentCell.RowIndex
                    Exit Sub
                End If
            Next
        End With

    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Dim newfrm As New frmSearchStudent

        With newfrm
            .Show()
        End With
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click

    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        GatStudentsAdmittedAll()
    End Sub
End Class