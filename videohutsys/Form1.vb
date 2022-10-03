Imports Npgsql

Public Class Form1
    Dim lv As ListViewItem
    Dim selected As String
    Private Sub PopListview()
        ListView1.Clear()

        With ListView1
            .View = View.Details
            .GridLines = True
            .Columns.Add("ID", 40)
            .Columns.Add("Last Name", 110)
            .Columns.Add("First Name", 110)
            .Columns.Add("M.I.", 40)
            .Columns.Add("Address", 110)
            .Columns.Add("Date of Membership", 110)
        End With

        openCon()
        sql = "Select * from tblmembersinfo"
        cmd = New NpgsqlCommand(sql, cn)
        dr = cmd.ExecuteReader()

        Do While dr.Read() = True
            lv = New ListViewItem(dr("memberid").ToString)
            lv.SubItems.Add(dr("last").ToString)
            lv.SubItems.Add(dr("given").ToString)
            lv.SubItems.Add(dr("middle").ToString)
            lv.SubItems.Add(dr("address").ToString)
            lv.SubItems.Add(dr("dateofmembership").ToString())
            ListView1.Items.Add(lv)
        Loop
        cn.Close()

    End Sub

    '--------------------------------------------------------ADD------------------------------------------------------'

    Private Sub AddNew()

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to save", "Add", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            openCon()
            sql = "Insert into tblmembersinfo(memberid,last,given,middle, address, dateofmembership)" & "Values('" & (Me.txtid.Text) & "', '" & (Me.txtlastname.Text) & "','" & (Me.txtfirstname.Text) & "','" & (Me.txtmiddlename.Text) & "','" & (Me.txtaddress.Text) & "','" & (Me.txtmembersince.Text) & "')"
            cmd = New NpgsqlCommand(sql, cn)
            cmd.ExecuteNonQuery()
            cn.Close()
            MsgBox("Added Succesfully")
            ClearAllText()
        ElseIf result = DialogResult.No Then
            MsgBox("Data discard", MsgBoxStyle.OkOnly, "Add")
        End If

        PopListview()
    End Sub
    '---------------------------------------------------------Delete-----------------------------
    Private Sub delete()
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete", "Delete", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            cn.Open()
            sql = "Delete from tblmembersinfo where memberid = '" & txtid.Text & "'"
            cmd = New NpgsqlCommand(sql, cn)
            cmd.ExecuteNonQuery()
            cn.Close()
            MsgBox("Deleted Succesfully")
            ClearAllText()
        ElseIf result = DialogResult.No Then
            MsgBox("Delete Cancelled", MsgBoxStyle.OkOnly, "Delete")
        End If
        PopListview()

    End Sub
    '-----------------------------------------------Update---------------------------------------------'

    Private Sub Update()
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to Update", "Update", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            cn.Open()
            sql = "Update tblmembersinfo set last = '" & txtlastname.Text & "',given = '" & txtfirstname.Text & "',middle = '" & txtmiddlename.Text & "',address = '" & txtaddress.Text & "',dateofmembership = '" & txtmembersince.Text & "' where memberid ='" & txtid.Text & "'"
            cmd = New NpgsqlCommand(sql, cn)
            cmd.ExecuteNonQuery()
            cn.Close()
            MsgBox("Update Succesfully")
            ClearAllText()
        ElseIf result = DialogResult.No Then
            MsgBox("Update Cancelled", MsgBoxStyle.OkOnly, "Update")
        End If
        PopListview()

    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Dim i As Integer
        For i = 0 To ListView1.SelectedItems.Count - 1
            selected = ListView1.SelectedItems(i).Text
            openCon()
            sql = "select * from tblmembersinfo where memberid = '" & selected & "'"
            cmd = New NpgsqlCommand(sql, cn)
            dr = cmd.ExecuteReader

            dr.Read()
            Me.txtid.Text = dr("memberid").ToString
            Me.txtlastname.Text = dr("last").ToString
            Me.txtfirstname.Text = dr("given").ToString
            Me.txtmiddlename.Text = dr("middle").ToString
            Me.txtaddress.Text = dr("address").ToString
            Me.txtmembersince.Text = dr("dateofmembership").ToString

            cn.Close()
        Next
    End Sub

    Private Sub ClearAllText()
        txtid.Clear()
        txtlastname.Clear()
        txtfirstname.Clear()
        txtmiddlename.Clear()
        txtaddress.Clear()
        txtmembersince.Clear()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PopListview()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtid.Text = "" Or txtlastname.Text = "" Or txtfirstname.Text = "" Or txtlastname.Text = "" Or txtmiddlename.Text = "" Or txtmembersince.Text = "" Then
            MsgBox("Please input any following textboxes to proceed")
        Else
            AddNew()
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Update()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        delete()
    End Sub

End Class
