--sp_helptext sp_CompareText  
---------------------------------  
CREATE PROCEDURE sp_CompareText(        
 @Spro VARCHAR(max),        
 @Sdev VARCHAR(max),
 @len int = 0    
)        
as        
begin
        
--http://www.sensefulsolutions.com/2010/10/format-text-as-table.html        
--http://www.tablesgenerator.com/text_tables        
--http://truben.no/table/        
DECLARE @xmlpro XML, @xmldev XML
          
          
declare @temp table(plataform varchar(max), line1 varchar(max))

SET @xmlpro = CAST('<col><row>' + REPLACE(@Spro, CHAR(13), '</row><row>') + '</row></col>' AS XML)
SELECT
	'PRO' AS plataform,
	REPLACE(LTRIM(RTRIM(T.Item.value('.', 'varchar(8000)'))), CHAR(10), '') AS line1 INTO #temp1
FROM @xmlpro.nodes('/col/row') AS T (Item)

SET @xmldev = CAST('<col><row>' + REPLACE(@Sdev, CHAR(13), '</row><row>') + '</row></col>' AS XML)
SELECT
	'DEV' AS plataform,
	REPLACE(LTRIM(RTRIM(T.Item.value('.', 'varchar(8000)'))), CHAR(10), '') AS line1 INTO #temp2
FROM @xmldev.nodes('/col/row') AS T (Item)

INSERT INTO @temp (plataform, line1)
	SELECT
		plataform,
		line1
	FROM #temp1 UNION ALL SELECT
		plataform,
		line1
	FROM #temp2

--select * from @temp        

SELECT
	--tpivot.*,
	ROW_NUMBER() OVER (ORDER BY tpivot.line1 ASC) AS Row,
	tpivot.line1 "Object",
	[PRO] =
			CASE WHEN tpivot.[PRO] > 0 THEN 'x'
				ELSE '-'
			END,
	[DEV] =
			CASE WHEN tpivot.[DEV] > 0 THEN 'x'
				ELSE '-'
			END
FROM @temp t
PIVOT (COUNT("plataform")                                                    -- Pivot on this column        
FOR "plataform" IN ([PRO], [DEV]))         -- Make colum where IncomeDay is in one of these.        
AS tpivot
WHERE LEN(tpivot.line1) > @len


SELECT
	t1.line1 AS "Produccion",
	t2.line1 "Desarrollo"
--into #temp_compare        
FROM #temp1 t1
FULL JOIN #temp2 t2
	ON t1.line1 = t2.line1
WHERE (LEN(t1.line1) > @len
OR LEN(t2.line1) > @len)

--select *         
--from #temp_compare                                     -- Colums to pivot        


--drop table #temp_compare          
DROP TABLE #temp1
DROP TABLE #temp2
END