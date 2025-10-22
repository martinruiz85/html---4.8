/****** Object:  StoredProcedure [dbo].[sp_AddColumn]    Script Date: 07/26/2017 09:28:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_AddColumn]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_AddColumn]
GO
/****** Object:  StoredProcedure [dbo].[sp_AddColumn]    Script Date: 07/26/2017 09:28:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure sp_AddColumn(    
 @TABLE_NAME varchar(max),    
 @COLUMN_NAME varchar(max),    
 @TABLE_SCHEMA varchar(max) = 'dbo'    
     
)    
as    
begin    
PRINT '--INICIA'        
BEGIN TRY    
      BEGIN TRAN    
      SET NOCOUNT ON    
      declare @Item varchar(max),    
     @ItemText varchar(max)    
      select * into #temp from [dbo].[Split](@COLUMN_NAME,'|')          
      WHILE (select count(*) from #temp) > 0    
         BEGIN    
                  SELECT TOP 1 @Item = Item FROM #temp    
                  -- Begin Custom Logic      
                  ---------------------                   
      SELECT @ItemText = 'ALTER TABLE '+QUOTENAME(TABLE_NAME)    
        +' ADD '+QUOTENAME(COLUMN_NAME)+' '     
        + QUOTENAME(DATA_TYPE)    
        + CASE     
         WHEN DATA_TYPE LIKE '%CHAR%' THEN '('+REPLACE(CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR(50)),'-1','MAX') +')'     
        ELSE ''     
        END     
         + CASE     
        WHEN DATA_TYPE IN ('NUMERIC','DECIMAL') THEN '('+CAST(NUMERIC_PRECISION AS VARCHAR(50)) + ',' + CAST(NUMERIC_SCALE AS VARCHAR(50)) + ')'     
        ELSE ''     
        END     
         + CASE     
        WHEN IS_NULLABLE = 'YES' THEN ' NULL'     
          ELSE ' NOT NULL'     
        END     
         + CASE     
          WHEN COLUMN_DEFAULT IS NULL THEN ''     
        ELSE ' DEFAULT '+ COLUMN_DEFAULT     
        END    
      FROM INFORMATION_SCHEMA.COLUMNS      
      WHERE TABLE_NAME = @TABLE_NAME AND TABLE_SCHEMA = @TABLE_SCHEMA and COLUMN_NAME= @Item    
         
         
      PRINT '--@Item:' +  convert(varchar(max),@Item)                        
      PRINT 'IF COL_LENGTH(''' + @TABLE_NAME + ''',''' + @Item + ''') IS NULL'    
      PRINT 'BEGIN'    
      PRINT ' ' + @ItemText          
      PRINT ' PRINT ''LA COLUMNA INSERTADA: ' + @Item + ''''    
      PRINT 'END'    
      --PRINT 'ELSE'    
      --PRINT 'BEGIN'    
      --PRINT ' PRINT ''LA COLUMNA YA EXISTE: ' + @Item + ''''    
      --PRINT 'END'    
                  ---------------------    
                  -- End Custom Logic               
                  DELETE T FROM #temp AS T WHERE T.Item = @Item                      
         END;    
      DROP TABLE #temp    
      SET NOCOUNT OFF    
      COMMIT TRAN    
PRINT '--EXITO'        
END TRY    
BEGIN CATCH    
      PRINT ERROR_MESSAGE()    
      ROLLBACK TRAN    
END CATCH    
END 
GO
