Public Class clsChooseFromList_Brand
    Inherits clsBase

#Region "Declarations"
    Public Shared ItemUID As String
    Public Shared SourceFormUID As String
    Public Shared SourceLabel As Integer
    Public Shared CFLChoice As String
    Public Shared ItemCode As String
    Public Shared sourceItemCode As String
    Public Shared choice As String
    Public Shared sourceColumID As String
    Public Shared sourcerowId As Integer
    Public Shared BinDescrUID As String
    Public Shared Documentchoice As String

    Private oDbDatasource As SAPbouiCOM.DBDataSource
    Private Ouserdatasource As SAPbouiCOM.UserDataSource
    Private oConditions As SAPbouiCOM.Conditions
    Private ocondition As SAPbouiCOM.Condition
    Private intRowId As Integer
    Private strRowNum As Integer
    Private i As Integer
    Private oedit As SAPbouiCOM.EditText
    '   Private oForm As SAPbouiCOM.Form
    Private objSoureceForm As SAPbouiCOM.Form
    Private objform As SAPbouiCOM.Form
    Private oMatrix As SAPbouiCOM.Grid
    Private osourcegrid As SAPbouiCOM.Matrix
    Private Const SEPRATOR As String = "~~~"
    Private SelectedRow As Integer
    Private sSearchColumn As String
    Private oItem As SAPbouiCOM.Item
    Public stritemid As SAPbouiCOM.Item
    Private intformmode As SAPbouiCOM.BoFormMode
    Private objGrid As SAPbouiCOM.Grid
    Private objSourcematrix As SAPbouiCOM.Matrix
    Private dtTemp As SAPbouiCOM.DataTable
    Private objStatic As SAPbouiCOM.StaticText
    Private inttable As Integer = 0
    Public strformid As String
    Public strStaticValue As String
    Public strSQL As String
    Private strSelectedItem1 As String
    Private strSelectedItem2 As String
    Private strSelectedItem3 As String
    Private strSelectedItem4, strSelectedItem5, strSelectedItem6 As String
    Private oRecSet As SAPbobsCOM.Recordset
    '   Private objSBOAPI As ClsSBO
    '   Dim objTransfer As clsTransfer
#End Region

#Region "New"
    '*****************************************************************
    'Type               : Constructor
    'Name               : New
    'Parameter          : 
    'Return Value       : 
    'Author             : DEV-2
    'Created Date       : 
    'Last Modified By   : 
    'Modified Date      : 
    'Purpose            : Create object for classes.
    '******************************************************************
    Public Sub New()
        '   objSBOAPI = New ClsSBO
        MyBase.New()
    End Sub
#End Region

#Region "Bind Data"
    '******************************************************************
    'Type               : Procedure
    'Name               : BindData
    'Parameter          : Form
    'Return Value       : 
    'Author             : DEV-2
    'Created Date       : 
    'Last Modified By   : 
    'Modified Date      : 
    'Purpose            : Binding the fields.
    '******************************************************************
    Public Sub databound(ByVal objform As SAPbouiCOM.Form)
        Try
            Dim strSQL As String = ""
            Dim ObjSegRecSet, oRecS As SAPbobsCOM.Recordset
            ObjSegRecSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            objform.Freeze(True)
            objform.DataSources.DataTables.Add("dtLevel3")
            Ouserdatasource = objform.DataSources.UserDataSources.Add("dbFind", SAPbouiCOM.BoDataType.dt_LONG_TEXT, 250)
            oedit = objform.Items.Item("etFind").Specific
            oedit.DataBind.SetBound(True, "", "dbFind")
            objGrid = objform.Items.Item("mtchoose").Specific
            dtTemp = objform.DataSources.DataTables.Item("dtLevel3")
            oRecS = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            'If blnIsHana = True Then
            '    oRecS.DoQuery("Select ifnull(""U_S_PTerms"",'') from OHEM where ""empID""=" & ItemCode)
            'Else
            '    oRecS.DoQuery("Select isnull(U_S_PTerms,'') from OHEM where ""empID""=" & ItemCode)

            'End If
            If choice = "Season" Then
                strSQL = "Select ""U_SEASON"",""U_SEASON"" ""U_SEASON1"" from ""@SEI_SEASONL"" order by ""U_SEASON"""
                objform.Title = "Season - Selection"
                objGrid.DataTable = dtTemp
                dtTemp.ExecuteQuery(strSQL)

                objGrid.Columns.Item(0).TitleObject.Caption = "Code"
                objGrid.Columns.Item(1).Visible = False '.TitleObject.Caption = "Code"
                objform.Items.Item("6").Visible = False
                objform.Items.Item("7").Visible = False
            ElseIf choice = "Brand" Then
                strSQL = "Update ""@SEI_BRANDL"" set ""U_Select""='Y'"
                oRecS.DoQuery(strSQL)
                '   strSQL = "Select ""U_BRAND"",""U_BRAND"" ""U_BRAND1"", 'Y' as ""Select"" from ""@SEI_BRANDL"" order by ""U_BRAND"""
                strSQL = "Select ""U_BRAND"",""U_BRAND"" ""U_BRAND1"", ""U_Select"" as ""Select"" from ""@SEI_BRANDL"" order by ""U_BRAND"""
                objform.Title = "Brand - Selection"
                objGrid.DataTable = dtTemp
                dtTemp.ExecuteQuery(strSQL)
                objform.Items.Item("mtchoose").Enabled = True

                objGrid.Columns.Item(0).TitleObject.Caption = "Code"
                objGrid.Columns.Item(0).Editable = False
                objGrid.Columns.Item(1).Visible = False '.TitleObject.Caption = "Code"
                objGrid.Columns.Item("Select").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox
                objGrid.Columns.Item("Select").Editable = True
                objform.Items.Item("6").Visible = True
                objform.Items.Item("7").Visible = True

            End If
           
            objGrid.AutoResizeColumns()
            objGrid.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single
            If objGrid.Rows.Count > 0 Then
                objGrid.Rows.SelectedRows.Add(0)
            End If
            objform.Freeze(False)
            objform.Update()
            'sSearchList = " "
            'Dim i As Integer = 0
            'While i <= objGrid.DataTable.Rows.Count - 1
            '    sSearchList += Convert.ToString(objGrid.DataTable.GetValue(0, i)) + SEPRATOR + i.ToString + " "
            '    System.Math.Min(System.Threading.Interlocked.Increment(i), i - 1)
            'End While
        Catch ex As Exception
            oApplication.SBO_Application.MessageBox(ex.Message)
            oApplication.SBO_Application.MessageBox(ex.Message)
        Finally
        End Try
    End Sub
#End Region

#Region "Update On hand Qty"
    Private Sub FillOnhandqty(ByVal strItemcode As String, ByVal strwhs As String, ByVal aGrid As SAPbouiCOM.Grid)
        Dim oTemprec As SAPbobsCOM.Recordset
        oTemprec = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Dim strBin, strSql As String
        For intRow As Integer = 0 To aGrid.DataTable.Rows.Count - 1
            strBin = aGrid.DataTable.GetValue(0, intRow)
            strSql = "Select isnull(Sum(U_InQty)-sum(U_OutQty),0) from ""@DABT_BTRN"" where U_Itemcode='" & strItemcode & "' and U_FrmWhs='" & strwhs & "' and U_BinCode='" & strBin & "'"
            oTemprec.DoQuery(strSql)
            Dim dblOnhand As Double
            dblOnhand = oTemprec.Fields.Item(0).Value

            aGrid.DataTable.SetValue(2, intRow, dblOnhand.ToString)
        Next
    End Sub
#End Region

#Region "Get Form"
    '******************************************************************
    'Type               : Function
    'Name               : GetForm
    'Parameter          : FormUID
    'Return Value       : 
    'Author             : DEV-2
    'Created Date       : 
    'Last Modified By   : 
    'Modified Date      : 
    'Purpose            : Get The Form
    '******************************************************************
    Public Function GetForm(ByVal FormUID As String) As SAPbouiCOM.Form
        Return oApplication.SBO_Application.Forms.Item(FormUID)
    End Function
#End Region

#Region "FormDataEvent"


#End Region

#Region "Class Menu Event"
    Public Overrides Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)

    End Sub
#End Region

#Region "getBOQReference"
    Private Function getBOQReference(ByVal aItemCode As String, ByVal aProject As String, ByVal aProcess As String, ByVal aActivity As String) As String
        Dim oTest As SAPbobsCOM.Recordset
        oTest = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oTest.DoQuery("Select isnull(U_S_PBOQREF,'') from ""@S_PPRJ2"" where U_S_PItemCode='" & aItemCode & "' and  U_S_PPRJCODE='" & aProject.Replace("'", "''") & "' and U_S_PMODNAME='" & aProcess.Replace("'", "''") & "' and U_S_PACTNAME='" & aActivity.Replace("'", "''") & "'")
        Return oTest.Fields.Item(0).Value
    End Function
#End Region
    Private Sub SelectAll(aForm As SAPbouiCOM.Form, aflag As Boolean)
        Try
            oApplication.Utilities.Message("Processing....", SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            aForm.Freeze(True)
            Dim oCheckbox As SAPbouiCOM.CheckBoxColumn
            objGrid = aForm.Items.Item("mtchoose").Specific
            Dim oRecS As SAPbobsCOM.Recordset

            oRecS = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            If aflag = True Then
                strSQL = "Update ""@SEI_BRANDL"" set ""U_Select""='Y'"
                oRecS.DoQuery(strSQL)
            Else
                strSQL = "Update ""@SEI_BRANDL"" set ""U_Select""='N'"
                oRecS.DoQuery(strSQL)
            End If
            'For intRow As Integer = 0 To objGrid.DataTable.Rows.Count - 1
            '    oCheckbox = objGrid.Columns.Item("Select")
            '    oCheckbox.Check(intRow, aflag)
            'Next
            objform = aForm

            objGrid = objform.Items.Item("mtchoose").Specific
            dtTemp = objform.DataSources.DataTables.Item("dtLevel3")
            strSQL = "Select ""U_BRAND"",""U_BRAND"" ""U_BRAND1"", ""U_Select"" as ""Select"" from ""@SEI_BRANDL"" order by ""U_BRAND"""
            objform.Title = "Brand - Selection"
            objGrid.DataTable = dtTemp
            dtTemp.ExecuteQuery(strSQL)
            objform.Items.Item("mtchoose").Enabled = True
            objGrid.Columns.Item(0).TitleObject.Caption = "Code"
            objGrid.Columns.Item(0).Editable = False
            objGrid.Columns.Item(1).Visible = False '.TitleObject.Caption = "Code"
            objGrid.Columns.Item("Select").Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox
            objGrid.Columns.Item("Select").Editable = True
            objform.Items.Item("6").Visible = True
            objform.Items.Item("7").Visible = True
            aForm.Freeze(False)
            oApplication.Utilities.Message("Completed....", SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Catch ex As Exception
            aForm.Freeze(False)
            oApplication.Utilities.Message("Completed....", SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try

    End Sub


#Region "Item Event"

    Public Overrides Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        BubbleEvent = True
        If pVal.FormTypeEx = frm_ChoosefromList_Leave Then
            If pVal.Before_Action = True Then
                If pVal.ItemUID = "mtchoose" Then
                    Try
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK And pVal.Row <> -1 Then
                            oForm = GetForm(pVal.FormUID)
                            oItem = CType(oForm.Items.Item(pVal.ItemUID), SAPbouiCOM.Item)
                            oMatrix = CType(oItem.Specific, SAPbouiCOM.Grid)
                            oMatrix.Rows.SelectedRows.Add(pVal.Row)
                        End If
                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK And pVal.Row <> -1 Then
                            oForm = GetForm(pVal.FormUID)
                            oItem = oForm.Items.Item("mtchoose")
                            oMatrix = CType(oItem.Specific, SAPbouiCOM.Grid)
                            Dim inti As Integer
                            inti = 0
                            inti = 0
                            strSelectedItem1 = ""
                            strSelectedItem2 = ""
                            If choice = "Brand" Then
                                'While inti <= oMatrix.DataTable.Rows.Count - 1
                                '    Dim ocheckbox As SAPbouiCOM.CheckBoxColumn
                                '    ocheckbox = oMatrix.Columns.Item("Select")
                                '    If ocheckbox.IsChecked(inti) = True Then
                                '        ' intRowId = inti
                                '        If strSelectedItem1 = "" Then
                                '            strSelectedItem1 = "'" & oMatrix.DataTable.GetValue(0, inti) & "'"
                                '            strSelectedItem2 = oMatrix.DataTable.GetValue(0, inti)
                                '        Else
                                '            strSelectedItem1 = strSelectedItem1 & ",'" & oMatrix.DataTable.GetValue(0, inti) & "'"
                                '            strSelectedItem2 = strSelectedItem2 & "," & oMatrix.DataTable.GetValue(0, inti)
                                '        End If
                                '    End If
                                '    System.Math.Min(System.Threading.Interlocked.Increment(inti), inti - 1)
                                'End While
                                Dim oTes As SAPbobsCOM.Recordset
                                oTes = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oTes.DoQuery("Select ""U_BRAND"" from ""@SEI_BRANDL"" where ""U_Select""='Y'")
                                For intLo As Integer = 0 To oTes.RecordCount - 1
                                    If strSelectedItem1 = "" Then
                                        strSelectedItem1 = "'" & oTes.Fields.Item(0).Value.ToString.Replace("'", "''") & "'"
                                        strSelectedItem2 = oTes.Fields.Item(0).Value.ToString.Replace("'", "''")
                                    Else
                                        strSelectedItem1 = strSelectedItem1 & ",'" & oTes.Fields.Item(0).Value.ToString.Replace("'", "''") & "'"
                                        strSelectedItem2 = strSelectedItem2 & "," & oTes.Fields.Item(0).Value.ToString.Replace("'", "''")
                                    End If
                                    oTes.MoveNext()
                                Next
                                oForm.Close()
                                oForm = GetForm(SourceFormUID)
                                If choice <> "Ticket" Then
                                    oForm.Freeze(True)
                                    oedit = oForm.Items.Item(ItemUID).Specific
                                    oedit.String = strSelectedItem2
                                    oedit = oForm.Items.Item(sourceColumID).Specific
                                    oedit.String = strSelectedItem1
                                    oForm.Freeze(False)
                                Else

                                    oForm.Freeze(True)
                                    oForm.Freeze(False)
                                End If
                            Else
                                While inti <= oMatrix.DataTable.Rows.Count - 1
                                    If oMatrix.Rows.IsSelected(inti) = True Then
                                        intRowId = inti
                                    End If
                                    System.Math.Min(System.Threading.Interlocked.Increment(inti), inti - 1)
                                End While
                                If CFLChoice <> "" Then
                                    If intRowId = 0 Then
                                        strSelectedItem1 = Convert.ToString(oMatrix.DataTable.GetValue(0, intRowId))
                                        strSelectedItem2 = Convert.ToString(oMatrix.DataTable.GetValue(1, intRowId))
                                    Else
                                        strSelectedItem1 = Convert.ToString(oMatrix.DataTable.GetValue(0, intRowId))
                                        strSelectedItem2 = Convert.ToString(oMatrix.DataTable.GetValue(1, intRowId))
                                    End If
                                    If CFLChoice = "E" Then
                                        strSelectedItem3 = Convert.ToString(oMatrix.DataTable.GetValue(3, intRowId))
                                    End If
                                    If choice = "Ticket" Then
                                        strSelectedItem2 = Convert.ToString(oMatrix.DataTable.GetValue(1, intRowId))
                                        strSelectedItem3 = Convert.ToString(oMatrix.DataTable.GetValue(2, intRowId))
                                        strSelectedItem4 = Convert.ToString(oMatrix.DataTable.GetValue(3, intRowId))
                                    End If
                                End If
                                oForm.Close()
                                oForm = GetForm(SourceFormUID)
                                If choice <> "Ticket" Then
                                    oForm.Freeze(True)
                                    oedit = oForm.Items.Item(ItemUID).Specific
                                    oedit.String = strSelectedItem1
                                    oForm.Freeze(False)
                                Else

                                    oForm.Freeze(True)
                                    oForm.Freeze(False)
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        oApplication.SBO_Application.MessageBox(ex.Message)

                    End Try
                End If


                If pVal.EventType = SAPbouiCOM.BoEventTypes.et_KEY_DOWN Then
                    Try
                        If pVal.ItemUID = "mtchoose" Then
                            oForm = GetForm(pVal.FormUID)
                            oItem = CType(oForm.Items.Item("mtchoose"), SAPbouiCOM.Item)
                            oMatrix = CType(oItem.Specific, SAPbouiCOM.Grid)
                            intRowId = pVal.Row - 1
                        End If
                        Dim inti As Integer
                        If pVal.CharPressed = 13 Then
                            inti = 0
                            inti = 0
                            oForm = GetForm(pVal.FormUID)
                            oItem = CType(oForm.Items.Item("mtchoose"), SAPbouiCOM.Item)

                            oMatrix = CType(oItem.Specific, SAPbouiCOM.Grid)
                            If choice = "Brand" Then
                                'While inti <= oMatrix.DataTable.Rows.Count - 1
                                '    Dim ocheckbox As SAPbouiCOM.CheckBoxColumn
                                '    ocheckbox = oMatrix.Columns.Item("Select")
                                '    If ocheckbox.IsChecked(inti) = True Then
                                '        ' intRowId = inti
                                '        If strSelectedItem1 = "" Then
                                '            strSelectedItem1 = "'" & oMatrix.DataTable.GetValue(0, inti) & "'"
                                '            strSelectedItem2 = oMatrix.DataTable.GetValue(0, inti)
                                '        Else
                                '            strSelectedItem1 = strSelectedItem1 & ",'" & oMatrix.DataTable.GetValue(0, inti) & "'"
                                '            strSelectedItem2 = strSelectedItem2 & "," & oMatrix.DataTable.GetValue(0, inti)
                                '        End If
                                '    End If
                                '    System.Math.Min(System.Threading.Interlocked.Increment(inti), inti - 1)
                                'End While
                                Dim oTes As SAPbobsCOM.Recordset
                                oTes = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oTes.DoQuery("Select ""U_BRAND"" from ""@SEI_BRANDL"" where ""U_Select""='Y'")
                                For intLo As Integer = 0 To oTes.RecordCount - 1
                                    If strSelectedItem1 = "" Then
                                        strSelectedItem1 = "'" & oTes.Fields.Item(0).Value & "'"
                                        strSelectedItem2 = oTes.Fields.Item(0).Value
                                    Else
                                        strSelectedItem1 = strSelectedItem1 & ",'" & oTes.Fields.Item(0).Value.ToString.Replace("'", "''") & "'"
                                        strSelectedItem2 = strSelectedItem2 & "," & oTes.Fields.Item(0).Value.ToString.Replace("'", "''")
                                    End If
                                    oTes.MoveNext()
                                Next
                                oForm.Close()
                                oForm = GetForm(SourceFormUID)
                                If choice <> "Ticket" Then
                                    oForm.Freeze(True)
                                    oedit = oForm.Items.Item(ItemUID).Specific
                                    oedit.String = strSelectedItem2
                                    oedit = oForm.Items.Item(sourceColumID).Specific
                                    oedit.String = strSelectedItem1
                                    oForm.Freeze(False)
                                Else

                                    oForm.Freeze(True)
                                    oForm.Freeze(False)
                                End If
                            Else
                                While inti <= oMatrix.DataTable.Rows.Count - 1
                                    If oMatrix.Rows.IsSelected(inti) = True Then
                                        intRowId = inti
                                    End If
                                    System.Math.Min(System.Threading.Interlocked.Increment(inti), inti - 1)
                                End While
                                If CFLChoice <> "" Then
                                    If intRowId = 0 Then
                                        strSelectedItem1 = Convert.ToString(oMatrix.DataTable.GetValue(0, intRowId))
                                        strSelectedItem2 = Convert.ToString(oMatrix.DataTable.GetValue(1, intRowId))
                                    Else
                                        strSelectedItem1 = Convert.ToString(oMatrix.DataTable.GetValue(0, intRowId))
                                        strSelectedItem2 = Convert.ToString(oMatrix.DataTable.GetValue(1, intRowId))
                                    End If
                                    If CFLChoice = "E" Then
                                        strSelectedItem3 = Convert.ToString(oMatrix.DataTable.GetValue(3, intRowId))

                                    End If
                                    If choice = "Ticket" Then
                                        strSelectedItem2 = Convert.ToString(oMatrix.DataTable.GetValue(1, intRowId))
                                        strSelectedItem3 = Convert.ToString(oMatrix.DataTable.GetValue(2, intRowId))
                                        strSelectedItem4 = Convert.ToString(oMatrix.DataTable.GetValue(3, intRowId))
                                    End If
                                    oForm.Close()
                                    oForm = GetForm(SourceFormUID)
                                    If choice <> "Ticket" Then
                                        oedit = oForm.Items.Item(ItemUID).Specific
                                        oedit.String = strSelectedItem1

                                        oForm.Freeze(False)
                                    Else
                                        oForm.Freeze(True)
                                        oForm.Freeze(False)
                                    End If
                                End If
                            End If

                        End If
                    Catch ex As Exception
                        oApplication.SBO_Application.MessageBox(ex.Message)
                    End Try
                End If


                If pVal.ItemUID = "btnChoose" AndAlso pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                    oForm = GetForm(pVal.FormUID)
                    oItem = oForm.Items.Item("mtchoose")
                    oApplication.Utilities.Message("Processing....", SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    oMatrix = CType(oItem.Specific, SAPbouiCOM.Grid)
                    Dim inti As Integer
                    inti = 0
                    inti = 0
                    If choice = "Brand" Then
                        'While inti <= oMatrix.DataTable.Rows.Count - 1
                        '    Dim ocheckbox As SAPbouiCOM.CheckBoxColumn
                        '    ocheckbox = oMatrix.Columns.Item("Select")
                        '    If ocheckbox.IsChecked(inti) = True Then
                        '        ' intRowId = inti
                        '        If strSelectedItem1 = "" Then
                        '            strSelectedItem1 = "'" & oMatrix.DataTable.GetValue(0, inti) & "'"
                        '            strSelectedItem2 = oMatrix.DataTable.GetValue(0, inti)
                        '        Else
                        '            strSelectedItem1 = strSelectedItem1 & ",'" & oMatrix.DataTable.GetValue(0, inti) & "'"
                        '            strSelectedItem2 = strSelectedItem2 & "," & oMatrix.DataTable.GetValue(0, inti)
                        '        End If
                        '    End If
                        '    System.Math.Min(System.Threading.Interlocked.Increment(inti), inti - 1)
                        'End While

                        Dim oTes As SAPbobsCOM.Recordset
                        oTes = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oTes.DoQuery("Select ""U_BRAND"" from ""@SEI_BRANDL"" where ""U_Select""='Y'")
                        For intLo As Integer = 0 To oTes.RecordCount - 1
                            If strSelectedItem1 = "" Then
                                strSelectedItem1 = "'" & oTes.Fields.Item(0).Value & "'"
                                strSelectedItem2 = oTes.Fields.Item(0).Value
                            Else
                                strSelectedItem1 = strSelectedItem1 & ",'" & oTes.Fields.Item(0).Value.ToString.Replace("'", "''") & "'"
                                strSelectedItem2 = strSelectedItem2 & "," & oTes.Fields.Item(0).Value.ToString.Replace("'", "''")
                            End If
                            oTes.MoveNext()
                        Next
                        'Dim ocheckbox As SAPbouiCOM.CheckBoxColumn
                        'ocheckbox = oMatrix.Columns.Item("Select")
                        'If ocheckbox.IsChecked(inti) = True Then
                        '    ' intRowId = inti
                        '    If strSelectedItem1 = "" Then
                        '        strSelectedItem1 = "'" & oMatrix.DataTable.GetValue(0, inti) & "'"
                        '        strSelectedItem2 = oMatrix.DataTable.GetValue(0, inti)
                        '    Else
                        '        strSelectedItem1 = strSelectedItem1 & ",'" & oMatrix.DataTable.GetValue(0, inti) & "'"
                        '        strSelectedItem2 = strSelectedItem2 & "," & oMatrix.DataTable.GetValue(0, inti)
                        '    End If
                        'End If
                        System.Math.Min(System.Threading.Interlocked.Increment(inti), inti - 1)

                        oForm.Close()
                        oForm = GetForm(SourceFormUID)
                        If choice <> "Ticket" Then
                            oForm.Freeze(True)
                            oedit = oForm.Items.Item(ItemUID).Specific
                            oedit.String = strSelectedItem2
                            oedit = oForm.Items.Item(sourceColumID).Specific
                            oedit.String = strSelectedItem1
                            oForm.Freeze(False)
                        Else
                            oForm.Freeze(True)
                            oForm.Freeze(False)
                        End If
                    Else
                        While inti <= oMatrix.DataTable.Rows.Count - 1
                            If oMatrix.Rows.IsSelected(inti) = True Then
                                intRowId = inti
                            End If
                            System.Math.Min(System.Threading.Interlocked.Increment(inti), inti - 1)
                        End While
                        If CFLChoice <> "" Then
                            If intRowId = 0 Then
                                strSelectedItem1 = Convert.ToString(oMatrix.DataTable.GetValue(0, intRowId))
                                strSelectedItem2 = Convert.ToString(oMatrix.DataTable.GetValue(1, intRowId))
                            Else
                                strSelectedItem1 = Convert.ToString(oMatrix.DataTable.GetValue(0, intRowId))
                                strSelectedItem2 = Convert.ToString(oMatrix.DataTable.GetValue(1, intRowId))
                            End If
                            If CFLChoice = "E" Then
                                strSelectedItem3 = Convert.ToString(oMatrix.DataTable.GetValue(3, intRowId))

                            End If
                            If choice = "Ticket" Then
                                strSelectedItem2 = Convert.ToString(oMatrix.DataTable.GetValue(1, intRowId))
                                strSelectedItem3 = Convert.ToString(oMatrix.DataTable.GetValue(2, intRowId))
                                strSelectedItem4 = Convert.ToString(oMatrix.DataTable.GetValue(3, intRowId))
                            End If
                            oForm.Close()
                            oForm = GetForm(SourceFormUID)
                            If choice <> "Ticket" Then
                                oForm.Freeze(True)
                                oedit = oForm.Items.Item(ItemUID).Specific
                                oedit.String = strSelectedItem1
                                oForm.Freeze(False)
                            Else
                                oForm.Freeze(True)
                                oForm.Freeze(False)
                            End If
                        End If
                    End If
                End If
            Else
                If pVal.BeforeAction = False Then
                    Try

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                            oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                            If pVal.ItemUID = "6" Then
                                SelectAll(oForm, True)
                            End If
                            If pVal.ItemUID = "7" Then
                                SelectAll(oForm, False)
                            End If
                            If pVal.ItemUID = "mtchoose" And pVal.ColUID = "Select" Then
                                Dim oTest As SAPbobsCOM.Recordset
                                oTest = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                objGrid = oForm.Items.Item("mtchoose").Specific
                                If objGrid.DataTable.GetValue("Select", pVal.Row) = "N" Then
                                    oTest.DoQuery("Update ""@SEI_BRANDL"" Set ""U_Select""='N' where ""U_BRAND""='" & objGrid.DataTable.GetValue(0, pVal.Row) & "'")
                                Else
                                    oTest.DoQuery("Update ""@SEI_BRANDL"" Set ""U_Select""='Y' where ""U_BRAND""='" & objGrid.DataTable.GetValue(0, pVal.Row) & "'")
                                End If
                            End If
                        End If

                        If pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK Then
                            oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                            If pVal.ItemUID = "mtchoose" And pVal.ColUID = "Select" Then
                                Dim oTest As SAPbobsCOM.Recordset
                                oTest = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                objGrid = oForm.Items.Item("mtchoose").Specific
                                If objGrid.DataTable.GetValue("Select", pVal.Row) = "N" Then
                                    oTest.DoQuery("Update ""@SEI_BRANDL"" Set ""U_Select""='N' where ""U_BRAND""='" & objGrid.DataTable.GetValue(0, pVal.Row).ToString.Replace("'", "''") & "'")
                                Else
                                    oTest.DoQuery("Update ""@SEI_BRANDL"" Set ""U_Select""='Y' where ""U_BRAND""='" & objGrid.DataTable.GetValue(0, pVal.Row).ToString.Replace("'", "''") & "'")
                                End If
                            End If
                        End If


                        If (pVal.EventType = SAPbouiCOM.BoEventTypes.et_KEY_DOWN Or pVal.EventType = SAPbouiCOM.BoEventTypes.et_CLICK) Then
                            BubbleEvent = False
                            Dim objGrid As SAPbouiCOM.Grid
                            Dim oedit As SAPbouiCOM.EditText
                            If pVal.ItemUID = "etFind" And pVal.CharPressed <> "13" Then
                                Dim i, j As Integer
                                Dim strItem As String
                                oForm = oApplication.SBO_Application.Forms.Item(pVal.FormUID) 'oApplication.SBO_Application.Forms.GetForm("TWBS_FA_CFL", pVal.FormTypeCount)
                                objGrid = oForm.Items.Item("mtchoose").Specific
                                oedit = oForm.Items.Item("etFind").Specific
                                For i = 0 To objGrid.DataTable.Rows.Count - 1
                                    strItem = ""
                                    strItem = objGrid.DataTable.GetValue(0, i)
                                    For j = 1 To oedit.String.Length
                                        If oedit.String.Length <= strItem.Length Then
                                            If strItem.Substring(0, j).ToUpper = oedit.String.ToUpper Then
                                                objGrid.Rows.SelectedRows.Add(i)
                                                Exit Sub
                                            End If
                                        End If
                                    Next
                                Next
                            End If
                        End If

                    Catch ex As Exception

                    End Try
                End If
            End If
        End If
    End Sub
#End Region

End Class
