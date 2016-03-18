Public Class clsCompMapping
    Inherits clsBase
    Private oCFLEvent As SAPbouiCOM.IChooseFromListEvent
    Private oDBSrc_Line As SAPbouiCOM.DBDataSource
    Private oMatrix As SAPbouiCOM.Matrix
    Private oEditText As SAPbouiCOM.EditText
    Private oCombobox As SAPbouiCOM.ComboBox
    Private oCheckbox As SAPbouiCOM.CheckBox
    Private oEditTextColumn As SAPbouiCOM.EditTextColumn
    Private oComboColumn As SAPbouiCOM.ComboBoxColumn
    Private oCheckboxColumn As SAPbouiCOM.CheckBoxColumn
    Private oGrid As SAPbouiCOM.Grid
    Private dtTemp As SAPbouiCOM.DataTable
    Private dtResult As SAPbouiCOM.DataTable
    Private oMode As SAPbouiCOM.BoFormMode
    Private oItem As SAPbobsCOM.Items
    Private oInvoice As SAPbobsCOM.Documents
    Private MatrixId As String
    Private RowtoDelete As Integer
    Private InvBaseDocNo, strPayment, strQry As String
    Private InvForConsumedItems As Integer
    Private blnFlag As Boolean = False

    Public Sub New()
        MyBase.New()
        InvForConsumedItems = 0
    End Sub

    Private Sub LoadForm()
        oForm = oApplication.Utilities.LoadForm(xml_CompMap, frm_CompMap)
        oForm = oApplication.SBO_Application.Forms.ActiveForm()
        oForm.Freeze(True)
        AddChooseFromList(oForm)
        Databind(oForm)
        oForm.Freeze(False)
    End Sub

    Private Sub AddChooseFromList(ByVal objForm As SAPbouiCOM.Form)
        Try

            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
            Dim oCons As SAPbouiCOM.Conditions
            Dim oCon As SAPbouiCOM.Condition


            oCFLs = objForm.ChooseFromLists
            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
            oCFLCreationParams = oApplication.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = 64
            oCFLCreationParams.UniqueID = "CFL2"
            oCFL = oCFLs.Add(oCFLCreationParams)

            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = 1
            oCFLCreationParams.UniqueID = "CFL_2"
            oCFL = oCFLs.Add(oCFLCreationParams)

            oCFL = oCFLs.Item("CFL_2")
            oCons = oCFL.GetConditions()
            oCon = oCons.Add()
            oCon.Alias = "Postable"
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
            oCon.CondVal = "Y"
            oCFL.SetConditions(oCons)
            oCon = oCons.Add()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Databind(ByVal sform As SAPbouiCOM.Form)
        Try
            oGrid = sform.Items.Item("3").Specific
            strQry = "Select ""Code"",""Name"",""U_SCompany"",""U_WCompany"",""U_SUserName"",""U_SPassword"",""U_SP"",""U_LC"",""U_CustSeries"",""U_WhsCode"",""U_PPPM"","
            strQry += " ""U_PPCC"",""U_PPCN"",""U_PPVD"",""U_CCPM"",""U_CC"",""U_CCCN"",""U_CCVD"",""U_AEPM"",""U_AECC"",""U_AECN"",""U_AEVD"",""U_CODACT"",""U_BIP"", "
            strQry += " ""U_RISE"",""U_IPSE"",""U_SRSE"",""U_CNSE"",""U_DNSE"",""U_Freight"" from ""@SEI_ECOMPANY""" ' where ""Code""=""Name"""
            oGrid.DataTable.ExecuteQuery(strQry)
            oGrid.Columns.Item("Code").TitleObject.Caption = "Code"
            oGrid.Columns.Item("Code").Visible = False
            oGrid.Columns.Item("Name").TitleObject.Caption = "Name"
            oGrid.Columns.Item("Name").Visible = False
            oGrid.Columns.Item("U_SCompany").TitleObject.Caption = "Company Name"
            oGrid.Columns.Item("U_SCompany").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_SCompany")
            Dim oRecordSet As SAPbobsCOM.Recordset
            oRecordSet = oApplication.Company.GetCompanyList
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(0).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description
            oGrid.Columns.Item("U_SUserName").TitleObject.Caption = "SAP UserName"
            oGrid.Columns.Item("U_SPassword").TitleObject.Caption = "SAP Password"
            oGrid.Columns.Item("U_WCompany").TitleObject.Caption = "Web Company Code"
            oGrid.Columns.Item("U_SP").TitleObject.Caption = "Sales Price List"
            oGrid.Columns.Item("U_SP").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_SP")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""ListNum"",""ListName"" from OPLN")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description
            oGrid.Columns.Item("U_LC").TitleObject.Caption = "Landing Cost Price List"
            oGrid.Columns.Item("U_LC").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_LC")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""ListNum"",""ListName"" from OPLN")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description
            oGrid.Columns.Item("U_CustSeries").TitleObject.Caption = "Customer Default Series"
            oGrid.Columns.Item("U_CustSeries").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_CustSeries")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""Series"",""SeriesName"" from NNM1 where ""ObjectCode""='2'")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            oGrid.Columns.Item("U_WhsCode").TitleObject.Caption = "Default Warehouse"
            oEditTextColumn = oGrid.Columns.Item("U_WhsCode")
            oEditTextColumn.ChooseFromListUID = "CFL2"
            oEditTextColumn.ChooseFromListAlias = "WhsCode"
            oEditTextColumn.LinkedObjectType = 64
            oGrid.Columns.Item("U_PPCC").TitleObject.Caption = "Paypal Card"
            oGrid.Columns.Item("U_PPCC").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_PPCC")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""CreditCard"",""CardName"" from OCRC")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            oGrid.Columns.Item("U_PPCN").TitleObject.Caption = "Paypal Card Number"
            oGrid.Columns.Item("U_PPVD").TitleObject.Caption = "Paypal Card ExpiryDate"

            oGrid.Columns.Item("U_PPPM").TitleObject.Caption = "Paypal Card Payment Method"
            oGrid.Columns.Item("U_PPPM").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_PPPM")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""CrTypeCode"",""CrTypeName"" from OCRP")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            oGrid.Columns.Item("U_CC").TitleObject.Caption = "Credit Card payment"
            oGrid.Columns.Item("U_CC").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_CC")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""CreditCard"",""CardName"" from OCRC")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description
            oGrid.Columns.Item("U_CCCN").TitleObject.Caption = "CreditCard Number"
            oGrid.Columns.Item("U_CCVD").TitleObject.Caption = "CreditCard ExpiryDate"

            oGrid.Columns.Item("U_CCPM").TitleObject.Caption = "Credit Card Payment Method"
            oGrid.Columns.Item("U_CCPM").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_CCPM")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""CrTypeCode"",""CrTypeName"" from OCRP")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            '----

            oGrid.Columns.Item("U_AECC").TitleObject.Caption = "American Ex Credit Card payment"
            oGrid.Columns.Item("U_AECC").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_AECC")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""CreditCard"",""CardName"" from OCRC")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            oGrid.Columns.Item("U_AECN").TitleObject.Caption = "America Ex CreditCard Number"
            oGrid.Columns.Item("U_AEVD").TitleObject.Caption = "America Ex CreditCard ExpiryDate"

            oGrid.Columns.Item("U_AEPM").TitleObject.Caption = "America Ex Credit Card Payment Method"
            oGrid.Columns.Item("U_AEPM").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_AEPM")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""CrTypeCode"",""CrTypeName"" from OCRP")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            '----

            oGrid.Columns.Item("U_CODACT").TitleObject.Caption = "COD Account"
            oEditTextColumn = oGrid.Columns.Item("U_CODACT")
            oEditTextColumn.ChooseFromListUID = "CFL_2"
            oEditTextColumn.ChooseFromListAlias = "FormatCode"
            oEditTextColumn.LinkedObjectType = 1
            oGrid.Columns.Item("U_BIP").TitleObject.Caption = "Default branch"
            oGrid.Columns.Item("U_BIP").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_BIP")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""BPLId"",""BPLName"" from OBPL")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            oGrid.Columns.Item("U_RISE").TitleObject.Caption = "Reserve Invoice Series"
            oGrid.Columns.Item("U_RISE").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_RISE")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""Series"",""SeriesName"" from NNM1 Where ""ObjectCode"" = '13'")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            oGrid.Columns.Item("U_IPSE").TitleObject.Caption = "Incoming Payment Series"
            oGrid.Columns.Item("U_IPSE").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_IPSE")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""Series"",""SeriesName"" from NNM1 Where ""ObjectCode"" = '24'")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            oGrid.Columns.Item("U_SRSE").TitleObject.Caption = "Sale Return Series"
            oGrid.Columns.Item("U_SRSE").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_SRSE")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""Series"",""SeriesName"" from NNM1 Where ""ObjectCode"" = '16'")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            oGrid.Columns.Item("U_CNSE").TitleObject.Caption = "Credit Note Series"
            oGrid.Columns.Item("U_CNSE").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_CNSE")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""Series"",""SeriesName"" from NNM1 Where ""ObjectCode"" = '14'")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description

            oGrid.Columns.Item("U_DNSE").TitleObject.Caption = "Delivery Note Series"
            oGrid.Columns.Item("U_DNSE").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_DNSE")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""Series"",""SeriesName"" from NNM1 Where ""ObjectCode"" = '15'")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description


            oGrid.Columns.Item("U_Freight").TitleObject.Caption = "Freight Code"
            oGrid.Columns.Item("U_Freight").Type = SAPbouiCOM.BoGridColumnType.gct_ComboBox
            oComboColumn = oGrid.Columns.Item("U_Freight")
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery("Select ""ExpnsCode"",""ExpnsName"" From OEXD")
            oComboColumn.ValidValues.Add("", "")
            Do Until oRecordSet.EoF = True
                oComboColumn.ValidValues.Add(oRecordSet.Fields.Item(0).Value, oRecordSet.Fields.Item(1).Value)
                oRecordSet.MoveNext()
            Loop
            oComboColumn.DisplayType = SAPbouiCOM.BoComboDisplayType.cdt_Description


            oGrid.AutoResizeColumns()
            oGrid.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single
        Catch ex As Exception
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
      
    End Sub

    Private Function Validation(ByVal aform As SAPbouiCOM.Form) As Boolean
        Dim strCo, strCo1, strCo2, strCo3, strCode1, strCode2, strCode3, strCode4 As String
        oGrid = aform.Items.Item("3").Specific
        For intRow As Integer = 0 To oGrid.DataTable.Rows.Count - 1
            If oGrid.DataTable.GetValue("U_SUserName", intRow) = "" Then
                oApplication.Utilities.Message("Enter SAP Userid", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            If oGrid.DataTable.GetValue("U_SPassword", intRow) = "" Then
                oApplication.Utilities.Message("Enter SAP Password", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If
            If oGrid.DataTable.GetValue("U_WCompany", intRow) = "" Then
                oApplication.Utilities.Message("Enter StoreKey", SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                Return False
            End If

            'strCo = oGrid.DataTable.GetValue("U_SCompany", intRow)
            'For intLoop As Integer = intRow + 1 To oGrid.DataTable.Rows.Count - 1
            '    strCode1 = oGrid.DataTable.GetValue("U_SCompany", intLoop)
            '    If strCode1 <> "" Then
            '        If strCo.ToUpper = strCode1.ToUpper Then
            '            oApplication.Utilities.Message("Company Name : This entry already exists : " & strCode1, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
            '            oGrid.Columns.Item("U_SCompany").Click(intLoop)
            '            Return False
            '        End If
            '    End If
            'Next
            strCo2 = oGrid.DataTable.GetValue("U_WCompany", intRow)
            For intLoop As Integer = intRow + 1 To oGrid.DataTable.Rows.Count - 1
                strCode3 = oGrid.DataTable.GetValue("U_WCompany", intLoop)
                If strCode3 <> "" Then
                    If strCo2.ToUpper = strCode3.ToUpper Then
                        oApplication.Utilities.Message("Store Key : This entry already exists : " & strCode3, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        oGrid.Columns.Item("U_WCompany").Click(intLoop)
                        Return False
                    End If
                End If
            Next

            If oGrid.DataTable.GetValue("U_SCompany", intRow) = oApplication.Company.CompanyDB Then

                Dim strBranch As String = oGrid.DataTable.GetValue("U_BIP", intRow)
                If strBranch <> "" Then
                    Dim strRISE As String = oGrid.DataTable.GetValue("U_RISE", intRow)
                    Dim strIPSE As String = oGrid.DataTable.GetValue("U_IPSE", intRow)
                    Dim strSRSE As String = oGrid.DataTable.GetValue("U_SRSE", intRow)
                    Dim strCNSE As String = oGrid.DataTable.GetValue("U_CNSE", intRow)
                    Dim strDNSE As String = oGrid.DataTable.GetValue("U_DNSE", intRow)
                    Dim strFreight As String = oGrid.DataTable.GetValue("U_Freight", intRow)
                    Dim strWCompany As String = oGrid.DataTable.GetValue("U_WCompany", intRow)

                    If strRISE = "" Then
                        oApplication.Utilities.Message("Select Series for Reserve Invoice for Company(WEB) : " & strWCompany, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    ElseIf strIPSE = "" Then
                        oApplication.Utilities.Message("Select Series for Incoming Payment for Company(WEB) : " & strWCompany, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    ElseIf strSRSE = "" Then
                        oApplication.Utilities.Message("Select Series for Sale Return for Company(WEB) : " & strWCompany, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    ElseIf strCNSE = "" Then
                        oApplication.Utilities.Message("Select Series for Credit Note for Company(WEB) : " & strWCompany, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    ElseIf strDNSE = "" Then
                        oApplication.Utilities.Message("Select Series for Delivery Note for Company(WEB) : " & strWCompany, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    ElseIf strFreight = "" Then
                        oApplication.Utilities.Message("Select Series for Freight for Company(WEB) : " & strWCompany, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                End If
                
            End If
        Next
        Return True
    End Function

    Private Function AddUDT(ByVal aform As SAPbouiCOM.Form) As Boolean
        Dim strTable, strCode, strType As String
        Dim strdate As String
        Dim oUsertable As SAPbobsCOM.UserTable
        oGrid = aform.Items.Item("3").Specific
        strTable = "@SEI_ECOMPANY"
        oUsertable = oApplication.Company.UserTables.Item("SEI_ECOMPANY")
        For intRow As Integer = 0 To oGrid.DataTable.Rows.Count - 1
            strCode = oGrid.DataTable.GetValue("Code", intRow)
            oEditTextColumn = oGrid.Columns.Item("U_SUserName")
            Try
                strType = oEditTextColumn.GetText(oGrid.DataTable.Rows.Count - 1).ToString
            Catch ex As Exception
                strType = ""
            End Try

            If strType <> "" Then
                If oUsertable.GetByKey(strCode) Then
                    oUsertable.Code = strCode
                    oUsertable.Name = strCode
                    oUsertable.UserFields.Fields.Item("U_WCompany").Value = oGrid.DataTable.GetValue("U_WCompany", intRow)
                    oUsertable.UserFields.Fields.Item("U_SCompany").Value = oGrid.DataTable.GetValue("U_SCompany", intRow)
                    oUsertable.UserFields.Fields.Item("U_SUserName").Value = oGrid.DataTable.GetValue("U_SUserName", intRow)
                    oUsertable.UserFields.Fields.Item("U_SPassword").Value = oGrid.DataTable.GetValue("U_SPassword", intRow)
                    oUsertable.UserFields.Fields.Item("U_WCompany").Value = oGrid.DataTable.GetValue("U_WCompany", intRow)
                    oUsertable.UserFields.Fields.Item("U_SP").Value = oGrid.DataTable.GetValue("U_SP", intRow)
                    oUsertable.UserFields.Fields.Item("U_LC").Value = oGrid.DataTable.GetValue("U_LC", intRow)
                    oUsertable.UserFields.Fields.Item("U_CustSeries").Value = oGrid.DataTable.GetValue("U_CustSeries", intRow)
                    oUsertable.UserFields.Fields.Item("U_WhsCode").Value = oGrid.DataTable.GetValue("U_WhsCode", intRow)
                    oUsertable.UserFields.Fields.Item("U_PPCC").Value = oGrid.DataTable.GetValue("U_PPCC", intRow)
                    oUsertable.UserFields.Fields.Item("U_PPPM").Value = oGrid.DataTable.GetValue("U_PPPM", intRow)
                    oUsertable.UserFields.Fields.Item("U_CC").Value = oGrid.DataTable.GetValue("U_CC", intRow)
                    oUsertable.UserFields.Fields.Item("U_BIP").Value = oGrid.DataTable.GetValue("U_BIP", intRow)
                    oUsertable.UserFields.Fields.Item("U_CODACT").Value = oGrid.DataTable.GetValue("U_CODACT", intRow)
                    oUsertable.UserFields.Fields.Item("U_PPCN").Value = oGrid.DataTable.GetValue("U_PPCN", intRow)
                    strdate = oGrid.DataTable.GetValue("U_PPVD", intRow)
                    If strdate <> "" Then
                        oUsertable.UserFields.Fields.Item("U_PPVD").Value = oGrid.DataTable.GetValue("U_PPVD", intRow)
                    End If
                    oUsertable.UserFields.Fields.Item("U_CCCN").Value = oGrid.DataTable.GetValue("U_CCCN", intRow)
                    strdate = oGrid.DataTable.GetValue("U_CCVD", intRow)
                    If strdate <> "" Then
                        oUsertable.UserFields.Fields.Item("U_CCVD").Value = oGrid.DataTable.GetValue("U_CCVD", intRow)
                    End If
                    oUsertable.UserFields.Fields.Item("U_CCPM").Value = oGrid.DataTable.GetValue("U_CCPM", intRow)

                    '---
                    oUsertable.UserFields.Fields.Item("U_AECC").Value = oGrid.DataTable.GetValue("U_AECC", intRow)
                    oUsertable.UserFields.Fields.Item("U_AEPM").Value = oGrid.DataTable.GetValue("U_AEPM", intRow)
                    oUsertable.UserFields.Fields.Item("U_AECN").Value = oGrid.DataTable.GetValue("U_AECN", intRow)
                    strdate = oGrid.DataTable.GetValue("U_AEVD", intRow)
                    If strdate <> "" Then
                        oUsertable.UserFields.Fields.Item("U_AEVD").Value = oGrid.DataTable.GetValue("U_AEVD", intRow)
                    End If
                    '---

                    oUsertable.UserFields.Fields.Item("U_RISE").Value = oGrid.DataTable.GetValue("U_RISE", intRow)
                    oUsertable.UserFields.Fields.Item("U_IPSE").Value = oGrid.DataTable.GetValue("U_IPSE", intRow)
                    oUsertable.UserFields.Fields.Item("U_SRSE").Value = oGrid.DataTable.GetValue("U_SRSE", intRow)
                    oUsertable.UserFields.Fields.Item("U_CNSE").Value = oGrid.DataTable.GetValue("U_CNSE", intRow)
                    oUsertable.UserFields.Fields.Item("U_DNSE").Value = oGrid.DataTable.GetValue("U_DNSE", intRow)
                    oUsertable.UserFields.Fields.Item("U_Freight").Value = oGrid.DataTable.GetValue("U_Freight", intRow)

                    If oUsertable.Update <> 0 Then
                        oApplication.Utilities.Message(oApplication.Company.GetLastErrorDescription, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    End If

                Else
                    strCode = oApplication.Utilities.getMaxCode(strTable, "Code")
                    oUsertable.Code = strCode
                    oUsertable.Name = strCode
                    oUsertable.UserFields.Fields.Item("U_WCompany").Value = oGrid.DataTable.GetValue("U_WCompany", intRow)
                    oUsertable.UserFields.Fields.Item("U_SCompany").Value = oGrid.DataTable.GetValue("U_SCompany", intRow)
                    oUsertable.UserFields.Fields.Item("U_SUserName").Value = oGrid.DataTable.GetValue("U_SUserName", intRow)
                    oUsertable.UserFields.Fields.Item("U_SPassword").Value = oGrid.DataTable.GetValue("U_SPassword", intRow)
                    oUsertable.UserFields.Fields.Item("U_WCompany").Value = oGrid.DataTable.GetValue("U_WCompany", intRow)
                    oUsertable.UserFields.Fields.Item("U_SP").Value = oGrid.DataTable.GetValue("U_SP", intRow)
                    oUsertable.UserFields.Fields.Item("U_LC").Value = oGrid.DataTable.GetValue("U_LC", intRow)
                    oUsertable.UserFields.Fields.Item("U_CustSeries").Value = oGrid.DataTable.GetValue("U_CustSeries", intRow)
                    oUsertable.UserFields.Fields.Item("U_WhsCode").Value = oGrid.DataTable.GetValue("U_WhsCode", intRow)
                    oUsertable.UserFields.Fields.Item("U_PPCC").Value = oGrid.DataTable.GetValue("U_PPCC", intRow)
                    oUsertable.UserFields.Fields.Item("U_PPPM").Value = oGrid.DataTable.GetValue("U_PPPM", intRow)
                    oUsertable.UserFields.Fields.Item("U_CC").Value = oGrid.DataTable.GetValue("U_CC", intRow)
                    oUsertable.UserFields.Fields.Item("U_BIP").Value = oGrid.DataTable.GetValue("U_BIP", intRow)
                    oUsertable.UserFields.Fields.Item("U_CODACT").Value = oGrid.DataTable.GetValue("U_CODACT", intRow)
                    oUsertable.UserFields.Fields.Item("U_PPCN").Value = oGrid.DataTable.GetValue("U_PPCN", intRow)
                    strdate = oGrid.DataTable.GetValue("U_PPVD", intRow)
                    If strdate <> "" Then
                        oUsertable.UserFields.Fields.Item("U_PPVD").Value = oGrid.DataTable.GetValue("U_PPVD", intRow)
                    End If
                    oUsertable.UserFields.Fields.Item("U_CCCN").Value = oGrid.DataTable.GetValue("U_CCCN", intRow)
                    strdate = oGrid.DataTable.GetValue("U_CCVD", intRow)
                    If strdate <> "" Then
                        oUsertable.UserFields.Fields.Item("U_CCVD").Value = oGrid.DataTable.GetValue("U_CCVD", intRow)
                    End If
                    oUsertable.UserFields.Fields.Item("U_CCPM").Value = oGrid.DataTable.GetValue("U_CCPM", intRow)
                    '---
                    oUsertable.UserFields.Fields.Item("U_AECC").Value = oGrid.DataTable.GetValue("U_AECC", intRow)
                    oUsertable.UserFields.Fields.Item("U_AEPM").Value = oGrid.DataTable.GetValue("U_AEPM", intRow)
                    oUsertable.UserFields.Fields.Item("U_AECN").Value = oGrid.DataTable.GetValue("U_AECN", intRow)
                    strdate = oGrid.DataTable.GetValue("U_AEVD", intRow)
                    If strdate <> "" Then
                        oUsertable.UserFields.Fields.Item("U_AEVD").Value = oGrid.DataTable.GetValue("U_AEVD", intRow)
                    End If
                    '---

                    oUsertable.UserFields.Fields.Item("U_RISE").Value = oGrid.DataTable.GetValue("U_RISE", intRow)
                    oUsertable.UserFields.Fields.Item("U_IPSE").Value = oGrid.DataTable.GetValue("U_IPSE", intRow)
                    oUsertable.UserFields.Fields.Item("U_SRSE").Value = oGrid.DataTable.GetValue("U_SRSE", intRow)
                    oUsertable.UserFields.Fields.Item("U_CNSE").Value = oGrid.DataTable.GetValue("U_CNSE", intRow)
                    oUsertable.UserFields.Fields.Item("U_DNSE").Value = oGrid.DataTable.GetValue("U_DNSE", intRow)
                    oUsertable.UserFields.Fields.Item("U_Freight").Value = oGrid.DataTable.GetValue("U_Freight", intRow)

                    If oUsertable.Add <> 0 Then
                        oApplication.Utilities.Message(oApplication.Company.GetLastErrorDescription, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        Return False
                    End If
                End If
            End If
        Next
        CommitTrans("Save")
        Return True
    End Function

    Private Sub AddRow(ByVal aForm As SAPbouiCOM.Form)
        oGrid = aForm.Items.Item("3").Specific
        oEditTextColumn = oGrid.Columns.Item("U_SUserName")

        Dim strCode As String
        If oGrid.DataTable.Rows.Count - 1 <= 0 Then
            oGrid.DataTable.Rows.Add()
        End If
        'oEditTextColumn = oGrid.Columns.Item("U_Z_HRPeoobjCode")
        strCode = oEditTextColumn.GetText(oGrid.DataTable.Rows.Count - 1).ToString
        ' strCode = oEditTextColumn.GetTex(oGrid.DataTable.Rows.Count - 1).Value
        If strCode <> "" Then
            oGrid.DataTable.Rows.Add()
            If aForm.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE And aForm.Mode <> SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                aForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
            End If
        End If
    End Sub

#Region "DeleteRow"
    Private Sub DeleteRow(ByVal aForm As SAPbouiCOM.Form)

        oGrid = aForm.Items.Item("3").Specific
        If oApplication.SBO_Application.MessageBox("Do you want to delete selected company details?", , "Continue", "Cancel") = 2 Then
            Exit Sub
        End If
        Dim strCode As String
        Dim oTemp As SAPbobsCOM.Recordset
        oTemp = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        For intRow As Integer = 0 To oGrid.DataTable.Rows.Count - 1
            If oGrid.Rows.IsSelected(intRow) Then
                strCode = oGrid.DataTable.GetValue("Code", intRow)
                oTemp.DoQuery("Update ""@SEI_ECOMPANY"" set ""Name""=""Name"" || '_XD' where ""Code""='" & strCode & "'")
                oGrid.DataTable.Rows.Remove(intRow)
                If aForm.Mode <> SAPbouiCOM.BoFormMode.fm_ADD_MODE And aForm.Mode <> SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                    aForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                End If
                Exit Sub
            End If
        Next
    End Sub

#End Region

    Private Sub BindPayment(ByVal aForm As SAPbouiCOM.Form, ByVal PaymentType As String, ByVal aChoice As String, ByVal Row As Integer)

        oGrid = aForm.Items.Item("3").Specific
        Dim oRec As SAPbobsCOM.Recordset
        oRec = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        Try
            oForm.Freeze(True)
            strQry = "Select ""U_CCCN"",""U_CCVD"" from OCRC where ""CreditCard""='" & PaymentType & "'"
            oRec.DoQuery(strQry)
            If oRec.RecordCount > 0 Then
                If aChoice = "P" Then
                    oGrid.DataTable.SetValue("U_PPCN", Row, oRec.Fields.Item("U_CCCN").Value)
                    oGrid.DataTable.SetValue("U_PPVD", Row, oRec.Fields.Item("U_CCVD").Value)
                ElseIf aChoice = "C" Then
                    oGrid.DataTable.SetValue("U_CCCN", Row, oRec.Fields.Item("U_CCCN").Value)
                    oGrid.DataTable.SetValue("U_CCVD", Row, oRec.Fields.Item("U_CCVD").Value)
                End If
            End If
            oForm.Freeze(False)
        Catch ex As Exception
            oForm.Freeze(False)
            oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
        End Try
    End Sub

    Private Sub CommitTrans(ByVal aChoice As String)
        Dim ORec As SAPbobsCOM.Recordset
        ORec = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        If aChoice = "Save" Then
            ORec.DoQuery("Delete from ""@SEI_ECOMPANY"" where  ""Name"" Like '%_XD'")
        Else
            ORec.DoQuery("Update ""@SEI_ECOMPANY"" set ""Name""=""Code""  where  ""Name"" Like '%_XD'")

        End If
    End Sub

#Region "Item Event"
    Public Overrides Sub ItemEvent(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        Try
            If pVal.FormTypeEx = frm_CompMap Then
                Select Case pVal.BeforeAction
                    Case True
                        Select Case pVal.EventType
                            Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD
                           
                            Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                If pVal.ItemUID = "2" Then
                                    CommitTrans("Cancel")
                                End If
                        End Select

                    Case False
                        Select Case pVal.EventType
                            Case SAPbouiCOM.BoEventTypes.et_FORM_LOAD
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)

                            Case SAPbouiCOM.BoEventTypes.et_COMBO_SELECT
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                If pVal.ItemUID = "3" And pVal.ColUID = "U_PPCC" Then
                                    oGrid = oForm.Items.Item("3").Specific
                                    oComboColumn = oGrid.Columns.Item("U_PPCC")
                                    strPayment = oComboColumn.GetSelectedValue(pVal.Row).Value
                                    BindPayment(oForm, strPayment, "P", pVal.Row)
                                End If
                                If pVal.ItemUID = "3" And pVal.ColUID = "U_CC" Then
                                    oGrid = oForm.Items.Item("3").Specific
                                    oComboColumn = oGrid.Columns.Item("U_CC")
                                    strPayment = oComboColumn.GetSelectedValue(pVal.Row).Value
                                    BindPayment(oForm, strPayment, "C", pVal.Row)
                                End If

                            Case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED
                                oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                Select Case pVal.ItemUID
                                    Case "11"
                                        If Validation(oForm) = True Then
                                            If AddUDT(oForm) = True Then
                                                oApplication.Utilities.Message("Operation Completed Successfully", SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                                                oForm.Close()
                                            Else
                                                oApplication.Utilities.Message(oApplication.Company.GetLastErrorDescription, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                            End If
                                        Else
                                            BubbleEvent = False
                                            Exit Sub
                                        End If

                                    Case "4"
                                        If oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                            AddRow(oForm)
                                        End If
                                    Case "5"
                                        If oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                                            DeleteRow(oForm)
                                        End If
                                End Select
                            Case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST
                                Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
                                Dim oCFL As SAPbouiCOM.ChooseFromList
                                Dim val1 As String
                                Dim sCHFL_ID, val, val2, val3, val4 As String
                                Dim intChoice As Integer
                                Dim codebar As String
                                Try
                                    oCFLEvento = pVal
                                    sCHFL_ID = oCFLEvento.ChooseFromListUID
                                    oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                    oCFL = oForm.ChooseFromLists.Item(sCHFL_ID)
                                    If (oCFLEvento.BeforeAction = False) Then
                                        Dim oDataTable As SAPbouiCOM.DataTable
                                        oDataTable = oCFLEvento.SelectedObjects
                                        intChoice = 0
                                        oForm.Freeze(True)

                                        If pVal.ItemUID = "3" And pVal.ColUID = "U_CODACT" Then
                                            oGrid = oForm.Items.Item("3").Specific
                                            val = oDataTable.GetValue("FormatCode", 0)
                                            oGrid.DataTable.SetValue("U_CODACT", pVal.Row, val)
                                        End If
                                        If pVal.ItemUID = "3" And pVal.ColUID = "U_WhsCode" Then
                                            oGrid = oForm.Items.Item("3").Specific
                                            val = oDataTable.GetValue("WhsCode", 0)
                                            oGrid.DataTable.SetValue("U_WhsCode", pVal.Row, val)
                                        End If
                                        oForm.Freeze(False)
                                    End If
                                Catch ex As Exception
                                    ' oApplication.Utilities.Message(ex.Message, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                                    oForm.Freeze(False)
                                End Try
                                'Case SAPbouiCOM.BoEventTypes.et_KEY_DOWN
                                '    oForm = oApplication.SBO_Application.Forms.Item(FormUID)
                                '    If (pVal.ItemUID = "3" And pVal.ColUID = "U_Z_TktCode") And pVal.CharPressed = 9 Then
                                '        Dim objChooseForm As SAPbouiCOM.Form
                                '        Dim objChoose As New clsChooseFromList
                                '        Dim strwhs, strProject, strGirdValue As String
                                '        Dim objMatrix As SAPbouiCOM.Grid
                                '        objMatrix = oForm.Items.Item(pVal.ItemUID).Specific
                                '        strwhs = "A"
                                '        strGirdValue = objMatrix.DataTable.GetValue("U_Z_TktCode", pVal.Row)

                                '        If strwhs <> "" Then
                                '            objChoose.ItemUID = pVal.ItemUID
                                '            objChoose.SourceFormUID = FormUID
                                '            objChoose.SourceLabel = 0 'pVal.Row
                                '            objChoose.CFLChoice = strwhs 'oCombo.Selected.Value
                                '            objChoose.choice = "Ticket"
                                '            objChoose.ItemCode = strwhs
                                '            objChoose.Documentchoice = "" ' oApplication.Utilities.GetDocType(oForm)
                                '            objChoose.sourceColumID = pVal.ColUID
                                '            objChoose.sourcerowId = pVal.Row
                                '            objChoose.BinDescrUID = ""
                                '            oApplication.Utilities.LoadForm("CFL1.xml", frm_ChoosefromList1)
                                '            objChooseForm = oApplication.SBO_Application.Forms.ActiveForm()
                                '            objChoose.databound(objChooseForm)
                                '        End If
                                '    End If
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
                Case mnu_CompMap
                    LoadForm()
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
            If BusinessObjectInfo.BeforeAction = False And BusinessObjectInfo.ActionSuccess = True And (BusinessObjectInfo.EventType = SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD) Then
                oForm = oApplication.SBO_Application.Forms.ActiveForm()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class
