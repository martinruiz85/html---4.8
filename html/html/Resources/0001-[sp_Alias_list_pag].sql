/*
	declare @total int
	exec sp_{0}_list_pag 1, 1, @total output, null
	select @total
*/
CREATE procedure sp_{0}_list_pag(
	@UsuarioID	int,
	@LenguajeID	int =1,	
	@total		BIGINT OUTPUT,  
	@Search		varchar(max) = null,
	@Start		INT = 0,  
	@Length		INT = 20,
	@Column		INT = 0,
	@Dir			varchar(20) = 'ASC'
)
AS
BEGIN

	SELECT	
			T.{0}ID, 
			T.Desc{0},
			T.FechaUltAct,
			T.RespUltAct,
			T.Activo
	INTO		#temp 
	FROM		t{0} T
	WHERE	((NULLIF(@Search,'') is NULL) 
			OR (T.Desc{0} like '%' + @Search + '%'))
	ORDER	BY
			T.{0}ID ASC

	SELECT @total = count(*)   
	FROM #temp  
     
	;WITH tempSet AS  
	(  
	SELECT	ROW_NUMBER() OVER (ORDER BY T.{0}ID ASC) AS RowNumber,  
			T.{0}ID,
			T.Desc{0},		
			T.FechaUltAct,
			T.RespUltAct,
			T.Activo
	FROM		#temp T WITH(NOLOCK)  
	)  
	SELECT	*   
	FROM		tempSet tset  
	WHERE	tset.RowNumber BETWEEN (@Start + 1) 
			AND (@Start + @Length)


END	
GO
