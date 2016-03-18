Public NotInheritable Class clsTable

#Region "Private Functions"
    '*************************************************************************************************************
    'Type               : Private Function
    'Name               : AddTables
    'Parameter          : 
    'Return Value       : none
    'Author             : Manu
    'Created Dt         : 
    'Last Modified By   : 
    'Modified Dt        : 
    'Purpose            : Generic Function for adding all Tables in DB. This function shall be called by 
    '                     public functions to create a table
    '**************************************************************************************************************
    Private Sub AddTables(ByVal strTab As String, ByVal strDesc As String, ByVal nType As SAPbobsCOM.BoUTBTableType)
        Dim oUserTablesMD As SAPbobsCOM.UserTablesMD
        Try

            oUserTablesMD = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables)
            'Adding Table
            If Not oUserTablesMD.GetByKey(strTab) Then
                oUserTablesMD.TableName = strTab
                oUserTablesMD.TableDescription = strDesc
                oUserTablesMD.TableType = nType
                If oUserTablesMD.Add <> 0 Then
                    Throw New Exception(oApplication.Company.GetLastErrorDescription)
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTablesMD)
            oUserTablesMD = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    '*************************************************************************************************************
    'Type               : Private Function
    'Name               : AddFields
    'Parameter          : SstrTab As String,strCol As String,
    '                     strDesc As String,nType As Integer,i,nEditSize,nSubType As Integer
    'Return Value       : none
    'Author             : Manu
    'Created Dt         : 
    'Last Modified By   : 
    'Modified Dt        : 
    'Purpose            : Generic Function for adding all Fields in DB Tables. This function shall be called by 
    '                     public functions to create a Field
    '**************************************************************************************************************
    Private Sub AddFields(ByVal strTab As String, _
                            ByVal strCol As String, _
                                ByVal strDesc As String, _
                                    ByVal nType As SAPbobsCOM.BoFieldTypes, _
                                        Optional ByVal i As Integer = 0, _
                                            Optional ByVal nEditSize As Integer = 10, _
                                                Optional ByVal nSubType As SAPbobsCOM.BoFldSubTypes = 0, _
                                                    Optional ByVal Mandatory As SAPbobsCOM.BoYesNoEnum = SAPbobsCOM.BoYesNoEnum.tNO)
        Dim oUserFieldMD As SAPbobsCOM.UserFieldsMD
        Try

            If Not (strTab = "ORCT" Or strTab = "OINV" Or strTab = "OITM" Or strTab = "OCRD" Or strTab = "OWTR" Or strTab = "OUSR" Or strTab = "OITW" Or strTab = "RDR1" Or strTab = "DSC1" Or strTab = "OCRC") Then
                strTab = "@" + strTab
            End If

            If Not IsColumnExists(strTab, strCol) Then
                oUserFieldMD = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)

                oUserFieldMD.Description = strDesc
                oUserFieldMD.Name = strCol
                oUserFieldMD.Type = nType
                oUserFieldMD.SubType = nSubType
                oUserFieldMD.TableName = strTab
                oUserFieldMD.EditSize = nEditSize
                oUserFieldMD.Mandatory = Mandatory
                If oUserFieldMD.Add <> 0 Then
                    Throw New Exception(oApplication.Company.GetLastErrorDescription)
                End If

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldMD)

            End If

        Catch ex As Exception
            Throw ex
        Finally
            oUserFieldMD = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    '*************************************************************************************************************
    'Type               : Private Function
    'Name               : AddFields
    'Parameter          : SstrTab As String,strCol As String,
    '                     strDesc As String,nType As Integer,i,nEditSize,nSubType As Integer
    'Return Value       : none
    'Author             : Manu
    'Created Dt         : 
    'Last Modified By   : 
    'Modified Dt        : 
    'Purpose            : Generic Function for adding all Fields in DB Tables. This function shall be called by 
    '                     public functions to create a Field
    '**************************************************************************************************************
    Public Sub addField(ByVal TableName As String, ByVal ColumnName As String, ByVal ColDescription As String, ByVal FieldType As SAPbobsCOM.BoFieldTypes, ByVal Size As Integer, ByVal SubType As SAPbobsCOM.BoFldSubTypes, ByVal ValidValues As String, ByVal ValidDescriptions As String, ByVal SetValidValue As String)
        Dim intLoop As Integer
        Dim strValue, strDesc As Array
        Dim objUserFieldMD As SAPbobsCOM.UserFieldsMD
        Try

            strValue = ValidValues.Split(Convert.ToChar(","))
            strDesc = ValidDescriptions.Split(Convert.ToChar(","))
            If (strValue.GetLength(0) <> strDesc.GetLength(0)) Then
                Throw New Exception("Invalid Valid Values")
            End If

            If Not (TableName = "OITM" Or TableName = "NNM1" Or TableName = "SPP1") Then
                TableName = "@" + TableName
            End If

            If (Not IsColumnExists(TableName, ColumnName)) Then
                objUserFieldMD = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)
                objUserFieldMD.TableName = TableName
                objUserFieldMD.Name = ColumnName
                objUserFieldMD.Description = ColDescription
                objUserFieldMD.Type = FieldType
                If (FieldType <> SAPbobsCOM.BoFieldTypes.db_Numeric) Then
                    objUserFieldMD.Size = Size
                Else
                    objUserFieldMD.EditSize = Size
                End If
                objUserFieldMD.SubType = SubType
                objUserFieldMD.DefaultValue = SetValidValue
                For intLoop = 0 To strValue.GetLength(0) - 1
                    objUserFieldMD.ValidValues.Value = strValue(intLoop)
                    objUserFieldMD.ValidValues.Description = strDesc(intLoop)
                    objUserFieldMD.ValidValues.Add()
                Next
                If (objUserFieldMD.Add() <> 0) Then
                    MsgBox(oApplication.Company.GetLastErrorDescription)
                End If
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objUserFieldMD)
            Else
            End If

        Catch ex As Exception
            MsgBox(ex.Message)

        Finally
            objUserFieldMD = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()

        End Try


    End Sub

    '*************************************************************************************************************
    'Type               : Private Function
    'Name               : IsColumnExists
    'Parameter          : ByVal Table As String, ByVal Column As String
    'Return Value       : Boolean
    'Author             : Manu
    'Created Dt         : 
    'Last Modified By   : 
    'Modified Dt        : 
    'Purpose            : Function to check if the Column already exists in Table
    '**************************************************************************************************************
    Private Function IsColumnExists(ByVal Table As String, ByVal Column As String) As Boolean
        Dim oRecordSet As SAPbobsCOM.Recordset
        Try
            strSQL = "SELECT COUNT(*) FROM CUFD WHERE Upper(""TableID"") = '" & Table.ToUpper & "' AND Upper(""AliasID"") = '" & Column.Trim.ToUpper & "'"
            oRecordSet = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRecordSet.DoQuery(strSQL)

            If oRecordSet.Fields.Item(0).Value = 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw ex
        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet)
            oRecordSet = Nothing
            GC.Collect()
        End Try
    End Function

    Private Sub AddKey(ByVal strTab As String, ByVal strColumn As String, ByVal strKey As String, ByVal i As Integer)
        Dim oUserKeysMD As SAPbobsCOM.UserKeysMD

        Try
            '// The meta-data object must be initialized with a
            '// regular UserKeys object
            oUserKeysMD = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserKeys)

            If Not oUserKeysMD.GetByKey("@" & strTab, i) Then

                '// Set the table name and the key name
                oUserKeysMD.TableName = strTab
                oUserKeysMD.KeyName = strKey

                '// Set the column's alias
                oUserKeysMD.Elements.ColumnAlias = strColumn
                oUserKeysMD.Elements.Add()
                oUserKeysMD.Elements.ColumnAlias = "RentFac"

                '// Determine whether the key is unique or not
                oUserKeysMD.Unique = SAPbobsCOM.BoYesNoEnum.tYES

                '// Add the key
                If oUserKeysMD.Add <> 0 Then
                    Throw New Exception(oApplication.Company.GetLastErrorDescription)
                End If

            End If

        Catch ex As Exception
            Throw ex

        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserKeysMD)
            oUserKeysMD = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Sub

    '********************************************************************
    'Type		            :   Function    
    'Name               	:	AddUDO
    'Parameter          	:   
    'Return Value       	:	Boolean
    'Author             	:	
    'Created Date       	:	
    'Last Modified By	    :	
    'Modified Date        	:	
    'Purpose             	:	To Add a UDO for Transaction Tables
    '********************************************************************
    'Private Sub AddUDO(ByVal strUDO As String, ByVal strDesc As String, ByVal strTable As String, _
    '                            Optional ByVal sFind1 As String = "", Optional ByVal sFind2 As String = "", _
    '                                    Optional ByVal strChildTbl As String = "", Optional ByVal nObjectType As SAPbobsCOM.BoUDOObjType = SAPbobsCOM.BoUDOObjType.boud_Document)

    '    Dim oUserObjectMD As SAPbobsCOM.UserObjectsMD
    '    Try
    '        oUserObjectMD = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD)
    '        If oUserObjectMD.GetByKey(strUDO) = 0 Then
    '            oUserObjectMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tYES
    '            oUserObjectMD.CanClose = SAPbobsCOM.BoYesNoEnum.tYES
    '            oUserObjectMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tNO
    '            oUserObjectMD.CanDelete = SAPbobsCOM.BoYesNoEnum.tNO
    '            oUserObjectMD.CanFind = SAPbobsCOM.BoYesNoEnum.tYES

    '            If sFind1 <> "" And sFind2 <> "" Then
    '                oUserObjectMD.FindColumns.ColumnAlias = sFind1
    '                oUserObjectMD.FindColumns.Add()
    '                oUserObjectMD.FindColumns.SetCurrentLine(1)
    '                oUserObjectMD.FindColumns.ColumnAlias = sFind2
    '                oUserObjectMD.FindColumns.Add()
    '            End If

    '            oUserObjectMD.CanLog = SAPbobsCOM.BoYesNoEnum.tNO
    '            oUserObjectMD.LogTableName = ""
    '            oUserObjectMD.CanYearTransfer = SAPbobsCOM.BoYesNoEnum.tNO
    '            oUserObjectMD.ExtensionName = ""

    '            If strChildTbl <> "" Then
    '                oUserObjectMD.ChildTables.TableName = strChildTbl
    '            End If

    '            oUserObjectMD.ManageSeries = SAPbobsCOM.BoYesNoEnum.tNO
    '            oUserObjectMD.Code = strUDO
    '            oUserObjectMD.Name = strDesc
    '            oUserObjectMD.ObjectType = nObjectType
    '            oUserObjectMD.TableName = strTable

    '            If oUserObjectMD.Add() <> 0 Then
    '                Throw New Exception(oApplication.Company.GetLastErrorDescription)
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Throw ex

    '    Finally
    '        System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD)
    '        oUserObjectMD = Nothing
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    End Try

    'End Sub

    Private Sub AddUDO(ByVal strUDO As String, ByVal strDesc As String, ByVal strTable As String, _
                                Optional ByVal sFind1 As String = "", Optional ByVal sFind2 As String = "", _
                                        Optional ByVal strChildTbl As String = "", _
                                        Optional ByVal blnMultiChild As Boolean = False,
                                        Optional ByVal nObjectType As SAPbobsCOM.BoUDOObjType = SAPbobsCOM.BoUDOObjType.boud_Document, _
                                                                                                 Optional ByVal blnDefault As Boolean = False _
                                                                                                     , Optional ByVal strDColumns As String = "")

        Dim oUserObjectMD As SAPbobsCOM.UserObjectsMD
        Try
            oUserObjectMD = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD)
            If oUserObjectMD.GetByKey(strUDO) = 0 Then
                oUserObjectMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tYES
                oUserObjectMD.CanClose = SAPbobsCOM.BoYesNoEnum.tYES

                If blnDefault Then
                    oUserObjectMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tYES
                    oUserObjectMD.EnableEnhancedForm = SAPbobsCOM.BoYesNoEnum.tNO

                    Dim strColumns As String()
                    strColumns = strDColumns.Split(",")
                    For Each strCol As String In strColumns
                        Dim strColumn As String() = strCol.Split("$")
                        oUserObjectMD.FormColumns.FormColumnAlias = strColumn(0)
                        oUserObjectMD.FormColumns.FormColumnDescription = strColumn(1)
                        oUserObjectMD.FormColumns.Add()
                    Next

                End If

                oUserObjectMD.CanDelete = SAPbobsCOM.BoYesNoEnum.tYES
                oUserObjectMD.CanFind = SAPbobsCOM.BoYesNoEnum.tYES

                If sFind1 <> "" And sFind2 <> "" Then
                    oUserObjectMD.FindColumns.ColumnAlias = sFind1
                    oUserObjectMD.FindColumns.Add()
                    oUserObjectMD.FindColumns.SetCurrentLine(1)
                    oUserObjectMD.FindColumns.ColumnAlias = sFind2
                    oUserObjectMD.FindColumns.Add()
                End If

                oUserObjectMD.CanLog = SAPbobsCOM.BoYesNoEnum.tNO
                oUserObjectMD.LogTableName = ""
                oUserObjectMD.CanYearTransfer = SAPbobsCOM.BoYesNoEnum.tNO
                oUserObjectMD.ExtensionName = ""

                If Not blnMultiChild Then
                    If strChildTbl <> "" Then
                        oUserObjectMD.ChildTables.TableName = strChildTbl
                    End If
                Else
                    Dim strChild As String()
                    strChild = strChildTbl.Split(",")
                    For Each strTabl As String In strChild
                        oUserObjectMD.ChildTables.TableName = strTabl
                        oUserObjectMD.ChildTables.Add()
                    Next
                End If

                'If strChildTbl <> "" Then
                '    oUserObjectMD.ChildTables.TableName = strChildTbl
                'End If

                oUserObjectMD.ManageSeries = SAPbobsCOM.BoYesNoEnum.tNO
                oUserObjectMD.Code = strUDO
                oUserObjectMD.Name = strDesc
                oUserObjectMD.ObjectType = nObjectType
                oUserObjectMD.TableName = strTable

                If oUserObjectMD.Add() <> 0 Then
                    Throw New Exception(oApplication.Company.GetLastErrorDescription)
                End If
            End If

        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD)
            oUserObjectMD = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub AddUDO_1(ByVal strUDO As String, ByVal strDesc As String, ByVal strTable As String, _
                                Optional ByVal sFind1 As String = "", Optional ByVal sFind2 As String = "", _
                                        Optional ByVal strChildTbl As String = "", _
                                        Optional ByVal nObjectType As SAPbobsCOM.BoUDOObjType = SAPbobsCOM.BoUDOObjType.boud_Document, _
                                                                                                 Optional ByVal blnDefault As Boolean = False _
                                                                                                     , Optional ByVal strDColumns As String = "")

        Dim oUserObjectMD As SAPbobsCOM.UserObjectsMD
        Try
            oUserObjectMD = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD)
            If oUserObjectMD.GetByKey(strUDO) = 0 Then
                oUserObjectMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tYES
                oUserObjectMD.CanClose = SAPbobsCOM.BoYesNoEnum.tYES

                If blnDefault Then
                    oUserObjectMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tYES
                    oUserObjectMD.EnableEnhancedForm = SAPbobsCOM.BoYesNoEnum.tNO

                    Dim strColumns As String()
                    strColumns = strDColumns.Split(",")
                    For Each strCol As String In strColumns
                        Dim strColumn As String() = strCol.Split("$")
                        oUserObjectMD.FormColumns.FormColumnAlias = strColumn(0)
                        oUserObjectMD.FormColumns.FormColumnDescription = strColumn(1)
                        oUserObjectMD.FormColumns.Add()
                    Next

                End If

                oUserObjectMD.CanDelete = SAPbobsCOM.BoYesNoEnum.tYES
                oUserObjectMD.CanFind = SAPbobsCOM.BoYesNoEnum.tYES

                If sFind1 <> "" And sFind2 <> "" Then
                    oUserObjectMD.FindColumns.ColumnAlias = sFind1
                    oUserObjectMD.FindColumns.Add()
                    oUserObjectMD.FindColumns.SetCurrentLine(1)
                    oUserObjectMD.FindColumns.ColumnAlias = sFind2
                    oUserObjectMD.FindColumns.Add()
                End If

                oUserObjectMD.CanLog = SAPbobsCOM.BoYesNoEnum.tYES
                oUserObjectMD.LogTableName = "A" & strTable
                oUserObjectMD.CanYearTransfer = SAPbobsCOM.BoYesNoEnum.tNO
                oUserObjectMD.ExtensionName = ""

                If strChildTbl <> "" Then
                    oUserObjectMD.ChildTables.TableName = strChildTbl
                End If

                oUserObjectMD.ManageSeries = SAPbobsCOM.BoYesNoEnum.tNO
                oUserObjectMD.Code = strUDO
                oUserObjectMD.Name = strDesc
                oUserObjectMD.ObjectType = nObjectType
                oUserObjectMD.TableName = strTable

                If oUserObjectMD.Add() <> 0 Then
                    Throw New Exception(oApplication.Company.GetLastErrorDescription)
                End If
            End If

        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD)
            oUserObjectMD = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub UpdateUDO_1(ByVal strUDO As String, ByVal strDesc As String, ByVal strTable As String, _
                                Optional ByVal sFind1 As String = "", Optional ByVal sFind2 As String = "", _
                                        Optional ByVal blnMultiChild As Boolean = False, _
                                        Optional ByVal strChildTbl As String = "", _
                                        Optional ByVal nObjectType As SAPbobsCOM.BoUDOObjType = SAPbobsCOM.BoUDOObjType.boud_Document)

        Dim oUserObjectMD As SAPbobsCOM.UserObjectsMD
        Dim blnUpdate As Boolean = False
        Try
            oUserObjectMD = oApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD)
            If oUserObjectMD.GetByKey(strUDO) Then

                If oUserObjectMD.Name <> strDesc Then
                    oUserObjectMD.Name = strDesc
                    blnUpdate = True
                End If

                If Not blnMultiChild Then
                    If strChildTbl <> "" Then
                        oUserObjectMD.ChildTables.TableName = strChildTbl
                    End If
                Else
                    Dim strChild As String()
                    strChild = strChildTbl.Split(",")

                    For Each strTabl As String In strChild
                        Dim blnTableExists As Boolean = False
                        For index As Integer = 0 To oUserObjectMD.ChildTables.Count - 1
                            oUserObjectMD.ChildTables.SetCurrentLine(index)
                            If oUserObjectMD.ChildTables.TableName = strTabl Then
                                blnTableExists = True
                            End If
                        Next
                        If Not blnTableExists Then
                            blnUpdate = True
                            oUserObjectMD.ChildTables.Add()
                            oUserObjectMD.ChildTables.SetCurrentLine(oUserObjectMD.ChildTables.Count - 1)
                            oUserObjectMD.ChildTables.TableName = strTabl
                        End If
                    Next
                End If

                If blnUpdate Then
                    If oUserObjectMD.Update() <> 0 Then
                        Throw New Exception(oApplication.Company.GetLastErrorDescription)
                    End If
                End If

            End If

        Catch ex As Exception
            Throw ex
            'oApplication.Log.oApplication.Log.Trace_DIET_AddOn_Error(ex)

        Finally
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD)
            oUserObjectMD = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Sub

#End Region

#Region "Public Functions"
    '*************************************************************************************************************
    'Type               : Public Function
    'Name               : CreateTables
    'Parameter          : 
    'Return Value       : none
    'Author             : Manu
    'Created Dt         : 
    'Last Modified By   : 
    'Modified Dt        : 
    'Purpose            : Creating Tables by calling the AddTables & AddFields Functions
    '**************************************************************************************************************
    Public Sub CreateTables()
        Try

            oApplication.SBO_Application.StatusBar.SetText("Initializing Database...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            'oApplication.Company.StartTransaction()

            '---- User Defined Fields
            AddFields("OCRD", "Z_SYNC", "WEB SYNC", SAPbobsCOM.BoFieldTypes.db_Alpha, , 1)
            AddFields("OINV", "Z_SYNC", "WEB SYNC", SAPbobsCOM.BoFieldTypes.db_Alpha, , 1)
            AddFields("OINV", "Z_PAYTYPE", "PAY TYPE", SAPbobsCOM.BoFieldTypes.db_Alpha, , 1)
            AddFields("OINV", "Z_CASHIER", "CASHIER", SAPbobsCOM.BoFieldTypes.db_Alpha, , 100)
            AddFields("OINV", "Z_DOCTIME", "DOC TIME", SAPbobsCOM.BoFieldTypes.db_Alpha, , 30)
            AddFields("OWTR", "Z_SYNC", "WEB SYNC", SAPbobsCOM.BoFieldTypes.db_Alpha, , 1)
            AddFields("OINV", "Z_TrnNum", "Transaction No.", SAPbobsCOM.BoFieldTypes.db_Alpha, , 50)

            AddTables("Z_INBOUNDMAPPING", "In Bound Configuration", SAPbobsCOM.BoUTBTableType.bott_MasterData)
            AddFields("Z_INBOUNDMAPPING", "COMPANY", "SAP Company", SAPbobsCOM.BoFieldTypes.db_Alpha, , 100)
            AddFields("Z_INBOUNDMAPPING", "SAPUSERNAME", "SAP USERNAME", SAPbobsCOM.BoFieldTypes.db_Alpha, , 30)
            AddFields("Z_INBOUNDMAPPING", "SAPPASSWORD", "SAP PASSWORD", SAPbobsCOM.BoFieldTypes.db_Alpha, , 30)
            AddFields("Z_INBOUNDMAPPING", "OUTACCT", "OUTGOING ACCT", SAPbobsCOM.BoFieldTypes.db_Alpha, , 30)
            AddFields("Z_INBOUNDMAPPING", "COUNTRY", "COUNTRY", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            AddFields("Z_INBOUNDMAPPING", "ISMAIN", "IS MAIN", SAPbobsCOM.BoFieldTypes.db_Alpha, , 1)

            'AddFields("Z_INBOUNDMAPPING", "CREDITCARD", "CREDIT CARD", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            'AddFields("Z_INBOUNDMAPPING", "CARDNUMBER", "CARDNUMBER", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            'AddFields("Z_INBOUNDMAPPING", "CARDVALID", "CARDVALID", SAPbobsCOM.BoFieldTypes.db_Date)
            'AddFields("Z_INBOUNDMAPPING", "PAYMENTMETHOD", "PAYMENTMETHOD", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)

            AddTables("Z_INBOUNDMAPPINGC", "In Configuration Child", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)
            AddFields("Z_INBOUNDMAPPINGC", "SHOPID", "SHOPID", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            AddFields("Z_INBOUNDMAPPINGC", "WAREHOUSE", "WAREHOUSE", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            AddFields("Z_INBOUNDMAPPINGC", "PRICELIST", "PRICELIST", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            AddFields("Z_INBOUNDMAPPINGC", "GIACCT", "OUTGOING ACCT", SAPbobsCOM.BoFieldTypes.db_Alpha, , 30)
            AddFields("Z_INBOUNDMAPPINGC", "GRACCT", "OUTGOING ACCT", SAPbobsCOM.BoFieldTypes.db_Alpha, , 30)
            AddFields("Z_INBOUNDMAPPINGC", "LOCATION", "LOCATION", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            AddFields("Z_INBOUNDMAPPINGC", "COSTCEN", "COST CENTER", SAPbobsCOM.BoFieldTypes.db_Alpha, , 40)

            AddTables("Z_INBOUNDMAPPINGC1", "In Configuration Child1", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)
            AddFields("Z_INBOUNDMAPPINGC1", "SCREDITCARD", "SHOP CREDIT CARD", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            AddFields("Z_INBOUNDMAPPINGC1", "CREDITCARD", "CREDIT CARD", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            AddFields("Z_INBOUNDMAPPINGC1", "CARDNUMBER", "CARDNUMBER", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)
            AddFields("Z_INBOUNDMAPPINGC1", "CARDVALID", "CARDVALID", SAPbobsCOM.BoFieldTypes.db_Date)
            AddFields("Z_INBOUNDMAPPINGC1", "PAYMENTMETHOD", "PAYMENTMETHOD", SAPbobsCOM.BoFieldTypes.db_Alpha, , 10)

            AddTables("Z_INBOUNDMAPPINGC2", "In Configuration Child2", SAPbobsCOM.BoUTBTableType.bott_MasterDataLines)
            AddFields("Z_INBOUNDMAPPINGC2", "COMPANY", "SAP Company", SAPbobsCOM.BoFieldTypes.db_Alpha, , 100)
            AddFields("Z_INBOUNDMAPPINGC2", "GIACCT", "OUTGOING ACCT", SAPbobsCOM.BoFieldTypes.db_Alpha, , 30)
            AddFields("Z_INBOUNDMAPPINGC2", "GRACCT", "OUTGOING ACCT", SAPbobsCOM.BoFieldTypes.db_Alpha, , 30)

            '---- User Defined Object
            CreateUDO()

            'If oApplication.Company.InTransaction() Then
            '    oApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
            'End If
            oApplication.SBO_Application.StatusBar.SetText("Database creation completed...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        Catch ex As Exception
            'If oApplication.Company.InTransaction() Then
            '    oApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            'End If
            Throw ex
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Public Sub CreateUDO()
        Try
            'Add UDO
            'Code$Code,U_COMPANY$SAP COMPANY,U_SAPUSERNAME$ SAP USERNAME,U_SAPPASSWORD$SAP PASSWORD,U_CREDITCARD$CREDIT CARD,U_CARDNUMBER$CARD NO,U_CARDVALID$CARD VALID,U_PAYMENTMETHOD$ PAYMENT METHOD 
            AddUDO("Z_INBOUNDMAPPING", "Z_INBOUNDMAPPING", "Z_INBOUNDMAPPING", "U_COMPANY", "U_SAPUSERNAME", "Z_INBOUNDMAPPINGC,Z_INBOUNDMAPPINGC1,Z_INBOUNDMAPPINGC2", True, SAPbobsCOM.BoUDOObjType.boud_MasterData, False, "") ' In Bound 
            UpdateUDO_1("Z_INBOUNDMAPPING", "Z_INBOUNDMAPPING", "Z_INBOUNDMAPPING", "U_COMPANY", "U_SAPUSERNAME", True, "Z_INBOUNDMAPPINGC2", SAPbobsCOM.BoUDOObjType.boud_MasterData)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

End Class
