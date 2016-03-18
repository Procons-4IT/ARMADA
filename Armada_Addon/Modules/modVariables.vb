Public Module modVariables
    Public oApplication As clsListener
    Public strSQL As String
    Public cfl_Text As String
    Public cfl_Btn As String
    Public blnIsHana As Boolean = False
    Public CompanyDecimalSeprator As String
    Public CompanyThousandSeprator As String
    Public frmSourceMatrix As SAPbouiCOM.Matrix

    Public Enum ValidationResult As Integer
        CANCEL = 0
        OK = 1
    End Enum

    Public Const frm_WAREHOUSES As Integer = 62
    Public Const frm_ITEM_MASTER As Integer = 150
    Public Const frm_INVOICES As Integer = 133
    Public Const frm_GRPO As Integer = 143
    Public Const frm_ORDR As Integer = 139
    Public Const frm_GR_INVENTORY As Integer = 721
    Public Const frm_Project As Integer = 711
    Public Const frm_ProdReceipt As Integer = 65214
    Public Const frm_Delivery As Integer = 140
    Public Const frm_SaleReturn As Integer = 180
    Public Const frm_ARCreditMemo As Integer = 179
    Public Const frm_Customer As Integer = 134

    Public Const mnu_FIND As String = "1281"
    Public Const mnu_ADD As String = "1282"
    Public Const mnu_CLOSE As String = "1286"
    Public Const mnu_NEXT As String = "1288"
    Public Const mnu_PREVIOUS As String = "1289"
    Public Const mnu_FIRST As String = "1290"
    Public Const mnu_LAST As String = "1291"
    Public Const mnu_ADD_ROW As String = "1292"
    Public Const mnu_DELETE_ROW As String = "1293"
    Public Const mnu_TAX_GROUP_SETUP As String = "8458"
    Public Const mnu_DEFINE_ALTERNATIVE_ITEMS As String = "11531"


    Public Const frm_ChoosefromList1 As String = "frm_CFL1"
    Public Const frm_ChoosefromList_Leave As String = "frm_CFL2"

    Public Const xml_MENU As String = "Menu.xml"
    Public Const xml_MENU_REMOVE As String = "RemoveMenus.xml"

    Public Const mnu_BarCode As String = "Menu_B01"
    Public Const xml_BarCode As String = "frm_BarCode.xml"
    Public Const frm_BarCode As String = "frm_BarCode"

    Public Const mnu_OSCL As String = "Menu_02"
    Public Const xml_OSCL As String = "frm_OSCL.xml"
    Public Const frm_OSCL As String = "frm_OSCL"

    Public Const mnu_OPRT As String = "Menu_03"
    Public Const xml_OPRT As String = "frm_OPRT.xml"
    Public Const frm_OPRT As String = "frm_OPRT"

    Public Const mnu_CompMap As String = "Menu_B04"
    Public Const frm_CompMap As String = "frm_CompMap"
    Public Const xml_CompMap As String = "frm_CompMap.xml"

    Public Const mnu_SEI_OUTB As String = "Menu_B05"
    Public Const frm_SEI_OUTB As String = "SEI_OUTB"

    Public Const mnu_DiscountMgt As String = "Menu_B06"
    Public Const frm_DiscountMgt As String = "frm_SEI_ODIS"
    Public Const xml_DiscountMgt As String = "frm_DiscountMgt.xml"

    Public Const mnu_InBound As String = "Menu_B01"
    Public Const xml_InBound As String = "frm_Z_INBOUNDMAPPING.xml"
    Public Const frm_InBound As String = "frm_Z_INBOUND"

    Public Const frm_ChoosefromList_Gen As String = "frm_CFLGEN"

End Module
