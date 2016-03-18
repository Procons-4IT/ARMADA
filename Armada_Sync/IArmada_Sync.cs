using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Armada_Sync
{
    public enum TransScenerio
    {
        Customer,
        ARInvoice,
        ARCreditMemo,
        InventoryTransfer,
        GoodsIssue,
        GoodsReceipt,
        GRPO
    };

    public enum TransType { A };    

    interface IArmada_Sync
    {
        void Sync(string strKey, TransType eTrnType, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues, Hashtable htCCdet);
        void Add(string strKey, SAPbobsCOM.Company oCompany, string strLogger, string strWareHouse, string[] strValues, Hashtable htCCdet);

        void Sync(string strKey, TransType eTrnType, SAPbobsCOM.Company oCompany_S, SAPbobsCOM.Company oCompany_D, string strLogger, string strFromWare, string strToWare, string[] strValues);
        void Add(string strKey, SAPbobsCOM.Company oCompany_S, SAPbobsCOM.Company oCompany_D, string strLogger, string strFromWare, string strToWare, string[] strValues);      
    }
}
