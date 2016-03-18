Public Class clsInBound
    Inherits clsBase

    Private oCFLEvent As SAPbouiCOM.IChooseFromListEvent
    Private oDataSrc As SAPbouiCOM.DBDataSource
    Private oDataSrc_Line0 As SAPbouiCOM.DBDataSource
    Private oDataSrc_Line1 As SAPbouiCOM.DBDataSource
    Private oDataSrc_Line2 As SAPbouiCOM.DBDataSource

    Private oMatrix As SAPbouiCOM.Matrix
    Private oEditText As SAPbouiCOM.EditText
    Private oCombobox As SAPbouiCOM.ComboBox
    Private oEditTextColumn As SAPbouiCOM.EditTextColumn
    Private oGrid As SAPbouiCOM.Grid
    Private dtTemp As SAPbouiCOM.DataTable
    Private dtResult As SAPbouiCOM.DataTable
    Private oMode As SAPbouiCOM.BoFormMode
    Private RowtoDelete As Integer
    Private oMenuobject As Object
    Private count As Integer
    Dim MatrixId As Integer
    Private oColumn As SAPbouiCOM.Column
    Private InvForConsumedItems As Integer
    Private intSelectedMatrixrow As Integer

    Public Sub New()
        MyBase.New()
        InvForConsumedItems = 0
    End Sub

    Public Sub LoadForm()
        Try
            oForm = oApplication.Utilities.LoadForm(xml_InBound, frm_InBound)
            oForm = oApplication.SBO_Application.Forms.ActiveForm()
            oForm.Freeze(True)
            oForm.DataBrowser.BrowseBy = "8"
            oForm.EnableMenu(mnu_ADD_ROW, True)
            oForm.EnableMenu(mnu_DELETE_ROW, True)

            oCombobox = oForm.Items.Item("3").Specific
            Dim oRecordSet As SAPbobsCOM.Recordset
            oRecordSet = oApplication.Company.GetCompanyList
            oCombobox.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oCombobox.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(0).Value)
                oRecordSet.MoveNext()
            Loop

            oRecordSet.MoveFirst()
            oMatrix = oForm.Items.Item("11").Specific
            oMatrix.AddRow(1, -1)
            Dim oCom As SAPbouiCOM.ComboBox = oMatrix.Columns.Item("V_0").Cells().Item(1).Specific
            oCom.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oCom.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(0).Value)
                oRecordSet.MoveNext()
            Loop

            oForm.Items.Item("8").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
            oForm.PaneLevel = 1
            oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
            oForm.Items.Item("_6").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
            oForm.Items.Item("9").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
            oForm.Freeze(False)
        Catch ex As Exception
            oForm.Freeze(False)
        End Try
    End Sub

#Region "Item Event"

    Public Overrides Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.FormTypeEx = frm_InBound Then
                Select Case pVal.BeforeAction
                    Case True
                        Select Case pVal.EventType
                            Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                            Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                If pVal.ItemUID = "1" And (oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If Validation(oForm) = False Then
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                            Case SAPbouiCOM.BoEventTypes.et_CLICK
                                oForm = oApplication.SBO_Application.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                                If (pVal.ItemUID = "6") And pVal.Row > 0 Then
                                    Me.RowtoDelete = pVal.Row
                                    intSelectedMatrixrow = pVal.Row
                                    Me.MatrixId = "6"
                                    frmSourceMatrix = oMatrix
                                ElseIf (pVal.ItemUID = "7") And pVal.Row > 0 Then
                                    Me.RowtoDelete = pVal.Row
                                    intSelectedMatrixrow = pVal.Row
                                    Me.MatrixId = "7"
                                    frmSourceMatrix = oMatrix
                                End If
                        End Select
                    Case False
                        Select Case pVal.EventType
                            Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                            Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                Select Case pVal.ItemUID
                                    Case "_6"
                                        oForm.PaneLevel = 1
                                    Case "_7"
                                        oForm.PaneLevel = 2
                                    Case "_11"
                                        oForm.PaneLevel = 3
                                End Select
                            Case SAPbouiCOM.BoEventTypes.et_KEY_DOWN
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                oDataSrc = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPING")

                                If pVal.ItemUID = "6" And pVal.ColUID = "V_1" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    oMatrix = oForm.Items.Item("6").Specific
                                    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "W"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "6" And pVal.ColUID = "V_2" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    oMatrix = oForm.Items.Item("6").Specific
                                    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "L"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "6" And pVal.ColUID = "V_3" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    oMatrix = oForm.Items.Item("6").Specific
                                    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "IA"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "6" And pVal.ColUID = "V_4" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    oMatrix = oForm.Items.Item("6").Specific
                                    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "RA"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                    'ElseIf pVal.ItemUID = "6" And pVal.ColUID = "V_5" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    '    oMatrix = oForm.Items.Item("6").Specific
                                    '    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    '    Dim objChooseForm As SAPbouiCOM.Form
                                    '    Dim objChoose As New clsChooseFromList_Gen
                                    '    objChoose.ItemUID = pVal.ItemUID
                                    '    objChoose.SourceFormUID = FormUID
                                    '    objChoose.SourceLabel = 0 'pVal.Row
                                    '    objChoose.CFLChoice = "LOC"
                                    '    objChoose.choice = "INBOUND"
                                    '    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    '    objChoose.ItemCode = strItemCode
                                    '    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    '    If pVal.ItemUID = "13" Then
                                    '        objChoose.sourceColumID = "28"
                                    '    Else
                                    '        objChoose.sourceColumID = "29"
                                    '    End If

                                    '    objChoose.sourcerowId = pVal.Row
                                    '    objChoose.BinDescrUID = ""
                                    '    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    '    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    '    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "6" And pVal.ColUID = "V_6" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    oMatrix = oForm.Items.Item("6").Specific
                                    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "CC"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "7" And pVal.ColUID = "V_1" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    oMatrix = oForm.Items.Item("7").Specific
                                    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    'filterUOMChooseFromList(oForm, "CFL_5", strItemCode)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "C"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "7" And pVal.ColUID = "V_5" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    oMatrix = oForm.Items.Item("7").Specific
                                    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_1", pVal.Row)
                                    'filterUOMChooseFromList(oForm, "CFL_5", strItemCode)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "P"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "9" Then
                                    'oMatrix = oForm.Items.Item("6").Specific
                                    Dim strItemCode As String '= oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "A"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "10" Then
                                    'oMatrix = oForm.Items.Item("6").Specific
                                    Dim strItemCode As String '= oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "R"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "11" And pVal.ColUID = "V_1" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    oMatrix = oForm.Items.Item("6").Specific
                                    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "PIA"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                ElseIf pVal.ItemUID = "11" And pVal.ColUID = "V_2" And pVal.CharPressed = 9 And oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                    oMatrix = oForm.Items.Item("6").Specific
                                    Dim strItemCode As String = oApplication.Utilities.getMatrixValues(oMatrix, "V_0", pVal.Row)
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Gen
                                    objChoose.ItemUID = pVal.ItemUID
                                    objChoose.SourceFormUID = FormUID
                                    objChoose.SourceLabel = 0 'pVal.Row
                                    objChoose.CFLChoice = "PRA"
                                    objChoose.choice = "INBOUND"
                                    objChoose.strSCompany = oDataSrc.GetValue("U_COMPANY", 0).ToString().Trim()
                                    objChoose.ItemCode = strItemCode
                                    objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)

                                    If pVal.ItemUID = "13" Then
                                        objChoose.sourceColumID = "28"
                                    Else
                                        objChoose.sourceColumID = "29"
                                    End If

                                    objChoose.sourcerowId = pVal.Row
                                    objChoose.BinDescrUID = ""
                                    oApplication.Utilities.LoadForm("CFL_GEN.xml", frm_ChoosefromList_Gen)
                                    objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    objChoose.databound(objChooseForm)
                                End If
                        End Select
                End Select
            End If
        Catch ex As Exception
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

#End Region

#Region "Menu Event"
    Public Overrides Sub MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean)
        Try
            Select Case pVal.MenuUID
                Case mnu_InBound
                    If pVal.BeforeAction = False Then
                        LoadForm()
                    End If
                Case mnu_ADD_ROW
                    oForm = oApplication.SBO_Application.Forms.ActiveForm()
                    AddRow(oForm)
                Case mnu_DELETE_ROW
                    oForm = oApplication.SBO_Application.Forms.ActiveForm()
                    If pVal.BeforeAction = False Then
                        RefereshDeleteRow(oForm)
                    Else

                    End If
                Case mnu_ADD
                    oForm = oApplication.SBO_Application.Forms.ActiveForm()
                    If pVal.BeforeAction = False Then
                        oForm.Items.Item("8").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                        Dim strCode As String = oApplication.Utilities.getMaxCode("@Z_INBOUNDMAPPING", "DocEntry")
                        CType(oForm.Items.Item("8").Specific, SAPbouiCOM.EditText).Value = strCode
                        oForm.Update()
                        'oForm.Items.Item("4").Enabled = True
                        'oForm.Items.Item("6").Enabled = True
                    End If
                Case mnu_FIND
                    oForm = oApplication.SBO_Application.Forms.ActiveForm()
                    If pVal.BeforeAction = False Then
                        'oForm.Items.Item("4").Enabled = True
                        'oForm.Items.Item("6").Enabled = True
                    End If
                Case mnu_FIRST, mnu_LAST, mnu_NEXT, mnu_PREVIOUS
            End Select
        Catch ex As Exception
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            oForm.Freeze(False)
        End Try
    End Sub
#End Region

    Public Sub FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean)
        Try
            If BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD) Then
                oForm = oApplication.SBO_Application.Forms.ActiveForm()
                If BusinessObjectInfo.BeforeAction = False Then
                    oForm.Items.Item("8").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    'oForm.Items.Item("4").Enabled = False
                End If
            ElseIf BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD Or BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE) Then
                oForm = oApplication.SBO_Application.Forms.ActiveForm()
            End If
        Catch ex As Exception
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Public Sub RightClickEvent(ByRef eventInfo As SAPbouiCOM.ContextMenuInfo, ByRef BubbleEvent As Boolean)
        oForm = oApplication.SBO_Application.Forms.Item(eventInfo.FormUID)
        'If eventInfo.FormUID = "RightClk" Then
        If 1 = 1 Then 'oForm.TypeEx = frm_HR_Trainner Then
            If (eventInfo.BeforeAction = True) Then
                Dim oMenuItem As SAPbouiCOM.MenuItem
                Dim oMenus As SAPbouiCOM.Menus
                Try
                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                       
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            Else
                Dim oMenuItem As SAPbouiCOM.MenuItem
                Dim oMenus As SAPbouiCOM.Menus
                Try
                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                        '  oApplication.SBO_Application.Menus.RemoveEx("TraDetails")
                    End If
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            End If
        End If
    End Sub

    Private Sub EnableControls(ByVal aform As SAPbouiCOM.Form)
        Try
            aform.Freeze(True)
            Select Case aform.Mode
                Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                    'aform.Items.Item("4").Enabled = True
                    'aform.Items.Item("6").Enabled = True
                Case SAPbouiCOM.BoFormMode.fm_FIND_MODE
                    'aform.Items.Item("4").Enabled = True
                    'aform.Items.Item("6").Enabled = True
            End Select
            aform.Freeze(False)
        Catch ex As Exception
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            aform.Freeze(False)
        End Try
    End Sub

#Region "AddRow /Delete Row"
    Private Sub AddRow(ByVal aForm As SAPbouiCOM.Form)
        Select Case aForm.PaneLevel
            Case "1"
                oMatrix = aForm.Items.Item("6").Specific
                oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC")
            Case "2"
                oMatrix = aForm.Items.Item("7").Specific
                oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC1")
            Case "3"
                oMatrix = aForm.Items.Item("11").Specific
                oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC2")
        End Select
        Try
            aForm.Freeze(True)
            ' oMatrix = aForm.Items.Item("7").Specific
            If oMatrix.RowCount <= 0 Then
                oMatrix.AddRow()
            End If
            Try
                oEditText = oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.RowCount).Specific
                If oEditText.String <> "" Then
                    oMatrix.AddRow()
                    oMatrix.ClearRowData(oMatrix.RowCount)
                End If
            Catch ex As Exception
                aForm.Freeze(False)
                oMatrix.AddRow()
            End Try

            Try
               

            Catch ex As Exception
                aForm.Freeze(False)
                oMatrix.AddRow()
            End Try

            oMatrix.FlushToDataSource()
            For count = 1 To oDataSrc_Line0.Size
                oDataSrc_Line0.SetValue("LineId", count - 1, count)
            Next
            oMatrix.LoadFromDataSource()
            oMatrix.Columns.Item("V_0").Cells.Item(oMatrix.RowCount).Click(SAPbouiCOM.BoCellClickType.ct_Regular)

            aForm.Freeze(False)
        Catch ex As Exception
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            aForm.Freeze(False)
        End Try

    End Sub


#End Region

    Private Sub RefereshDeleteRow(ByVal aForm As SAPbouiCOM.Form)
        aForm.Freeze(True)
        oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC")
        If aForm.PaneLevel = 1 Then
            oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC")
            frmSourceMatrix = aForm.Items.Item("6").Specific
        ElseIf aForm.PaneLevel = 2 Then
            frmSourceMatrix = aForm.Items.Item("7").Specific
            oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPING1")
        ElseIf aForm.PaneLevel = 3 Then
            frmSourceMatrix = aForm.Items.Item("11").Specific
            oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPING2")
        End If

        If intSelectedMatrixrow <= 0 Then
            Exit Sub
        End If

        Me.RowtoDelete = intSelectedMatrixrow
        oDataSrc_Line0.RemoveRecord(Me.RowtoDelete - 1)
        oMatrix = frmSourceMatrix
        oMatrix.FlushToDataSource()
        For count = 1 To oDataSrc_Line0.Size - 1
            oDataSrc_Line0.SetValue("LineId", count - 1, count)
        Next

        oMatrix.LoadFromDataSource()
        If oMatrix.RowCount > 0 Then
            oMatrix.DeleteRow(oMatrix.RowCount)
            If aForm.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE And aForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                aForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            End If
        End If

        aForm.Freeze(False)
    End Sub

    Private Sub DeleteRow(ByVal aform As SAPbouiCOM.Form)
        aform.Freeze(True)
        Select Case aform.PaneLevel
            Case "1"
                oMatrix = aform.Items.Item("6").Specific
                oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC")
            Case "2"
                oMatrix = aform.Items.Item("7").Specific
                oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC1")
            Case "3"
                oMatrix = aform.Items.Item("11").Specific
                oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC2")
        End Select
        For introw As Integer = 1 To oMatrix.RowCount
            If oMatrix.IsRowSelected(introw) Then
                oMatrix.DeleteRow(introw)
            End If
        Next
        aform.Freeze(False)
    End Sub

    Private Function Validation(ByVal aForm As SAPbouiCOM.Form) As Boolean

        aForm.Freeze(True)
        If oApplication.Utilities.getEdittextvalue(oForm, "4") = "" Then
            oApplication.Utilities.Message("Sap User Name is missing...", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            aForm.Freeze(False)
            Return False
        End If

        If oApplication.Utilities.getEditTextvalue(oForm, "5") = "" Then
            oApplication.Utilities.Message("Sap Password is missing...", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            aForm.Freeze(False)
            Return False
        End If

        'Dim oRec As SAPbobsCOM.Recordset
        'oRec = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        'strWeekEndCode = oApplication.Utilities.getEdittextvalue(aForm, "4")
        'If aForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
        '    oRec.DoQuery("Select * from ""@Z_OLUSR"" where ""U_Z_UserCode""='" & strWeekEndCode & "'")
        '    If oRec.RecordCount > 0 Then
        '        oApplication.Utilities.Message("User Mapping already exists", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        '        aForm.Freeze(False)
        '        Return False
        '    End If
        'End If

        oMatrix = aForm.Items.Item("6").Specific
        oMatrix.FlushToDataSource()
        oDataSrc_Line0 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC")
        For count = 1 To oDataSrc_Line0.Size
            oDataSrc_Line0.SetValue("LineId", count - 1, count)
        Next
        oMatrix.LoadFromDataSource()
        oMatrix.LoadFromDataSource()

        oMatrix = aForm.Items.Item("7").Specific
        oMatrix.FlushToDataSource()
        oDataSrc_Line1 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC1")
        For count = 1 To oDataSrc_Line1.Size ' - 1
            oDataSrc_Line1.SetValue("LineId", count - 1, count)
        Next
        oMatrix.LoadFromDataSource()
        oMatrix.LoadFromDataSource()

        oMatrix = aForm.Items.Item("11").Specific
        oMatrix.FlushToDataSource()
        oDataSrc_Line2 = oForm.DataSources.DBDataSources.Item("@Z_INBOUNDMAPPINGC2")
        For count = 1 To oDataSrc_Line1.Size ' - 1
            oDataSrc_Line2.SetValue("LineId", count - 1, count)
        Next
        oMatrix.LoadFromDataSource()
        oMatrix.LoadFromDataSource()

        aForm.Freeze(False)
        Return True
    End Function


End Class
