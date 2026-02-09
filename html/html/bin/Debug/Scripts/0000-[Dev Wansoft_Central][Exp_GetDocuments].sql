/****** Object:  StoredProcedure [dbo].[Exp_GetDocuments]    Script Date: 03/02/2026 04:29:26 p. m. ******/
DROP PROCEDURE IF EXISTS [dbo].[Exp_GetDocuments]
GO
/****** Object:  StoredProcedure [dbo].[Exp_GetDocuments]    Script Date: 03/02/2026 04:29:26 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE  PROCEDURE [dbo].[Exp_GetDocuments]
@SubsidiaryId INT,
@StartDate DATETIME,
@EndDate DATETIME,
@DocumentType INT
AS
BEGIN

 SET ARITHABORT ON;    

 SELECT ISNULL(DI.BuyerId,0) BuyerId,
   ISNULL(B.[Name],'-') BuyerName,
   DI.SupplierId,
   DI.documentId,
   S.[Name],
   DI.LegalNumber,
   DI.ExpeditionDate,
   DI.ExpirationDate,
   DI.Subtotal,
   DI.Iva,
   DI.Total,
   DI.StatusId,
   total - dbo.fnGetTotalPaid(DI.documentid) TotalDebtor,
   InventoryStatus,
   DI.SubsidiaryId,
   DI.SubAccountId,
   CASE DI.SubAccountId WHEN 0 THEN 0 ELSE SA.AccountId END AccountId,
   CASE DI.SubAccountId WHEN 0 THEN '' ELSE SA.[Name] END SubAccountName,
   CASE DI.SubAccountId WHEN 0 THEN '' ELSE A.[Name] END AccountName,
   CASE DI.SubAccountId WHEN 0 THEN 0 ELSE A.[Type] END [Type],
   CASE DI.SubAccountId WHEN 0 THEN '-' ELSE CASE WHEN A.[Type]=1 THEN 'Costo de producci?n' ELSE 'Gasto' END END TypeOfEgress,
   s.Rfc,
   di.Ieps,        
   DI.UUID,
   dbo.fnGetPurchaseOrderByDocumentId(DI.documentId) AS PurchaseOrders,
   DI.RegistrationDate,
   DI.Discount,
   ISNULL(Inv.InvDocumentId,0) AS InvDocumentId,
	1 as HasRecDocument, --dbo.fnHasRecurringDocumentRelationated(Di.documentId) AS HasRecDocument,
   ISNULL(DI.Retentions, 0) + ISNULL(DI.RetentionsISR, 0) + ISNULL(DI.rCedular, 0) AS Retentions,
   DI.Comments
 FROM dbo.Exp_DocumentInfo DI WITH(NOLOCK)
 INNER JOIN dbo.Exp_Supplier S WITH(NOLOCK) ON S.Id = DI.SupplierID
 LEFT JOIN dbo.Exp_Buyer B WITH(NOLOCK) ON B.Id = DI.BuyerId
 LEFT JOIN dbo.Acc_SubAccount SA WITH(NOLOCK) ON SA.Id = DI.SubAccountId
 LEFT JOIN dbo.Acc_Account A WITH(NOLOCK) ON A.Id = SA.AccountId
 LEFT JOIN dbo.Exp_DocumentInv_CN Inv WITH(NOLOCK) ON Di.documentId = Inv.CNDocumentId AND @DocumentType = 2
 WHERE DI.SubsidiaryId = @SubsidiaryId
 AND DI.ExpeditionDate >= @StartDate
 AND DI.ExpeditionDate < @EndDate
 AND DI.DocumentTypeId = @DocumentType   
 OPTION (RECOMPILE)

END

GO
