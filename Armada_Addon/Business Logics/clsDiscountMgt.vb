
Public Class clsDiscountMgt
    Inherits clsBase
    Private oCFLEvent As SAPbouiCOM.IChooseFromListEvent
    Private oDBDataSource As SAPbouiCOM.DBDataSource
    Private oMatrix As SAPbouiCOM.Matrix
    Private oEditText As SAPbouiCOM.EditText
    Private oCombobox As SAPbouiCOM.ComboBox
    Private oEditTextColumn As SAPbouiCOM.EditTextColumn
    Private oGrid As SAPbouiCOM.Grid
    Private dtTemp As SAPbouiCOM.DataTable
    Private dtResult As SAPbouiCOM.DataTable
    Private oMode As SAPbouiCOM.BoFormMode
    Private oItem As SAPbobsCOM.Items
    Private oInvoice As SAPbobsCOM.Documents
    Private InvBaseDocNo As String
    Private InvForConsumedItems As Integer
    Private blnFlag As Boolean = False
    Dim ostatic As SAPbouiCOM.StaticText

    Public Sub New()
        MyBase.New()
        InvForConsumedItems = 0
    End Sub

#Region "Item Event"
    Public Overrides Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.FormTypeEx = frm_DiscountMgt Then
                Select Case pVal.BeforeAction
                    Case True
                        Select Case pVal.EventType
                            Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD
                            Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                If pVal.ItemUID = "1" And (oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) Then
                                    If Validation(oForm) = False Then
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                            Case SAPbouiCOM.BoEventTypes.et_KEY_DOWN
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                    If pVal.CharPressed <> 9 Then
                                        If pVal.ItemUID = "3" Then
                                            BubbleEvent = False
                                            Exit Sub
                                        End If
                                    End If
                                End If
                            Case SAPbouiCOM.BoEventTypes.et_CLICK
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                                    If 1 = 1 Then
                                        If pVal.ItemUID = "3" Then
                                            BubbleEvent = False
                                            Exit Sub
                                        End If
                                    End If
                                End If
                        End Select

                    Case False
                        Select Case pVal.EventType
                            Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)

                            Case SAPbouiCOM.BoEventTypes.et_KEY_DOWN
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                If (pVal.ItemUID = "7" Or pVal.ItemUID = "8") And pVal.CharPressed = 9 Then
                                    Dim objChooseForm As SAPbouiCOM.Form
                                    Dim objChoose As New clsChooseFromList_Brand
                                    Dim strwhs, strProject, strGirdValue As String
                                    Dim objMatrix As SAPbouiCOM.Grid
                                    Try
                                        strwhs = oApplication.Utilities.getEditTextvalue(oForm, pVal.ItemUID) ' oComboColumn.GetSelectedValue(pVal.Row).Value
                                    Catch ex As Exception
                                        strwhs = ""
                                    End Try
                                    strGirdValue = strwhs 'objMatrix.DataTable.GetValue("U_S_PTrnsCode", pVal.Row)
                                    If strwhs = "" Then
                                        objChoose.ItemUID = pVal.ItemUID
                                        objChoose.SourceFormUID = FormUID
                                        objChoose.SourceLabel = 0 'pVal.Row
                                        objChoose.CFLChoice = "L" 'oCombo.Selected.Value
                                        If pVal.ItemUID = "7" Then
                                            objChoose.choice = "Season"
                                        Else
                                            objChoose.choice = "Brand"
                                        End If
                                        objChoose.ItemCode = strwhs
                                        objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)
                                        objChoose.sourceColumID = "25" ' pVal.ColUID
                                        objChoose.sourcerowId =
                                        objChoose.BinDescrUID = "" 'objMatrix.DataTable.GetValue("Code", pVal.Row)
                                        oApplication.Utilities.LoadForm("CFL_2.xml", frm_ChoosefromList_Leave)
                                        objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                        objChoose.databound(objChooseForm)
                                    End If
                                End If
                            Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                oDBDataSource = oForm.DataSources.DBDataSources.Item(0)
                                Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
                                Dim oCFL As SAPbouiCOM.ChooseFromList
                                Dim val As String
                                Dim sCHFL_ID As String
                                Try
                                    oCFLEvento = pVal
                                    sCHFL_ID = oCFLEvento.ChooseFromListUID
                                    oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                    oCFL = oForm.ChooseFromLists.Item(sCHFL_ID)
                                    If (oCFLEvento.BeforeAction = False) Then
                                        Dim oDataTable As SAPbouiCOM.DataTable
                                        oDataTable = oCFLEvento.SelectedObjects

                                        oForm.Freeze(True)

                                        If pVal.ItemUID = "6" Then
                                            val = oDataTable.GetValue("ItmsGrpNam", 0)
                                            oDBDataSource.SetValue("U_GrpName", 0, val)
                                        ElseIf pVal.ItemUID = "7" Then
                                            val = oDataTable.GetValue("U_SEASON", 0)
                                            oDBDataSource.SetValue("U_Season", 0, val)
                                        ElseIf pVal.ItemUID = "8" Then
                                            val = oDataTable.GetValue("U_BRAND", 0)
                                            oDBDataSource.SetValue("U_Brand", 0, val)
                                        ElseIf pVal.ItemUID = "9" Then
                                            val = oDataTable.GetValue("ItemCode", 0)
                                            oDBDataSource.SetValue("U_Item", 0, val)
                                        End If

                                        oForm.Freeze(False)
                                    End If
                                Catch ex As Exception
                                    oForm.Freeze(False)
                                End Try
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
                Case mnu_DiscountMgt
                    If pVal.BeforeAction = False Then
                        LoadForm()
                    End If
                Case mnu_ADD
                    If pVal.BeforeAction = False Then
                        oForm = oApplication.SBO_Application.Forms.ActiveForm()
                        enablecontrols(oForm, True)
                        oForm.Items.Item("3").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    End If
                Case mnu_FIND
                    If pVal.BeforeAction = False Then
                        oForm = oApplication.SBO_Application.Forms.ActiveForm()
                        oForm.Items.Item("3").Enabled = True
                        oForm.Items.Item("4").Enabled = True
                        enablecontrols(oForm, True)
                        oForm.Items.Item("3").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    End If
                Case mnu_FIRST, mnu_LAST, mnu_NEXT, mnu_PREVIOUS
            End Select
        Catch ex As Exception
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            oForm.Freeze(False)
        End Try
    End Sub
#End Region


    Private Sub AddSpeicalPrice_External(aCompany As SAPbobsCOM.Company)
        Dim oMain As SAPbobsCOM.Recordset
        oMain = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oMain.DoQuery("Select * from ""@SEI_ODIS"" where ""U_SEI_SYNC""='X' order by ""DocEntry""")
        For intLoop As Integer = 0 To oMain.RecordCount - 1
            AddSpeicalPrice(oMain.Fields.Item("DocEntry").Value, aCompany)
            oMain.MoveNext()
        Next
    End Sub

    Private Sub AddSpeicalPrice(aIntDocNum As String, aCompany As SAPbobsCOM.Company)
        Dim oSpp As SAPbobsCOM.SpecialPrices
        Dim intItemGroup, strBrand, strSession, strItem As String
        Dim dtFromdate, dtEndDate As Date
        Dim intPriceList As Integer
        Dim dbldiscount As Double
        Dim strQuery As String = "Select * from  ""@SEI_ODIS""  where ""DocEntry""='" & aIntDocNum & "'"
        Dim otest, oTest1 As SAPbobsCOM.Recordset
        otest = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        otest.DoQuery(strQuery)
        intItemGroup = otest.Fields.Item("U_GrpName").Value
        strBrand = otest.Fields.Item("U_Brand1").Value
        strSession = otest.Fields.Item("U_Season").Value
        strItem = otest.Fields.Item("U_Item").Value
        dtFromdate = otest.Fields.Item("U_From").Value
        dtEndDate = otest.Fields.Item("U_To").Value
        dbldiscount = otest.Fields.Item("U_Discount").Value
        intPriceList = otest.Fields.Item("U_PriceList").Value
        Dim strCondition, strGroupCondition As String

        '  strBrand = strBrand.Replace("'", "''")
        If strBrand <> "" Then
            strCondition = "T0.""U_BRAND"" in (" & strBrand & ")"
        Else
            strCondition = "1=1"
        End If
        If strSession <> "" Then
            strCondition = strCondition & " and T0.""U_SEASON""='" & strSession & "'"
        Else
            strCondition = strCondition & " and 1=1"
        End If
        If strItem <> "" Then
            strCondition = strCondition & " and T0.""ItemCode""='" & strItem & "'"
        Else
            strCondition = strCondition & " and 1=1"
        End If

        If intItemGroup <> "" Then
            strCondition = strCondition & " and T1.""ItmsGrpNam""='" & intItemGroup & "'"
        Else
            strCondition = strCondition & " and 1=1"
        End If
        strSQL = "Select ""ItemCode"" from OITM T0 inner Join OITB T1 on T1.""ItmsGrpCod""=T0.""ItmsGrpCod"" where " & strCondition
        otest.DoQuery(strSQL)
        Dim oSPRec As SAPbobsCOM.Recordset
        oSPRec = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Dim s As String
        Dim dtDate, dtDate2 As Date
        oApplication.Utilities.Message("Processing....", SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        For intRow As Integer = 0 To otest.RecordCount - 1
            Try
                strItem = otest.Fields.Item("ItemCode").Value
                oSpp = aCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSpecialPrices)
                If oSpp.GetByKeyDiscounts(strItem, intPriceList) Then
                    Dim intRownum As Integer '= GetPriceListNo(strItem, intPriceList, dtFromdate, dtEndDate)
                    s = "select Top 1 * from SPP1 where ""ListNum""=" & intPriceList & " and ""ItemCode""='" & strItem & "' and ""CardCode""='*" & intPriceList.ToString & "'  order by isnull(ToDate,getdate()) Desc"
                    If blnIsHana = True Then
                        s = "select Top 1 * from SPP1 where ""ListNum""=" & intPriceList & " and ""ItemCode""='" & strItem & "' and ""CardCode""='*" & intPriceList.ToString & "'  order by ifnull(""ToDate"",NOW()) Desc"
                    End If
                    oSPRec.DoQuery(s)
                    If oSPRec.RecordCount > 0 Then
                        dtDate = oSPRec.Fields.Item("FromDate").Value
                        dtDate2 = oSPRec.Fields.Item("ToDate").Value
                        If Year(dtDate2) = 1899 Then 'End Date is null
                            dtDate2 = dtEndDate
                            intRownum = oSPRec.Fields.Item("LINENUM").Value
                            oSpp.PriceListNum = intPriceList
                            oSpp.ItemCode = strItem
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRownum)
                            'oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtDate ' dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtDate2 ' dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"
                            Dim retV3 As Integer = oSpp.Update
                           
                        ElseIf dtDate = dtFromdate And dtDate2 = dtEndDate Then 'if new from date is EQUAL = from date and END DATE = end date
                            intRownum = oSPRec.Fields.Item("LINENUM").Value
                            oSpp.PriceListNum = intPriceList
                            oSpp.ItemCode = strItem
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRownum)
                            oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"
                            Dim retV2 As Integer = oSpp.Update
                        ElseIf dtDate2 > dtFromdate And dtDate < dtFromdate Then 'if new from date is greater than from date and less than existing end date
                            dtDate2 = dtFromdate.AddDays(-1)
                            intRownum = oSPRec.Fields.Item("LINENUM").Value
                            oSpp.PriceListNum = intPriceList
                            oSpp.ItemCode = strItem
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRownum)
                            oSpp.SpecialPricesDataAreas.Discount = oSPRec.Fields.Item("Discount").Value ' dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtDate ' dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtDate2 ' dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"

                            intRow = oSpp.SpecialPricesDataAreas.Count
                            oSpp.SpecialPricesDataAreas.Add()
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRow) '+ 1)
                            oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"
                            Dim retV1 As Integer = oSpp.Update
                            '   MsgBox(retV1)
                        ElseIf dtDate2 <= dtFromdate Then
                            oSpp.PriceListNum = intPriceList
                            oSpp.ItemCode = strItem
                            intRow = oSpp.SpecialPricesDataAreas.Count
                            oSpp.SpecialPricesDataAreas.Add()
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRow) '+ 1)
                            oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"
                            Dim retV1 As Integer = oSpp.Update
                            '  MsgBox(retV1)
                        ElseIf dtDate > dtFromdate Then

                        End If
                    End If


                Else
                    oSpp.PriceListNum = intPriceList
                    oSpp.ItemCode = strItem
                    oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                    oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                    oSpp.SpecialPricesDataAreas.DateFrom = dtFromdate
                    oSpp.SpecialPricesDataAreas.Dateto = dtEndDate
                    oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                    Dim retV As Integer = oSpp.Add()
                    '  MsgBox(retV)
                End If

            Catch ex As Exception

            End Try
            otest.MoveNext()
        Next
    End Sub

    Private Sub AddSpeicalPrice(aIntDocNum As String, aform As SAPbouiCOM.Form)
        Dim oSpp As SAPbobsCOM.SpecialPrices
        Dim intItemGroup, strBrand, strSession, strItem As String
        Dim dtFromdate, dtEndDate As Date
        Dim intPriceList As Integer
        Dim dbldiscount As Double

        Dim strQuery As String = "Select * from  ""@SEI_ODIS""  where ""DocEntry""='" & aIntDocNum & "'"
        Dim otest, oTest1 As SAPbobsCOM.Recordset
        otest = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        otest.DoQuery(strQuery)
        intItemGroup = otest.Fields.Item("U_GrpName").Value
        strBrand = otest.Fields.Item("U_Brand1").Value
        strSession = otest.Fields.Item("U_Season").Value
        strItem = otest.Fields.Item("U_Item").Value
        dtFromdate = otest.Fields.Item("U_From").Value
        dtEndDate = otest.Fields.Item("U_To").Value
        dbldiscount = otest.Fields.Item("U_Discount").Value
        intPriceList = otest.Fields.Item("U_PriceList").Value
        Dim strCondition, strGroupCondition As String
        '  strBrand = strBrand.Replace("'", "''")
        If strBrand <> "" Then
            strCondition = "T0.""U_BRAND"" in (" & strBrand & ")"
        Else
            strCondition = "1=1"
        End If
        If strSession <> "" Then
            strCondition = strCondition & " and T0.""U_SEASON""='" & strSession & "'"
        Else
            strCondition = strCondition & " and 1=1"
        End If
        If strItem <> "" Then
            strCondition = strCondition & " and T0.""ItemCode""='" & strItem & "'"
        Else
            strCondition = strCondition & " and 1=1"
        End If

        If intItemGroup <> "" Then
            strCondition = strCondition & " and T1.""ItmsGrpNam""='" & intItemGroup & "'"
        Else
            strCondition = strCondition & " and 1=1"
        End If
        strSQL = "Select ""ItemCode"" from OITM T0 inner Join OITB T1 on T1.""ItmsGrpCod""=T0.""ItmsGrpCod"" where " & strCondition
        otest.DoQuery(strSQL)
        Dim oSPRec As SAPbobsCOM.Recordset
        oSPRec = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Dim s As String
        Dim dtDate, dtDate2 As Date

        oApplication.Utilities.Message("Processing....", SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        For intRow As Integer = 0 To otest.RecordCount - 1
            Try

                strItem = otest.Fields.Item("ItemCode").Value
                ostatic = aform.Items.Item("24").Specific
                ostatic.Caption = "Processing ItemCode : " & strItem

                oSpp = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oSpecialPrices)
                If oSpp.GetByKeyDiscounts(strItem, intPriceList) Then
                    Dim intRownum As Integer '= GetPriceListNo(strItem, intPriceList, dtFromdate, dtEndDate)
                    s = "select Top 1 * from SPP1 where ""ListNum""=" & intPriceList & " and ""ItemCode""='" & strItem & "' and ""CardCode""='*" & intPriceList.ToString & "'  order by isnull(ToDate,getdate()) Desc"
                    If blnIsHana = True Then
                        s = "select Top 1 * from SPP1 where ""ListNum""=" & intPriceList & " and ""ItemCode""='" & strItem & "' and ""CardCode""='*" & intPriceList.ToString & "'  order by ifnull(""ToDate"",NOW()) Desc"
                    End If
                    oSPRec.DoQuery(s)
                    If oSPRec.RecordCount > 0 Then
                        dtDate = oSPRec.Fields.Item("FromDate").Value
                        dtDate2 = oSPRec.Fields.Item("ToDate").Value
                        If Year(dtDate2) = 1899 Then 'End Date is null
                            'If dtDate > dtFromdate Then
                            '    dtDate2 = dtFromdate.AddDays(-1)
                            'Else
                            '    dtDate2 = dtEndDate
                            'End If
                            dtDate2 = dtEndDate
                            intRownum = oSPRec.Fields.Item("LINENUM").Value
                            oSpp.PriceListNum = intPriceList
                            oSpp.ItemCode = strItem
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRownum)
                            'oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtDate ' dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtDate2 ' dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"
                            Dim retV3 As Integer = oSpp.Update
                            ''addnew price discount
                            '' oSpp.PriceListNum = intPriceList
                            ''  oSpp.ItemCode = strItem
                            'intRow = oSpp.SpecialPricesDataAreas.Count
                            'oSpp.SpecialPricesDataAreas.Add()
                            'oSpp.SpecialPricesDataAreas.SetCurrentLine(intRow) '+ 1)
                            'oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            'oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            'oSpp.SpecialPricesDataAreas.DateFrom = dtFromdate
                            'oSpp.SpecialPricesDataAreas.Dateto = dtEndDate
                            'oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            'oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"
                            'Dim retV1 As Integer = oSpp.Update
                        ElseIf dtDate = dtFromdate And dtDate2 = dtEndDate Then 'if new from date is EQUAL = from date and END DATE = end date
                            'dtDate2 = dtFromdate.AddDays(-1)
                            intRownum = oSPRec.Fields.Item("LINENUM").Value
                            oSpp.PriceListNum = intPriceList
                            oSpp.ItemCode = strItem
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRownum)
                            oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"
                            Dim retV2 As Integer = oSpp.Update
                        ElseIf dtDate2 > dtFromdate And dtDate < dtFromdate Then 'if new from date is greater than from date and less than existing end date
                            dtDate2 = dtFromdate.AddDays(-1)
                            intRownum = oSPRec.Fields.Item("LINENUM").Value
                            oSpp.PriceListNum = intPriceList
                            oSpp.ItemCode = strItem
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRownum)
                            oSpp.SpecialPricesDataAreas.Discount = oSPRec.Fields.Item("Discount").Value ' dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtDate ' dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtDate2 ' dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"

                            ' Dim retV2 As Integer = oSpp.Update

                            'addnew price discount
                            ' oSpp.PriceListNum = intPriceList
                            '  oSpp.ItemCode = strItem
                            intRow = oSpp.SpecialPricesDataAreas.Count
                            oSpp.SpecialPricesDataAreas.Add()
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRow) '+ 1)
                            oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"
                            Dim retV1 As Integer = oSpp.Update
                            '   MsgBox(retV1)
                        ElseIf dtDate2 <= dtFromdate Then
                            oSpp.PriceListNum = intPriceList
                            oSpp.ItemCode = strItem
                            intRow = oSpp.SpecialPricesDataAreas.Count
                            oSpp.SpecialPricesDataAreas.Add()
                            oSpp.SpecialPricesDataAreas.SetCurrentLine(intRow) '+ 1)
                            oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                            oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                            oSpp.SpecialPricesDataAreas.DateFrom = dtFromdate
                            oSpp.SpecialPricesDataAreas.Dateto = dtEndDate
                            oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                            oSpp.SpecialPricesDataAreas.UserFields.Fields.Item("U_SEI_SYNC").Value = "N"
                            Dim retV1 As Integer = oSpp.Update
                            '  MsgBox(retV1)
                        ElseIf dtDate > dtFromdate Then

                        End If
                    End If


                Else
                    oSpp.PriceListNum = intPriceList
                    oSpp.ItemCode = strItem
                    oSpp.SpecialPricesDataAreas.Discount = dbldiscount
                    oSpp.SpecialPricesDataAreas.PriceListNo = intPriceList
                    oSpp.SpecialPricesDataAreas.DateFrom = dtFromdate
                    oSpp.SpecialPricesDataAreas.Dateto = dtEndDate
                    oSpp.SpecialPricesDataAreas.AutoUpdate = SAPbobsCOM.BoYesNoEnum.tNO
                    Dim retV As Integer = oSpp.Add()
                    '  MsgBox(retV)
                End If

            Catch ex As Exception

            End Try
            otest.MoveNext()
        Next
        ostatic = aform.Items.Item("24").Specific
        ostatic.Caption = "Process Completed "
        oApplication.Utilities.Message("Operation completed successfully....", SAPbouiCOM.BoStatusBarMessageType.smt_Success)
    End Sub
    Private Function GetPriceListNo(aItemCode As String, aPriceList As Integer, aDatefrom As Date, atodate As Date, aDiscount As Double) As Integer
        Dim oRec As SAPbobsCOM.Recordset
        Dim dtFromDate, dtToDate As Date
        oRec = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oRec.DoQuery("select Top 1 ""LINENUM"" from SPP1 where ""ListNum""=" & aPriceList & " and ""ItemCode""='" & aItemCode & "' and ""CardCode""='*1' and '" & aDatefrom.ToString("yyyy-MM-dd") & "' between ""FromDate"" and isnull(""ToDate"",getdate()) order by isnull(ToDate,getdate()) Desc")
        If oRec.RecordCount > 0 Then
            Return oRec.Fields.Item(0).Value
        End If
        oRec.DoQuery("select ""LINENUM"" from SPP1 where ""ListNum""=" & aPriceList & " and ""ItemCode""='" & aItemCode & "' and ""CardCode""='*1' and '" & atodate.ToString("yyyy-MM-dd") & "' between ""FromDate"" and ""ToDate""")
        If oRec.RecordCount > 0 Then
            Return oRec.Fields.Item(0).Value
        End If

        'oRec.DoQuery("select Top 1 * from SPP1 where ""ListNum""=" & aPriceList & " and ""ItemCode""='" & aItemCode & "' and ""CardCode""='*1'  order by isnull(ToDate,FromDate) Desc")
        'If oRec.RecordCount > 0 Then
        '    dtFromDate = oRec.Fields.Item("FromDate").Value
        '    dtToDate = oRec.Fields.Item("ToDate").Value
        '    If Year(dtToDate) = 1899 And aDatefrom > dtFromDate Then
        '        Return -1
        '    End If

        '    If Year(dtToDate) <> 1899 And aDatefrom >= dtToDate Then
        '        Return -1
        '    End If
        '    If Year(dtToDate) <> 1899 And aDatefrom < dtToDate Then
        '        Return -1
        '    End If

        'End If


        Return -1
    End Function

    Private Function UpdateExistingDiscount(aItemCode As String, aPriceList As Integer, aDatefrom As Date, atodate As Date) As Integer
        Dim oRec As SAPbobsCOM.Recordset
        Dim dtFromDate, dtToDate As Date
        oRec = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        oRec.DoQuery("select ""LINENUM"" from SPP1 where ""ListNum""=" & aPriceList & " and ""ItemCode""='" & aItemCode & "' and ""CardCode""='*1' and '" & aDatefrom.ToString("yyyy-MM-dd") & "' between ""FromDate"" and ""ToDate"" order by isnull(ToDate,FromDate) Desc")
        'If oRec.RecordCount > 0 Then
        '    Return oRec.Fields.Item(0).Value

        'End If
        'oRec.DoQuery("select ""LINENUM"" from SPP1 where ""ListNum""=" & aPriceList & " and ""ItemCode""='" & aItemCode & "' and ""CardCode""='*1' and '" & atodate.ToString("yyyy-MM-dd") & "' between ""FromDate"" and ""ToDate""")
        'If oRec.RecordCount > 0 Then
        '    Return oRec.Fields.Item(0).Value
        'End If

        oRec.DoQuery("select Top 1 * from SPP1 where ""ListNum""=" & aPriceList & " and ""ItemCode""='" & aItemCode & "' and ""CardCode""='*1'  order by isnull(ToDate,FromDate) Desc")
        If oRec.RecordCount > 0 Then
            dtFromDate = oRec.Fields.Item("FromDate").Value
            dtToDate = oRec.Fields.Item("ToDate").Value
            If Year(dtToDate) = 1899 And aDatefrom > dtToDate Then
                Return -1
            End If

            If Year(dtToDate) <> 1899 And aDatefrom >= dtToDate Then
                Return -1
            End If
            If Year(dtToDate) <> 1899 And aDatefrom < dtToDate Then
                Return -1
            End If

        End If


        Return -1
    End Function
    Private Sub enablecontrols(aform As SAPbouiCOM.Form, aflag As Boolean)
        aform.Freeze(True)
        ostatic = aform.Items.Item("24").Specific
        ostatic.Caption = ""
        aform.Items.Item("1000002").Enabled = True
        aform.Items.Item("1000002").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
        aform.Items.Item("3").Enabled = aflag
        aform.Items.Item("4").Enabled = aflag
        aform.Items.Item("23").Enabled = aflag
        aform.Items.Item("5").Enabled = aflag
        aform.Items.Item("5_").Enabled = aflag
        aform.Items.Item("6").Enabled = aflag
        aform.Items.Item("7").Enabled = aflag
        aform.Items.Item("8").Enabled = aflag
        aform.Items.Item("9").Enabled = aflag
        aform.Items.Item("10").Enabled = aflag
        aform.Freeze(False)
    End Sub

#Region "Data Event"
    Public Sub FormDataEvent(ByRef BusinessObjectInfo As SAPbouiCOM.BusinessObjectInfo, ByRef BubbleEvent As Boolean)
        Try


            Try
                Select Case BusinessObjectInfo.BeforeAction
                    Case True
                    Case False
                        Select Case BusinessObjectInfo.EventType
                            Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD
                                oForm = oApplication.SBO_Application.Forms.Item(BusinessObjectInfo.FormUID)
                                If BusinessObjectInfo.ActionSuccess Then
                                    oForm = oApplication.SBO_Application.Forms.ActiveForm()
                                    Dim oXmlDoc As System.Xml.XmlDocument = New Xml.XmlDocument()
                                    oXmlDoc.LoadXml(BusinessObjectInfo.ObjectKey)
                                    Dim DocEntry As String = oXmlDoc.SelectSingleNode("/SEI_ODISParams/DocEntry").InnerText
                                    Try
                                        Dim oRecordSet As SAPbobsCOM.Recordset
                                        oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                        Dim strQuery As String = "Update ""@SEI_ODIS"" set  ""U_SEI_SYNC""='X' where ""DocEntry""='" & DocEntry & "'"
                                        oRecordSet.DoQuery(strQuery)
                                        ' AddSpeicalPrice(DocEntry, oForm)
                                    Catch ex As Exception
                                    End Try
                                End If
                            Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE
                                oForm = oApplication.SBO_Application.Forms.Item(BusinessObjectInfo.FormUID)
                                'If BusinessObjectInfo.ActionSuccess = True And BusinessObjectInfo.BeforeAction = False Then
                                '    oForm = oApplication.SBO_Application.Forms.Item(BusinessObjectInfo.FormUID)
                                '    Dim oXmlDoc As System.Xml.XmlDocument = New Xml.XmlDocument()
                                '    oXmlDoc.LoadXml(BusinessObjectInfo.ObjectKey)
                                '    Dim DocEntry As String = oXmlDoc.SelectSingleNode("/SEI_ODISParams/DocEntry").InnerText
                                '    Try
                                '        Dim oRecordSet As SAPbobsCOM.Recordset
                                '        oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                '        Dim strQuery As String = "Update ""@SEI_ODIS"" set ""U_SEI_SYNC""='N' where ""DocEntry""='" & DocEntry & "'"
                                '        oRecordSet.DoQuery(strQuery)
                                '        AddSpeicalPrice(DocEntry)
                                '    Catch ex As Exception

                                '    End Try
                                'End If
                            Case SAPbouiCOM.BoEventTypes.et_FORM_DATA_LOAD
                                oForm = oApplication.SBO_Application.Forms.Item(BusinessObjectInfo.FormUID)
                                If BusinessObjectInfo.ActionSuccess Then
                                    'oForm.Items.Item("4").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                                    'oForm.Items.Item("3").Enabled = False
                                    'oForm.Items.Item("4").Enabled = True
                                    enablecontrols(oForm, False)
                                End If
                        End Select
                End Select
            Catch ex As Exception
                oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                oForm.Freeze(False)
            End Try


        Catch ex As Exception
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            oForm.Freeze(False)
        End Try
    End Sub
#End Region

#Region "Methods"

    Private Sub LoadForm()
        oForm = oApplication.Utilities.LoadForm(xml_DiscountMgt, frm_DiscountMgt)
        oForm = oApplication.SBO_Application.Forms.ActiveForm()
        oForm.Freeze(True)
        oForm.DataBrowser.BrowseBy = "3"
        oCombobox = oForm.Items.Item("23").Specific
        Dim otest As SAPbobsCOM.Recordset
        otest = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        otest.DoQuery("Select ""ListNum"",""ListName"" from OPLN order by ""ListNum""")
        For intRow As Integer = 0 To otest.RecordCount - 1
            oCombobox.ValidValues.Add(otest.Fields.Item(0).Value, otest.Fields.Item(1).Value)
            otest.MoveNext()
        Next
        oForm.Items.Item("23").DisplayDesc = True
        oCombobox.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly
        oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
        oForm.Items.Item("3").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
        oForm.Freeze(False)
    End Sub

    Private Sub EnableControls(ByVal aform As SAPbouiCOM.Form)
        Try
            aform.Freeze(True)
            ostatic = aform.Items.Item("24").Specific
            ostatic.Caption = ""
            Select Case aform.Mode
                Case SAPbouiCOM.BoFormMode.fm_ADD_MODE
                    aform.Items.Item("3").Enabled = True
                    aform.Items.Item("4").Enabled = True
                Case SAPbouiCOM.BoFormMode.fm_FIND_MODE
                    aform.Items.Item("3").Enabled = True
                    aform.Items.Item("4").Enabled = True
            End Select
            aform.Freeze(False)
        Catch ex As Exception
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            aform.Freeze(False)
        End Try
    End Sub

    Private Function Validation(aform As SAPbouiCOM.Form) As Boolean
        Dim oRec As SAPbobsCOM.Recordset
        oRec = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Try
            If aform.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Or aform.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then
                Return True
            End If

            Dim strCode As String = oApplication.Utilities.getEditTextvalue(aform, "3")
            Dim strFromDate As String = oApplication.Utilities.getEditTextvalue(aform, "5")
            Dim strToDate As String = oApplication.Utilities.getEditTextvalue(aform, "5_")
            Dim strDiscount As String = oApplication.Utilities.getEditTextvalue(aform, "10")

            If aform.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                If oApplication.Utilities.getEditTextvalue(aform, "3") = "" Then
                    oApplication.Utilities.Message("Discount Code is missing...", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return False
                End If
                oRec.DoQuery("Select * from ""@SEI_ODIS"" where ""U_Code""='" & strCode & "'")
                If oRec.RecordCount > 0 Then
                    oApplication.Utilities.Message("Discount Code already exists", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                    Return False
                End If
            End If

            If oApplication.Utilities.getEditTextvalue(aform, "4") = "" Then
                oApplication.Utilities.Message("Discount Description is missing...", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            ElseIf oApplication.Utilities.getEditTextvalue(aform, "5") = "" Then
                oApplication.Utilities.Message("Enter Valid From...", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            ElseIf oApplication.Utilities.getEditTextvalue(aform, "5_") = "" Then
                oApplication.Utilities.Message("Enter Valid To...", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If

            If CInt(strFromDate) > CInt(strToDate) Then
                oApplication.Utilities.Message("Valid From Should be Less than or Equal to Valid To Date...", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If

            Dim dblDouble As Double = 0
            Double.TryParse(strDiscount, dblDouble)
            If dblDouble <= 0 Then
                oApplication.Utilities.Message("Discount Percentage Should be Greater than Zero...", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If

            Return True
        Catch ex As Exception
                oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End Try

    End Function

    'Private Sub AddChooseFromList(ByVal objForm As SAPbouiCOM.Form)
    '    Try

    '        Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
    '        Dim oCons As SAPbouiCOM.Conditions
    '        Dim oCon As SAPbouiCOM.Condition


    '        oCFLs = objForm.ChooseFromLists
    '        Dim oCFL As SAPbouiCOM.ChooseFromList
    '        Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
    '        oCFLCreationParams = oApplication.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

    '        ' Adding 2 CFL, one for the button and one for the edit text.
    '        oCFLCreationParams.MultiSelection = False
    '        oCFLCreationParams.ObjectType = "Z_HR_OBUOB"
    '        oCFLCreationParams.UniqueID = "CFL1"
    '        oCFL = oCFLs.Add(oCFLCreationParams)


    '        oCFLCreationParams.ObjectType = "Z_HR_OPEOB"
    '        oCFLCreationParams.UniqueID = "CFL2"
    '        oCFL = oCFLs.Add(oCFLCreationParams)


    '        oCFLCreationParams.ObjectType = "Z_HR_OCOMP"
    '        oCFLCreationParams.UniqueID = "CFL3"
    '        oCFL = oCFLs.Add(oCFLCreationParams)

    '        oCFLCreationParams.ObjectType = "Z_HR_OPOSIN"
    '        oCFLCreationParams.UniqueID = "CFL4"
    '        oCFL = oCFLs.Add(oCFLCreationParams)

    '        oCFLCreationParams.ObjectType = "Z_HR_OCOCA"
    '        oCFLCreationParams.UniqueID = "CFL5"
    '        oCFL = oCFLs.Add(oCFLCreationParams)

    '        oCons = oCFL.GetConditions()
    '        oCon = oCons.Add()
    '        oCon.Alias = "U_Z_Status"
    '        oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
    '        oCon.CondVal = "Y"
    '        oCFL.SetConditions(oCons)
    '        oCon = oCons.Add()



    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

#End Region
    
End Class
